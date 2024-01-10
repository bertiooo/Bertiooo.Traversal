using Bertiooo.Traversal;
using System.Diagnostics;
using Tests.Model;

namespace Tests
{
    public class ConvertibleTraverserTests : IClassFixture<ConvertibleFixture>
	{
		private readonly ConvertibleFixture fixture;

		public ConvertibleTraverserTests(ConvertibleFixture fixture)
		{
			this.fixture = fixture;
		}

		[Fact]
		public void TraverserTraversesDepthFirst()
		{
			var root = this.fixture.Root;

			var expectedLevels = new int[]
			{
				0, 1, 2, 2, 1, 2, 2
			};

			var actualLevels = root.Traverse(x => x.Children)
				.Use(TraversalMode.DepthFirst)
				.GetNodes()
				.Select(x => x.GetLevel(x => x.Parent));

			Assert.Equal(expectedLevels, actualLevels);
		}

		[Fact]
		public void TraverserTraversesBreadthFirst()
		{
			var root = this.fixture.Root;

			var expectedLevels = new int[]
			{
				0, 1, 1, 2, 2, 2, 2
			};

			var actualLevels = root.Traverse(x => x.Children)
				.Use(TraversalMode.BreadthFirst)
				.GetNodes()
				.Select(x => x.GetLevel(x => x.Parent));

			Assert.Equal(expectedLevels, actualLevels);
		}

		[Fact]
		public void TraverserSkipsNode()
		{
			var root = this.fixture.Root;
			var child = root.Children.First();

			var nodes = root.Traverse(x => x.Children).Skip(child).GetNodes();

			Assert.True(nodes.Any());
			Assert.True(nodes.Contains(child) == false);
			Assert.True(child.Children.All(x => nodes.Contains(x)) == false);

			nodes = root.Traverse(x => x.Children).Skip(x => x.Equals(child)).GetNodes();

			Assert.True(nodes.Any());
			Assert.True(nodes.Contains(child) == false);
			Assert.True(child.Children.All(x => nodes.Contains(x)) == false);
		}

		[Fact]
		public void TraverserExcludesNode()
		{
			var root = this.fixture.Root;
			var child = root.Children.First();

			var nodes = root.Traverse(x => x.Children).Exclude(child).GetNodes();

			Assert.True(nodes.Any());
			Assert.True(nodes.Contains(child) == false);

			// child nodes still are traversed
			Assert.True(child.Children.All(x => nodes.Contains(x)));

			nodes = root.Traverse(x => x.Children).Exclude(x => x.Equals(child)).GetNodes();

			Assert.True(nodes.Any());
			Assert.True(nodes.Contains(child) == false);

			// child nodes still are traversed
			Assert.True(child.Children.All(x => nodes.Contains(x)));
		}

		[Fact]
		public void TraverserDisablesCallbacksForNode()
		{
			var root = this.fixture.Root;
			var child = root.Children.First();
			var secondChild = root.Children.ElementAt(1);

			var numberOfNodes = root.WithDescendants(x => x.Children).Count();

			var numberOfCallbacks = 0;

			root.Traverse(x => x.Children)
				.DisableCallbacksFor(child)
				.DisableCallbacksFor(x => x.Equals(secondChild))
				.WithAction(() => numberOfCallbacks++)
				.Execute();

			Assert.Equal(numberOfNodes - 2, numberOfCallbacks);
		}

		[Fact]
		public void TraverserIgnoresNode()
		{
			var root = this.fixture.Root;
			var child = root.Children.First();
			var secondChild = root.Children.ElementAt(1);

			var numberOfNodes = root.WithDescendants(x => x.Children).Count();

			var numberOfCallbacks = 0;

			var expectedNodes = new List<Convertible>() { root };
			expectedNodes.AddRange(child.Children);
			expectedNodes.AddRange(secondChild.Children);

			var nodes = root.Traverse(x => x.Children)
				.Use(TraversalMode.BreadthFirst)
				.Ignore(child)
				.Ignore(x => x.Equals(secondChild))
				.WithAction(() => numberOfCallbacks++)
				.GetNodes()
				.ToList();

			Assert.Equal(numberOfNodes - 2, numberOfCallbacks);
			Assert.Equal(expectedNodes, nodes);
		}

		[Fact]
		public async void TraverserExecutesAsync()
		{
			var root = this.fixture.Root;

			var traversal = root.Traverse(x => x.Children)
				.GetNodesAsync();

			Debug.WriteLine("awaiting");

			var nodes = await traversal;
			Assert.Equal(root.WithDescendants(x => x.Children), nodes);
		}

		[Fact]
		public void TraverserInvokesCallbacks()
		{
			var root = this.fixture.Root;
			var secondChild = root.Children.ElementAt(1);

			var prepareInvoked = false;
			var failureInvoked = false;
			var successInvoked = false;
			var finishInvoked = false;

			root.Traverse(x => x.Children)
				.Prepare(() => prepareInvoked = true)
				.OnSuccess(() => successInvoked = true)
				.Finish(() => finishInvoked = true)
				.Execute();

			Assert.True(prepareInvoked);
			Assert.True(successInvoked);
			Assert.True(finishInvoked);

			root.Traverse(x => x.Children)
				.WithAction(node =>
				{
					if (node.Equals(secondChild))
						throw new InvalidOperationException();
				})
				.Catch<InvalidOperationException>(e => 
				{
					failureInvoked = true;
					return true;
				})
				.Execute();

			Assert.True(failureInvoked);

			Assert.Throws<InvalidOperationException>(() =>
			{
				root.Traverse(x => x.Children)
					.WithAction(node =>
					{
						if (node.Equals(secondChild))
							throw new InvalidOperationException();
					})
					.Catch<InvalidOperationException>(e =>
					{
						return false;
					})
					.Execute();
			});
		}

		[Fact]
		public void TraverserUsesCancellationTokenAndThrowsException()
		{
			var root = this.fixture.Root;

			var canceledInvoked = false;

			using var tokenSource = new CancellationTokenSource();
			CancellationToken ct = tokenSource.Token;

			var task = root.Traverse(x => x.Children)
				.Use(ct, true)
				.OnCanceled(() => canceledInvoked = true)
				.WithAction(() =>
				{
					// make sure the operation has started before the cancellation
					// in order to assure that the OnCanceled callback will be invoked
					tokenSource.Cancel();
				})
				.ExecuteAsync();

			Assert.Throws<AggregateException>(() => task.Wait());

			Assert.True(canceledInvoked);
			Assert.Equal(TaskStatus.Canceled, task.Status);

			return;
		}

		[Fact]
		public void TraverserUsesCancellationTokenWithoutThrowingException()
		{
			var root = this.fixture.Root;

			var canceledInvoked = false;

			using var tokenSource = new CancellationTokenSource();
			CancellationToken ct = tokenSource.Token;

			var task = root.Traverse(x => x.Children)
				.Use(ct, false)
				.OnCanceled(() => canceledInvoked = true)
				.WithAction(() =>
				{
					// make sure the operation has started before the cancellation
					// in order to assure that the OnCanceled callback will be invoked
					tokenSource.Cancel();
				})
				.ExecuteAsync();

			task.Wait();

			Assert.True(canceledInvoked);
			Assert.Equal(TaskStatus.RanToCompletion, task.Status);
		}

		[Fact]
		public void TraverserCancelsOnPredicate()
		{
			var root = this.fixture.Root;
			var secondChild = root.Children.ElementAt(1);

			var canceledInvoked = false;

			root.Traverse(x => x.Children)
				.CancelIf(x => x.Equals(secondChild))
				.OnCanceled(() => canceledInvoked = true)
				.Execute();

			Assert.True(canceledInvoked);
		}

		[Fact]
		public void TraverserCancelsAfterTimeSpan()
		{
			var root = this.fixture.Root;

			var canceledInvoked = false;

			using var tokenSource = new CancellationTokenSource();
			CancellationToken ct = tokenSource.Token;

			var task = root.Traverse(x => x.Children)
				.Use(ct, false)
				.OnCanceled(() => canceledInvoked = true)
				.WithAction(() => Thread.Sleep(110))
				.ExecuteAsync();

			tokenSource.CancelAfter(100);
			task.Wait();

			Assert.True(canceledInvoked);
		}
	}
}

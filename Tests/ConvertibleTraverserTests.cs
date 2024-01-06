using Bertiooo.Traversal;
using System.Diagnostics;
using Tests.Fixtures;
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

			var actualLevels = root.Traverse()
				.Use(TraversalMode.DepthFirst)
				.GetNodes()
				.Select(x => x.GetLevel());

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

			var actualLevels = root.Traverse()
				.Use(TraversalMode.BreadthFirst)
				.GetNodes()
				.Select(x => x.GetLevel());

			Assert.Equal(expectedLevels, actualLevels);
		}

		[Fact]
		public void TraverserSkipsNode()
		{
			var root = this.fixture.Root;
			var child = root.Children.First();

			var secondChild = root.Children.ElementAt(1) as DerivativeConvertible;
			Assert.NotNull(secondChild);

			var nodes = root.Traverse().Skip(child).GetNodes();

			Assert.True(nodes.Any());
			Assert.True(nodes.Contains(child) == false);
			Assert.True(child.Children.All(x => nodes.Contains(x)) == false);

			nodes = root.Traverse().Skip(x => x.Equals(child)).GetNodes();

			Assert.True(nodes.Any());
			Assert.True(nodes.Contains(child) == false);
			Assert.True(child.Children.All(x => nodes.Contains(x)) == false);

			nodes = root.Traverse().Skip<DerivativeConvertible>().GetNodes();

			var numberOfNodes = root.WithDescendants().Count();
			var numberOfDerivatives = root.WithDescendants().OfType<DerivativeConvertible>().Count();

			Assert.True(nodes.Any());
			Assert.Equal(numberOfNodes - numberOfDerivatives, nodes.Count());
		}

		[Fact]
		public void TraverserExcludesNode()
		{
			var root = this.fixture.Root;
			var child = root.Children.First();

			var secondChild = root.Children.ElementAt(1) as DerivativeConvertible;
			Assert.NotNull(secondChild);

			var nodes = root.Traverse().Exclude(child).GetNodes();

			Assert.True(nodes.Any());
			Assert.True(nodes.Contains(child) == false);

			// child nodes still are traversed
			Assert.True(child.Children.All(x => nodes.Contains(x)));

			nodes = root.Traverse().Exclude(x => x.Equals(child)).GetNodes();

			Assert.True(nodes.Any());
			Assert.True(nodes.Contains(child) == false);

			// child nodes still are traversed
			Assert.True(child.Children.All(x => nodes.Contains(x)));

			nodes = root.Traverse().Exclude<DerivativeConvertible>(x => x.Equals(secondChild)).GetNodes();

			Assert.True(nodes.Any());
			Assert.Equal(root.WithDescendants().Count() - 1, nodes.Count());
		}

		[Fact]
		public void TraverserDisablesCallbacksForNode()
		{
			var root = this.fixture.Root;
			var child = root.Children.First();
			var secondChild = root.Children.ElementAt(1);

			var numberOfNodes = root.WithDescendants().Count();

			var numberOfCallbacks = 0;

			root.Traverse()
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

			var numberOfNodes = root.WithDescendants().Count();

			var numberOfCallbacks = 0;

			var expectedNodes = new List<Convertible>() { root };
			expectedNodes.AddRange(child.Children);
			expectedNodes.AddRange(secondChild.Children);

			var nodes = root.Traverse()
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

			var traversal = root.Traverse()
				.GetNodesAsync();

			Debug.WriteLine("awaiting");

			var nodes = await traversal;
			Assert.Equal(root.WithDescendants(), nodes);
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

			root.Traverse()
				.Prepare(() => prepareInvoked = true)
				.OnSuccess(() => successInvoked = true)
				.Finish(() => finishInvoked = true)
				.Execute();

			Assert.True(prepareInvoked);
			Assert.True(successInvoked);
			Assert.True(finishInvoked);

			root.Traverse()
				.WithAction(node =>
				{
					if (node.Equals(secondChild))
						throw new InvalidOperationException();
				})
				.OnFailure<InvalidOperationException>(e => 
				{
					failureInvoked = true;
					return true;
				})
				.Execute();

			Assert.True(failureInvoked);

			Assert.Throws<InvalidOperationException>(() =>
			{
				root.Traverse()
					.WithAction(node =>
					{
						if (node.Equals(secondChild))
							throw new InvalidOperationException();
					})
					.OnFailure<InvalidOperationException>(e =>
					{
						return false;
					})
					.Execute();
			});
		}

		[Fact]
		public void TraverserCallsDerivativeAction()
		{
			var root = this.fixture.Root;

			var expectedCalls = root.WithDescendants().OfType<DerivativeConvertible>().Count();
			Assert.True(expectedCalls > 0);

			var expectedTotalCalls = root.WithDescendants().Count();
			Assert.True(expectedTotalCalls > 0);

			var actualCalls = 0;
			var actualTotalCalls = 0;			

			root.Traverse()
				.WithAction(x => actualTotalCalls++)
				.WithAction<DerivativeConvertible>(x => actualCalls++)
				.Execute();

			Assert.Equal(expectedCalls, actualCalls);
			Assert.Equal(expectedTotalCalls, actualTotalCalls);
		}

		[Fact]
		public void TraverserUsesCancellationTokenAndThrowsException()
		{
			var root = this.fixture.Root;

			var canceledInvoked = false;

			using var tokenSource = new CancellationTokenSource();
			CancellationToken ct = tokenSource.Token;

			var task = root.Traverse()
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

			var task = root.Traverse()
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

			root.Traverse()
				.CancelIf(x => x.Equals(secondChild))
				.OnCanceled(() => canceledInvoked = true)
				.Execute();

			Assert.True(canceledInvoked);
		}

		[Fact]
		public void TraverserCancelsOnDerivativePredicate()
		{
			var root = this.fixture.Root;

			var secondChild = root.Children.ElementAt(1) as DerivativeConvertible;
			Assert.NotNull(secondChild);

			var numberOfVisitedNodes = 0;
			var canceledInvoked = false;

			root.Traverse()
				.Use(TraversalMode.BreadthFirst)
				.WithAction(() => numberOfVisitedNodes++)
				.CancelIf<DerivativeConvertible>(x => x.Equals(secondChild))
				.OnCanceled(() => canceledInvoked = true)
				.Execute();

			Assert.True(canceledInvoked);
			Assert.Equal(2, numberOfVisitedNodes); // root and first child
		}

		[Fact]
		public void TraverserCancelsAfterTimeSpan()
		{
			var root = this.fixture.Root;

			var canceledInvoked = false;

			using var tokenSource = new CancellationTokenSource();
			CancellationToken ct = tokenSource.Token;

			var task = root.Traverse()
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

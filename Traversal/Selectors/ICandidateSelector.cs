using System;

namespace Bertiooo.Traversal.Selectors
{
	public interface ICandidateSelector<T> : ICloneable
    {
        bool HasItems { get; }

        void Add(T item);

        void Reset();

        T Next();
    }
}

﻿using System;

namespace Bertiooo.Traversal.Selectors
{
	public interface ICandidateSelector<T>
    {
        bool HasItems { get; }

        void Add(T item);

        void Reset();

        T Next();
    }
}

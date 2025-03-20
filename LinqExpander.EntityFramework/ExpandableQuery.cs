﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqExpander
{
    internal class ExpandableQuery<T> : IQueryable<T>, IOrderedQueryable<T>//, IAsyncEnumerable<T>
    {
		private readonly ExtendableQueryProvider _provider;
        private readonly Expression _expression;

		public ExpandableQuery(ExtendableQueryProvider provider, Expression expression)
		{
			_provider = provider;
			_expression = expression;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _provider.ExecuteQuery<T>(_expression).GetEnumerator();
		}

        // IAsyncEnumerable<T> removed by Richard Barraclough. 
        //IAsyncEnumerator<T> IAsyncEnumerable<T>.GetEnumerator()
        //{
        //    return _provider.ExecuteAsync<T>(_expression).GetEnumerator();
        //}

        IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

        public Type ElementType => typeof(T);

        public Expression Expression => _expression;

        public IQueryProvider Provider => _provider;
    }
}

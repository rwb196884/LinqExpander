﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace LinqExpander
{
	internal class ExtendableQueryProvider : IAsyncQueryProvider
	{
	    private readonly IQueryProvider _underlyingQueryProvider;

		private ExtendableQueryProvider()
		{
		}

		internal ExtendableQueryProvider(IQueryProvider underlyingQueryProvider)
		{
			_underlyingQueryProvider = underlyingQueryProvider;
		}

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			return new ExpandableQuery<TElement>(this, expression);
		}

		public IQueryable CreateQuery(Expression expression)
		{
			Type elementType = expression.Type.GetElementType();
			try
			{
				return (IQueryable)Activator.CreateInstance(typeof(ExpandableQuery<>).MakeGenericType(elementType), new object[] { this, expression });
			}
			catch (System.Reflection.TargetInvocationException tie)
			{
				throw tie.InnerException;
			}
		}

		internal IEnumerable<T> ExecuteQuery<T>(Expression expression)
		{
			return _underlyingQueryProvider.CreateQuery<T>(Visit(expression)).AsEnumerable();
		}

		public TResult Execute<TResult>(Expression expression)
		{
			return _underlyingQueryProvider.Execute<TResult>(Visit(expression));
		}

		public object Execute(Expression expression)
		{
			return _underlyingQueryProvider.Execute(Visit(expression));
		}

        // Removed by Richard Barraclough. 
        //public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        //{
        //    return ((IAsyncQueryProvider)_underlyingQueryProvider).ExecuteAsync<TResult>(Visit(expression));
        //}

        // Removed by Richard Barraclough. 
        //public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        //{
        //    return ((IAsyncQueryProvider)_underlyingQueryProvider).ExecuteAsync<TResult>(Visit(expression), cancellationToken);
        //}

        private Expression Visit(Expression exp)
	    {
	        ExpandableVisitor vstr = new ExpandableVisitor(_underlyingQueryProvider);
	        Expression visitedExp = vstr.Visit(exp);

	        return visitedExp;
	    }

        // Aded by Richard Barraclough.
        TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            return ((IAsyncQueryProvider)_underlyingQueryProvider).ExecuteAsync<TResult>(Visit(expression), cancellationToken);
        }
    }
}

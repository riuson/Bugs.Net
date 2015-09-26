using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BugTracker.DB.Extensions
{
    public static class IQuerableExt
    {
        //public static INhFetchRequest<TOriginating, TRelated> Fetch<TOriginating, TRelated>(this IQueryable<TOriginating> query, Expression<Func<TOriginating, TRelated>> relatedObjectSelector);

        public static IQueryable<T> FetchField<T, TRelated>(this IQueryable<T> query, Expression<Func<T, TRelated>> relatedObjectSelector)
        {
            INhFetchRequest<T, TRelated> result = query.Fetch<T, TRelated>(relatedObjectSelector);
            return query.Fetch(relatedObjectSelector);
        }
    }
}

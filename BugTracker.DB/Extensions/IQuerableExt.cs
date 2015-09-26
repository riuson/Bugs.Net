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

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool ascending)
        {
            if (String.IsNullOrEmpty(propertyName))
            {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, String.Empty);

            MemberExpression property = null;

            // Access to object.Field.Field2.Field3 ...
            string[] propertyNames = propertyName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var pn in propertyNames)
            {
                if (property == null)
                {
                    property = Expression.Property(parameter, pn);
                }
                else
                {
                    property = Expression.Property(property, pn);
                }
            }

            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = ascending ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                                new Type[] { source.ElementType, property.Type },
                                                source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }
    }
}

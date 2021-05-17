using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bank.Issuer.Data.Extensions
{
    public static class EnumerableExtensions
    {
        public static IDictionary<TKey, TValue> NullIfEmpty<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null || !dictionary.Any())
            {
                return null;
            }

            return dictionary;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
            {
                action(element);
            }

            return source;
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> source, string sortExpression)
        {
            try
            {
                string[] parts = sortExpression.Split(new char[] { ' ' });
                ParameterExpression param = Expression.Parameter(typeof(T), string.Empty);
                Expression<Func<T, object>> sortLambda = Expression.Lambda<Func<T, object>>(Expression.Convert(Expression.Property(param, parts[0]), typeof(object)), new ParameterExpression[] { param });
                if ((parts.Length > 1) && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    return source.AsQueryable<T>().OrderByDescending<T, object>(sortLambda);
                }
                return source.AsQueryable<T>().OrderBy<T, object>(sortLambda);
            }
            catch (ArgumentException)
            {
                return source;
            }
        }
    }
}

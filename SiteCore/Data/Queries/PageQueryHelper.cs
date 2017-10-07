namespace BITOJ.Core.Data.Queries
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// 为分页查询提供帮助类。
    /// </summary>
    internal static class PageQueryHelper
    {
        private static int GetPages(int itemsCount, int itemsPerPage)
        {
            return itemsCount / itemsPerPage + Math.Sign(itemsCount % itemsPerPage);
        }

        /// <summary>
        /// 计算给定数据集中的数据条目在给定的分页参数下可以填充多少页面。
        /// </summary>
        /// <typeparam name="T">数据条目类型。</typeparam>
        /// <param name="query">原始数据集。</param>
        /// <param name="itemsPerPage">每一页上的数据条目数量。</param>
        /// <returns>在给定的分页参数下原始数据集中的数据条目可以填充的页面数量。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static int GetPages<T>(this IQueryable<T> query, int itemsPerPage)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (itemsPerPage <= 0)
                throw new ArgumentOutOfRangeException(nameof(itemsPerPage));

            int count = query.Count();
            return GetPages(count, itemsPerPage);
        }

        /// <summary>
        /// 在给定的数据集上执行分页。
        /// </summary>
        /// <typeparam name="T">数据对象类型。</typeparam>
        /// <param name="query">原始可查询对象。</param>
        /// <param name="parameter">分页参数。</param>
        /// <returns>分页后的可查询对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public static IQueryable<T> Page<T>(this IQueryable<T> query, PageQueryParameter parameter)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (parameter.Page <= 0)
                throw new ArgumentOutOfRangeException(nameof(parameter.Page));
            if (parameter.ItemsPerPage <= 0)
                throw new ArgumentOutOfRangeException(nameof(parameter.ItemsPerPage));

            int totalPages = query.GetPages(parameter.ItemsPerPage);
            if (parameter.Page > totalPages)
                throw new ArgumentOutOfRangeException(nameof(parameter.Page));
            
            return query.Skip(parameter.ItemsPerPage * (parameter.Page - 1)).Take(parameter.ItemsPerPage);
        }
    }
}

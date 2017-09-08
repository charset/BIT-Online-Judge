namespace BITOJ.Core.Data.Queries
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 对数据查询结果进行封装。
    /// </summary>
    /// <typeparam name="T">查询结果对象数据类型。</typeparam>
    public class PageableQueryResult<T> : IPageableQueryResult<T> where T: class
    {
        private IQueryable<T> m_origin;

        /// <summary>
        /// 使用给定的可查询对象创建 PageableQueryResult 类的新实例。
        /// </summary>
        /// <param name="origin">包含查询结果的可查询对象。</param>
        public PageableQueryResult(IQueryable<T> origin)
        {
            m_origin = origin;
        }

        /// <summary>
        /// 创建一个空的可分页查询结果对象。
        /// </summary>
        public PageableQueryResult() : this(null)
        { }

        /// <summary>
        /// 获取查询结果中的元素数量。
        /// </summary>
        public int Count
        {
            get => m_origin?.Count() ?? 0;
        }

        /// <summary>
        /// 对当前查询结果进行分页并返回新的查询结果。
        /// </summary>
        /// <param name="page">当前页码。页码从 1 开始编号。</param>
        /// <param name="itemsPerPage">每一页上的元素数量。</param>
        /// <returns>分页后的查询结果对象。</returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public virtual IQueryResult<T> Page(int page, int itemsPerPage)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            if (itemsPerPage <= 0)
                throw new ArgumentOutOfRangeException(nameof(itemsPerPage));

            if (m_origin == null)
            {
                // 当前对象为空查询结果对象。
                return new PageableQueryResult<T>();
            }
            else
            {
                IQueryable<T> pagedData = m_origin.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);
                return new PageableQueryResult<T>(pagedData);
            }
        }

        /// <summary>
        /// 计算当前查询对象中的元素数量可以填充多少视图页数据。
        /// </summary>
        /// <param name="itemsPerPage">每一视图页上的数据元素个数。</param>
        /// <returns>可以填充的视图页数目。</returns>
        public int GetTotalPages(int itemsPerPage)
        {
            return Count / itemsPerPage + Math.Sign(Count % itemsPerPage);
        }

        /// <summary>
        /// 创建可以迭代到所有查询结果元素的迭代器。
        /// </summary>
        /// <returns>可迭代到所有查询结果元素的迭代器。</returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            if (m_origin == null)
            {
                // 当前对象为空查询结果对象。
                yield break;
            }

            foreach (T item in m_origin)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

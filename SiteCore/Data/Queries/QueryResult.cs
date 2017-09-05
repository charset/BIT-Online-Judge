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
    public class QueryResult<T> : IEnumerable<T> where T: class
    {
        /// <summary>
        /// 获取空查询结果对象。
        /// </summary>
        public static QueryResult<T> Empty
        {
            get
            {
                return new QueryResult<T>();
            }
        }

        private IQueryable<object> m_origin;
        private Func<object, T> m_mapping;

        /// <summary>
        /// 使用给定的可查询对象以及查询映射创建 DataQueryResult 类的新实例。
        /// </summary>
        /// <param name="result">包含原始查询结果的可查询对象。</param>
        /// <param name="mapping">在迭代阶段将原始结果转换为外部接口对象的映射方法。</param>
        /// <exception cref="ArgumentNullException"/>
        public QueryResult(IQueryable<object> result, Func<object, T> mapping)
        {
            m_origin = result ?? throw new ArgumentNullException(nameof(result));
            m_mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
        }

        /// <summary>
        /// 使用给定的可查询对象创建 DataQueryResult 类的新实例。
        /// </summary>
        /// <param name="result">包含查询结果的可查询对象。</param>
        public QueryResult(IQueryable<T> result)
            : this(result, item => (T)item)
        { }

        /// <summary>
        /// 创建一个空的查询结果对象。
        /// </summary>
        private QueryResult()
        {
            m_origin = null;
            m_mapping = null;
        }

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
        /// <param name="page">分页参数。</param>
        /// <returns>分页结果。</returns>
        /// <exception cref="ArgumentNullException"/>
        public QueryResult<T> Page(PagedQueryParameters page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            return Page(page.Page, page.EntriesPerPage);
        }

        /// <summary>
        /// 对当前查询结果进行分页并返回新的查询结果。
        /// </summary>
        /// <param name="page">当前页码。页码从 1 开始编号。</param>
        /// <param name="itemsPerPage">每一页上的元素数量。</param>
        /// <returns>分页后的查询结果对象。</returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public QueryResult<T> Page(int page, int itemsPerPage)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            if (itemsPerPage <= 0)
                throw new ArgumentOutOfRangeException(nameof(itemsPerPage));

            if (m_origin == null)
            {
                // 当前对象为空查询结果对象。
                return new QueryResult<T>();
            }
            else
            {
                IQueryable<object> pagedData = m_origin.Skip((page - 1) * itemsPerPage).Take(itemsPerPage);
                return new QueryResult<T>(pagedData, m_mapping);
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
        public IEnumerator<T> GetEnumerator()
        {
            if (m_origin == null)
            {
                // 当前对象为空查询结果对象。
                yield break;
            }

            foreach (object item in m_origin)
            {
                yield return m_mapping(item);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

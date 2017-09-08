namespace BITOJ.Core.Data.Queries
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// 封装映射后的查询结果对象。
    /// </summary>
    public class MappedQueryResult<TFrom, TTo> : IPageableQueryResult<TTo>
        where TFrom: class
        where TTo: class
    {
        private IQueryResult<TFrom> m_origin;
        private Func<TFrom, TTo> m_mapping;

        /// <summary>
        /// 从给定的源查询结果对象与映射函数创建 MappedQueryResult 类的新实例。
        /// </summary>
        /// <param name="origin">源查询结果对象。</param>
        /// <param name="mapping">类型映射函数。</param>
        public MappedQueryResult(IQueryResult<TFrom> origin, Func<TFrom, TTo> mapping)
        {
            m_origin = origin;
            m_mapping = mapping;
        }

        /// <summary>
        /// 创建一个空的 MappedQueryResult 对象。
        /// </summary>
        public MappedQueryResult()
            : this(null, null)
        { }

        /// <summary>
        /// 执行分页操作。
        /// </summary>
        /// <param name="page">要跳转到的页码。页码应从 1 开始编码。</param>
        /// <param name="itemsPerPage">每一页上的结果元素数量。</param>
        /// <returns>一个新的查询结果接口对象，其中包含给定页上所有的查询元素。</returns>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="InvalidOperationException"/>
        public IQueryResult<TTo> Page(int page, int itemsPerPage)
        {
            if (page <= 0)
                throw new ArgumentOutOfRangeException(nameof(page));
            if (itemsPerPage <= 0)
                throw new ArgumentOutOfRangeException(nameof(itemsPerPage));

            IPageableQueryResult<TFrom> pageableOrigin = m_origin as IPageableQueryResult<TFrom>;
            if (pageableOrigin == null)
                throw new InvalidOperationException("源查询对象不可分页。");

            return new MappedQueryResult<TFrom, TTo>(pageableOrigin.Page(page, itemsPerPage), m_mapping);
        }

        /// <summary>
        /// 根据一页上的元素数量计算当前查询结果对象中的元素可以填充的页面数量。
        /// </summary>
        /// <param name="itemsPerPage">一个页面上的元素数目。</param>
        /// <returns>当前查询结果对象中的元素可以填充的页面数量。</returns>
        public int GetTotalPages(int itemsPerPage)
        {
            if (m_origin == null)
            {
                return 0;
            }

            IPageableQueryResult<TFrom> pageableOrigin = m_origin as IPageableQueryResult<TFrom>;
            if (pageableOrigin == null)
                throw new InvalidOperationException("源查询结果对象不可分页。");

            return pageableOrigin.GetTotalPages(itemsPerPage);
        }

        /// <summary>
        /// 获取当前对象上的迭代器。
        /// </summary>
        /// <returns>当前对象上的迭代器。</returns>
        public IEnumerator<TTo> GetEnumerator()
        {
            if (m_origin == null)
            {
                yield break;
            }

            foreach (TFrom item in m_origin)
            {
                yield return m_mapping(item);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}

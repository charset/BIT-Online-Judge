namespace BITOJ.Core.Data.Queries
{
    /// <summary>
    /// 定义可分页的数据查询结果。
    /// </summary>
    public interface IPageableQueryResult<out T> : IQueryResult<T> where T: class
    {
        /// <summary>
        /// 对查询结果进行分页。
        /// </summary>
        /// <param name="page">要跳转到的页码。页码应从 1 开始编码。</param>
        /// <param name="itemsPerPage">每一个页面上的元素数量。</param>
        /// <returns>一个新的可查询结果，其中包含了给定页面上的所有结果元素。</returns>
        IQueryResult<T> Page(int page, int itemsPerPage);

        /// <summary>
        /// 根据一页上的元素数量计算当前查询结果中的元素可以填充的页数量。
        /// </summary>
        /// <param name="itemsPerPage">每一页上的元素数量。</param>
        /// <returns>当前查询结果中的元素可以填充的页数量。</returns>
        int GetTotalPages(int itemsPerPage);
    }
}

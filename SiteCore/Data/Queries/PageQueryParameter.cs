namespace BITOJ.Core.Data.Queries
{
    /// <summary>
    /// 为分页查询提供查询参数。
    /// </summary>
    public struct PageQueryParameter
    {
        /// <summary>
        /// 获取或设置查询页。
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 获取或设置每一页上的数据条目数量。
        /// </summary>
        public int ItemsPerPage { get; set; }

        /// <summary>
        /// 创建 PageQueryParameter 结构的新实例。
        /// </summary>
        /// <param name="page">页码。</param>
        /// <param name="itemsPerPage">每一页上的数据条目数量。</param>
        public PageQueryParameter(int page, int itemsPerPage)
        {
            Page = page;
            ItemsPerPage = itemsPerPage;
        }
    }
}

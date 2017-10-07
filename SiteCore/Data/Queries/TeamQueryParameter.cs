namespace BITOJ.Core.Data.Queries
{
    /// <summary>
    /// 封装队伍查询参数。
    /// </summary>
    public sealed class TeamQueryParameter
    {
        /// <summary>
        /// 获取或设置队伍名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否根据名称进行查询。
        /// </summary>
        public bool QueryByName { get; set; }

        /// <summary>
        /// 获取或设置队伍领队。
        /// </summary>
        public string Leader { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否根据领队用户名进行查询。
        /// </summary>
        public bool QueryByLeader { get; set; }

        /// <summary>
        /// 获取或设置分页查询参数。
        /// </summary>
        public PageQueryParameter PageQuery { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否应启用分页查询。
        /// </summary>
        public bool EnablePageQuery { get; set; }

        /// <summary>
        /// 创建 TeamQueryParameter 类的新实例。
        /// </summary>
        public TeamQueryParameter()
        {
            Name = string.Empty;
            QueryByName = false;
            Leader = string.Empty;
            QueryByLeader = false;
            PageQuery = new PageQueryParameter();
            EnablePageQuery = false;
        }
    }
}

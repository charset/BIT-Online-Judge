namespace BITOJ.Core.Data.Queries
{
    using BITOJ.Core;

    /// <summary>
    /// 为比赛实体数据查询提供查询参数。
    /// </summary>
    public sealed class ContestQueryParameter
    {
        /// <summary>
        /// 当 QueryByTitle 为 true 时，获取或设置要查询的标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否根据标题进行查询。
        /// </summary>
        public bool QueryByTitle { get; set; }

        /// <summary>
        /// 当 QueryByCreator 为 true 时，获取或设置要查询的作者。
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否按照作者进行查询。
        /// </summary>
        public bool QueryByCreator { get; set; }

        /// <summary>
        /// 当 QueryByStatus 为 true 时，获取或设置要查询的比赛状态。
        /// </summary>
        public ContestStatus Status { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否使用比赛状态对比赛进行查询。
        /// </summary>
        public bool QueryByStatus { get; set; }

        /// <summary>
        /// 创建 ContestQueryParameter 类的新实例。
        /// </summary>
        public ContestQueryParameter()
        {
            Title = string.Empty;
            QueryByTitle = false;
            Creator = string.Empty;
            QueryByCreator = false;
            Status = ContestStatus.Pending;
            QueryByStatus = false;
        }
    }
}

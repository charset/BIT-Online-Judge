﻿namespace BITOJ.Core.Data.Queries
{
    /// <summary>
    /// 为 BITOJ 主题目库的数据查询提供参数。
    /// </summary>
    public sealed class ProblemArchieveQueryParameter
    {
        /// <summary>
        /// 获取或设置要查询的题目标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否通过题目标题进行查询。
        /// </summary>
        public bool QueryByTitle { get; set; }

        /// <summary>
        /// 获取或设置要查询的题目源。
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否通过题目源进行查询。
        /// </summary>
        public bool QueryBySource { get; set; }

        /// <summary>
        /// 获取或设置要查询的题目作者。
        /// </summary>
        public string Author { get; set; }
        
        /// <summary>
        /// 获取或设置一个值，该值指示是否通过题目作者进行查询。
        /// </summary>
        public bool QueryByAuthor { get; set; }

        /// <summary>
        /// 获取或设置要查询的题目的源 OJ 平台。
        /// </summary>
        public OJSystem Origin { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否通过题目源 OJ 进行查询。
        /// </summary>
        public bool QueryByOrigin { get; set; }

        /// <summary>
        /// 获取或设置要查询的比赛 ID。
        /// </summary>
        public int ContestId { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否通过题目所在的比赛 ID 进行查询。
        /// </summary>
        public bool QueryByContestId { get; set; }

        /// <summary>
        /// 获取或设置分页查询参数。
        /// </summary>
        public PageQueryParameter PageQuery { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否启用分页查询。
        /// </summary>
        public bool EnablePageQuery { get; set; }

        /// <summary>
        /// 创建 ProblemArchieveQueryParameter 类的新实例。
        /// </summary>
        public ProblemArchieveQueryParameter()
        {
            Title = string.Empty;
            Source = string.Empty;
            Author = string.Empty;
            Origin = OJSystem.BIT;
            ContestId = -1;
            PageQuery = new PageQueryParameter();
            QueryByTitle = false;
            QueryBySource = false;
            QueryByAuthor = false;
            QueryByOrigin = false;
            QueryByContestId = true;
            EnablePageQuery = false;
        }
    }
}

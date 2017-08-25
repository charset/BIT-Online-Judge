namespace BITOJ.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 表示主题目库题目实体。
    /// </summary>
    public sealed class ProblemEntity
    {
        /// <summary>
        /// 获取或设置题目实体的 ID。
        /// </summary>
        [Key][MaxLength(32)]
        public string Id { get; set; }

        /// <summary>
        /// 获取或设置题目的标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置题目作者的用户名。
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 获取或设置题目的来源。
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 获取或设置题目的来源 OJ 系统。
        /// </summary>
        public OJSystem Origin { get; set; }

        /// <summary>
        /// 获取或设置访问当前题目所需的最低用户权限。
        /// </summary>
        public UserGroup AuthorizationGroup { get; set; }

        /// <summary>
        /// 获取或设置题目的总提交数目。
        /// </summary>
        public int TotalSubmissions { get; set; }

        /// <summary>
        /// 获取或设置题目的 AC 提交数目。
        /// </summary>
        public int AcceptedSubmissions { get; set; }

        /// <summary>
        /// 获取或设置题目的配置目录。
        /// </summary>
        public string ProblemDirectory { get; set; }

        /// <summary>
        /// 获取或设置题目的类别。
        /// </summary>
        public ICollection<ProblemCategoryEntity> Categories { get; set; }

        /// <summary>
        /// 初始化 ProblemEntity 类的新实例。
        /// </summary>
        public ProblemEntity()
        {
            Id = "BIT0000";
            Title = string.Empty;
            Author = string.Empty;
            Source = string.Empty;
            AuthorizationGroup = UserGroup.Guests;
            TotalSubmissions = 0;
            AcceptedSubmissions = 0;
            ProblemDirectory = string.Empty;
            Categories = new List<ProblemCategoryEntity>();
        }
    }
}

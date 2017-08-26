namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;
    using BITOJ.Core.Convert;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 为添加题目页面提供数据模型。
    /// </summary>
    public class AddProblemModel
    {
        /// <summary>
        /// 获取或设置题目 ID。
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Problem ID is required.")]
        [MaxLength(32, ErrorMessage = "Problem ID should be no longer than 32 characters.")]
        public string Id { get; set; }

        /// <summary>
        /// 获取或设置题目标题。
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Problem title is required.")]
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置题目描述。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 获取或设置输入描述。
        /// </summary>
        public string InputDescription { get; set; }

        /// <summary>
        /// 获取或设置输出描述。
        /// </summary>
        public string OutputDescription { get; set; }

        /// <summary>
        /// 获取或设置输入样例。
        /// </summary>
        public string InputExample { get; set; }

        /// <summary>
        /// 获取或设置输出样例。
        /// </summary>
        public string OutputExample { get; set; }

        /// <summary>
        /// 获取或设置题目提示。
        /// </summary>
        public string Hint { get; set; }

        /// <summary>
        /// 获取或设置题目来源。
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 获取或设置题目作者。
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 获取或设置访问题目所需的最低权限。
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "UserGroup is required.")]
        public string UserGroupName { get; set; }

        /// <summary>
        /// 获取或设置关联到 ID 域的错误消息。
        /// </summary>
        public string IdErrorMessage { get; set; }

        /// <summary>
        /// 获取或设置关联到 Title 域的错误消息。
        /// </summary>
        public string TitleErrorMessage { get; set; }

        /// <summary>
        /// 创建 AddProblemModel 类的新实例。
        /// </summary>
        public AddProblemModel()
        {
            Id = string.Empty;
            Title = string.Empty;
            Description = string.Empty;
            InputDescription = string.Empty;
            OutputDescription = string.Empty;
            InputExample = string.Empty;
            OutputExample = string.Empty;
            Hint = string.Empty;
            Source = string.Empty;
            Author = string.Empty;
            UserGroupName = UsergroupConvert.ConvertToString(UserGroup.Guests);
            IdErrorMessage = string.Empty;
            TitleErrorMessage = string.Empty;
        }

        /// <summary>
        /// 重置当前数据模型上的错误消息。
        /// </summary>
        public void ResetErrorMessages()
        {
            IdErrorMessage = string.Empty;
            TitleErrorMessage = string.Empty;
        }

        /// <summary>
        /// 将当前数据模型中所有的 null 字符串替换为空字符串。
        /// </summary>
        public void ReplaceNullStringsToEmptyStrings()
        {
            if (Description == null)
            {
                Description = string.Empty;
            }
            if (InputDescription == null)
            {
                InputDescription = string.Empty;
            }
            if (OutputDescription == null)
            {
                OutputDescription = string.Empty;
            }
            if (InputExample == null)
            {
                InputExample = string.Empty;
            }
            if (OutputExample == null)
            {
                OutputExample = string.Empty;
            }
            if (Source == null)
            {
                Source = string.Empty;
            }
            if (Author == null)
            {
                Author = string.Empty;
            }
            if (Hint == null)
            {
                Hint = string.Empty;
            }
        }
    }
}
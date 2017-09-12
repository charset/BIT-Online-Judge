namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;
    using BITOJ.Core.Convert;
    using BITOJ.Core.Data;
    using BITOJ.SiteUI.Models.Validation;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 为题目的详细信息提供数据模型。
    /// </summary>
    public class ProblemDetailModel
    {
        /// <summary>
        /// 获取或设置题目 ID。
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Problem ID is required.")]
        [MaxLength(32, ErrorMessage = "Problem ID should be no longer than 32 characters.")]
        public string ProblemId { get; set; }

        /// <summary>
        /// 获取或设置题目标题。
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Problem title is required.")]
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置题目叙述。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 获取或设置题目输入描述。
        /// </summary>
        public string InputDescription { get; set; }

        /// <summary>
        /// 获取或设置题目输出描述。
        /// </summary>
        public string OutputDescription { get; set; }

        /// <summary>
        /// 获取或设置题目输入样例。
        /// </summary>
        public string InputExample { get; set; }

        /// <summary>
        /// 获取或设置题目输出样例。
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
        /// 获取或设置用户权限集名称。
        /// </summary>
        [EqualTo("Administrator", "Insider", "Standard", "Guest", ErrorMessage = "Invalid user group.")]
        public string UserGroupName { get; set; }

        /// <summary>
        /// 创建 ProblemDetailModel 类的新实例。
        /// </summary>
        public ProblemDetailModel()
        {
            ProblemId = string.Empty;
            Title = string.Empty;
            Description = string.Empty;
            InputDescription = string.Empty;
            OutputDescription = string.Empty;
            InputExample = string.Empty;
            OutputExample = string.Empty;
            Hint = string.Empty;
            Source = string.Empty;
            Author = string.Empty;
        }

        /// <summary>
        /// 将当前数据模型中的所有 null 字符串替换为空字符串。
        /// </summary>
        public void ResetNullStrings()
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
            if (Hint == null)
            {
                Hint = string.Empty;
            }
            if (Source == null)
            {
                Source = string.Empty;
            }
            if (Author == null)
            {
                Author = string.Empty;
            }
        }

        /// <summary>
        /// 从给定的题目句柄创建 ProblemDetailModel 类的新实例。
        /// </summary>
        /// <param name="handle">题目句柄。</param>
        /// <exception cref="ArgumentNullException"/>
        public static ProblemDetailModel FromProblemHandle(ProblemHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            ProblemDetailModel model = new ProblemDetailModel();
            using (ProblemDataProvider data = ProblemDataProvider.Create(handle, true))
            {
                model.ProblemId = data.ProblemId;
                model.Title = data.Title;
                model.Description = data.Description;
                model.InputDescription = data.InputDescription;
                model.OutputDescription = data.OutputDescription;
                model.InputExample = data.InputExample;
                model.OutputExample = data.OutputExample;
                model.Hint = data.Hint;
                model.Source = data.Source;
                model.Author = data.Author;
                model.UserGroupName = UsergroupConvert.ConvertToString(data.AuthorizationGroup);
            }

            return model;
        }

        /// <summary>
        /// 将当前对象中的数据写入给定的题目中。
        /// </summary>
        /// <param name="handle">题目句柄。</param>
        /// <exception cref="ArgumentNullException"/>
        public void SaveTo(ProblemHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            using (ProblemDataProvider data = ProblemDataProvider.Create(handle, false))
            {
                data.Title = Title;
                data.Description = Description;
                data.InputDescription = InputDescription;
                data.OutputDescription = OutputDescription;
                data.InputExample = InputExample;
                data.OutputExample = OutputExample;
                data.Hint = Hint;
                data.Source = Source;
                data.Author = Author;
                data.AuthorizationGroup = UsergroupConvert.ConvertFromString(UserGroupName);
            }
        }
    }
}
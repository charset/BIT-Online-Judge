namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;
    using BITOJ.Core.Convert;
    using BITOJ.Core.Data;
    using System;

    /// <summary>
    /// 为题目显示提供数据模型。
    /// </summary>
    public class ProblemDisplayModel
    {
        /// <summary>
        /// 获取或设置题目 ID。
        /// </summary>
        public string ProblemId { get; set; }

        /// <summary>
        /// 获取或设置题目标题。
        /// </summary>
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
        /// 获取或设置题目的时间限制，以毫秒为单位。
        /// </summary>
        public int TimeLimit { get; set; }

        /// <summary>
        /// 获取或设置题目的内存限制，以 KB 为单位。
        /// </summary>
        public int MemoryLimit { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示当前题目的判题过程是否需要用户提供的 Judge 程序。
        /// </summary>
        public bool IsSpecialJudge { get; set; }

        /// <summary>
        /// 获取或设置访问题目所需的最低权限。
        /// </summary>
        public string UserGroupName { get; set; }

        /// <summary>
        /// 创建 AddProblemModel 类的新实例。
        /// </summary>
        public ProblemDisplayModel()
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
            TimeLimit = 1000;
            MemoryLimit = 64;
            IsSpecialJudge = false;
            UserGroupName = UsergroupConvert.ConvertToString(UserGroup.Guests);
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

        /// <summary>
        /// 从给定的题目数据提供器加载当前数据模型对象。
        /// </summary>
        /// <param name="data">题目数据提供器。</param>
        public void LoadFromProblemDataProvider(ProblemDataProvider data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            ProblemId = data.ProblemId;
            Title = data.Title;
            Description = data.Description;
            InputDescription = data.InputDescription;
            OutputDescription = data.OutputDescription;
            InputExample = data.InputExample;
            OutputExample = data.OutputExample;
            Hint = data.Hint;
            Source = data.Source;
            Author = data.Author;
            TimeLimit = data.TimeLimit;
            MemoryLimit = data.MemoryLimit;
            IsSpecialJudge = data.IsSpecialJudge;
        }

        /// <summary>
        /// 将当前数据模型对象中的数据存入给定的题目数据提供器。
        /// </summary>
        /// <param name="data">要存入的题目数据提供器。</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        public void SaveToProblemDataProvider(ProblemDataProvider data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            
            data.Title = Title;
            data.Description = Description;
            data.InputDescription = InputDescription;
            data.OutputDescription = OutputDescription;
            data.InputExample = InputExample;
            data.OutputExample = OutputExample;
            data.Hint = Hint;
            data.Source = Source;
            data.Author = Author;
            data.TimeLimit = TimeLimit;
            data.MemoryLimit = MemoryLimit;
            data.IsSpecialJudge = IsSpecialJudge;
        }
    }
}
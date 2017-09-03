namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;
    using BITOJ.Core.Data;
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 为题目运行环境提供数据模型。
    /// </summary>
    public class ProblemEnvironmentModel
    {
        /// <summary>
        /// 获取或设置题目ID。
        /// </summary>
        public string ProblemId { get; set; }

        /// <summary>
        /// 获取或设置题目标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置题目的时间限制，以毫秒为单位。
        /// </summary>
        [Range(1000, 3600 * 1000, ErrorMessage = "Time limit out of range.")]
        public int TimeLimit { get; set; }

        /// <summary>
        /// 获取或设置题目的内存限制，以 KB 为单位。
        /// </summary>
        [Range(32, 2 * 1024 * 1024, ErrorMessage = "Memory limit out of range.")]
        public int MemoryLimit { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示题目的判题过程是否需要用户提供的 Judge 例程。
        /// </summary>
        public bool UseSpecialJudge { get; set; }

        /// <summary>
        /// 创建 ProblemEnvironmentModel 类的新实例。
        /// </summary>
        public ProblemEnvironmentModel()
        {
            ProblemId = string.Empty;
            Title = string.Empty;
            TimeLimit = 0;
            MemoryLimit = 0;
            UseSpecialJudge = false;
        }

        /// <summary>
        /// 从给定的题目句柄创建 ProblemEnvironmentModel 数据对象。
        /// </summary>
        /// <param name="handle">题目句柄。</param>
        /// <returns>从给定题目句柄创建的 ProblemEnvironmentModel 对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static ProblemEnvironmentModel FromProblemHandle(ProblemHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            ProblemEnvironmentModel model = new ProblemEnvironmentModel();
            using (ProblemDataProvider data = ProblemDataProvider.Create(handle, true))
            {
                model.ProblemId = data.ProblemId;
                model.Title = data.Title;
                model.TimeLimit = data.TimeLimit;
                model.MemoryLimit = data.MemoryLimit;
                model.UseSpecialJudge = data.IsSpecialJudge;
            }

            return model;
        }
    }
}
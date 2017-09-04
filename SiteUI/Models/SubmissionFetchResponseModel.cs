namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;
    using BITOJ.Core.Data;
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// 为用户提交拉取操作的响应数据交换提供数据模型。
    /// </summary>
    public sealed class SubmissionFetchResponseModel
    {
        private const int BooleanFalse = 0;
        private const int BooleanTrue = 1;

        /// <summary>
        /// 获取或设置用户提交 ID。
        /// </summary>
        [JsonProperty("runid")]
        public int SubmissionId { get; set; }

        /// <summary>
        /// 获取或设置用户提交的题目 ID。
        /// </summary>
        [JsonProperty("proid")]
        public string ProblemId { get; set; }

        /// <summary>
        /// 获取或设置编码后的用户提交语言信息。
        /// </summary>
        [JsonProperty("lang")]
        public int Language { get; set; }

        /// <summary>
        /// 获取或设置用户提交的运行内存限制，以 KB 为单位。
        /// </summary>
        [JsonProperty("mlimit")]
        public int MemoryLimit { get; set; }

        /// <summary>
        /// 获取或设置用户提交的运行时间限制，以毫秒为单位。
        /// </summary>
        [JsonProperty("tlimit")]
        public int TimeLimit { get; set; }

        /// <summary>
        /// 获取或设置编码后的一个布尔值。该值指示评测过程是否需要用户自定义的 Judge 例程支持。
        /// </summary>
        [JsonProperty("spj")]
        public int UseSpecialJudge { get; set; }

        /// <summary>
        /// 获取或设置用户提交代码。
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// 创建 SubmissionHandle 类的新实例。
        /// </summary>
        public SubmissionFetchResponseModel()
        {
            SubmissionId = 0;
            ProblemId = string.Empty;
            Language = (int)SubmissionLanguage.GnuC;
            MemoryLimit = 0;
            TimeLimit = 0;
            UseSpecialJudge = BooleanFalse;
            Code = string.Empty;
        }

        /// <summary>
        /// 从给定的用户提交句柄创建 SubmissionFetchHandle 类的新实例。
        /// </summary>
        /// <param name="handle">用户提交句柄。</param>
        /// <returns>相应的 SubmissionFetchModel 数据模型对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static SubmissionFetchResponseModel FromSubmissionHandle(SubmissionHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            SubmissionFetchResponseModel model = new SubmissionFetchResponseModel();
            using (SubmissionDataProvider submissionData = SubmissionDataProvider.Create(handle, true))
            {
                model.SubmissionId = submissionData.SubmissionId;
                model.ProblemId = submissionData.ProblemId;
                model.Language = (int)submissionData.Language;
                model.Code = submissionData.Code;

                using (ProblemDataProvider problemData = 
                    ProblemDataProvider.Create(new ProblemHandle(submissionData.ProblemId), true))
                {
                    model.TimeLimit = problemData.TimeLimit;
                    model.MemoryLimit = problemData.MemoryLimit;
                    model.UseSpecialJudge = problemData.IsSpecialJudge ? BooleanTrue : BooleanFalse;
                }
            }

            return model;
        }

        /// <summary>
        /// 获取空响应对象。
        /// </summary>
        public static SubmissionFetchResponseModel Empty
        {
            get
            {
                return new SubmissionFetchResponseModel()
                {
                    SubmissionId = -1
                };
            }
        }
    }
}
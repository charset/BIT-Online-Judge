namespace BITOJ.SiteUI.Models.Ajax
{
    using BITOJ.Core;
    using BITOJ.Core.Convert;
    using BITOJ.Core.Data;
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// 为用户提交评测状态异步查询提供数据模型。
    /// </summary>
    public class SubmissionVerdictRequestModel
    {
        /// <summary>
        /// 获取或设置提交 ID。
        /// </summary>
        [JsonProperty("submissionId")]
        public int SubmissionId { get; set; }

        /// <summary>
        /// 获取或设置提交评测状态。
        /// </summary>
        [JsonProperty("verdict")]
        public string Verdict { get; set; }

        /// <summary>
        /// 获取或设置用户提交运行时间，以毫秒为单位。
        /// </summary>
        [JsonProperty("exeTime")]
        public int ExecutionTime { get; set; }

        /// <summary>
        /// 获取或设置用户提交运行内存，以 KB 为单位。
        /// </summary>
        [JsonProperty("exeMem")]
        public int ExecutionMemory { get; set; }

        /// <summary>
        /// 获取或设置评测消息。
        /// </summary>
        [JsonProperty("message")]
        public string VerdictMessage { get; set; }

        /// <summary>
        /// 创建 SubmissionVerdictRequestModel 类的新实例。
        /// </summary>
        [JsonConstructor]
        public SubmissionVerdictRequestModel()
        {
            SubmissionId = 0;
            Verdict = SubmissionVerdictConvert.ConvertToString(SubmissionVerdict.Unknown);
            ExecutionTime = 0;
            ExecutionMemory = 0;
            VerdictMessage = string.Empty;
        }

        /// <summary>
        /// 从给定的提交句柄创建 SubmissionVerdictRequestModel 类的新实例。
        /// </summary>
        /// <param name="handle">提交句柄。</param>
        /// <returns>从给定的提交句柄创建的 SubmissionVerdictRequestModel 类的新实例。</returns>
        ///  <exception cref="ArgumentNullException"/>
        public static SubmissionVerdictRequestModel FromSubmissionHandle(SubmissionHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            SubmissionVerdictRequestModel model = new SubmissionVerdictRequestModel();
            using (SubmissionDataProvider data = SubmissionDataProvider.Create(handle, true))
            {
                model.SubmissionId = data.SubmissionId;
                model.ExecutionTime = data.ExecutionTime;
                model.ExecutionMemory = data.ExecutionMemory;
                model.VerdictMessage = data.VerdictMessage;

                if (data.VerdictStatus == SubmissionVerdictStatus.Completed)
                {
                    model.Verdict = SubmissionVerdictConvert.ConvertToString(data.Verdict);
                }
                else
                {
                    model.Verdict = SubmissionVerdictStatusConvert.ConvertToString(data.VerdictStatus);
                }
            }

            return model;
        }
    }
}
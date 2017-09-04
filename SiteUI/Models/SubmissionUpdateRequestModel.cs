namespace BITOJ.SiteUI.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// 为用户提交更新请求提供请求数据报文模型。
    /// </summary>
    public class SubmissionUpdateRequestModel
    {
        /// <summary>
        /// 获取或设置用户提交 ID。
        /// </summary>
        [JsonProperty("runid")]
        public int SubmissionId { get; set; }

        /// <summary>
        /// 获取或设置客户程序运行时间，以毫秒为单位。
        /// </summary>
        [JsonProperty("exetime")]
        public int ExecutionTime { get; set; }

        /// <summary>
        /// 获取或设置客户程序占用的峰值内存总量，以 KB 为单位。
        /// </summary>
        [JsonProperty("exemem")]
        public int ExecutionMemory { get; set; }

        /// <summary>
        /// 获取或设置评测结果。
        /// </summary>
        [JsonProperty("status")]
        public int Verdict { get; set; }

        /// <summary>
        /// 获取或设置评测消息。
        /// </summary>
        [JsonProperty("errinfo")]
        public string VerdictMessage { get; set; }

        /// <summary>
        /// 获取或设置客户端验证密钥。
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// 创建 SubmissionUpdateRequestModel 类的新实例。
        /// </summary>
        [JsonConstructor]
        public SubmissionUpdateRequestModel()
        {
            SubmissionId = 0;
            ExecutionTime = 0;
            ExecutionMemory = 0;
            Verdict = 0;
            VerdictMessage = string.Empty;
            Password = string.Empty;
        }
    }
}

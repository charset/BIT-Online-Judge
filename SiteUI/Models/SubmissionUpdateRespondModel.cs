namespace BITOJ.SiteUI.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// 为用户提交更新请求响应报文提供模型。
    /// </summary>
    public class SubmissionUpdateRespondModel
    {
        /// <summary>
        /// 获取或设置主机操作状态码。
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; set; }

        /// <summary>
        /// 创建 SubmissionUpdateRespondModel 类的新实例。
        /// </summary>
        [JsonConstructor]
        public SubmissionUpdateRespondModel()
        {
            Status = 0;
        }

        /// <summary>
        /// 获取代表主机操作成功的响应。
        /// </summary>
        public static SubmissionUpdateRespondModel Succeed
        {
            get
            {
                return new SubmissionUpdateRespondModel()
                {
                    Status = 0
                };
            }
        }

        /// <summary>
        /// 获取代表主机操作失败的响应。
        /// </summary>
        public static SubmissionUpdateRespondModel Failed
        {
            get
            {
                return new SubmissionUpdateRespondModel()
                {
                    Status = -1
                };
            }
        }
    }
}
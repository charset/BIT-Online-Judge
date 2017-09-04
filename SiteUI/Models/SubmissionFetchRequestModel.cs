namespace BITOJ.SiteUI.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// 为用户提交拉取操作请求数据交换提供数据模型。
    /// </summary>
    public class SubmissionFetchRequestModel
    {
        /// <summary>
        /// 获取或设置客户端身份验证哈希字符串。
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// 创建 SubmissionFetchRequestModel 类的新实例。
        /// </summary>
        [JsonConstructor]
        public SubmissionFetchRequestModel()
        {
            Password = string.Empty;
        }
    }
}
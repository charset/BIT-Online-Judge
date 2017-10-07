namespace BITOJ.SiteUI.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// 为 BITOJ 异步查询提供数据模型。
    /// </summary>
    public class QueryModel
    {
        /// <summary>
        /// 获取嚯设置操作错误码。
        /// </summary>
        [JsonProperty("errcode")]
        public int ErrorCode { get; set; }

        /// <summary>
        /// 获取或设置操作错误消息。
        /// </summary>
        [JsonProperty("errmsg")]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 获取或设置响应数据对象。
        /// </summary>
        [JsonProperty("data")]
        public object Data { get; set; }

        /// <summary>
        /// 创建 QueryModel 类的新实例。
        /// </summary>
        [JsonConstructor]
        public QueryModel()
        {
            ErrorCode = 0;
            ErrorMessage = string.Empty;
            Data = null;
        }
    }
}
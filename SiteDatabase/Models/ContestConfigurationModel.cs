namespace BITOJ.Data.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// 包装比赛的配置数据。
    /// </summary>
    public sealed class ContestConfigurationModel
    {
        /// <summary>
        /// 获取或设置比赛的身份验证数据模型。
        /// </summary>
        [JsonProperty("auth")]
        public ContestAuthorizationModel Authorization { get; set; }

        /// <summary>
        /// 获取或设置比赛的题目集合。
        /// </summary>
        [JsonProperty("problems")]
        public ICollection<string> Problems { get; set; }

        /// <summary>
        /// 创建 ContestConfigurationModel 类的新实例。
        /// </summary>
        [JsonConstructor]
        public ContestConfigurationModel()
        {
            Authorization = new ContestAuthorizationModel();
            Problems = new List<string>();
        }
    }
}

namespace BITOJ.Data.Models
{
    using BITOJ.Data.Entities;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// 表示比赛授权配置数据模型。
    /// </summary>
    public sealed class ContestAuthorizationModel
    {
        /// <summary>
        /// 获取或设置比赛的参与模式。
        /// </summary>
        [JsonProperty("participate_mode")]
        public ContestParticipationMode ParticipationMode { get; set; }

        /// <summary>
        /// 获取或设置比赛的授权模式。
        /// </summary>
        [JsonProperty("mode")]
        public ContestAuthorizationMode AuthorizationMode { get; set; }

        /// <summary>
        /// 当授权模式在 Protected 模式时，获取或设置比赛密码的哈希值。
        /// </summary>
        public byte[] PasswordHash { get; set; }

        /// <summary>
        /// 当授权模式在 Private 模式时，获取或设置处在白名单或黑名单中的用户。
        /// </summary>
        [JsonProperty("users")]
        public ICollection<string> Users { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否应当将用户列表作为黑名单列表。若该属性值为 false，用户列表将被作为白名单列表。
        /// </summary>
        [JsonProperty("users_as_blacklist")]
        public bool UseUsersAsBlacklist { get; set; }

        /// <summary>
        /// 当授权模式在 Private 模式时，获取或设置处在白名单或黑名单中的队伍。
        /// </summary>
        [JsonProperty("teams")]
        public ICollection<int> Teams { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否应当将队伍列表作为黑名单列表。若该属性值为 false，队伍列表将被作为白名单列表。
        /// </summary>
        [JsonProperty("teams_as_blacklist")]
        public bool UseTeamsAsBlacklist { get; set; }

        /// <summary>
        /// 获取或设置访问此比赛的最低用户权限。
        /// </summary>
        [JsonProperty("usergroup")]
        public UserGroup AuthorizationGroup { get; set; }

        /// <summary>
        /// 初始化 ContestAuthorizationModel 类的新实例。
        /// </summary>
        [JsonConstructor]
        public ContestAuthorizationModel()
        {
            ParticipationMode = ContestParticipationMode.IndividualAndTeamwork;
            AuthorizationMode = ContestAuthorizationMode.Private;
            PasswordHash = new byte[0];
            Users = new List<string>();
            UseUsersAsBlacklist = false;
            Teams = new List<int>();
            UseTeamsAsBlacklist = false;
            AuthorizationGroup = UserGroup.Standard;
        }
    }
}

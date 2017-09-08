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
        /// 获取或设置访问此比赛的最低用户权限。
        /// </summary>
        [JsonProperty("usergroup")]
        public UserGroup AuthorizationGroup { get; set; }

        /// <summary>
        /// 获取或设置获得访问权限的用户集合。
        /// </summary>
        [JsonProperty("users")]
        public ICollection<string> AuthorizedUsers { get; set; }

        /// <summary>
        /// 获取或设置获得访问权限的队伍集合。
        /// </summary>
        [JsonProperty("teams")]
        public ICollection<int> AuthorizedTeams { get; set; }

        /// <summary>
        /// 初始化 ContestAuthorizationModel 类的新实例。
        /// </summary>
        [JsonConstructor]
        public ContestAuthorizationModel()
        {
            ParticipationMode = ContestParticipationMode.IndividualAndTeamwork;
            AuthorizationMode = ContestAuthorizationMode.Private;
            PasswordHash = new byte[0];
            AuthorizationGroup = UserGroup.Standard;
            AuthorizedUsers = new List<string>();
            AuthorizedTeams = new List<int>();
        }
    }
}

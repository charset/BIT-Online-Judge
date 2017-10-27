namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;
    using BITOJ.Core.Data;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 提供队伍详细信息数据模型。
    /// </summary>
    public class TeamDetailModel
    {
        /// <summary>
        /// 获取或设置队伍 ID。
        /// </summary>
        [JsonProperty("id")]
        public int TeamId { get; set; }

        /// <summary>
        /// 获取或设置队伍名称。
        /// </summary>
        [JsonProperty("name")]
        public string TeamName { get; set; }

        /// <summary>
        /// 获取或设置队伍领队。
        /// </summary>
        [JsonProperty("leader")]
        public string Leader { get; set; }

        /// <summary>
        /// 获取或设置队伍成员。
        /// </summary>
        [JsonProperty("members")]
        public ICollection<UserProfileModel> Members { get; set; }

        /// <summary>
        /// 创建 TeamDetailModel 类的新实例。
        /// </summary>
        public TeamDetailModel()
        {
            TeamId = 0;
            TeamName = string.Empty;
            Leader = string.Empty;
            Members = new List<UserProfileModel>();
        }

        /// <summary>
        /// 从给定的队伍句柄创建队伍详细信息数据模型。
        /// </summary>
        /// <param name="handle">队伍句柄。</param>
        /// <returns>队伍详细信息数据模型。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static TeamDetailModel FromTeamHandle(TeamHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            TeamDetailModel model = new TeamDetailModel();
            using (TeamDataProvider data = TeamDataProvider.Create(handle, true))
            {
                model.TeamId = data.TeamId;
                model.TeamName = data.Name;
                model.Leader = data.Leader;
                
                foreach (UserHandle user in data.GetMembers())
                {
                    model.Members.Add(UserProfileModel.FromUserHandle(user));
                }
            }

            return model;
        }
    }
}
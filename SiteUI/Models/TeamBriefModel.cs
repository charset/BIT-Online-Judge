namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;
    using BITOJ.Core.Data;
    using System;

    /// <summary>
    /// 提供队伍简要信息数据模型。
    /// </summary>
    public class TeamBriefModel
    {
        /// <summary>
        /// 获取队伍 ID。
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// 获取或设置队伍名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置队伍成员数目。
        /// </summary>
        public int MembersCount { get; set; }

        /// <summary>
        /// 创建 TeamBriefModel 类的新实例。
        /// </summary>
        public TeamBriefModel()
        {
            TeamId = 0;
            Name = string.Empty;
            MembersCount = 0;
        }

        /// <summary>
        /// 从给定的队伍句柄加载队伍数据到当前数据模型中。
        /// </summary>
        /// <param name="data">队伍句柄。</param>
        /// <exception cref="ArgumentNullException"/>
        public static TeamBriefModel FromTeamHandle(TeamHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            TeamBriefModel model = new TeamBriefModel();
            using (TeamDataProvider data = TeamDataProvider.Create(handle, true))
            {
                model.TeamId = data.TeamId;
                model.Name = data.Name;
                model.MembersCount = data.GetMembers().Length;
            }

            return model;
        }
    }
}
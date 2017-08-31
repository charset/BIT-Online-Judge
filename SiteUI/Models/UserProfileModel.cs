namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;
    using BITOJ.Core.Convert;
    using BITOJ.Core.Data;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 提供用户个人信息数据模型。
    /// </summary>
    public class UserProfileModel
    {
        /// <summary>
        /// 获取或设置用户名。
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 获取或设置用户组织名称。
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// 获取或设置用户权限集。
        /// </summary>
        public UserGroup UserGroup { get; set; }

        /// <summary>
        /// 获取或设置用户性别的字符串表示。
        /// </summary>
        public UserSex Sex { get; set; }

        /// <summary>
        /// 获取或设置用户所在的队伍。
        /// </summary>
        public ICollection<TeamBriefModel> Teams { get; set; }

        /// <summary>
        /// 获取或设置用户提交统计信息。
        /// </summary>
        public UserSubmissionStatistics SubmissionStatistics { get; set; }

        /// <summary>
        /// 创建 UserProfileModel 类的新实例。
        /// </summary>
        public UserProfileModel()
        {
            Username = string.Empty;
            Organization = string.Empty;
            UserGroup = UserGroup.Guests;
            Sex = UserSex.Male;
            Teams = new List<TeamBriefModel>();
            SubmissionStatistics = new UserSubmissionStatistics();
        }

        /// <summary>
        /// 从用户句柄创建 UserProfileModel 模型对象。
        /// </summary>
        /// <param name="handle">用户句柄。</param>
        /// <returns>UserProfileModel 模型对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static UserProfileModel FromUserHandle(UserHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            UserProfileModel model = new UserProfileModel()
            {
                Username = handle.Username
            };
            using (UserDataProvider userData = UserDataProvider.Create(handle, true))
            {
                model.Organization = userData.Organization;
                model.UserGroup = userData.UserGroup;
                model.Sex = userData.Sex;

                // 加载用户队伍信息。
                foreach (TeamHandle team in userData.GetTeams())
                {
                    model.Teams.Add(TeamBriefModel.FromTeamHandle(team));
                }

                // TODO: 完成用户提交统计模块后，在这里添加代码将用户提交统计信息复制入模型中。
            }

            return model;
        }
    }
}
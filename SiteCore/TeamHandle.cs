namespace BITOJ.Core
{
    using BITOJ.Core.Data;
    using BITOJ.Data.Entities;
    using System;

    /// <summary>
    /// 表示队伍标识句柄。
    /// </summary>
    public sealed class TeamHandle
    {
        /// <summary>
        /// 获取或设置 Team ID.
        /// </summary>
        public int TeamId { get; private set; }

        /// <summary>
        /// 使用给定的 Team ID 初始化 TeamHandle 类的新实例。
        /// </summary>
        /// <param name="id">队伍ID。</param>
        public TeamHandle(int id)
        {
            TeamId = id;
        }

        /// <summary>
        /// 获取队伍中的成员。
        /// </summary>
        /// <returns>一个数组，其中包含队伍中的成员用户句柄。</returns>
        public UserHandle[] GetMembers()
        {
            using (TeamDataProvider teamData = TeamDataProvider.Create(this, true))
            {
                return teamData.GetMembers();
            }
        }

        /// <summary>
        /// 确定给定的用户是否在当前的队伍中。
        /// </summary>
        /// <param name="user">用户句柄。</param>
        /// <returns>一个值，该值指示给定的用户是否在当前队伍中。</returns>
        public bool IsUserIn(UserHandle user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            foreach (UserHandle handle in GetMembers())
            {
                if (user == handle)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 从给定的队伍信息实体对象创建 TeamHandle 类的新实例。
        /// </summary>  
        /// <param name="entity">队伍信息实体对象。</param>
        /// <returns>从给定的队伍信息实体对象创建的 TeamHandle 对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static TeamHandle FromTeamEntity(TeamProfileEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new TeamHandle(entity.Id);
        }
    }
}

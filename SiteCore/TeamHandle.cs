namespace BITOJ.Core
{
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

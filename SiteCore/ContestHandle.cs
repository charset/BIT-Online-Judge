namespace BITOJ.Core
{
    using BITOJ.Data.Entities;
    using System;

    /// <summary>
    /// 封装比赛句柄。
    /// </summary>
    public class ContestHandle
    {
        /// <summary>
        /// 获取比赛 ID。
        /// </summary>
        public int ContestId { get; private set; }

        /// <summary>
        /// 使用给定的比赛 ID 创建 ContestHandle 类的新实例。
        /// </summary>
        /// <param name="contestId">比赛 ID。</param>
        public ContestHandle(int contestId)
        {
            ContestId = contestId;
        }

        /// <summary>
        /// 从给定的比赛实体对象创建 ContestHandle 类的新实例。
        /// </summary>
        /// <param name="entity">比赛实体对象。</param>
        /// <returns>从给定的比赛实体对象创建的 ContestHandle 对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        internal static ContestHandle FromContestEntity(ContestEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new ContestHandle(entity.Id);
        }
    }
}

namespace BITOJ.Core
{
    using BITOJ.Data.Entities;
    using System;

    /// <summary>
    /// 封装一个用户提交的信息。
    /// </summary>
    public sealed class SubmissionHandle
    {
        /// <summary>
        /// 获取提交 ID。
        /// </summary>
        public int SubmissionId { get; }

        /// <summary>
        /// 使用给定的用户提交 ID 创建 SubmissionHandle 类的新实例。
        /// </summary>
        /// <param name="id">用户提交 ID。</param>
        public SubmissionHandle(int id)
        {
            SubmissionId = id;
        }

        /// <summary>
        /// 从给定的用户提交实体对象创建 SubmissionEntity 类的新实例。
        /// </summary>
        /// <param name="entity">用户提交实体对象。</param>
        /// <returns>创建的用户提交句柄。</returns>
        /// <exception cref="ArgumentNullException"/>
        internal static SubmissionHandle FromSubmissionEntity(SubmissionEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new SubmissionHandle(entity.Id);
        }
    }
}

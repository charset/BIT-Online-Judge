namespace BITOJ.Core
{
    using BITOJ.Data.Entities;
    using System;

    /// <summary>
    /// 封装一道题目的基本信息。
    /// </summary>
    /// <remarks>
    /// 题目的具体信息，例如题干、样例输入等不被封装在该类中。
    /// </remarks>
    public class ProblemHandle
    {
        /// <summary>
        /// 获取或设置题目的 ID。该 ID 包括但不限于 BITOJ 的题目 ID 格式。
        /// </summary>
        public string ProblemId { get; private set; }

        /// <summary>
        /// 初始化 ProblemHandle 类的新实例。
        /// </summary>
        public ProblemHandle(string problemId)
        {
            ProblemId = problemId;
        }

        /// <summary>
        /// 获取一个值，该值指示当前题目句柄所引用的题目是否为 BITOJ 本地题目。
        /// </summary>
        public bool IsNativeProblem
        {
            get
            {
                return ProblemId.StartsWith("BIT", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        /// <summary>
        /// 从给定的 ProblemEntity 对象创建对应的 ProblemHandle 对象。
        /// </summary>
        /// <param name="entity">底层数据库管理的实体对象。</param>
        /// <returns>问题句柄对象。</returns>
        internal static ProblemHandle FromProblemEntity(ProblemEntity entity)
        {
            return new ProblemHandle(entity.Id);
        }
    }
}

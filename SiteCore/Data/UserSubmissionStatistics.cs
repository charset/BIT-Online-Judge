namespace BITOJ.Core.Data
{
    using BITOJ.Core;
    using System.Collections.Generic;

    /// <summary>
    /// 提供用户提交统计信息。
    /// </summary>
    public sealed class UserSubmissionStatistics
    {
        /// <summary>
        /// 获取或设置用户的总提交数目。
        /// </summary>
        public int TotalSubmissions { get; set; }

        /// <summary>
        /// 获取或设置用户的 AC 提交数目。
        /// </summary>
        public int AcceptedSubmissions { get; set; }

        /// <summary>
        /// 获取或设置用户的所有提交的提交 ID。
        /// </summary>
        public ICollection<SubmissionHandle> Submissions { get; set; }

        /// <summary>
        /// 获取或设置用户所有已尝试的题目 ID。
        /// </summary>
        public ICollection<ProblemHandle> AttemptedProblems { get; set; }

        /// <summary>
        /// 获取或设置用户所有 AC 的题目 ID。
        /// </summary>
        public ICollection<ProblemHandle> AcceptedProblems { get; set; }

        /// <summary>
        /// 初始化 UserSubmissionStatistics 类的新实例。
        /// </summary>
        public UserSubmissionStatistics()
        {
            TotalSubmissions = 0;
            AcceptedSubmissions = 0;
            Submissions = new List<SubmissionHandle>();
            AttemptedProblems = new List<ProblemHandle>();
            AcceptedProblems = new List<ProblemHandle>();
        }
    }
}

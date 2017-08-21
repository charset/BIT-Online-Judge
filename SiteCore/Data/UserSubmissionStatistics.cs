namespace BITOJ.Core.Data
{
    using BITOJ.Data.Models;
    using BITOJ.Core;
    using System;
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

        /// <summary>
        /// 从当前对象创建底层模型对象。
        /// </summary>
        /// <returns>当前对象的底层模型对象。</returns>
        internal UserSubmissionStatisticsModel ToStatisticsModel()
        {
            UserSubmissionStatisticsModel model = new UserSubmissionStatisticsModel()
            {
                TotalSubmissions = TotalSubmissions,
                AcceptedSubmissions = AcceptedSubmissions,
            };

            // 复制列表对象。
            foreach (SubmissionHandle handle in Submissions)
            {
                model.Submissions.Add(handle.SubmissionId);
            }
            foreach (ProblemHandle handle in AttemptedProblems)
            {
                model.AttemptedProblemId.Add(handle.ProblemId);
            }
            foreach (ProblemHandle handle in AcceptedProblems)
            {
                model.AttemptedProblemId.Add(handle.ProblemId);
            }

            return model;
        }

        /// <summary>
        /// 从底层模型对象创建 UserSubmissionStatistics 类的新实例。
        /// </summary>
        /// <param name="model">模型对象。</param>
        /// <returns>从模型对象创建的 UserSubmissionStatistics 类。</returns>
        /// <exception cref="ArgumentNullException"/>
        internal static UserSubmissionStatistics FromStatisticsModel(UserSubmissionStatisticsModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            UserSubmissionStatistics stat = new UserSubmissionStatistics()
            {
                TotalSubmissions = model.TotalSubmissions,
                AcceptedSubmissions = model.AcceptedSubmissions
            };

            // 复制列表对象。
            
            // TODO: 复制用户提交 ID 列表到提交句柄列表。

            foreach (string probId in model.AttemptedProblemId)
            {
                stat.AttemptedProblems.Add(new ProblemHandle(probId));
            }
            foreach (string probId in model.AcceptedProblemId)
            {
                stat.AcceptedProblems.Add(new ProblemHandle(probId));
            }

            return stat;
        }
    }
}

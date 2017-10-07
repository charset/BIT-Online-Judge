namespace BITOJ.Core.Data.Queries
{
    using BITOJ.Core;
    using BITOJ.Data;
    using DatabaseLanguage = BITOJ.Data.Entities.SubmissionLanguage;
    using DatabaseVerdict = BITOJ.Data.Entities.SubmissionVerdict;

    /// <summary>
    /// 为用户提交查询提供查询数据。
    /// </summary>
    public sealed class SubmissionQueryParameter
    {
        /// <summary>
        /// 当 QueryByProblemId 为 true 时，获取或设置要查询的题目 ID。
        /// </summary>
        public string ProblemId { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否按照题目 ID 进行查询。
        /// </summary>
        public bool QueryByProblemId { get; set; }

        /// <summary>
        /// 当 QueryByUsername 为 true 时，获取或设置要查询的作者用户名。
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否使用用户名进行查询。
        /// </summary>
        public bool QueryByUsername { get; set; }

        /// <summary>
        /// 当 QueryByTeamId 为 true 时，获取或设置要查询的队伍 ID 。
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否使用队伍 ID 进行查询。
        /// </summary>
        public bool QueryByTeamId { get; set; }

        /// <summary>
        /// 当 QueryByContestId 为 true 时，获取或设置提交所属的比赛 ID。
        /// </summary>
        public int ContestId { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否使用比赛 ID 进行查询。
        /// </summary>
        public bool QueryByContestId { get; set; }

        /// <summary>
        /// 当 QueryByLanguage 为 true 时，获取或设置要查询的程序设计语言。
        /// </summary>
        public SubmissionLanguage Language { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否使用语言进行查询。
        /// </summary>
        public bool QueryByLanguage { get; set; }

        /// <summary>
        /// 当 QueryByVerdict 为 true 时，获取或设置要查询的判题结果。
        /// </summary>
        public SubmissionVerdict Verdict { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否使用判题结果进行查询。
        /// </summary>
        public bool QueryByVerdict { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否应按创建时间戳降序排序查询结果。
        /// </summary>
        public bool OrderByDescending { get; set; }

        /// <summary>
        /// 获取或设置分页查询参数。
        /// </summary>
        public PageQueryParameter PageQuery { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否应启用分页查询。
        /// </summary>
        public bool EnablePageQuery { get; set; }

        /// <summary>
        /// 创建 SubmissionQueryParameter 类的新实例。
        /// </summary>
        public SubmissionQueryParameter()
        {
            ProblemId = string.Empty;
            QueryByProblemId = false;
            Username = string.Empty;
            QueryByUsername = false;
            TeamId = 0;
            QueryByTeamId = false;
            Language = SubmissionLanguage.GnuC;
            QueryByLanguage = false;
            Verdict = SubmissionVerdict.Accepted;
            QueryByVerdict = false;
            OrderByDescending = true;
            PageQuery = new PageQueryParameter();
            EnablePageQuery = false;
        }

        /// <summary>
        /// 从当前对象创建数据库查询句柄对象。
        /// </summary>
        /// <returns>数据库查询句柄。</returns>
        internal SubmissionQueryHandle GetQueryHandle()
        {
            SubmissionQueryHandle handle = new SubmissionQueryHandle()
            {
                ProblemId = ProblemId,
                UseProblemId = QueryByProblemId,
                ContestId = ContestId,
                UseContestId = QueryByContestId,
                Username = Username,
                UseUsername = QueryByUsername,
                TeamId = TeamId,
                UseTeamId = QueryByTeamId,
                Language = (DatabaseLanguage)Language,
                UseLanguage = QueryByLanguage,
                VerdictResult = (DatabaseVerdict)Verdict,
                UseVerdictResult = QueryByVerdict
            };

            return handle;
        }
    }
}

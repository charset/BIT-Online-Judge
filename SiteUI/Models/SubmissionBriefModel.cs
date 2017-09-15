namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;
    using BITOJ.Core.Convert;
    using BITOJ.Core.Data;
    using System;

    /// <summary>
    /// 为用户提交简要信息提供数据模型。
    /// </summary>
    public class SubmissionBriefModel
    {
        /// <summary>
        /// 获取或设置提交 ID。
        /// </summary>
        public int SubmissionId { get; set; }

        /// <summary>
        /// 获取或设置用户名。
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 获取或设置题目 ID。
        /// </summary>
        public string ProblemId { get; set; }

        /// <summary>
        /// 获取或设置题目标题。
        /// </summary>
        public string ProblemTitle { get; set; }

        /// <summary>
        /// 获取或设置用户提交的程序设计语言与运行环境。
        /// </summary>
        public SubmissionLanguage Language { get; set; }

        /// <summary>
        /// 获取或设置用户提交的评测状态。
        /// </summary>
        public SubmissionVerdictStatus VerdictStatus { get; set; }

        /// <summary>
        /// 获取或设置用户提交的评测结果。
        /// </summary>
        public SubmissionVerdict Verdict { get; set; }

        /// <summary>
        /// 获取或设置用户提交的创建时间。
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 创建 SubmissionBriefModel 类的新实例。
        /// </summary>
        public SubmissionBriefModel()
        {
            SubmissionId = 0;
            Username = string.Empty;
            ProblemId = string.Empty;
            ProblemTitle = string.Empty;
            Language = SubmissionLanguage.GnuC;
            VerdictStatus = SubmissionVerdictStatus.Pending;
            Verdict = SubmissionVerdict.Accepted;
            CreationTime = new DateTime();
        }

        /// <summary>
        /// 获取当前用户提交的评测状态结果字符串。
        /// </summary>
        /// <returns>当前用户提交的评测状态结果字符串。</returns>
        public string GetVerdictString()
        {
            if (VerdictStatus == SubmissionVerdictStatus.Completed)
            {
                return SubmissionVerdictConvert.ConvertToString(Verdict);
            }
            else
            {
                return SubmissionVerdictStatusConvert.ConvertToString(VerdictStatus);
            }
        }

        /// <summary>
        /// 获取当前用户提交的评测结果的渲染 class 名称。
        /// </summary>
        /// <returns>当前用户提交的评测结果的渲染 class 名称。</returns>
        public string GetVerdictClassName()
        {
            if (VerdictStatus == SubmissionVerdictStatus.Completed)
            {
                if (Verdict == SubmissionVerdict.Accepted)
                {
                    return "text-verdict-ac";
                }
                else if (Verdict == SubmissionVerdict.SystemError)
                {
                    return "text-verdict-se";
                }
                else if (Verdict == SubmissionVerdict.Unknown)
                {
                    return "text-verdict-unknown";
                }
                else if (Verdict == SubmissionVerdict.PresentationError)
                {
                    return "text-verdict-pe";
                }
                else
                {
                    return "text-verdict-err";
                }
            }
            else
            {
                return "text-verdict-unknown";
            }
        }

        /// <summary>
        /// 从给定的用户提交句柄创建 SubmissionHandle 类的新实例。
        /// </summary>
        /// <param name="handle">用户提交句柄。</param>
        /// <returns>从给定用户提交句柄创建的 SubmissionHandle 类的新实例。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static SubmissionBriefModel FromSubmissionHandle(SubmissionHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            SubmissionBriefModel model = new SubmissionBriefModel();
            using (SubmissionDataProvider data = SubmissionDataProvider.Create(handle, true))
            {
                model.SubmissionId = data.SubmissionId;
                model.ProblemId = data.ProblemId;
                model.Username = data.Username;
                model.Language = data.Language;
                model.VerdictStatus = data.VerdictStatus;
                model.Verdict = data.Verdict;
                model.CreationTime = data.CreationTimeStamp;
            }

            return model;
        }
    }
}
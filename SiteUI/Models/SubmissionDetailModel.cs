namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;
    using BITOJ.Core.Convert;
    using BITOJ.Core.Data;
    using System;

    /// <summary>
    /// 为用户提交详细数据提供数据模型。
    /// </summary>
    public class SubmissionDetailModel
    {
        /// <summary>
        /// 获取或设置提交 ID。
        /// </summary>
        public int SubmissionId { get; set; }

        /// <summary>
        /// 获取或设置题目 ID。
        /// </summary>
        public string ProblemId { get; set; }

        /// <summary>
        /// 获取或设置比赛 ID。
        /// </summary>
        public int ContestId { get; set; }

        /// <summary>
        /// 获取或设置题目标题。
        /// </summary>
        public string ProblemTitle { get; set; }

        /// <summary>
        /// 获取或设置提交用户名。
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 获取或设置提交队伍 ID。
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// 获取或设置创建时间。
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 获取或设置源代码。
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 获取或设置用户提交的运行时间，以毫秒为单位。
        /// </summary>
        public int ExecutionTime { get; set; }

        /// <summary>
        /// 获取或设置用户提交的运行峰值内存，以 KB 为单位。
        /// </summary>
        public int ExecutionMemory { get; set; }

        /// <summary>
        /// 获取或设置用户提交的编程语言。
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
        /// 获取或设置用户提交的评测消息。
        /// </summary>
        public string VerdictMessage { get; set; }

        /// <summary>
        /// 创建 SubmissionDetailModel 类的新实例。
        /// </summary>
        public SubmissionDetailModel()
        {
            SubmissionId = 0;
            ProblemId = string.Empty;
            ContestId = 0;
            ProblemTitle = string.Empty;
            Username = string.Empty;
            TeamId = 0;
            CreationTime = new DateTime();
            Code = string.Empty;
            ExecutionTime = 0;
            ExecutionMemory = 0;
            Language = SubmissionLanguage.GnuC;
            VerdictStatus = SubmissionVerdictStatus.Pending;
            Verdict = SubmissionVerdict.Unknown;
            VerdictMessage = string.Empty;
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
        /// 获取当前用户提交的 tr 元素的渲染 class 名称。
        /// </summary>
        /// <returns>当前用户提交的 tr 元素的渲染 class 名称。</returns>
        public string GetRowClassName()
        {
            if (VerdictStatus == SubmissionVerdictStatus.Completed)
            {
                switch (Verdict)
                {
                    case SubmissionVerdict.Accepted:
                        return "success";

                    case SubmissionVerdict.PresentationError:
                        return "warning";

                    case SubmissionVerdict.SystemError:
                        return string.Empty;

                    default:
                        return "danger";
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 从给定的用户提交句柄创建 SubmissionDetailModel 类的新实例。
        /// </summary>
        /// <param name="handle">用户提交句柄。</param>
        /// <returns>从给定的句柄创建的 SubmissionDetailModel 类的新实例。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static SubmissionDetailModel FromSubmissionHandle(SubmissionHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            SubmissionDetailModel model = new SubmissionDetailModel();
            using (SubmissionDataProvider submissionData = SubmissionDataProvider.Create(handle, true))
            {
                model.SubmissionId = submissionData.SubmissionId;
                model.ProblemId = submissionData.ProblemId;
                model.ContestId = submissionData.ContestId;
                model.Username = submissionData.Username;
                model.TeamId = submissionData.TeamId;
                model.CreationTime = submissionData.CreationTimeStamp;
                model.Language = submissionData.Language;
                model.ExecutionTime = submissionData.ExecutionTime;
                model.ExecutionMemory = submissionData.ExecutionMemory;
                model.VerdictStatus = submissionData.VerdictStatus;
                model.Verdict = submissionData.Verdict;
                model.VerdictMessage = submissionData.VerdictMessage;
                
                using (ProblemDataProvider problemData = 
                    ProblemDataProvider.Create(new ProblemHandle(submissionData.ProblemId), true))
                {
                    model.ProblemTitle = problemData.Title;
                }
            }

            return model;
        }
    }
}
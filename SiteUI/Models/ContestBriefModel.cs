namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;
    using BITOJ.Core.Data;
    using System;
    using System.Text;

    public class ContestBriefModel
    {
        /// <summary>
        /// 获取或设置比赛ID。
        /// </summary>
        public int ContestId { get; set; }

        /// <summary>
        /// 获取或设置比赛标题。
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置比赛的举办者。
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 获取或设置比赛起始时间。
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 获取或设置比赛终止时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 获取或设置比赛的身份验证模式。
        /// </summary>
        public ContestAuthorizationMode AuthorizationMode { get; set; }

        /// <summary>
        /// 获取或设置比赛的参与模式。
        /// </summary>
        public ContestParticipationMode ParticipationMode { get; set; }

        /// <summary>
        /// 获取或设置比赛的当前执行状态。
        /// </summary>
        public ContestStatus Status { get; set; }

        /// <summary>
        /// 创建 ContestBriefMode 类的新实例。
        /// </summary>
        public ContestBriefModel()
        {
            ContestId = 0;
            Title = string.Empty;
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
            AuthorizationMode = ContestAuthorizationMode.Private;
            ParticipationMode = ContestParticipationMode.Both;
            Status = ContestStatus.Pending;
            Creator = string.Empty;
        }

        /// <summary>
        /// 获取比赛时长的字符串表示。
        /// </summary>
        /// <returns>比赛时长的字符串表示。</returns>
        private string GetDurationString()
        {
            double minutes = (EndTime - StartTime).TotalMinutes;
            if (minutes >= 2 * 24 * 60)
            {
                // 大于两天的时长，按照天为单位进行格式化
                return string.Format("{0:N1} day(s)", minutes / (24 * 60));
            }
            else if (minutes >= 60)
            {
                // 大于一个小时并且小于两天，按照小时为单位进行格式化
                return string.Format("{0:N1} hour(s)", minutes / 60D);
            }
            else
            {
                // 少于一个小时。按照分钟为单位进行格式化。
                return string.Format("{0:N0} minute(s)", minutes);
            }
        }

        /// <summary>
        /// 获取比赛日历的 HTML 表示。
        /// </summary>
        /// <returns>比赛日历的 HTML 表示。</returns>
        public string GetCalendarHtml()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("<p>Start time: {0:yyyy-MM-dd HH:mm:ss}</p>", StartTime);
            builder.AppendFormat("<p>End time: {0:yyyy-MM-dd HH:mm:ss}</p>", EndTime);
            builder.AppendFormat("<p>Duration: {0}</p>", GetDurationString());

            return builder.ToString();
        }

        /// <summary>
        /// 从给定的比赛句柄创建 ContestBriefModel 类的新实例。
        /// </summary>
        /// <param name="handle">比赛句柄。</param>
        /// <returns>从给定的比赛句柄创建的 ContestBriefModel 类对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static ContestBriefModel FromContestHandle(ContestHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            ContestBriefModel model = new ContestBriefModel();
            using (ContestDataProvider data = ContestDataProvider.Create(handle, true))
            {
                model.ContestId = data.ContestId;
                model.Title = data.Title;
                model.Creator = data.Creator;
                model.StartTime = data.StartTime;
                model.EndTime = data.EndTime;
                model.AuthorizationMode = data.AuthorizationMode;
                model.ParticipationMode = data.ParticipationMode;
                model.Status = data.Status;
            }

            return model;
        }
    }
}
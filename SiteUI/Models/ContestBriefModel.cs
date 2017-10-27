namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;
    using BITOJ.Core.Data;
    using Newtonsoft.Json;
    using System;

    public class ContestBriefModel
    {
        /// <summary>
        /// 获取或设置比赛ID。
        /// </summary>
        [JsonProperty("id")]
        public int ContestId { get; set; }

        /// <summary>
        /// 获取或设置比赛标题。
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置比赛的举办者。
        /// </summary>
        [JsonProperty("creator")]
        public string Creator { get; set; }

        /// <summary>
        /// 获取或设置比赛起始时间。
        /// </summary>
        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 获取或设置比赛终止时间
        /// </summary>
        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 获取或设置比赛的身份验证模式。
        /// </summary>
        [JsonProperty("authMode")]
        public ContestAuthorizationMode AuthorizationMode { get; set; }

        /// <summary>
        /// 获取或设置比赛的参与模式。
        /// </summary>
        [JsonProperty("partMode")]
        public ContestParticipationMode ParticipationMode { get; set; }

        /// <summary>
        /// 获取或设置比赛的当前执行状态。
        /// </summary>
        [JsonProperty("status")]
        public ContestStatus Status { get; set; }

        /// <summary>
        /// 创建 ContestBriefMode 类的新实例。
        /// </summary>
        [JsonConstructor]
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
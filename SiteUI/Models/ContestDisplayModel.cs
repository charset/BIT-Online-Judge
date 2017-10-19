namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;
    using BITOJ.Core.Data;
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// 对比赛呈现视图提供数据模型。
    /// </summary>
    public sealed class ContestDisplayModel
    {
        /// <summary>
        /// 获取或设置比赛 ID。
        /// </summary>
        [JsonProperty("id")]
        public int ContestId { get; set; }

        /// <summary>
        /// 获取或设置比赛标题。
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置比赛创建者。
        /// </summary>
        [JsonProperty("creator")]
        public string Creator { get; set; }

        /// <summary>
        /// 获取或设置比赛开始时间。
        /// </summary>
        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 获取或设置比赛结束时间。
        /// </summary>
        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 获取或设置比赛状态。
        /// </summary>
        [JsonProperty("status")]
        public ContestStatus Status { get; set; }

        /// <summary>
        /// 获取或设置比赛的参与模式。
        /// </summary>
        [JsonProperty("partMode")]
        public ContestParticipationMode ParticipationMode { get; set; }

        /// <summary>
        /// 获取或设置比赛的身份验证模式。
        /// </summary>
        [JsonProperty("autoMode")]
        public ContestAuthorizationMode AuthorizationMode { get; set; }

        /// <summary>
        /// 获取或设置比赛中包含的所有题目。
        /// </summary>
        [JsonProperty("problems")]
        public ProblemBriefModel[] Problems { get; set; }

        /// <summary>
        /// 创建 ContestDisplayModel 的新实例。
        /// </summary>
        [JsonConstructor]
        public ContestDisplayModel()
        {
            ContestId = 0;
            Title = string.Empty;
            Creator = string.Empty;
            StartTime = default(DateTime);
            EndTime = default(DateTime);
            Status = ContestStatus.Pending;
            ParticipationMode = ContestParticipationMode.Both;
            AuthorizationMode = ContestAuthorizationMode.Private;
            Problems = new ProblemBriefModel[0];
        }

        /// <summary>
        /// 从给定的比赛句柄创建 ContestDisplayMode 类的新实例。
        /// </summary>
        /// <param name="handle">比赛句柄。</param>
        /// <returns>从给定比赛创建的 ContestDisplayModel 对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static ContestDisplayModel FromContestHandle(ContestHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            ContestDisplayModel model = new ContestDisplayModel();
            using (ContestDataProvider data = ContestDataProvider.Create(handle, true))
            {
                model.ContestId = data.ContestId;
                model.Title = data.Title;
                model.Creator = data.Creator;
                model.StartTime = data.StartTime;
                model.EndTime = data.EndTime;
                model.Status = data.Status;
                model.AuthorizationMode = data.AuthorizationMode;
                model.ParticipationMode = data.ParticipationMode;

                ProblemHandle[] handles = data.GetProblems();
                model.Problems = new ProblemBriefModel[handles.Length];

                for (int i = 0; i < handles.Length; ++i)
                {
                    model.Problems[i] = ProblemBriefModel.FromProblemHandle(handles[i]);
                }
            }

            return model;
        }
    }
}
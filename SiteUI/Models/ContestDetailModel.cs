namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;
    using BITOJ.Core.Convert;
    using BITOJ.Core.Data;
    using BITOJ.SiteUI.Models.Validation;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    /// <summary>
    /// 为比赛提供详细数据模型。
    /// </summary>
    public class ContestDetailModel
    {
        /// <summary>
        /// 获取或设置比赛 ID。
        /// </summary>
        public int ContestId { get; set; }

        /// <summary>
        /// 获取或设置比赛标题。
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置比赛创建者。
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Creator is required.")]
        public string Creator { get; set; }

        /// <summary>
        /// 获取或设置比赛所需的最低身份权限集名称。
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [EqualTo("Administrator", "Insider", "Standard", "Guest", ErrorMessage = "User group is invalid.")]
        public string UsergroupName { get; set; }

        /// <summary>
        /// 获取或设置比赛的创建时间。
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 获取或设置比赛的开始时间的字符串表示。
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Start time is required.")]
        [RegularExpression(@"^\s*\d{4}-\d{2}-\d{2} [012]?\d:[0-5]\d:[0-5]\d\s*$", ErrorMessage = "Start time is invalid.")]
        public string StartTimeString { get; set; }

        /// <summary>
        /// 获取或设置比赛的结束时间。
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "End time is required.")]
        [RegularExpression(@"^\s*\d{4}-\d{2}-\d{2} [012]?\d:[0-5]\d:[0-5]\d\s*$", ErrorMessage = "End time is invalid.")]
        public string EndTimeString { get; set; }

        /// <summary>
        /// 获取或设置比赛的参与模式名称。
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Participation mode is required.")]
        [EqualTo("Individual only", "Teamwork only", "Individual and Teamwork")]
        public string ParticipationModeName { get; set; }

        /// <summary>
        /// 获取或设置比赛的身份验证模式。
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Authorization mode is required.")]
        [EqualTo("Public", "Protected", "Private", ErrorMessage = "Authorization mode is invalid.")]
        public string AuthorizationModeName { get; set; }

        /// <summary>
        /// 当身份验证模式为 Protected 时，获取或设置比赛的密码。
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 创建 ContestDetailModel 类的新实例。
        /// </summary>
        public ContestDetailModel()
        {
            ContestId = 0;
            Title = string.Empty;
            Creator = string.Empty;
            UsergroupName = UsergroupConvert.ConvertToString(UserGroup.Standard);
            CreationTime = DateTime.Now;
            StartTimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            EndTimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ParticipationModeName = ContestParticipationModeConvert.ConvertToString(ContestParticipationMode.Both);
            AuthorizationModeName = ContestAuthorizationModeConvert.ConvertToString(ContestAuthorizationMode.Protected);
            Password = string.Empty;
        }

        /// <summary>
        /// 将当前数据模型中的内容保存至给定的比赛。
        /// </summary>
        /// <param name="handle">比赛句柄。</param>
        /// <exception cref="ArgumentNullException"/>
        public void SaveTo(ContestHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            using (ContestDataProvider data = ContestDataProvider.Create(handle, false))
            {
                data.Title = Title;
                data.Creator = Creator;
                data.AuthorizationGroup = UsergroupConvert.ConvertFromString(UsergroupName);
                data.StartTime = DateTime.ParseExact(StartTimeString, "yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"));
                data.EndTime = DateTime.ParseExact(EndTimeString, "yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"));
                data.ParticipationMode = ContestParticipationModeConvert.ConvertFromString(ParticipationModeName);
                data.AuthorizationMode = ContestAuthorizationModeConvert.ConvertFromString(AuthorizationModeName);
            }
        }

        /// <summary>
        /// 从给定的比赛句柄创建 ContestDetailModel 类的新实例。
        /// </summary>
        /// <param name="handle">比赛句柄。</param>
        /// <returns>从给定的比赛句柄创建的比赛详细信息数据模型对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static ContestDetailModel FromContestHandle(ContestHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            ContestDetailModel model = new ContestDetailModel();
            using (ContestDataProvider data = ContestDataProvider.Create(handle, true))
            {
                model.ContestId = data.ContestId;
                model.Title = data.Title;
                model.Creator = data.Creator;
                model.UsergroupName = UsergroupConvert.ConvertToString(data.AuthorizationGroup);
                model.CreationTime = data.CreationTime;
                model.StartTimeString = data.CreationTime.ToString("yyyy-MM-dd HH:mm:ss");
                model.EndTimeString = data.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                model.ParticipationModeName = ContestParticipationModeConvert.ConvertToString(data.ParticipationMode);
            }

            return model;
        }
    }
}
namespace BITOJ.SiteUI.Models
{
    using BITOJ.Core;
    using BITOJ.Core.Convert;
    using BITOJ.Core.Data;
    using System;
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
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置比赛创建者。
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 获取或设置比赛所需的最低身份权限集名称。
        /// </summary>
        public string UsergroupName { get; set; }

        /// <summary>
        /// 获取或设置比赛的创建时间。
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 获取或设置比赛的开始时间的字符串表示。
        /// </summary>
        public string StartTimeString { get; set; }

        /// <summary>
        /// 获取或设置比赛的结束时间。
        /// </summary>
        public string EndTimeString { get; set; }

        /// <summary>
        /// 获取或设置比赛的参与模式名称。
        /// </summary>
        public string ParticipationModeName { get; set; }

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
        }

        /// <summary>
        /// 验证数据模型有效性。
        /// </summary>
        /// <param name="errors">用于存储错误信息的动态对象。</param>
        /// <returns>一个值，该值指示数据验证是否通过。</returns>
        public bool Validate(dynamic errors)
        {
            bool pass = true;

            if (string.IsNullOrEmpty(Title))
            {
                pass = false;
                if (errors != null)
                {
                    errors.TitleErrorMessage = "Title is required.";
                }
            }

            if (string.IsNullOrEmpty(Creator))
            {
                pass = false;
                if (errors != null)
                {
                    errors.CreatorErrorMessage = "Creator is required.";
                }
            }

            if (string.IsNullOrEmpty(UsergroupName))
            {
                pass = false;
                if (errors != null)
                {
                    errors.UsergroupNameErrorMessage = "Usergroup is required.";
                }
            }
            else
            {
                try
                {
                    UsergroupConvert.ConvertFromString(UsergroupName);
                }
                catch (ArgumentException)
                {
                    pass = false;
                    if (errors != null)
                    {
                        errors.UsergroupNameErrorMessage = "Usergroup is invalid.";
                    }
                }
            }

            DateTime startTime;
            if (!DateTime.TryParseExact(StartTimeString, "yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"),
                DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal, out startTime))
            {
                pass = false;
                errors.StartTimeStringErrorMessage = "Start time is invalid.";
            }

            DateTime endTime;
            if (!DateTime.TryParseExact(EndTimeString, "yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"),
                DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal, out endTime))
            {
                pass = false;
                errors.EndTimeStringErrorMessage = "End time is invalid.";
            }

            if (endTime <= startTime)
            {
                pass = false;
                errors.EndTimeStringErrorMessage = "End time must be later than start time.";
            }

            if (string.IsNullOrEmpty(ParticipationModeName))
            {
                pass = false;
                errors.ParticipationModeNameErrorMessage = "Participation mode is required.";
            }
            else
            {
                try
                {
                    ContestParticipationModeConvert.ConvertFromString(ParticipationModeName);
                }
                catch (ArgumentException)
                {
                    errors.ParticipationModeNameErrorMessage = "Participation mode is invalid.";
                }
            }

            return pass;
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
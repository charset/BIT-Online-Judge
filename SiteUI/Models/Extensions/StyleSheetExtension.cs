namespace BITOJ.SiteUI.Models.Extensions
{
    using BITOJ.Core;
    using System;

    /// <summary>
    /// 为 CSS 样式表提供拓展方法。
    /// </summary>
    public static class StyleSheetExtension
    {
        /// <summary>
        /// 获取比赛授权模式的样式表类。
        /// </summary>
        /// <param name="authorizationMode">比赛授权模式。</param>
        /// <returns>比赛授权模式的样式表类。</returns>
        /// <exception cref="ArgumentException"/>
        public static string GetContestAuthorizationModeClass(ContestAuthorizationMode authorizationMode)
        {
            switch (authorizationMode)
            {
                case ContestAuthorizationMode.Private:
                    return "text-contest-private";

                case ContestAuthorizationMode.Protected:
                    return "text-contest-protected";

                case ContestAuthorizationMode.Public:
                    return "text-contest-public";

                default:
                    throw new ArgumentException("传入的 ContestAuthorizationMode 枚举值无效。");
            }
        }

        /// <summary>
        /// 获取比赛状态的样式表类。
        /// </summary>
        /// <param name="status">比赛状态。</param>
        /// <returns>比赛状态的样式表类。</returns>
        /// <exception cref="ArgumentException"/>
        public static string GetContestStatusClass(ContestStatus status)
        {
            switch (status)
            {
                case ContestStatus.Pending:
                    return "text-contest-pending";

                case ContestStatus.Running:
                    return "text-contest-running";

                case ContestStatus.Ended:
                    return "text-contest-ended";

                default:
                    throw new ArgumentException("传入的 ContestStatus 枚举值无效。");
            }
        }
    }
}
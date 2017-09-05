namespace BITOJ.Core.Convert
{
    using System;

    /// <summary>
    /// 提供 ContestParticipationMode 枚举与字符串的转换操作。
    /// </summary>
    public static class ContestParticipationModeConvert
    {
        private const string IndividualOnlyString = "Individual only";
        private const string TeamworkOnlyString = "Teamwork only";
        private const string BothString = "Individual and Teamwork";

        /// <summary>
        /// 将给定的 ContestParticipationMode 枚举值转换为对应的字符串。
        /// </summary>
        /// <param name="value">要转换的枚举值。</param>
        /// <returns>对应的字符串表示。</returns>
        /// <exception cref="ArgumentException"/>
        public static string ConvertToString(ContestParticipationMode value)
        {
            switch (value)
            {
                case ContestParticipationMode.IndividualOnly:
                    return IndividualOnlyString;
                case ContestParticipationMode.TeamworkOnly:
                    return TeamworkOnlyString;
                case ContestParticipationMode.Both:
                    return BothString;
                default:
                    throw new ArgumentException("传入的 ContestParticipationMode 枚举值不合法。");
            }
        }

        /// <summary>
        /// 从给定的字符串中转换出对应的 ContestParticipationMode 枚举值。
        /// </summary>
        /// <param name="str">字符串表示。</param>
        /// <returns>转换出的 ContestParticipationMode 枚举值。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static ContestParticipationMode ConvertFromString(string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            if (string.Compare(str, IndividualOnlyString, true) == 0)
            {
                return ContestParticipationMode.IndividualOnly;
            }
            else if (string.Compare(str, TeamworkOnlyString, true) == 0)
            {
                return ContestParticipationMode.TeamworkOnly;
            }
            else if (string.Compare(str, BothString, true) == 0)
            {
                return ContestParticipationMode.Both;
            }
            else
            {
                throw new ArgumentException("传入的字符串值不合法。");
            }
        }
    }
}

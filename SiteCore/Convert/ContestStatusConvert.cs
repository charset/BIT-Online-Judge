namespace BITOJ.Core.Convert
{
    using System;

    /// <summary>
    /// 提供对 ContestStatus 枚举值及其相应的字符串表示之间的转换操作。
    /// </summary>
    public static class ContestStatusConvert
    {
        private const string PendingString = "Pending";
        private const string RunningString = "Running";
        private const string EndedString = "Ended";

        /// <summary>
        /// 将给定的 ContestStatus 枚举值转换为与其对应的字符串表示。
        /// </summary>
        /// <param name="value">ContestStatus 枚举值。</param>
        /// <returns>对应 ContestStatus 枚举值的字符串表示。</returns>
        /// <exception cref="ArgumentException"/>
        public static string ConvertToString(ContestStatus value)
        {
            switch (value)
            {
                case ContestStatus.Pending:
                    return PendingString;
                case ContestStatus.Running:
                    return RunningString;
                case ContestStatus.Ended:
                    return EndedString;
                default:
                    throw new ArgumentException("传入的 ContestStatus 枚举值不合法。");
            }
        }

        /// <summary>
        /// 将给定的字符串转换为对应的 ContestStatus 枚举值。
        /// </summary>
        /// <param name="str">字符串。</param>
        /// <returns>对应的 ContestStatus 枚举值。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static ContestStatus ConvertFromString(string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            if (string.Compare(str, PendingString, true) == 0)
            {
                return ContestStatus.Pending;
            }
            else if (string.Compare(str, RunningString, true) == 0)
            {
                return ContestStatus.Running;
            }
            else if (string.Compare(str, EndedString, true) == 0)
            {
                return ContestStatus.Ended;
            }
            else
            {
                throw new ArgumentException("传入的字符串不合法。");
            }
        }
    }
}

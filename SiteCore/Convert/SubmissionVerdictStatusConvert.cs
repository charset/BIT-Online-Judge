namespace BITOJ.Core.Convert
{
    using System;

    /// <summary>
    /// 提供 SubmissionVerdictStatus 枚举值到字符串的转换。
    /// </summary>
    public static class SubmissionVerdictStatusConvert
    {
        private const string PendingString = "Pending";
        private const string SubmittedString = "Submitted";
        private const string CompilingString = "Compiling";
        private const string RunningString = "Running";
        private const string CompletedString = "Completed";

        /// <summary>
        /// 将给定的 SubmissionVerdictStatus 枚举值转换为对应的字符串值。
        /// </summary>
        /// <param name="value">SubmissionVerdictStatus 枚举值。</param>
        /// <returns>转换后的字符串值。</returns>
        public static string ConvertToString(SubmissionVerdictStatus value)
        {
            switch (value)
            {
                case SubmissionVerdictStatus.Pending:
                    return PendingString;
                case SubmissionVerdictStatus.Submitted:
                    return SubmittedString;
                case SubmissionVerdictStatus.Compiling:
                    return CompilingString;
                case SubmissionVerdictStatus.Running:
                    return RunningString;
                case SubmissionVerdictStatus.Completed:
                    return CompletedString;
                default:
                    throw new ArgumentException("传入的 SubmissionVerdictStatus 枚举值不合法。");
            }
        }

        /// <summary>
        /// 从给定的字符串提取出 SubmissionVerdictStatus 枚举值。
        /// </summary>
        /// <param name="value">字符串值。</param>
        /// <returns>从给定字符串中提取出的 SubmissionVerdictStatus 枚举值。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static SubmissionVerdictStatus ConvertFromString(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (string.Compare(value, PendingString, true) == 0)
                return SubmissionVerdictStatus.Pending;
            else if (string.Compare(value, SubmittedString, true) == 0)
                return SubmissionVerdictStatus.Submitted;
            else if (string.Compare(value, CompilingString, true) == 0)
                return SubmissionVerdictStatus.Compiling;
            else if (string.Compare(value, RunningString, true) == 0)
                return SubmissionVerdictStatus.Running;
            else if (string.Compare(value, CompletedString, true) == 0)
                return SubmissionVerdictStatus.Completed;
            else
                throw new ArgumentException("传入的字符串无效。");
        }
    }
}

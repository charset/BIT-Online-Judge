namespace BITOJ.Core.Convert
{
    using BITOJ.Core;
    using System;

    /// <summary>
    /// 为 SubmissionVerdict 枚举值提供到字符串的转换操作。
    /// </summary>
    public static class SubmissionVerdictConvert
    {
        private const string UnknownString = "Unknown";
        private const string AcceptedString = "Accepted";
        private const string WrongAnswerString = "Wrong answer";
        private const string RuntimeErrorString = "Runtime error";
        private const string TimeLimitExceededString = "Time limit exceeded";
        private const string MemoryLimitExceededString = "Memory limit exceeded";
        private const string OutputLimitExceededString = "Output limit exceeded";
        private const string PresentationErrorString = "Presentation error";
        private const string CompilationErrorString = "Compilation error";
        private const string SystemErrorString = "System error";

        /// <summary>
        /// 将给定的 SubmissionVerdict 枚举值转换为其对应的字符串值。
        /// </summary>
        /// <param name="value">SubmissionVerdict 枚举值。</param>
        /// <returns>给定的枚举值所对应的字符串值。</returns>
        /// <exception cref="ArgumentException"/>
        public static string ConvertToString(SubmissionVerdict value)
        {
            switch (value)
            {
                case SubmissionVerdict.Unknown:
                    return UnknownString;
                case SubmissionVerdict.Accepted:
                    return AcceptedString;
                case SubmissionVerdict.WrongAnswer:
                    return WrongAnswerString;
                case SubmissionVerdict.RuntimeError:
                    return RuntimeErrorString;
                case SubmissionVerdict.TimeLimitExceeded:
                    return TimeLimitExceededString;
                case SubmissionVerdict.MemoryLimitExceeded:
                    return MemoryLimitExceededString;
                case SubmissionVerdict.OutputLimitExceeded:
                    return OutputLimitExceededString;
                case SubmissionVerdict.PresentationError:
                    return PresentationErrorString;
                case SubmissionVerdict.CompilationError:
                    return CompilationErrorString;
                case SubmissionVerdict.SystemError:
                    return SystemErrorString;
                default:
                    throw new ArgumentException("传入的 SubmissionVerdict 枚举值非法。");
            }
        }

        /// <summary>
        /// 从给定的字符串中解析出 SubmissionVerdict 枚举值。
        /// </summary>
        /// <param name="value">字符串值。</param>
        /// <returns>从给定字符串值中解析出的 SubmissionVerdict 枚举值。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static SubmissionVerdict ConvertFromString(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (string.Compare(value, UnknownString, true) == 0)
                return SubmissionVerdict.Unknown;
            else if (string.Compare(value, AcceptedString, true) == 0)
                return SubmissionVerdict.Accepted;
            else if (string.Compare(value, WrongAnswerString, true) == 0)
                return SubmissionVerdict.WrongAnswer;
            else if (string.Compare(value, RuntimeErrorString, true) == 0)
                return SubmissionVerdict.RuntimeError;
            else if (string.Compare(value, TimeLimitExceededString, true) == 0)
                return SubmissionVerdict.TimeLimitExceeded;
            else if (string.Compare(value, MemoryLimitExceededString, true) == 0)
                return SubmissionVerdict.MemoryLimitExceeded;
            else if (string.Compare(value, OutputLimitExceededString, true) == 0)
                return SubmissionVerdict.OutputLimitExceeded;
            else if (string.Compare(value, PresentationErrorString, true) == 0)
                return SubmissionVerdict.PresentationError;
            else if (string.Compare(value, CompilationErrorString, true) == 0)
                return SubmissionVerdict.CompilationError;
            else if (string.Compare(value, SystemErrorString, true) == 0)
                return SubmissionVerdict.SystemError;
            else
                throw new ArgumentException("传入的字符串不合法。");
        }
    }
}

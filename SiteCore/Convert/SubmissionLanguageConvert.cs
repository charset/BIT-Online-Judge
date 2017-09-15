namespace BITOJ.Core.Convert
{
    using BITOJ.Core;
    using System;

    /// <summary>
    /// 为 SubmissionLanguage 枚举值提供到字符串的转换操作。
    /// </summary>
    public static class SubmissionLanguageConvert
    {
        private const string GnuCString = "GNU C";
        private const string GnuCPlusPlusString = "GNU C++";
        private const string GnuCPlusPlus11String = "GNU C++ 11";
        private const string GnuCPlusPlus14String = "GNU C++ 14";
        private const string GnuCPlusPlus17String = "GNU C++ 17";
        private const string MicrosoftCPlusPlusString = "Microsoft Visual C++";
        private const string JavaString = "Java";
        private const string PascalString = "Pascal";
        private const string Python2String = "Python 2";
        private const string Python3String = "Python 3";

        /// <summary>
        /// 将给定的 SubmissionLanguage 枚举值转换为对应的字符串。
        /// </summary>
        /// <param name="value">要转换的 SubmissionLanguage 枚举值。</param>
        /// <returns>转换后的字符串。</returns>
        /// <exception cref="ArgumentException"/>
        public static string ConvertToString(SubmissionLanguage value)
        {
            switch (value)
            {
                case SubmissionLanguage.GnuC:
                    return GnuCString;

                case SubmissionLanguage.GnuCPlusPlus:
                    return GnuCPlusPlusString;

                case SubmissionLanguage.GnuCPlusPlus11:
                    return GnuCPlusPlus11String;

                case SubmissionLanguage.GnuCPlusPlus14:
                    return GnuCPlusPlus14String;

                case SubmissionLanguage.GnuCPlusPlus17:
                    return GnuCPlusPlus17String;

                case SubmissionLanguage.MicrosoftCPlusPlus:
                    return MicrosoftCPlusPlusString;

                case SubmissionLanguage.Java:
                    return JavaString;

                case SubmissionLanguage.Pascal:
                    return PascalString;

                case SubmissionLanguage.Python2:
                    return Python2String;

                case SubmissionLanguage.Python3:
                    return Python3String;

                default:
                    throw new ArgumentException("给定的 SubmissionLanguage 枚举值无效。");
            }
        }

        /// <summary>
        /// 从给定的字符串形式解析出 SubmissionLanguage 枚举。
        /// </summary>
        /// <param name="value">字符串。</param>
        /// <returns>从给定字符串解析出的 SubmissionLanguage 枚举值。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static SubmissionLanguage ConvertFromString(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (string.Compare(value, GnuCString, true) == 0)
                return SubmissionLanguage.GnuC;
            else if (string.Compare(value, GnuCPlusPlusString, true) == 0)
                return SubmissionLanguage.GnuCPlusPlus;
            else if (string.Compare(value, GnuCPlusPlus11String, true) == 0)
                return SubmissionLanguage.GnuCPlusPlus11;
            else if (string.Compare(value, GnuCPlusPlus14String, true) == 0)
                return SubmissionLanguage.GnuCPlusPlus14;
            else if (string.Compare(value, GnuCPlusPlus17String, true) == 0)
                return SubmissionLanguage.GnuCPlusPlus17;
            else if (string.Compare(value, JavaString, true) == 0)
                return SubmissionLanguage.Java;
            else if (string.Compare(value, PascalString, true) == 0)
                return SubmissionLanguage.Pascal;
            else if (string.Compare(value, Python2String, true) == 0)
                return SubmissionLanguage.Python2;
            else if (string.Compare(value, Python3String, true) == 0)
                return SubmissionLanguage.Python3;
            else
                throw new ArgumentException("传入的字符串无效。");
        }
    }
}

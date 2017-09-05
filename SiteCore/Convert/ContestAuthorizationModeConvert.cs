namespace BITOJ.Core.Convert
{
    using BITOJ.Core;
    using System;

    /// <summary>
    /// 提供对 ContestAuthorizationMode 枚举与字符串之间的转换操作。
    /// </summary>
    public static class ContestAuthorizationModeConvert
    {
        private const string PublicString = "Public";
        private const string ProtectedString = "Protected";
        private const string PrivateString = "Private";

        /// <summary>
        /// 将 ContestAuthorizationMode 枚举转换为对应的字符串。
        /// </summary>
        /// <param name="value">要转换的 ContestAuthorizationMode 枚举值。</param>
        /// <returns>转换后的字符串。</returns>
        /// <exception cref="ArgumentException"/>
        public static string ConvertToString(ContestAuthorizationMode value)
        {
            switch (value)
            {
                case ContestAuthorizationMode.Private:
                    return PrivateString;
                case ContestAuthorizationMode.Protected:
                    return ProtectedString;
                case ContestAuthorizationMode.Public:
                    return PublicString;
                default:
                    throw new ArgumentException("无效的 ContestAuthorizationMode 枚举值。");
            }
        }

        /// <summary>
        /// 将 ContestAuthorizationMode 枚举值的字符串表示转换为其原始枚举值。
        /// </summary>
        /// <param name="value">要转换的字符串表示。</param>
        /// <returns>转换后的枚举值。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static ContestAuthorizationMode ConvertFromString(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            if (string.Compare(value, PublicString, true) == 0)
            {
                return ContestAuthorizationMode.Public;
            }
            else if (string.Compare(value, ProtectedString, true) == 0)
            {
                return ContestAuthorizationMode.Protected;
            }
            else if (string.Compare(value, PrivateString, true) == 0)
            {
                return ContestAuthorizationMode.Public;
            }
            else
            {
                throw new ArgumentException("无效的参数。");
            }
        }
    }
}

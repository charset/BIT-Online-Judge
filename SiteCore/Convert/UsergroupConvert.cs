namespace BITOJ.Core.Convert
{
    using BITOJ.Core;
    using System;

    /// <summary>
    /// 提供 UserGroup 枚举类型与其他类型的相互转换。
    /// </summary>
    public static class UsergroupConvert
    {
        private const string AdministratorString = "Administrator";
        private const string InsiderString = "Insider";
        private const string StandardString = "Standard";
        private const string GuestString = "Guest";

        /// <summary>
        /// 将 UserGroup 枚举值转换至其相应的字符串表示。
        /// </summary>
        /// <param name="value">要转换的枚举值。</param>
        /// <returns>传入枚举值的字符串表示。</returns>
        /// <exception cref="ArgumentException"/>
        public static string ConvertToString(UserGroup value)
        {
            switch (value)
            {
                case UserGroup.Administrators:
                    return AdministratorString;
                case UserGroup.Insiders:
                    return InsiderString;
                case UserGroup.Standard:
                    return StandardString;
                case UserGroup.Guests:
                    return GuestString;
                default:
                    throw new ArgumentException("传入的 UserGroup 枚举非法。");
            }
        }

        /// <summary>
        /// 从枚举值的字符串表示中转换出 UserGroup 枚举值。
        /// </summary>
        /// <param name="str">枚举值的字符串表示。</param>
        /// <returns>枚举值。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static UserGroup ConvertFromString(string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            if (string.Compare(str, AdministratorString, true) == 0)
                return UserGroup.Administrators;
            else if (string.Compare(str, InsiderString, true) == 0)
                return UserGroup.Insiders;
            else if (string.Compare(str, StandardString, true) == 0)
                return UserGroup.Standard;
            else if (string.Compare(str, GuestString, true) == 0)
                return UserGroup.Guests;
            else
                throw new ArgumentException("传入的字符串不合法。");
        }
    }
}

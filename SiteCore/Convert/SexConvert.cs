namespace BITOJ.Core.Convert
{
    using BITOJ.Core;
    using System;

    /// <summary>
    /// 提供 UserSex 枚举类型与其他类型之间的相互转换。
    /// </summary>
    public static class SexConvert
    {
        private const string FemaleString = "Female";
        private const string MaleString = "Male";

        /// <summary>
        /// 将 UserSex 枚举值转换为其对应的字符串表示。
        /// </summary>
        /// <param name="value">UserSex 枚举值。</param>
        /// <returns>枚举值对应的字符串表示。</returns>
        /// <exception cref="ArgumentException"/>
        public static string ConvertToString(UserSex value)
        {
            switch (value)
            {
                case UserSex.Female:
                    return FemaleString;
                case UserSex.Male:
                    return MaleString;
                default:
                    throw new ArgumentException("传入的 UserSex 枚举值非法。");
            }
        }

        /// <summary>
        /// 从字符串转换为其对应的 UserSex 枚举值。
        /// </summary>
        /// <param name="str">字符串。</param>
        /// <returns>UserSex 枚举值。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="FormatException"/>
        public static UserSex ConvertFromString(string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            if (string.Compare(str, FemaleString, true) == 0)
                return UserSex.Female;
            else if (string.Compare(str, MaleString, true) == 0)
                return UserSex.Male;
            else
                throw new FormatException("传入的字符串格式不正确。");
        }
    }
}

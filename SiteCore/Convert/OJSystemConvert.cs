namespace BITOJ.Core.Convert
{
    using System;

    /// <summary>
    /// 提供从 OJSystem 枚举到其他类型的转换操作。
    /// </summary>
    public static class OJSystemConvert
    {
        private const string BitString = "BIT";
        private const string BzojString = "BZOJ";
        private const string CodeForcesString = "CF";
        private const string GymString = "GYM";
        private const string HduString = "HDU";
        private const string PojString = "POJ";
        private const string UvaString = "UVa";
        private const string UvaLaString = "LA";
        private const string ZojString = "ZOJ";

        /// <summary>
        /// 将 OJSystem 枚举值转换为其对应的字符串表示。
        /// </summary>
        /// <param name="value">要转换的 OJSystem 枚举值。</param>
        /// <returns>对应的字符串表示。</returns>
        /// <exception cref="ArgumentException"/>
        public static string ConvertToString(OJSystem value)
        {
            switch (value)
            {
                case OJSystem.BIT:
                    return BitString;
                case OJSystem.BZOJ:
                    return BzojString;
                case OJSystem.CodeForces:
                    return CodeForcesString;
                case OJSystem.Gym:
                    return GymString;
                case OJSystem.HDU:
                    return HduString;
                case OJSystem.POJ:
                    return PojString;
                case OJSystem.UVa:
                    return UvaString;
                case OJSystem.UVaLiveArchieve:
                    return UvaLaString;
                case OJSystem.ZOJ:
                    return ZojString;

                default:
                    throw new ArgumentException("传入的 OJSystem 枚举值非法。");
            }
        }

        /// <summary>
        /// 从给定的字符串解析 OJSystem 枚举值。
        /// </summary>
        /// <param name="str">包含 OJSystem 字符串表示的字符串。</param>
        /// <returns>对应的 OJSystem 枚举值。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public static OJSystem ConvertFromString(string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));

            if (string.Compare(str, BitString, true) == 0)
                return OJSystem.BIT;
            else if (string.Compare(str, BzojString, true) == 0)
                return OJSystem.BZOJ;
            else if (string.Compare(str, CodeForcesString, true) == 0)
                return OJSystem.CodeForces;
            else if (string.Compare(str, GymString, true) == 0)
                return OJSystem.Gym;
            else if (string.Compare(str, HduString, true) == 0)
                return OJSystem.HDU;
            else if (string.Compare(str, PojString, true) == 0)
                return OJSystem.POJ;
            else if (string.Compare(str, UvaString, true) == 0)
                return OJSystem.UVa;
            else if (string.Compare(str, UvaLaString, true) == 0)
                return OJSystem.UVaLiveArchieve;
            else if (string.Compare(str, ZojString, true) == 0)
                return OJSystem.ZOJ;
            else
                throw new FormatException("传入的字符串不包含有效的 OJSystem 枚举。");
        }
    }
}
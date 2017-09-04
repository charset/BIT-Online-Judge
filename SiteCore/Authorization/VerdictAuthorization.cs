namespace BITOJ.Core.Authorization
{
    using BITOJ.Common.Cache.Settings;
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// 为评测主机提供身份验证服务。
    /// </summary>
    public static class VerdictAuthorization
    {
        private static readonly int[] MonthDays = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        private static readonly string Password;
        private const string PasswordSettingName = "verdict_password";

        static VerdictAuthorization()
        {
            for (int i = 1; i < MonthDays.Length; ++i)
            {
                MonthDays[i] += MonthDays[i - 1];
            }

            // 从配置文件中加载客户端验证密钥。
            FileSystemSettingProvider setting = new FileSystemSettingProvider();
            if (setting.Contains(PasswordSettingName))
            {
                Password = setting.Get<string>(PasswordSettingName);
            }
            else
            {
                Password = string.Empty;
            }
        }

        private static bool IsLeapYear(int year)
        {
            return ((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0));
        }

        /// <summary>
        /// 获取当前时刻的哈希密钥。
        /// </summary>
        /// <returns>当前时刻的哈希密钥。</returns>
        private static byte[] GetHashKey()
        {
            DateTime date = DateTime.Now;
            int days = (date.Year - 1) * 365 + (date.Year - 1) / 4 - (date.Year - 1) / 100 + (date.Year - 1) / 400
                + MonthDays[date.Month - 1] + date.Day;

            if (IsLeapYear(date.Year) && date.Month >= 3)
            {
                days++;
            }

            return BitConverter.GetBytes(days);
        }

        /// <summary>
        /// 解析给定的十六进制数字位。
        /// </summary>
        /// <param name="digit">十六进制数字位。</param>
        /// <returns>解析出的 byte 数据格式。</returns>
        /// <exception cref="FormatException"/>
        private static byte ParseHexDigit(char digit)
        {
            if (char.IsDigit(digit))
            {
                return unchecked((byte)(digit - '0'));
            }
            else if (digit >= 'A' && digit <= 'F')
            {
                return unchecked((byte)(digit - 'A' + 10));
            }
            else if (digit >= 'a' && digit <= 'f')
            {
                return unchecked((byte)(digit - 'a' + 10));
            }
            else
            {
                throw new FormatException();
            }
        }

        /// <summary>
        /// 检查客户端密钥是否正确。
        /// </summary>
        /// <param name="agentHash">客户端密钥哈希。</param>
        /// <returns>一个值，该值指示客户端密钥是否正确。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static bool CheckAuthorization(byte[] agentHash)
        {
            // 计算密钥哈希。
            using (HMACSHA512 hash = new HMACSHA512(GetHashKey()))
            {
                byte[] expectedHash = hash.ComputeHash(Encoding.Unicode.GetBytes(Password));
                return Buffer.IsByteArraysEqual(agentHash, expectedHash);
            }
        }

        /// <summary>
        /// 从给定的哈希字符串中解析出相应的哈希数据。
        /// </summary>
        /// <param name="hashStr">哈希字符串。</param>
        /// <returns>从哈希字符串中解析出的哈希数据。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="FormatException"/>
        public static byte[] GetHashBytes(string hashStr)
        {
            if (hashStr == null)
                throw new ArgumentNullException(nameof(hashStr));

            byte[] buffer = null;
            using (MemoryStream stream = new MemoryStream())
            {
                // 小端编码。从字符串尾开始向前解析。
                int ptr = Math.Max(hashStr.Length - 2, 0);
                while (ptr >= 0)
                {
                    unchecked
                    {
                        stream.WriteByte((byte)((ParseHexDigit(hashStr[ptr]) << 4) | ParseHexDigit(hashStr[ptr + 1])));
                    }

                    ptr -= 2;
                }
                if (ptr == -1)
                {
                    stream.WriteByte(ParseHexDigit(hashStr[0]));
                }

                buffer = new byte[stream.Length];
                stream.GetBuffer().CopyTo(buffer, 0);
            }

            return buffer;
        }
    }
}

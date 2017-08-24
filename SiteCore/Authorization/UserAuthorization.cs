namespace BITOJ.Core.Authorization
{
    using BITOJ.Core;
    using BITOJ.Core.Data;
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// 对用户权限提供认证服务。
    /// </summary>
    public static class UserAuthorization
    {
        private static readonly HashAlgorithm ms_hash;
        private static readonly Encoding ms_encoding;

        static UserAuthorization()
        {
            // 使用 SHA512 哈希算法对用户密码提取数字指纹。
            ms_hash = SHA512.Create();
            // 使用 Unicode 编码对用户密码的字符串格式进行编码。
            ms_encoding = Encoding.Unicode;
        }

        private static bool IsByteArraysEqual(byte[] array1, byte[] array2)
        {
            if (array1 == null && array2 == null)
            {
                return true;
            }
            else if (array1 == null || array2 == null)
            {
                return false;
            }
            else if (array1.Length != array2.Length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < array1.Length; ++i)
                {
                    if (array1[i] != array2[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 检查用户登录验证信息是否正确。
        /// </summary>
        /// <param name="username">用户名。</param>
        /// <param name="password">密码。</param>
        /// <returns>一个值，该值指示用户登录验证信息是否正确。</returns>
        public static bool CheckAuthorization(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            UserHandle handle = UserManager.Default.QueryUserByName(username);
            if (handle == null)
            {
                // 用户数据库中没有对应的信息。
                return false;
            }

            // 计算传入的密码的哈希值。
            byte[] pwdHash = GetPasswordHash(password);

            using (UserDataProvider userData = UserDataProvider.Create(handle, true))
            {
                // 比较密码哈希值是否相同。
                return IsByteArraysEqual(pwdHash, userData.PasswordHash);
            }
        }

        /// <summary>
        /// 更新用户密码信息。
        /// </summary>
        /// <param name="username">用户名。</param>
        /// <param name="password">用户密码。</param>
        /// <exception cref="ArgumentNullException"/>
        public static void UpdatePassword(string username, string password)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            UserHandle handle = UserManager.Default.QueryUserByName(username);
            if (handle == null)
            {
                // 数据库中没有对应用户的实体。
                return;
            }

            byte[] hash = GetPasswordHash(password);
            using (UserDataProvider data = UserDataProvider.Create(handle, false))
            {
                // 更新密码哈希值。
                data.PasswordHash = hash;
            }
        }

        /// <summary>
        /// 检查用户是否有权限执行某个操作。
        /// </summary>
        /// <param name="expected">执行操作所需最低权限。</param>
        /// <param name="user">用户具有的权限。</param>
        /// <returns>一个值，指示用户是否有足够的权限执行操作。</returns>
        public static bool CheckAccessRights(UserGroup expected, UserGroup user)
        {
            return (int)user >= (int)expected;
        }

        /// <summary>
        /// 获取指定密码的哈希值。
        /// </summary>
        /// <param name="password">密码。</param>
        /// <returns>给定密码的哈希值。</returns>
        /// <exception cref="ArgumentNullException"/>
        private static byte[] GetPasswordHash(string password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            return ms_hash.ComputeHash(ms_encoding.GetBytes(password));
        }
    }
}

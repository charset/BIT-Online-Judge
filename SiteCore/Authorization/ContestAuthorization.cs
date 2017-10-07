namespace BITOJ.Core.Authorization
{
    using BITOJ.Core.Data;
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// 为比赛提供身份验证服务。
    /// </summary>
    public static class ContestAuthorization
    {
        private static readonly HashAlgorithm PasswordHashAlgorithm;
        private static readonly Encoding PasswordEncoding;

        static ContestAuthorization()
        {
            PasswordHashAlgorithm = SHA512.Create();
            PasswordEncoding = Encoding.Unicode;
        }

        /// <summary>
        /// 获取给定用户在给定比赛中的授权状态。
        /// </summary>
        /// <param name="contest">比赛句柄。</param>
        /// <param name="user">用户句柄。</param>
        /// <returns>给定用户在给定比赛中的的授权状态。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static ContestAuthorizationState GetAuthorizationState(ContestHandle contest, UserHandle user)
        {
            if (contest == null)
                throw new ArgumentNullException(nameof(contest));
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!ContestManager.Default.IsContestExist(contest.ContestId))
            {
                return ContestAuthorizationState.AuthorizationFailed;
            }
            if (!UserManager.Default.IsUserExist(user.Username))
            {
                return ContestAuthorizationState.AuthorizationFailed;
            }

            using (ContestDataProvider data = ContestDataProvider.Create(contest, true))
            {
                if (string.Compare(data.Creator, user.Username, false) == 0)
                {
                    // 比赛创建者总是授权用户。
                    return ContestAuthorizationState.Authorized;
                }

                // 检查用户身份是否满足参加比赛的最低要求身份。
                if (!UserAuthorization.CheckAccessRights(data.AuthorizationGroup, user))
                {
                    return ContestAuthorizationState.AuthorizationFailed;
                }

                if (data.AuthorizationMode == ContestAuthorizationMode.Public)
                {
                    // Public 比赛总是授权通过。
                    return ContestAuthorizationState.Authorized;
                }

                foreach (UserHandle authorized in data.GetAuthorizedUsers())
                {
                    if (authorized == user)
                    {
                        return ContestAuthorizationState.Authorized;
                    }
                }

                // 给定用户未通过授权。检查比赛是否支持动态授权。
                if (data.AuthorizationMode == ContestAuthorizationMode.Protected)
                {
                    return ContestAuthorizationState.AuthorizationRequired;
                }
                else        // data.AuthorizationMode == ContestAuthorizationMode.Private
                {
                    return ContestAuthorizationState.AuthorizationFailed;
                }
            }
        }

        /// <summary>
        /// 获取给定用户对给定比赛的数据访问权限。
        /// </summary>
        /// <param name="contest">比赛句柄。</param>
        /// <param name="user">用户句柄。</param>
        /// <returns>给定用户对给定比赛的数据访问权限。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static DataAccess GetAccess(ContestHandle contest, UserHandle user)
        {
            if (contest == null)
                throw new ArgumentNullException(nameof(contest));
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!ContestManager.Default.IsContestExist(contest.ContestId))
            {
                return DataAccess.None;
            }
            if (!UserManager.Default.IsUserExist(user.Username))
            {
                return DataAccess.None;
            }

            if (GetAuthorizationState(contest, user) != ContestAuthorizationState.Authorized)
            {
                return DataAccess.None;
            }
            else
            {
                if (UserAuthorization.GetUserGroup(user) == UserGroup.Administrators)
                {
                    return DataAccess.ReadWrite;
                }
                else
                {
                    using (ContestDataProvider data = ContestDataProvider.Create(contest, true))
                    {
                        if (string.Compare(data.Creator, user.Username, false) == 0)
                        {
                            return DataAccess.ReadWrite;
                        }
                        else
                        {
                            return DataAccess.Read;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 更新比赛密码。
        /// </summary>
        /// <param name="contest">比赛句柄。</param>
        /// <param name="password">新的比赛密码。</param>
        /// <exception cref="ArgumentNullException"/>
        public static void UpdateContestPassword(ContestHandle contest, string password)
        {
            if (contest == null)
                throw new ArgumentNullException(nameof(contest));
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            using (ContestDataProvider data = ContestDataProvider.Create(contest, false))
            {
                data.PasswordHash = PasswordHashAlgorithm.ComputeHash(PasswordEncoding.GetBytes(password));
            }
        }
    }
}

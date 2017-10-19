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
        /// 获取给定的用户在给定的比赛中的注册信息。
        /// </summary>
        /// <param name="contest">比赛句柄。</param>
        /// <param name="user">用户句柄。若当前登录会话中不存在活动用户，传入 null。</param>
        /// <returns>一个 ContestAuthorizationState 类实例对象，其中封装了给定的用户在给定的比赛中的注册信息。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static ContestAuthorizationState GetUserAuthorizationState(ContestHandle contest, UserHandle user)
        {
            if (contest == null)
                throw new ArgumentNullException(nameof(contest));
            
            /*
             * 检查顺序如下：
             * 1. 当前会话中有活动的登录用户；
             * 2. 用户以个人名义注册了比赛；
             * 3. 用户以队伍名义注册了比赛；
             * 4. 用户为比赛创建者；
             * 5. 用户用户组权限低于比赛的最低访问用户组权限；
             * 5. 比赛为开放比赛；
             * 6. 比赛为密码保护比赛。
             * 
             */
            
            // 检查当前会话中是否存在活动的登录用户。
            if (user == null)
            {
                return new ContestAuthorizationState(ContestRegisterState.NotRegistered);
            }

            using (ContestDataProvider contestData = ContestDataProvider.Create(contest, true))
            {
                // 检查用户是否以个人名义参加比赛。
                if (contestData.ParticipationMode == ContestParticipationMode.IndividualOnly ||
                    contestData.ParticipationMode == ContestParticipationMode.Both)
                {
                    foreach (UserHandle registeredUser in contestData.GetAuthorizedUsers())
                    {
                        if (registeredUser == user)
                        {
                            return new ContestAuthorizationState(ContestRegisterState.IndividualRegistered);
                        }
                    }
                }

                // 检查用户所在的队伍是否参加比赛。
                if (contestData.ParticipationMode == ContestParticipationMode.TeamworkOnly ||
                    contestData.ParticipationMode == ContestParticipationMode.Both)
                {
                    foreach (TeamHandle registeredTeam in contestData.GetAuthorizedTeams())
                    {
                        if (registeredTeam.IsUserIn(user))
                        {
                            return new ContestAuthorizationState(ContestRegisterState.TeamRegistered)
                            {
                                TeamId = registeredTeam.TeamId
                            };
                        }
                    }
                }

                // 检查用户是否为比赛创建者。
                if (string.Compare(contestData.Creator, user.Username, false) == 0)
                {
                    return new ContestAuthorizationState(ContestRegisterState.IndividualRegistered);
                }

                // 检查用户权限。
                if (!UserAuthorization.CheckAccessRights(contestData.AuthorizationGroup,
                    UserAuthorization.GetUserGroup(user)))
                {
                    return new ContestAuthorizationState(ContestRegisterState.NotRegistered);
                }
                else if (UserAuthorization.CheckAccessRights(UserGroup.Administrators,
                    UserAuthorization.GetUserGroup(user)))
                {
                    return new ContestAuthorizationState(ContestRegisterState.IndividualRegistered);
                }

                // 检查比赛是否为开放或者密码保护比赛。
                if (contestData.AuthorizationMode == ContestAuthorizationMode.Public)
                {
                    return new ContestAuthorizationState(ContestRegisterState.IndividualRegistered);
                }
                else if (contestData.AuthorizationMode == ContestAuthorizationMode.Protected)
                {
                    return new ContestAuthorizationState(ContestRegisterState.PasswordRequired);
                }
            }

            // 所有检查均不成立。
            return new ContestAuthorizationState(ContestRegisterState.NotRegistered);
        }

        /// <summary>
        /// 获取给定用户对给定比赛的数据操作权限。
        /// </summary>
        /// <param name="contest">比赛句柄。</param>
        /// <param name="user">用户句柄。若当前活动会话中不存在活动的登录用户，传入 null。</param>
        /// <returns>一个 DataAccess 枚举，表示给定用户对于给定比赛的数据操作权限。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static DataAccess GetUserAccess(ContestHandle contest, UserHandle user)
        {
            if (contest == null)
                throw new ArgumentNullException(nameof(contest));

            /*
             * 检查顺序如下：
             * 1. 当前活动会话中无活动登录用户。
             * 2. 用户为管理员身份；
             * 3. 用户为比赛创建者；
             * 4. 用户为比赛注册用户。
             * 
             */

            if (user == null)
            {
                return DataAccess.None;
            }
            
            // 检查用户是否为管理员身份。
            if (UserAuthorization.CheckAccessRights(UserGroup.Administrators, UserAuthorization.GetUserGroup(user)))
            {
                return DataAccess.ReadWrite;
            }

            // 检查用户是否为比赛创建者。
            using (ContestDataProvider contestData = ContestDataProvider.Create(contest, true))
            {
                if (string.Compare(contestData.Creator, user.Username, false) == 0)
                {
                    return DataAccess.ReadWrite;
                }
            }

            // 检查用户是否已经注册。
            if (GetUserAuthorizationState(contest, user).RegisterState == ContestRegisterState.NotRegistered)
            {
                // 未注册用户。不具备数据访问及操作权限。
                return DataAccess.None;
            }
            else
            {
                // 注册用户。具有读权限。
                return DataAccess.Read;
            }
        }

        /// <summary>
        /// 更新比赛密码。
        /// </summary>
        /// <param name="contest">比赛句柄。</param>
        /// <param name="password">更新后的密码。</param>
        /// <exception cref="ArgumentNullException"/>
        public static void UpdateContestPassword(ContestHandle contest, string password)
        {
            if (contest == null)
                throw new ArgumentNullException(nameof(contest));
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            // 计算密码哈希值。
            byte[] hash = PasswordHashAlgorithm.ComputeHash(PasswordEncoding.GetBytes(password));

            using (ContestDataProvider contestData = ContestDataProvider.Create(contest, false))
            {
                contestData.PasswordHash = hash;
            }
        }

        /// <summary>
        /// 检查给定的密码是否与给定比赛的密码匹配。
        /// </summary>
        /// <param name="contest">比赛句柄。</param>
        /// <param name="password">密码。</param>
        /// <returns>一个值，该值指示给定的密码是否与给定比赛的密码匹配。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static bool CheckContestPassword(ContestHandle contest, string password)
        {
            if (contest == null)
                throw new ArgumentNullException(nameof(contest));
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            byte[] hash = PasswordHashAlgorithm.ComputeHash(PasswordEncoding.GetBytes(password));

            // 比较密码哈希值是否相同。
            using (ContestDataProvider contestData = ContestDataProvider.Create(contest, true))
            {
                return Buffer.IsByteArraysEqual(hash, contestData.PasswordHash);
            }
        }
    }
}

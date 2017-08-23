namespace BITOJ.Core.Authorization
{
    using BITOJ.Core;
    using BITOJ.Core.Data;
    using System;
    using System.Web;

    /// <summary>
    /// 对用户会话提供管理。
    /// </summary>
    public static class UserSession
    {
        private static readonly string SessionUsername = "boj_username";
        private static readonly int SessionTimeout = 30;        // Session 超时时间 30 分钟。

        /// <summary>
        /// 判断给定的会话是否已经经过登录验证。
        /// </summary>
        /// <param name="session">待检查的会话。</param>
        /// <returns>一个值，指示给定的会话是否已经经过登录验证。</returns>
        public static bool IsAuthorized(HttpSessionStateBase session)
        {
            if (session == null)
                return false;

            return !string.IsNullOrEmpty(session[SessionUsername] as string);
        }

        /// <summary>
        /// 重置会话过期时间。
        /// </summary>
        /// <param name="session">会话。</param>
        public static void RenewSession(HttpSessionStateBase session)
        {
            if (session != null)
            {
                session.Timeout = SessionTimeout;
            }
        }

        /// <summary>
        /// 从给定的 Session 中抽取当前会话用户名。
        /// </summary>
        /// <param name="session">当前会话上下文。</param>
        /// <returns>存在于当前会话中的用户名。若当前会话没有注册任何一个活动用户，返回 null。</returns>
        public static string GetUsername(HttpSessionStateBase session)
        {
            if (session == null)
                return null;

            return session[SessionUsername] as string;
        }

        /// <summary>
        /// 从给定的 Session 中抽取当前会话用户权限集。
        /// </summary>
        /// <param name="session">当前会话上下文。</param>
        /// <returns>存在于当前会话中的用户权限集。</returns>
        public static UserGroup GetUserGroup(HttpSessionStateBase session)
        {
            if (session == null)
            {
                return UserGroup.Guests;
            }

            // 从数据库中查询权限集。
            string username = GetUsername(session);
            if (username == null)
            {
                return UserGroup.Guests;
            }

            UserHandle handle = UserManager.Default.QueryUserByName(username);
            if (handle == null)
            {
                return UserGroup.Guests;
            }

            using (UserDataProvider userData = UserDataProvider.Create(handle, true))
            {
                return userData.UserGroup;
            }
        }

        /// <summary>
        /// 使用给定的用户名和密码对给定的会话进行登录验证。
        /// </summary>
        /// <param name="session">用户会话。</param>
        /// <param name="username">用户名。</param>
        /// <param name="password">密码。</param>
        /// <exception cref="ArgumentNullException"/>
        public static bool Authorize(HttpSessionStateBase session, string username, string password)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            if (UserAuthorization.CheckAuthorization(username, password))
            {
                // 登录验证成功。
                session.Add(SessionUsername, username);
                RenewSession(session);
                return true;
            }
            else
            {
                // 登录验证失败。
                return false;
            }
        }

        /// <summary>
        /// 解除给定用户会话上的登录验证。
        /// </summary>
        /// <param name="session">会话。</param>
        public static void Deauthorize(HttpSessionStateBase session)
        {
            if (session != null)
            {
                session.Remove(SessionUsername);
            }
        }
    }
}

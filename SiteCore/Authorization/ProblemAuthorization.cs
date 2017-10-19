namespace BITOJ.Core.Authorization
{
    using BITOJ.Core;
    using BITOJ.Core.Data;
    using System;

    /// <summary>
    /// 为题目访问权限提供验证操作。
    /// </summary>
    public static class ProblemAuthorization
    {
        /// <summary>
        /// 检查给定的用户能否访问给定的题目。
        /// </summary>
        /// <param name="problemHandle">题目句柄。</param>
        /// <param name="userHandle">用户句柄。若当前会话尚未活动登录用户，传入 null。</param>
        /// <returns>一个值，该值只是给定身份权限的用户能否访问给定的题目。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static DataAccess GetUserAccess(ProblemHandle problemHandle, UserHandle userHandle)
        {
            if (problemHandle == null)
                throw new ArgumentNullException(nameof(problemHandle));

            /*
             * 检查顺序如下：
             * 1. 用户为管理员；
             * 2. 题目为比赛题目；
             * 3. 用户用户组权限低于题目要求的最低用户组权限。
             * 
             */
            
            // 检查用户是否为管理员身份。
            if (userHandle != null && UserAuthorization.CheckAccessRights(UserGroup.Administrators, 
                UserAuthorization.GetUserGroup(userHandle)))
            {
                return DataAccess.ReadWrite;
            }

            using (ProblemDataProvider problemData = ProblemDataProvider.Create(problemHandle, true))
            {
                if (problemData.ContestId != -1)
                {
                    // 比赛题目。
                    if (userHandle == null)
                    {
                        return DataAccess.None;
                    }

                    ContestHandle contestHandle = ContestManager.Default.QueryContestById(problemData.ContestId);
                    return ContestAuthorization.GetUserAccess(contestHandle, userHandle);
                }
                else
                {
                    // 主题目库题目
                    UserGroup usergroup = (userHandle == null) 
                        ? UserGroup.Guests : UserAuthorization.GetUserGroup(userHandle);

                    if (UserAuthorization.CheckAccessRights(problemData.AuthorizationGroup, usergroup))
                    {
                        return DataAccess.Read;
                    }
                    else
                    {
                        return DataAccess.None;
                    }
                }
            }
        }
    }
}

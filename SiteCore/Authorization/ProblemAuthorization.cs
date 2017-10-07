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
        public static DataAccess Check(ProblemHandle problemHandle, UserHandle userHandle)
        {
            if (problemHandle == null)
                throw new ArgumentNullException(nameof(problemHandle));

            if (!ProblemArchieveManager.Default.IsProblemExist(problemHandle.ProblemId))
            {
                return DataAccess.None;
            }

            using (ProblemDataProvider problemData = ProblemDataProvider.Create(problemHandle, true))
            {
                if (problemData.ContestId != -1)
                {
                    // 比赛题目
                    if (userHandle == null)
                    {
                        return DataAccess.None;
                    }

                    ContestHandle contestHandle = ContestManager.Default.QueryContestById(problemData.ContestId);
                    return ContestAuthorization.GetAccess(contestHandle, userHandle);
                }
                else
                {
                    // 主题目库题目
                    UserGroup usergroup = userHandle == null 
                        ? UserGroup.Guests 
                        : UserAuthorization.GetUserGroup(userHandle);
                    if (UserAuthorization.CheckAccessRights(UserGroup.Administrators, usergroup))
                    {
                        return DataAccess.ReadWrite;
                    }
                    else if (UserAuthorization.CheckAccessRights(problemData.AuthorizationGroup, usergroup))
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

namespace BITOJ.Core
{
    using DatabaseAuthorizationMode = BITOJ.Data.Models.ContestAuthorizationMode;

    /// <summary>
    /// 表示比赛的身份验证模式。
    /// </summary>
    public enum ContestAuthorizationMode : int
    {
        /// <summary>
        /// 公共比赛。任何人可以查看并参与。
        /// </summary>
        Public = DatabaseAuthorizationMode.Public,

        /// <summary>
        /// 受密码保护的比赛。仅正确输入密码的用户或队伍才能查看并参与。
        /// </summary>
        Protected = DatabaseAuthorizationMode.Protected,

        /// <summary>
        /// 私有比赛。仅在白名单或者不在黑名单中的队伍或个人才能查看并参与。
        /// </summary>
        Private = DatabaseAuthorizationMode.Private,
    }
}

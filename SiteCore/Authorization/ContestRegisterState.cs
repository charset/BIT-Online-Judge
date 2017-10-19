namespace BITOJ.Core.Authorization
{
    /// <summary>
    /// 编码比赛的注册状态。
    /// </summary>
    public enum ContestRegisterState : int
    {
        /// <summary>
        /// 未注册且不能注册。
        /// </summary>
        NotRegistered = 0,

        /// <summary>
        /// 未注册但能通过输入比赛密码进行注册。
        /// </summary>
        PasswordRequired = 1,

        /// <summary>
        /// 以个人名义注册。
        /// </summary>
        IndividualRegistered = 1,

        /// <summary>
        /// 以队伍名义注册。
        /// </summary>
        TeamRegistered = 2
    }
}

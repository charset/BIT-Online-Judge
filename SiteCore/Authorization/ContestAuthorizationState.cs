namespace BITOJ.Core.Authorization
{
    /// <summary>
    /// 表示比赛身份验证状态。
    /// </summary>
    public enum ContestAuthorizationState
    {
        /// <summary>
        /// 已经通过身份验证。
        /// </summary>
        Authorized,

        /// <summary>
        /// 未通过身份验证，但可进行身份验证。
        /// </summary>
        AuthorizationRequired,

        /// <summary>
        /// 未通过身份验证，且无法进行身份验证。
        /// </summary>
        AuthorizationFailed
    }
}

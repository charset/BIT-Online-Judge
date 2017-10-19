namespace BITOJ.Core.Authorization
{
    /// <summary>
    /// 封装表示比赛的授权状态。
    /// </summary>
    public sealed class ContestAuthorizationState
    {
        /// <summary>
        /// 获取或设置比赛的注册状态。
        /// </summary>
        public ContestRegisterState RegisterState { get; set; }

        /// <summary>
        /// 当 RegisterState 为 TeamRegistered 时，获取或设置注册的队伍 ID。
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// 创建 ContestAuthorizationState 类的新实例。
        /// </summary>
        public ContestAuthorizationState()
        {
            RegisterState = ContestRegisterState.NotRegistered;
            TeamId = 0;
        }

        /// <summary>
        /// 使用给定的注册状态创建 ContestAuthorizationState 类的新实例。
        /// </summary>
        /// <param name="state">注册状态。</param>
        public ContestAuthorizationState(ContestRegisterState state)
        {
            RegisterState = state;
            TeamId = 0;
        }
    }
}

namespace BITOJ.Core
{
    using NativeOJSystem = BITOJ.Data.Entities.OJSystem;

    /// <summary>
    /// 编码 Online Judge 平台。
    /// </summary>
    public enum OJSystem : int
    {
        /// <summary>
        /// 北京理工大学 Online Judge.
        /// </summary>
        BIT = NativeOJSystem.BIT,

        /// <summary>
        /// 大视野在线测评。
        /// </summary>
        BZOJ = NativeOJSystem.BZOJ,

        /// <summary>
        /// CodeForces online judge platform.
        /// </summary>
        CodeForces = NativeOJSystem.CodeForces,

        /// <summary>
        /// CodeForces gym.
        /// </summary>
        Gym = NativeOJSystem.Gym,

        /// <summary>
        /// 杭州电子科技大学 Online Judge.
        /// </summary>
        HDU = NativeOJSystem.HDU,

        /// <summary>
        /// 北京大学 Online Judge.
        /// </summary>
        POJ = NativeOJSystem.POJ,

        /// <summary>
        /// UVa Online Judge.
        /// </summary>
        UVa = NativeOJSystem.UVa,

        /// <summary>
        /// UVa Live Archieve.
        /// </summary>
        UVaLiveArchieve = NativeOJSystem.UVaLiveArchieve,

        /// <summary>
        /// 浙江大学 Online Judge.
        /// </summary>
        ZOJ = NativeOJSystem.ZOJ,
    }
}

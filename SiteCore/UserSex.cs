namespace BITOJ.Core
{
    using NativeUserSex = BITOJ.Data.Entities.UserSex;

    /// <summary>
    /// 编码用户性别信息。
    /// </summary>
    public enum UserSex : int
    {
        /// <summary>
        /// 女性。
        /// </summary>
        Female = NativeUserSex.Female,

        /// <summary>
        /// 男性。
        /// </summary>
        Male = NativeUserSex.Male,
    }
}

namespace BITOJ.Core
{
    using NativeUserGroup = BITOJ.Data.Entities.UserGroup;

    /// <summary>
    /// 表示用户权限集。
    /// </summary>
    public enum UserGroup : int
    {
        /// <summary>
        /// 访客权限集。
        /// </summary>
        Guests = NativeUserGroup.Guests,

        /// <summary>
        /// 标准权限集。
        /// </summary>
        Standard = NativeUserGroup.Standard,

        /// <summary>
        /// 内部用户权限集。
        /// </summary>
        Insiders = NativeUserGroup.Insiders,

        /// <summary>
        /// 管理员权限集。
        /// </summary>
        Administrators = NativeUserGroup.Administrators,
    }
}

namespace BITOJ.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 表示用户信息的实体对象。
    /// </summary>
    [Table("UserProfiles")]
    public sealed class UserProfileEntity
    {
        /// <summary>
        /// 获取或设置用户名。
        /// </summary>
        [Key]
        public string Username { get; set; }

        /// <summary>
        /// 获取或设置用户密码哈希值。
        /// </summary>
        public byte[] PasswordHash { get; set; }

        /// <summary>
        /// 获取或设置用户的组织名称。
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// 获取或设置用户的权限集。
        /// </summary>
        public UserGroup UserGroup { get; set; }

        /// <summary>
        /// 获取或设置包含用户信息的文件名。
        /// </summary>
        public string ProfileFileName { get; set; }

        /// <summary>
        /// 初始化 UserProfileEntity 类的新实例。
        /// </summary>
        public UserProfileEntity()
        {
            Username = string.Empty;
            PasswordHash = new byte[0];
            Organization = string.Empty;
            UserGroup = UserGroup.Guests;
            ProfileFileName = string.Empty;
        }
    }
}

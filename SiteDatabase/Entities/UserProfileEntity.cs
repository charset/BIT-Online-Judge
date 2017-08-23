namespace BITOJ.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 表示用户信息的实体对象。
    /// </summary>
    [Table("UserProfiles")]
    public class UserProfileEntity
    {
        /// <summary>
        /// 获取或设置用户名。
        /// </summary>
        [Key]
        public string Username { get; set; }

        /// <summary>
        /// 获取或设置用户的组织名称。
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// 获取或设置用户的性别信息。
        /// </summary>
        public UserSex Sex { get; set; }

        /// <summary>
        /// 获取或设置用户的权限集。
        /// </summary>
        public UserGroup UserGroup { get; set; }

        /// <summary>
        /// 获取或设置用户密码哈希值。
        /// </summary>
        public byte[] PasswordHash { get; set; }

        /// <summary>
        /// 获取或设置该用户所处的队伍。
        /// </summary>
        public virtual ICollection<TeamProfileEntity> Teams { get; set; }

        /// <summary>
        /// 初始化 UserProfileEntity 类的新实例。
        /// </summary>
        public UserProfileEntity()
        {
            Username = string.Empty;
            Organization = string.Empty;
            Sex = UserSex.Male;
            UserGroup = UserGroup.Guests;
            PasswordHash = new byte[0];

            Teams = new List<TeamProfileEntity>();
        }
    }
}

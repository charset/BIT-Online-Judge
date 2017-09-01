namespace BITOJ.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 表示队伍信息的实体对象。
    /// </summary>
    [Table("TeamProfiles")]
    public class TeamProfileEntity
    {
        /// <summary>
        /// 获取或设置该实体对象在数据库中的主键。
        /// </summary>
        /// <remarks>
        /// 注意：不应在外部代码中手动修改该属性的值。
        /// </remarks>
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置队伍的名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置队伍领队用户名。
        /// </summary>
        public string Leader { get; set; }

        /// <summary>
        /// 获取或设置加入当前队伍所需的验证密钥。
        /// </summary>
        public byte[] PasswordHash { get; set; }

        /// <summary>
        /// 获取或设置
        /// </summary>
        public virtual ICollection<UserProfileEntity> Members { get; set; }
        
        /// <summary>
        /// 初始化 TeamProfileEntity 类的新实例。
        /// </summary>
        public TeamProfileEntity()
        {
            Id = 0;
            Name = string.Empty;
            Leader = string.Empty;
            PasswordHash = new byte[0];

            Members = new List<UserProfileEntity>();
        }
    }
}

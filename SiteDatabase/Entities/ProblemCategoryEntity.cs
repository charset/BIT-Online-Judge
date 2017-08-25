namespace BITOJ.Data.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 表示题目分类类别对象实体数据。
    /// </summary>
    [Table("ProblemCategories")]
    public class ProblemCategoryEntity
    {
        /// <summary>
        /// 获取或设置该记录在数据库中的主键值。
        /// </summary>
        /// <remarks>
        /// 注意：该字段不应该被外部代码手动修改。
        /// </remarks>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置该类别的名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置当前类别下的所有题目。
        /// </summary>
        public virtual ICollection<ProblemEntity> Problems { get; set; }

        /// <summary>
        /// 初始化 ProblemCategoryEntity 类的新实例。
        /// </summary>
        public ProblemCategoryEntity()
        {
            Id = 0;
            Name = string.Empty;
            Problems = new List<ProblemEntity>();
        }
    }
}

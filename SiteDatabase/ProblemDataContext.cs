namespace BITOJ.Data
{
    using BITOJ.Data.Entities;
    using System;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// 为题目库提供数据上下文。
    /// </summary>
    public partial class ProblemDataContext : DbContext
    {
        /// <summary>
        /// 初始化 ProblemArchieveDataContext 类的新实例。
        /// </summary>
        public ProblemDataContext()
            : base("name=ProblemDataContext")
        {
        }

        ~ProblemDataContext()
        {
            SaveChanges();
            Dispose();
        }

        /// <summary>
        /// 使用给定的题目标题在给定的数据集中查询题目实体对象并返回。
        /// </summary>
        /// <param name="dataset">全体数据集。</param>
        /// <param name="title">要查询的题目标题。</param>
        /// <returns>一个可查询对象，该对象可查询到所有的符合要求的实体对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <remarks>
        /// 在第一个迭代版本中，暂不支持模糊查询，仅支持完全匹配查询。
        /// </remarks>
        public static IQueryable<ProblemEntity> QueryProblemEntitiesByTitle(IQueryable<ProblemEntity> dataset, string title)
        {
            if (dataset == null)
                throw new ArgumentNullException(nameof(dataset));
            if (title == null)
                throw new ArgumentNullException(nameof(title));

            var entities = from item in dataset
                           where item.Title == title
                           select item;
            return entities;
        }

        /// <summary>
        /// 使用给定的题目作者在给定的数据集中查询题目实体对象并返回。
        /// </summary>
        /// <param name="dataset">全体数据集。</param>
        /// <param name="author">要查询的作者。</param>
        /// <returns>一个可查询对象，该对象可查询到所有的符合要求的实体对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static IQueryable<ProblemEntity> QueryProblemEntitiesByAuthor(IQueryable<ProblemEntity> dataset, 
            string author)
        {
            if (author == null)
                throw new ArgumentNullException(nameof(author));

            var entities = from item in dataset
                           where item.Author == author
                           select item;
            return entities;
        }

        /// <summary>
        /// 使用给定的题目来源在给定的数据集中查询题目实体对象并返回。
        /// </summary>
        /// <param name="dataset">全体数据集。</param>
        /// <param name="source">要查询的题目来源。</param>
        /// <returns>一个可查询对象，该对象可查询到所有的符合要求的实体对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static IQueryable<ProblemEntity> QueryProblemEntitiesBySource(IQueryable<ProblemEntity> dataset, 
            string source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var entities = from item in dataset
                           where item.Source == source
                           select item;
            return entities;
        }

        /// <summary>
        /// 使用给定的题目来源 OJ 系统在给定的数据集中查询题目实体对象并返回。
        /// </summary>
        /// <param name="dataset">数据集。</param>
        /// <param name="oj">题目的来源 OJ 系统。</param>
        /// <returns>一个可查询对象，该对象可查询到所有的符合要求的实体对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static IQueryable<ProblemEntity> QueryProblemEntitiesByOrigin(IQueryable<ProblemEntity> dataset,
            OJSystem oj)
        {
            if (dataset == null)
                throw new ArgumentNullException(nameof(dataset));

            var entities = from item in dataset
                           where item.Origin == oj
                           select item;
            return entities;
        }

        /// <summary>
        /// 使用给定的名称在给定的数据集中查询题目类别实体对象并返回。
        /// </summary>
        /// <param name="dataset">原始数据集。</param>
        /// <param name="name">题目类别实体对象名称。</param>
        /// <returns>一个可查询对象，该对象可查询到所有的符合要求的实体对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static IQueryable<ProblemCategoryEntity> QueryCategoryEntitiesByName(IQueryable<ProblemCategoryEntity> dataset,
            string name)
        {
            if (dataset == null)
                throw new ArgumentNullException(nameof(dataset));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var entities = from item in dataset
                           where item.Name == name
                           select item;
            return entities;
        }

        /// <summary>
        /// 将给定的题目实体对象添加至数据集中。
        /// </summary>
        /// <param name="entity">要添加的题目实体对象。</param>
        /// <exception cref="ArgumentNullException"/>
        public void AddProblemEntity(ProblemEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Problems.Add(entity);
            SaveChanges();
        }

        /// <summary>
        /// 将给定的题目类别实体对象添加至数据集中。
        /// </summary>
        /// <param name="entity">要添加的题目类别实体对象。</param>
        /// <exception cref="ArgumentNullException"/>
        public void AddProblemCategory(ProblemCategoryEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Categories.Add(entity);
        }

        /// <summary>
        /// 使用给定的题目实体 ID 查询题目实体对象并返回。
        /// </summary>
        /// <param name="id">要查询的题目 ID。</param>
        /// <returns>题目实体对象。若没有符合要求的题目实体对象，返回 null 。</returns>
        public ProblemEntity GetProblemEntityById(string id)
        {
            return Problems.Find(id);
        }

        /// <summary>
        /// 查询数据集中的所有题目实体对象。
        /// </summary>
        /// <returns>一个可查询对象，该对象可查询到数据集中的所有题目实体对象。</returns>
        public IQueryable<ProblemEntity> GetAllProblemEntities()
        {
            return Problems;
        }

        /// <summary>
        /// 查询数据集中的所有题目类别实体对象。
        /// </summary>
        /// <returns></returns>
        public IQueryable<ProblemCategoryEntity> GetAllCategories()
        {
            return Categories;
        }

        /// <summary>
        /// 从数据集中移除给定题目实体对象。
        /// </summary>
        /// <param name="entity">要移除的题目实体对象。</param>
        /// <exception cref="ArgumentNullException"/>
        public void RemoveProblemEntity(ProblemEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Problems.Remove(entity);
        }

        /// <summary>
        /// 从数据集中移除给定题目类别实体对象。
        /// </summary>
        /// <param name="entity">要移除的题目类别实体对象。</param>
        /// <exception cref="ArgumentNullException"/>
        public void RemoveCategoryEntity(ProblemCategoryEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Categories.Remove(entity);
        }

        /// <summary>
        /// 获取或设置主题目库题目实体数据集。
        /// </summary>
        public virtual DbSet<ProblemEntity> Problems { get; set; }

        /// <summary>
        /// 获取或设置题目类别实体数据集。
        /// </summary>
        public virtual DbSet<ProblemCategoryEntity> Categories { get; set; }
    }
}

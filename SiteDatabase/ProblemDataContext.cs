namespace BITOJ.Data
{
    using BITOJ.Data.Entities;
    using System;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// Ϊ��Ŀ���ṩ���������ġ�
    /// </summary>
    public partial class ProblemDataContext : DbContext
    {
        /// <summary>
        /// ��ʼ�� ProblemArchieveDataContext �����ʵ����
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
        /// ʹ�ø�������Ŀ�����ڸ��������ݼ��в�ѯ��Ŀʵ����󲢷��ء�
        /// </summary>
        /// <param name="dataset">ȫ�����ݼ���</param>
        /// <param name="title">Ҫ��ѯ����Ŀ���⡣</param>
        /// <returns>һ���ɲ�ѯ���󣬸ö���ɲ�ѯ�����еķ���Ҫ���ʵ�����</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <remarks>
        /// �ڵ�һ�������汾�У��ݲ�֧��ģ����ѯ����֧����ȫƥ���ѯ��
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
        /// ʹ�ø�������Ŀ�����ڸ��������ݼ��в�ѯ��Ŀʵ����󲢷��ء�
        /// </summary>
        /// <param name="dataset">ȫ�����ݼ���</param>
        /// <param name="author">Ҫ��ѯ�����ߡ�</param>
        /// <returns>һ���ɲ�ѯ���󣬸ö���ɲ�ѯ�����еķ���Ҫ���ʵ�����</returns>
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
        /// ʹ�ø�������Ŀ��Դ�ڸ��������ݼ��в�ѯ��Ŀʵ����󲢷��ء�
        /// </summary>
        /// <param name="dataset">ȫ�����ݼ���</param>
        /// <param name="source">Ҫ��ѯ����Ŀ��Դ��</param>
        /// <returns>һ���ɲ�ѯ���󣬸ö���ɲ�ѯ�����еķ���Ҫ���ʵ�����</returns>
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
        /// ʹ�ø�������Ŀ��Դ OJ ϵͳ�ڸ��������ݼ��в�ѯ��Ŀʵ����󲢷��ء�
        /// </summary>
        /// <param name="dataset">���ݼ���</param>
        /// <param name="oj">��Ŀ����Դ OJ ϵͳ��</param>
        /// <returns>һ���ɲ�ѯ���󣬸ö���ɲ�ѯ�����еķ���Ҫ���ʵ�����</returns>
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
        /// ʹ�ø����������ڸ��������ݼ��в�ѯ��Ŀ���ʵ����󲢷��ء�
        /// </summary>
        /// <param name="dataset">ԭʼ���ݼ���</param>
        /// <param name="name">��Ŀ���ʵ��������ơ�</param>
        /// <returns>һ���ɲ�ѯ���󣬸ö���ɲ�ѯ�����еķ���Ҫ���ʵ�����</returns>
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
        /// ����������Ŀʵ�������������ݼ��С�
        /// </summary>
        /// <param name="entity">Ҫ��ӵ���Ŀʵ�����</param>
        /// <exception cref="ArgumentNullException"/>
        public void AddProblemEntity(ProblemEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Problems.Add(entity);
            SaveChanges();
        }

        /// <summary>
        /// ����������Ŀ���ʵ�������������ݼ��С�
        /// </summary>
        /// <param name="entity">Ҫ��ӵ���Ŀ���ʵ�����</param>
        /// <exception cref="ArgumentNullException"/>
        public void AddProblemCategory(ProblemCategoryEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Categories.Add(entity);
        }

        /// <summary>
        /// ʹ�ø�������Ŀʵ�� ID ��ѯ��Ŀʵ����󲢷��ء�
        /// </summary>
        /// <param name="id">Ҫ��ѯ����Ŀ ID��</param>
        /// <returns>��Ŀʵ�������û�з���Ҫ�����Ŀʵ����󣬷��� null ��</returns>
        public ProblemEntity GetProblemEntityById(string id)
        {
            return Problems.Find(id);
        }

        /// <summary>
        /// ��ѯ���ݼ��е�������Ŀʵ�����
        /// </summary>
        /// <returns>һ���ɲ�ѯ���󣬸ö���ɲ�ѯ�����ݼ��е�������Ŀʵ�����</returns>
        public IQueryable<ProblemEntity> GetAllProblemEntities()
        {
            return Problems;
        }

        /// <summary>
        /// ��ѯ���ݼ��е�������Ŀ���ʵ�����
        /// </summary>
        /// <returns></returns>
        public IQueryable<ProblemCategoryEntity> GetAllCategories()
        {
            return Categories;
        }

        /// <summary>
        /// �����ݼ����Ƴ�������Ŀʵ�����
        /// </summary>
        /// <param name="entity">Ҫ�Ƴ�����Ŀʵ�����</param>
        /// <exception cref="ArgumentNullException"/>
        public void RemoveProblemEntity(ProblemEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Problems.Remove(entity);
        }

        /// <summary>
        /// �����ݼ����Ƴ�������Ŀ���ʵ�����
        /// </summary>
        /// <param name="entity">Ҫ�Ƴ�����Ŀ���ʵ�����</param>
        /// <exception cref="ArgumentNullException"/>
        public void RemoveCategoryEntity(ProblemCategoryEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Categories.Remove(entity);
        }

        /// <summary>
        /// ��ȡ����������Ŀ����Ŀʵ�����ݼ���
        /// </summary>
        public virtual DbSet<ProblemEntity> Problems { get; set; }

        /// <summary>
        /// ��ȡ��������Ŀ���ʵ�����ݼ���
        /// </summary>
        public virtual DbSet<ProblemCategoryEntity> Categories { get; set; }
    }
}

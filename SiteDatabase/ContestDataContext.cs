namespace BITOJ.Data
{
    using BITOJ.Data.Entities;
    using System;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// Ϊ���������ṩ������֧�֡�
    /// </summary>
    public partial class ContestDataContext : DbContext
    {
        /// <summary>
        /// ��ʼ�� ContestDataContext �����ʵ����
        /// </summary>
        public ContestDataContext()
            : base("name=ContestDataContext")
        {
        }

        /// <summary>
        /// �������ı���ʵ��������������ݿ��С�
        /// </summary>
        /// <param name="entity">Ҫ��ӵı���ʵ�塣</param>
        /// <exception cref="ArgumentNullException"/>
        public ContestEntity AddContest(ContestEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity = Contests.Add(entity);
            SaveChanges();

            return entity;
        }

        /// <summary>
        /// ������ ID ��ѯ����ʵ�����
        /// </summary>
        /// <param name="contestId">Ҫ��ѯ�ı��� ID ��</param>
        /// <returns>
        /// ���и������� ID ֵ�ñ���ʵ�������δ�����ݿ����ҵ���Ӧ�ı���ʵ����󣬷��� null ��
        /// </returns>
        public ContestEntity QueryContestById(int contestId)
        {
            return Contests.Find(contestId);
        }

        /// <summary>
        /// ������Ӹ���������Դ�в�ѯ����ʵ�����
        /// </summary>
        /// <param name="title">Ҫ��ѯ�ı��⡣</param>
        /// <param name="source">����Դ��</param>
        /// <returns>һ���ɲ�ѯ���󣬸ö�����������б���Ϊ����ֵ�ı���ʵ�����</returns>
        /// <exception cref="ArgumentNullException"/>
        public static IQueryable<ContestEntity> QueryContestsByTitle(IQueryable<ContestEntity> source, string title)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (title == null)
                throw new ArgumentNullException(nameof(title));

            var entities = from item in source
                           where item.Title == title
                           select item;
            return entities;
        }

        /// <summary>
        /// �����ߴӸ���������Դ�в�ѯ����ʵ�����
        /// </summary>
        /// <param name="creator">Ҫ��ѯ�����ߵ��û�����</param>
        /// <param name="source">����Դ��</param>
        /// <returns>һ���ɲ�ѯ���󣬸ö����������������Ϊ����ֵ�ı���ʵ�����</returns>
        /// <exception cref="ArgumentNullException"/>
        public static IQueryable<ContestEntity> QueryContestsByCreator(IQueryable<ContestEntity> source, string creator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (creator == null)
                throw new ArgumentNullException(nameof(creator));

            var entities = from item in source
                           where item.Creator == creator
                           select item;
            return entities;
        }

        /// <summary>
        /// ��ѯ���еı���ʵ�����
        /// </summary>
        /// <returns>һ���ɲ�ѯ���󣬸ö���ɲ�ѯ�����еı���ʵ�����</returns>
        public IQueryable<ContestEntity> QueryAllContests()
        {
            return Contests;
        }

        /// <summary>
        /// �Ӹ���������Դ�в�ѯ����δ��ʼ�ı���ʵ�����
        /// </summary>
        /// <param name="source">����Դ��</param>
        /// <returns>һ���ɲ�ѯ���󣬸ö���ɲ�ѯ�����е�δ��ʼ�ı���ʵ�����</returns>
        public static IQueryable<ContestEntity> QueryUnstartedContests(IQueryable<ContestEntity> source)
        {
            DateTime now = DateTime.Now;
            var entities = from item in source
                           where item.StartTime > now
                           select item;
            return entities;
        }

        /// <summary>
        /// �Ӹ���������Դ�в�ѯ�������ڽ��еı���ʵ�����
        /// </summary>
        /// <param name="source">����Դ��</param>
        /// <returns>һ���ɲ�ѯ���󣬸ö���ɲ�ѯ�����е����ڽ��еı���ʵ�����</returns>
        public static IQueryable<ContestEntity> QueryRunningContests(IQueryable<ContestEntity> source)
        {
            DateTime now = DateTime.Now;
            var entities = from item in source
                           where item.StartTime <= now && item.EndTime >= now
                           select item;
            return entities;
        }

        /// <summary>
        /// �Ӹ���������Դ�в�ѯ�����ѽ����ı���ʵ�����
        /// </summary>
        /// <param name="source">����Դ��</param>
        /// <returns>һ���ɲ�ѯ���󣬸ö���ɲ�ѯ�����е��ѽ����ı���ʵ�����</returns>
        public static IQueryable<ContestEntity> QueryEndedContests(IQueryable<ContestEntity> source)
        {
            DateTime now = DateTime.Now;
            var entities = from item in source
                           where item.EndTime < now
                           select item;
            return entities;
        }

        /// <summary>
        /// �����ݿ����Ƴ������ı�������ʵ�塣
        /// </summary>
        /// <param name="entity">Ҫ�Ƴ��ı���ʵ�����</param>
        /// <exception cref="ArgumentNullException"/>
        public void RemoveContest(ContestEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Contests.Remove(entity);
        }

        /// <summary>
        /// ��ȡ�����ñ������ݼ���
        /// </summary>
        public virtual DbSet<ContestEntity> Contests { get; set; }
    }
}

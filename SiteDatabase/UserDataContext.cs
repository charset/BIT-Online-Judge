namespace BITOJ.Data
{
    using BITOJ.Data.Entities;
    using System;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// Ϊ�û����ݿ������ṩ������֧�֡�
    /// </summary>
    public partial class UserDataContext : DbContext
    {
        /// <summary>
        /// ��ʼ�� UserDataContext �����ʵ����
        /// </summary>
        public UserDataContext()
            : base("name=UserDataContext")
        {
        }

        /// <summary>
        /// ���������û���Ϣʵ��������������ݿ��С�
        /// </summary>
        /// <param name="entity">Ҫ��ӵ��û���Ϣʵ�����ݡ�</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        /// <remarks>
        /// ��������ʵ�������Ѿ����������ݿ��У��׳� InvalidOperationException �쳣��
        /// ��Ҫ���¸�����ʵ�����ݣ���ʹ�� UpdateUserProfileEntity ������
        /// </remarks>
        public void AddUserProfileEntity(UserProfileEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (QueryUserProfileEntity(entity.Username) != null)
                throw new InvalidOperationException("������ʵ������Ѿ����������ݿ��С�");

            UserProfiles.Add(entity);
            SaveChanges();
        }

        /// <summary>
        /// �������Ķ�����Ϣʵ��������������ݿ��С�
        /// </summary>
        /// <param name="entity">Ҫ��ӵĶ�����Ϣʵ�����ݡ�</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        /// <remarks>
        /// ��������ʵ������Ѿ����������ݿ��У��׳� InvalidOperationException �쳣��
        /// ��Ҫ�������ݿ��ж�Ӧ��ʵ�����ݣ������ UpdateTeamProfileEntity ������
        /// </remarks>
        public void AddTeamProfileEntity(TeamProfileEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            TeamProfiles.Add(entity);
            SaveChanges();
        }

        /// <summary>
        /// ��ȡ���е��û�������Ϣʵ�����
        /// </summary>
        /// <returns>һ���ɲ�ѯ���󣬰��������е��û�������Ϣʵ�����</returns>
        public IQueryable<UserProfileEntity> GetAllUserProfiles()
        {
            return UserProfiles;
        }

        /// <summary>
        /// ��ȡ���еĶ�����Ϣʵ�����
        /// </summary>
        /// <returns>һ���ɲ�ѯ���󣬰��������е��û�Ȩ����Ϣʵ�����</returns>
        public IQueryable<TeamProfileEntity> GetAllTeamProfiles()
        {
            return TeamProfiles;
        }

        /// <summary>
        /// ʹ��ָ�����û�����ѯ�û���Ϣʵ�����
        /// </summary>
        /// <param name="username">Ҫ��ѯ���û�����</param>
        /// <returns>��Ӧ���û���Ϣʵ���������������û���δ�����ݿ����ҵ������� null��</returns>
        /// <exception cref="ArgumentNullException"/>
        public UserProfileEntity QueryUserProfileEntity(string username)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));

            return UserProfiles.Find(username);
        }

        /// <summary>
        /// ʹ�ø����Ķ��� ID ��ѯ������Ϣʵ�����
        /// </summary>
        /// <param name="teamId">Ҫ��ѯ�Ķ��� ID ��</param>
        /// <returns>����� ID ���Ӧ�Ķ�����Ϣʵ������������Ķ��� ID δ�����ݿ����ҵ������� null ��</returns>
        public TeamProfileEntity QueryTeamProfileEntity(int teamId)
        {
            return TeamProfiles.Find(teamId);
        }

        /// <summary>
        /// �����ݿ���ɾ���������û���Ϣʵ�����ݡ�
        /// </summary>
        /// <param name="entity">Ҫɾ�����û���Ϣʵ�����ݡ�</param>
        /// <exception cref="ArgumentNullException"/>
        public void RemoveUserProfileEntity(UserProfileEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            UserProfiles.Remove(entity);
            SaveChanges();
        }

        /// <summary>
        /// �����ݿ���ɾ�������Ķ�����Ϣʵ�����ݡ�
        /// </summary>
        /// <param name="entity">Ҫɾ���Ķ�����Ϣʵ�����ݡ�</param>
        /// <exception cref="ArgumentNullException"/>
        public void RemoveTeamProfileEntity(TeamProfileEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            TeamProfiles.Remove(entity);
            SaveChanges();
        }

        /// <summary>
        /// ����ָ������֯���ƴӸ���������Դ�в�ѯ������ͬ��֯���Ƶ��û���Ϣʵ�����
        /// </summary>
        /// <param name="source">����Դ��</param>
        /// <param name="organization">��֯���ơ�</param>
        /// <returns>һ���ɲ�ѯ���󣬰��������е������������û���Ϣʵ�����</returns>
        /// <exception cref="ArgumentNullException"/>
        public static IQueryable<UserProfileEntity> QueryUserProfileEntitiesByOrganization(
            IQueryable<UserProfileEntity> source, string organization)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (organization == null)
                throw new ArgumentNullException(nameof(organization));

            var entities = from item in source
                           where item.Organization == organization
                           select item;
            return entities;
        }

        /// <summary>
        /// ����ָ�����û�Ȩ����Ӹ���������Դ�в�ѯ�û���Ϣʵ�����
        /// </summary>
        /// <param name="source">����Դ��</param>
        /// <param name="userGroup">�û�Ȩ���顣</param>
        /// <returns>һ���ɲ�ѯ���󣬰��������е������������û���Ϣʵ�����</returns>
        /// <exception cref="ArgumentNullException"/>
        public static IQueryable<UserProfileEntity> QueryUserProfileEntitiesByUsergroup(
            IQueryable<UserProfileEntity> source, UserGroup userGroup)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var entities = from item in source
                           where item.UserGroup == userGroup
                           select item;
            return entities;
        }

        /// <summary>
        /// ����ָ�����û��Ա�Ӹ���������Դ�в�ѯ�û���Ϣʵ�����
        /// </summary>
        /// <param name="source">����Դ��</param>
        /// <param name="sex">�û��Ա�</param>
        /// <returns>һ���ɲ�ѯ���󣬰��������е������������û���Ϣʵ�����</returns>
        /// <exception cref="ArgumentNullException"/>
        public static IQueryable<UserProfileEntity> QueryUserProfileEntitiesBySex(
            IQueryable<UserProfileEntity> source, UserSex sex)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var entities = from item in source
                           where item.Sex == sex
                           select item;
            return entities;
        }

        /// <summary>
        /// ʹ�ø����Ķ������ƴӸ���������Դ�в�ѯ������Ϣʵ�����ݡ�
        /// </summary>
        /// <param name="source">����Դ��</param>
        /// <param name="teamName">Ҫ��ѯ�Ķ������ơ�</param>
        /// <returns>
        /// һ���ɲ�ѯ���󣬸ö���ɲ�ѯ������������������Ӧ�Ķ�����Ϣʵ�����
        /// </returns>
        /// <exception cref="ArgumentNullException"/>
        public static IQueryable<TeamProfileEntity> QueryTeamProfileEntityByName(IQueryable<TeamProfileEntity> source,
            string teamName)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (teamName == null)
                throw new ArgumentNullException(nameof(teamName));

            var entities = from item in source
                           where item.Name == teamName
                           select item;
            return entities;
        }

        /// <summary>
        /// ��ȡ�������û���Ϣ���ݼ���
        /// </summary>
        public virtual DbSet<UserProfileEntity> UserProfiles { get; set; }

        /// <summary>
        /// ��ȡ�����ö�����Ϣ���ݼ���
        /// </summary>
        public virtual DbSet<TeamProfileEntity> TeamProfiles { get; set; }
    }
}

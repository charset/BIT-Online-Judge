namespace BITOJ.Core.Data
{
    using BITOJ.Core;
    using BITOJ.Core.Context;
    using BITOJ.Data;
    using BITOJ.Data.Entities;
    using System;

    using CoreAuthorizationMode = ContestAuthorizationMode;
    using CoreParticipationMode = ContestParticipationMode;
    using CoreUserGroup = UserGroup;
    using DatabaseAuthorizationMode = BITOJ.Data.Models.ContestAuthorizationMode;
    using DatabaseParticipationMode = BITOJ.Data.Models.ContestParticipationMode;
    using DatabaseUserGroup = BITOJ.Data.Entities.UserGroup;

    /// <summary>
    /// 提供对比赛数据的访问。
    /// </summary>
    public sealed class ContestDataProvider : IDisposable
    {
        /// <summary>
        /// 从给定的题目句柄创建 ContestDataProvider 类的新实例。
        /// </summary>
        /// <param name="handle">比赛句柄。</param>
        /// <param name="isReadonly">一个值，该值指示当前对象是否为只读对象。</param>
        /// <returns>从给定的题目句柄创建的 ContestDataProvider 对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ContestNotFoundException"/>
        public static ContestDataProvider Create(ContestHandle handle, bool isReadonly)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            ContestDataContext context = new ContestDataContextFactory().CreateContext();
            ContestEntity entity = context.QueryContestById(handle.ContestId);
            if (entity == null)
            {
                context.Dispose();
                throw new ContestNotFoundException(handle);
            }

            return new ContestDataProvider(context, entity, isReadonly);
        }

        private ContestDataContext m_context;
        private ContestEntity m_entity;
        private ContestAccessHandle m_access;
        private bool m_readonly;
        private bool m_disposed;

        /// <summary>
        /// 使用给定的比赛实体对象创建 ContestDataProvider 类的新实例。
        /// </summary>
        /// <param name="context">数据上下文对象。</param>
        /// <param name="entity">比赛实体对象。</param>
        /// <param name="access">比赛数据底层访问对象。</param>
        /// <param name="isReadonly">一个值，该值指示当前对象是否为只读对象。</param>
        /// <exception cref="ArgumentNullException"/>
        private ContestDataProvider(ContestDataContext context, ContestEntity entity, bool isReadonly)
        {
            m_context = context ?? throw new ArgumentNullException(nameof(context));
            m_entity = entity ?? throw new ArgumentNullException(nameof(entity));
            m_access = new ContestAccessHandle(entity);
            m_readonly = isReadonly;
            m_disposed = false;
        }

        ~ContestDataProvider()
        {
            Dispose();
        }

        /// <summary>
        /// 检查当前对象是否为只读。
        /// </summary>
        private void CheckAccess()
        {
            if (m_disposed)
                throw new ObjectDisposedException(GetType().Name);
            if (m_readonly)
                throw new InvalidOperationException("试图向只读对象写入内容。");
        }

        /// <summary>
        /// 获取比赛 ID。
        /// </summary>
        public int ContestId
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.Id;
        }

        /// <summary>
        /// 获取或设置比赛的标题。
        /// </summary>
        public string Title
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.Title;
            set
            {
                CheckAccess();
                m_entity.Title = value;
            }
        }

        /// <summary>
        /// 获取或设置比赛创建者。
        /// </summary>
        public string Creator
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.Creator;
            set
            {
                CheckAccess();
                m_entity.Creator = value;
            }
        }

        /// <summary>
        /// 获取或设置比赛的 Announcement。
        /// </summary>
        public string Announcement
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_access.Configuration.Announcement;
            set
            {
                CheckAccess();
                m_access.Configuration.Announcement = value;
            }
        }

        /// <summary>
        /// 获取或设置参与比赛所需的最低用户权限。
        /// </summary>
        public CoreUserGroup AuthorizationGroup
        {
            get => m_disposed 
                ? throw new ObjectDisposedException(GetType().Name) : 
                (CoreUserGroup)m_access.Configuration.Authorization.AuthorizationGroup;
            set
            {
                CheckAccess();
                m_access.Configuration.Authorization.AuthorizationGroup = (DatabaseUserGroup)value;
            }
        }

        /// <summary>
        /// 获取比赛的创建时间。
        /// </summary>
        public DateTime CreationTime
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.CreationTime;
        }

        /// <summary>
        /// 获取或设置比赛的开始时间。
        /// </summary>
        public DateTime StartTime
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.StartTime;
            set
            {
                CheckAccess();
                m_entity.StartTime = value;
            }
        }

        /// <summary>
        /// 获取或设置比赛的结束时间。
        /// </summary>
        public DateTime EndTime
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.EndTime;
            set
            {
                CheckAccess();
                m_entity.EndTime = value;
            }
        }

        /// <summary>
        /// 获取比赛的状态。
        /// </summary>
        public ContestStatus Status
        {
            get
            {
                DateTime now = DateTime.Now;
                if (now < StartTime)
                {
                    return ContestStatus.Pending;
                }
                else if (now <= EndTime)
                {
                    return ContestStatus.Running;
                }
                else
                {
                    return ContestStatus.Ended;
                }
            }
        }

        /// <summary>
        /// 获取或设置比赛的参与模式。
        /// </summary>
        public CoreParticipationMode ParticipationMode
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : (CoreParticipationMode)m_access.Configuration.Authorization.ParticipationMode;
            set
            {
                CheckAccess();
                m_access.Configuration.Authorization.ParticipationMode = (DatabaseParticipationMode)value;
            }
        }

        /// <summary>
        /// 获取或设置比赛的身份验证模式。
        /// </summary>
        public CoreAuthorizationMode AuthorizationMode
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : (CoreAuthorizationMode)m_access.Configuration.Authorization.AuthorizationMode;
            set
            {
                CheckAccess();
                m_access.Configuration.Authorization.AuthorizationMode = (DatabaseAuthorizationMode)value;
            }
        }

        /// <summary>
        /// 获取或设置比赛的密码哈希值。
        /// </summary>
        internal byte[] PasswordHash
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : m_access.Configuration.Authorization.PasswordHash;
            set
            {
                CheckAccess();
                m_access.Configuration.Authorization.PasswordHash = value;
            }
        }

        /// <summary>
        /// 获取当前比赛中的所有题目。
        /// </summary>
        /// <returns>一个数组，其中包含了当前比赛中的所有题目的题目句柄。</returns>
        /// <exception cref="ObjectDisposedException"/>
        public ProblemHandle[] GetProblems()
        {
            if (m_disposed)
                throw new ObjectDisposedException(GetType().Name);

            string[] problemIds = m_access.GetProblems();
            ProblemHandle[] handles = new ProblemHandle[problemIds.Length];

            for (int i = 0; i < handles.Length; ++i)
            {
                handles[i] = new ProblemHandle(problemIds[i]);
            }

            return handles;
        }

        /// <summary>
        /// 获取所有已经通过比赛身份验证的用户名单。
        /// </summary>
        /// <returns>所有已经通过比赛身份验证的用户句柄。</returns>
        internal UserHandle[] GetAuthorizedUsers()
        {
            UserHandle[] handles = new UserHandle[m_access.Configuration.Authorization.AuthorizedUsers.Count];

            int i = 0;
            foreach (string username in m_access.Configuration.Authorization.AuthorizedUsers)
            {
                handles[i++] = new UserHandle(username);
            }

            return handles;
        }

        /// <summary>
        /// 获取所有已经通过比赛身份验证的队伍名单。
        /// </summary>
        /// <returns>所有已经通过比赛身份验证的队伍句柄。</returns>
        internal TeamHandle[] GetAuthorizedTeams()
        {
            TeamHandle[] handles = new TeamHandle[m_access.Configuration.Authorization.AuthorizedTeams.Count];

            int i = 0;
            foreach (int teamId in m_access.Configuration.Authorization.AuthorizedTeams)
            {
                handles[i++] = new TeamHandle(teamId);
            }

            return handles;
        }

        /// <summary>
        /// 将给定的题目加入到当前比赛中。
        /// </summary>
        /// <param name="handle">要加入的题目。</param>
        /// <exception cref="ArgumentNullException"/>
        public void AddProblem(ProblemHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            CheckAccess();
            m_access.AddProblem(handle.ProblemId);
        }

        /// <summary>
        /// 从当前比赛中移除给定的题目。
        /// </summary>
        /// <param name="handle">要移除的题目句柄。</param>
        /// <exception cref="ArgumentNullException"/>
        public void RemoveProblem(ProblemHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            CheckAccess();
            m_access.RemoveProblem(handle.ProblemId);
        }

        /// <summary>
        /// 将给定的用户句柄添加至当前比赛的白名单或黑名单中。
        /// </summary>
        /// <param name="user">要添加的用户句柄。</param>
        /// <exception cref="ArgumentNullException"/>
        public void AddUserToAuthorizedList(UserHandle user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            m_access.Configuration.Authorization.AuthorizedUsers.Add(user.Username);
        }

        /// <summary>
        /// 将给定的用户句柄从当前比赛的白名单或黑名单中移除。
        /// </summary>
        /// <param name="user">要移除的用户。</param>
        /// <exception cref="ArgumentNullException"/>
        public void RemoveUserFromAuthorizedList(UserHandle user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            CheckAccess();

            foreach (string username in m_access.Configuration.Authorization.AuthorizedUsers)
            {
                if (string.Compare(username, user.Username, false) == 0)
                {
                    m_access.Configuration.Authorization.AuthorizedUsers.Remove(username);
                }
            }
        }

        /// <summary>
        /// 将给定的队伍句柄添加至当前比赛的白名单或黑名单中。
        /// </summary>
        /// <param name="team">要添加的队伍句柄。</param>
        /// <exception cref="ArgumentNullException"/>
        public void AddTeamToAuthorizedList(TeamHandle team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team));

            CheckAccess();
            m_access.Configuration.Authorization.AuthorizedTeams.Add(team.TeamId);
        }

        /// <summary>
        /// 将给定的队伍从当前比赛的白名单或黑名单中移除。
        /// </summary>
        /// <param name="team">要移除的队伍句柄。</param>
        public void RemoveTeamFromAuthorizedList(TeamHandle team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team));

            CheckAccess();

            foreach (int teamId in m_access.Configuration.Authorization.AuthorizedTeams)
            {
                if (teamId == team.TeamId)
                {
                    m_access.Configuration.Authorization.AuthorizedTeams.Remove(teamId);
                }
            }
        }

        /// <summary>
        /// 获取下一个为该比赛分配的题目 ID。
        /// </summary>
        /// <returns>下一个为该比赛分配的题目 ID。</returns>
        public string PeekNextProblemId()
        {
            return string.Format("BIT_CONTEST_{0}_{1}", ContestId, m_access.GetProblems().Length);
        }

        /// <summary>
        /// 将挂起的更改写入文件系统及数据库中。
        /// </summary>
        public void Save()
        {
            if (!m_disposed && !m_readonly)
            {
                m_context.SaveChanges();
                m_access.Save();
            }
        }

        /// <summary>
        /// 释放当前对象占有的所有资源。
        /// </summary>
        public void Dispose()
        {
            if (!m_disposed)
            {
                Save();
                m_context.Dispose();
                m_access.Dispose();

                m_disposed = true;
            }
        }
    }
}

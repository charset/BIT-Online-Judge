namespace BITOJ.Core
{
    using BITOJ.Core.Context;
    using BITOJ.Core.Data;
    using BITOJ.Core.Data.Queries;
    using BITOJ.Data;
    using BITOJ.Data.Entities;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using NativeUserGroup = BITOJ.Data.Entities.UserGroup;
    using NativeUserSex = BITOJ.Data.Entities.UserSex;

    /// <summary>
    /// 对 BITOJ 的用户数据提供访问服务。
    /// </summary>
    public sealed class UserManager
    {
        private static UserManager ms_default;
        private static object ms_lock;

        /// <summary>
        /// 获取当前 AppDomain 中的唯一 UserManager 对象。
        /// </summary>
        public static UserManager Default
        {
            get
            {
                if (ms_default == null)
                {
                    lock (ms_lock)
                    {
                        if (ms_default == null)
                        {
                            ms_default = new UserManager();
                        }
                    }
                }
                return ms_default;
            }
        }

        static UserManager()
        {
            ms_default = null;
            ms_lock = new object();
        }

        private UserDataContextFactory m_factory;

        /// <summary>
        /// 初始化 UserManager 类的新实例。
        /// </summary>
        private UserManager()
        {
            m_factory = new UserDataContextFactory();
        }

        ~UserManager()
        {
        }

        /// <summary>
        /// 在用户数据库中创建一个新的用户实体对象。
        /// </summary>
        /// <param name="username">新用户的用户名。</param>
        /// <returns>新创建的用户的句柄。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UsernameAlreadyExistsException"/>
        public UserHandle CreateUser(string username)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (IsUserExist(username))
                throw new UsernameAlreadyExistsException(new UserHandle(username));

            UserProfileEntity entity = new UserProfileEntity()
            {
                Username = username,
            };
            // 将实体数据对象添加到数据库中。
            m_factory.WithContext(context =>
            {
                context.AddUserProfileEntity(entity);
                context.SaveChanges();
            });

            return new UserHandle(username);
        }

        /// <summary>
        /// 在数据库中创建一个新的队伍实体对象。
        /// </summary>
        /// <returns>新创建的队伍句柄。</returns>
        public TeamHandle CreateTeam()
        {
            TeamProfileEntity entity = new TeamProfileEntity();
            m_factory.WithContext(context =>
            {
                entity = context.AddTeamProfileEntity(entity);
                context.SaveChanges();
            });

            return TeamHandle.FromTeamEntity(entity);
        }

        /// <summary>
        /// 使用指定的用户名查询用户。
        /// </summary>
        /// <param name="username">要查询的用户名。</param>
        /// <returns>查找到的用户的用户句柄。若未找到这样的用户，返回 null。</returns>
        /// <exception cref="ArgumentNullException"/>
        public UserHandle QueryUserByName(string username)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));

            UserProfileEntity entity = m_factory.WithContext(context => context.QueryUserProfileEntity(username));
            if (entity == null)
            {
                return null;
            }

            return UserHandle.FromUserProfileEntity(entity);
        }

        /// <summary>
        /// 使用指定的 Team ID 查询队伍。
        /// </summary>
        /// <param name="teamId">队伍 ID。</param>
        /// <returns>查找到的队伍的队伍句柄。若未找到这样的队伍，返回 null。</returns>
        public TeamHandle QueryTeamById(int teamId)
        {
            TeamProfileEntity entity = m_factory.WithContext(context => context.QueryTeamProfileEntity(teamId));
            if (entity == null)
            {
                return null;
            }

            return new TeamHandle(teamId);
        }

        /// <summary>
        /// 根据指定的查询参数查询用户句柄。
        /// </summary>
        /// <param name="query">查询参数。</param>
        /// <returns>一个包含了所有满足查询条件的用户句柄的查询结果对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        public ReadOnlyCollection<UserHandle> QueryUsers(UserQueryParameter query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (query.QueryByOrganization && query.Organization == null)
                throw new ArgumentNullException(nameof(query.Organization));

            bool hasQuery = query.QueryByOrganization || query.QueryByUsergroup;
            if (!hasQuery)
            {
                // 没有查询参数。返回空列表。
                return new ReadOnlyCollection<UserHandle>(new List<UserHandle>());
            }
            else
            {
                return m_factory.WithContext(context =>
                {
                    IQueryable<UserProfileEntity> profiles = context.GetAllUserProfiles();
                    if (query.QueryByOrganization)
                    {
                        profiles = UserDataContext.QueryUserProfileEntitiesByOrganization(profiles, query.Organization);
                    }
                    if (query.QueryBySex)
                    {
                        profiles = UserDataContext.QueryUserProfileEntitiesBySex(profiles, (NativeUserSex)query.Sex);
                    }
                    if (query.QueryByUsergroup)
                    {
                        profiles = UserDataContext.QueryUserProfileEntitiesByUsergroup(profiles,
                            (NativeUserGroup)query.UserGroup);
                    }

                    // 对查询结果进行排序以准备随时分页。
                    profiles.OrderBy(item => item.Username);

                    if (query.EnablePageQuery)
                    {
                        profiles = profiles.Page(query.PageQuery);
                    }

                    List<UserHandle> handles = new List<UserHandle>();
                    foreach (UserProfileEntity ent in profiles)
                    {
                        handles.Add(UserHandle.FromUserProfileEntity(ent));
                    }

                    return new ReadOnlyCollection<UserHandle>(handles);
                });
            }
        }

        /// <summary>
        /// 根据指定的队伍查询参数查询队伍句柄。
        /// </summary>
        /// <param name="query">查询参数。</param>
        /// <returns>查询到的队伍句柄结果对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        public ReadOnlyCollection<TeamHandle> QueryTeams(TeamQueryParameter query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (query.QueryByName && query.Name == null)
                throw new ArgumentNullException(nameof(query.Name));
            if (query.QueryByLeader && query.Leader == null)
                throw new ArgumentNullException(nameof(query.Leader));

            return m_factory.WithContext(context =>
            {
                IQueryable<TeamProfileEntity> set =  context.GetAllTeamProfiles();
                if (query.QueryByName)
                {
                    set = UserDataContext.QueryTeamProfileEntityByName(set, query.Name);
                }
                if (query.QueryByLeader)
                {
                    set = UserDataContext.QueryTeamProfileEntityByLeader(set, query.Leader);
                }

                // 对查询结果进行排序以准备分页。
                set = set.OrderBy(ent => ent.Id);

                if (query.EnablePageQuery)
                {
                    set = set.Page(query.PageQuery);
                }

                List<TeamHandle> handles = new List<TeamHandle>();
                foreach (TeamProfileEntity ent in set)
                {
                    handles.Add(TeamHandle.FromTeamEntity(ent));
                }

                return new ReadOnlyCollection<TeamHandle>(handles);
            });
        }

        /// <summary>
        /// 添加给定的用户到给定的队伍中。
        /// </summary>
        /// <param name="team">队伍句柄。</param>
        /// <param name="user">用户句柄。</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="TeamNotFoundException"/>
        /// <exception cref="UserNotFoundException"/>
        public void AddUserToTeam(TeamHandle team, UserHandle user)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team));
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            m_factory.WithContext(context =>
            {
                TeamProfileEntity teamEntity = context.QueryTeamProfileEntity(team.TeamId);
                if (teamEntity == null)
                    throw new TeamNotFoundException(team);

                UserProfileEntity userEntity = context.QueryUserProfileEntity(user.Username);
                if (userEntity == null)
                    throw new UserNotFoundException(user);

                teamEntity.Members.Add(userEntity);
                context.SaveChanges();
            });
        }

        /// <summary>
        /// 从给定的队伍中移除给定的用户。
        /// </summary>
        /// <param name="team">队伍句柄。</param>
        /// <param name="user">用户句柄。</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="TeamNotFoundException"/>
        /// <exception cref="UserNotFoundException"/>
        public void RemoveUserFromTeam(TeamHandle team, UserHandle user)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team));
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            m_factory.WithContext(context =>
            {
                TeamProfileEntity teamEntity = context.QueryTeamProfileEntity(team.TeamId);
                if (teamEntity == null)
                    throw new TeamNotFoundException(team);

                UserProfileEntity userEntity = context.QueryUserProfileEntity(user.Username);
                if (userEntity == null)
                    throw new UserNotFoundException(user);

                foreach (UserProfileEntity teamUser in teamEntity.Members)
                {
                    if (string.Compare(teamUser.Username, userEntity.Username, false) == 0)
                    {
                        teamEntity.Members.Remove(teamUser);
                        break;
                    }
                }

                context.SaveChanges();
            });
        }

        /// <summary>
        /// 从数据库中移除给定的队伍。
        /// </summary>
        /// <param name="team">要移除的队伍句柄。</param>
        /// <exception cref="ArgumentNullException"/>
        public void RemoveTeam(TeamHandle team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team));

            m_factory.WithContext(context =>
            {
                TeamProfileEntity entity = context.QueryTeamProfileEntity(team.TeamId);
                if (entity == null)
                {
                    return;
                }

                context.RemoveTeamProfileEntity(entity);
                context.SaveChanges();
            });
        }

        /// <summary>
        /// 测试一个用户名是否已经被占用。
        /// </summary>
        /// <param name="username">要测试的用户名。</param>
        /// <returns>被测试的用户名是否已经被占用。</returns>
        /// <exception cref="ArgumentNullException"/>
        public bool IsUserExist(string username)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));

            UserProfileEntity entity = m_factory.WithContext(context => context.QueryUserProfileEntity(username));
            return entity != null;
        }

        /// <summary>
        /// 检查给定句柄所对应的队伍 ID 是否存在于数据库中。
        /// </summary>
        /// <param name="teamId">队伍 ID。</param>
        /// <returns>一个值，该值指示给定的队伍 ID 是否存在于数据库中。</returns>
        public bool IsTeamExist(int teamId)
        {
            return m_factory.WithContext(context => context.QueryTeamProfileEntity(teamId) != null);
        }
    }
}

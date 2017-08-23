namespace BITOJ.Core
{
    using BITOJ.Core.Data.Queries;
    using BITOJ.Data;
    using BITOJ.Data.Entities;
    using System;
    using System.Collections.Generic;
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

        private UserDataContext m_context;

        /// <summary>
        /// 初始化 UserManager 类的新实例。
        /// </summary>
        private UserManager()
        {
            m_context = new UserDataContext();
        }

        ~UserManager()
        {
            m_context.Dispose();
        }

        /// <summary>
        /// 在用户数据库中创建一个新的用户实体对象。
        /// </summary>
        /// <param name="username">新用户的用户名。</param>
        /// <returns>新创建的用户的句柄。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="UsernameAlreadyExistsException"/>
        public UserHandle Create(string username)
        {
            if (username == null)
                throw new ArgumentNullException(nameof(username));
            if (IsUserExist(username))
                throw new UsernameAlreadyExistsException(username);

            UserProfileEntity entity = new UserProfileEntity()
            {
                Username = username,
            };
            // 将实体数据对象添加到数据库中。
            m_context.AddUserProfileEntity(entity);

            return new UserHandle(username);
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

            UserProfileEntity entity = m_context.QueryUserProfileEntity(username);
            if (entity == null)
            {
                return null;
            }

            return UserHandle.FromUserProfileEntity(entity);
        }

        /// <summary>
        /// 根据指定的查询参数查询用户句柄。
        /// </summary>
        /// <param name="query">查询参数。</param>
        /// <returns>一个列表，列表中包含了所有满足查询条件的用户句柄。</returns>
        /// <exception cref="ArgumentNullException"/>
        public IList<UserHandle> QueryUsers(UserQueryParameter query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (query.QueryByUsername && query.Username == null)
                throw new ArgumentNullException(nameof(query.Username));
            if (query.QueryByOrganization && query.Organization == null)
                throw new ArgumentNullException(nameof(query.Organization));

            if (query.QueryByUsername)
            {
                // 从用户名开始查找。
                // 由于满足条件的实体对象至多一个，做特殊化处理。
                UserProfileEntity profile = m_context.QueryUserProfileEntity(query.Username);
                if (profile == null)
                {
                    // 未找到符合要求的实体对象。
                    return new List<UserHandle>();
                }
                else
                {
                    // 检查其他条件是否满足。
                    if (query.QueryByOrganization && string.Compare(query.Organization, profile.Organization, false) != 0)
                    {
                        return new List<UserHandle>();
                    }

                    if (query.QueryBySex && profile.Sex != (NativeUserSex)query.Sex)
                    {
                        return new List<UserHandle>();
                    }

                    if (query.QueryByUsergroup && profile.UserGroup != (NativeUserGroup)query.UserGroup)
                    {
                        return new List<UserHandle>();
                    }

                    // 唯一的用户信息实体对象满足条件。
                    return new List<UserHandle>() { UserHandle.FromUserProfileEntity(profile) };
                }
            }
            else    // query.QueryByUsername == false
            {
                bool hasQuery = query.QueryByOrganization || query.QueryByUsergroup;
                if (!hasQuery)
                {
                    // 没有查询参数。返回空列表。
                    return new List<UserHandle>();
                }
                else
                {
                    IQueryable<UserProfileEntity> profiles = m_context.GetAllUserProfiles();
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

                    List<UserHandle> handles = new List<UserHandle>();
                    foreach (UserProfileEntity entity in profiles)
                    {
                        handles.Add(UserHandle.FromUserProfileEntity(entity));
                    }

                    return handles;
                }
            }
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

            UserProfileEntity entity = DataContext.QueryUserProfileEntity(username);
            return entity != null;
        }

        /// <summary>
        /// 获取数据上下文对象。
        /// </summary>
        internal UserDataContext DataContext
        {
            get { return m_context; }
        }
    }
}

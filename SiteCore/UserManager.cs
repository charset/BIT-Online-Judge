namespace BITOJ.Core
{
    using BITOJ.Data;
    using BITOJ.Data.Entities;
    using NativeUserGroup = BITOJ.Data.Entities.UserGroup;
    using BITOJ.Core.Data.Queries;
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        /// 获取数据上下文对象。
        /// </summary>
        internal UserDataContext DataContext
        {
            get { return m_context; }
        }
    }
}

namespace BITOJ.Core
{
    using BITOJ.Common.Cache;
    using BITOJ.Common.Cache.Settings;
    using BITOJ.Core.Context;
    using BITOJ.Core.Data.Queries;
    using BITOJ.Data;
    using BITOJ.Data.Entities;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// 提供对比赛数据的访问服务。
    /// </summary>
    public sealed class ContestManager
    {
        private static ContestManager ms_default;
        private static object ms_syncLock;

        private static readonly string ContestDirectory;
        private const string ContestDirectorySettingName = "contest_directory";

        static ContestManager()
        {
            ms_default = null;
            ms_syncLock = new object();

            // 加载比赛目录信息。
            using (FileSystemSettingProvider settings = new FileSystemSettingProvider())
            {
                if (settings.Contains(ContestDirectorySettingName))
                {
                    ContestDirectory = settings.Get<string>(ContestDirectorySettingName);
                }
                else
                {
                    // 加载默认比赛目录。
                    ContestDirectory = ApplicationDirectory.GetAppSubDirectory("Contests");
                }
            }
        }

        /// <summary>
        /// 获取全局默认 ContestManager 对象。
        /// </summary>
        public static ContestManager Default
        {
            get
            {
                if (ms_default == null)
                {
                    lock (ms_syncLock)
                    {
                        if (ms_default == null)
                        {
                            ms_default = new ContestManager();
                        }
                    }
                }

                return ms_default;
            }
        }

        private ContestDataContextFactory m_factory;

        /// <summary>
        /// 创建 ContestManager 类的新实例。
        /// </summary>
        private ContestManager()
        {
            m_factory = new ContestDataContextFactory();
        }

        ~ContestManager()
        {
        }

        /// <summary>
        /// 创建一个新的比赛并返回相应的比赛句柄。
        /// </summary>
        /// <returns></returns>
        public ContestHandle CreateContest()
        {
            // 创建新的比赛实体对象。
            return m_factory.WithContext(context =>
            {
                ContestEntity entity = new ContestEntity();
                entity = context.AddContest(entity);

                // 为新创建的比赛分配配置文件。
                entity.ContestConfigurationFile = string.Concat(ContestDirectory, "\\", entity.Id);
                context.SaveChanges();

                return ContestHandle.FromContestEntity(entity);
            });
        }

        /// <summary>
        /// 从给定的比赛 ID 创建 ContestHandle 类的新实例。
        /// </summary>
        /// <param name="contestId">比赛 ID。</param>
        /// <returns>从给定的比赛 ID 创建的 ContestHandle 类对象。若数据库中不存在这样的比赛实体对象，返回 null。</returns>
        public ContestHandle QueryContestById(int contestId)
        {
            return m_factory.WithContext(context =>
            {
                ContestEntity entity = context.QueryContestById(contestId);
                if (entity == null)
                {
                    return null;
                }
                else
                {
                    return ContestHandle.FromContestEntity(entity);
                }
            });
        }

        private static IQueryable<ContestEntity> DoQuery(ContestDataContext context, ContestQueryParameter query)
        {
            IQueryable<ContestEntity> set = context.QueryAllContests();

            if (query.QueryByTitle)
            {
                set = ContestDataContext.QueryContestsByTitle(set, query.Title);
            }
            if (query.QueryByCreator)
            {
                set = ContestDataContext.QueryContestsByCreator(set, query.Creator);
            }
            if (query.QueryByStatus)
            {
                switch (query.Status)
                {
                    case ContestStatus.Pending:
                        set = ContestDataContext.QueryUnstartedContests(set);
                        break;
                    case ContestStatus.Running:
                        set = ContestDataContext.QueryRunningContests(set);
                        break;
                    case ContestStatus.Ended:
                        set = ContestDataContext.QueryEndedContests(set);
                        break;
                    default:
                        throw new ArgumentException(nameof(query.Status));
                }
            }

            return set;
        }

        /// <summary>
        /// 使用给定的查询参数查询比赛句柄。
        /// </summary>
        /// <param name="query">封装查询参数的对象。</param>
        /// <returns>一个查询结果对象，其中包含了所有查询到的比赛句柄。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public ReadOnlyCollection<ContestHandle> QueryContests(ContestQueryParameter query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (query.QueryByTitle && query.Title == null)
                throw new ArgumentNullException(nameof(query.Title));
            if (query.QueryByCreator && query.Creator == null)
                throw new ArgumentNullException(nameof(query.Creator));

            return m_factory.WithContext(context =>
            {
                IQueryable<ContestEntity> set = DoQuery(context, query);

                // 对数据实体对象进行排序以准备执行分页操作。
                set = set.OrderByDescending(entity => entity.CreationTime);

                if (query.EnablePagedQuery)
                {
                    // 执行分页。
                    set = set.Page(query.PageQuery);
                }

                List<ContestHandle> handles = new List<ContestHandle>();
                foreach (ContestEntity ent in set)
                {
                    handles.Add(ContestHandle.FromContestEntity(ent));
                }

                return new ReadOnlyCollection<ContestHandle>(handles);
            });
        }

        /// <summary>
        /// 使用指定的查询参数计算满足条件的数据条目一共能填充多少页面。
        /// </summary>
        /// <param name="query">查询参数。</param>
        /// <returns>满足条件的数据条目能填充多少页面。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public int QueryPages(ContestQueryParameter query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (!query.EnablePagedQuery)
                throw new ArgumentException("给定的查询参数未启用分页。");
            if (query.PageQuery.ItemsPerPage <= 0)
                throw new ArgumentOutOfRangeException(nameof(query.PageQuery.ItemsPerPage));

            return m_factory.WithContext(context =>
            {
                IQueryable<ContestEntity> set = DoQuery(context, query);
                return set.GetPages(query.PageQuery.ItemsPerPage);
            });
        }

        /// <summary>
        /// 删除给定比赛 ID 所对应的比赛。
        /// </summary>
        /// <param name="contestId">比赛 ID。</param>
        public void RemoveContest(int contestId)
        {
            m_factory.WithContext(context =>
            {
                ContestEntity entity = context.QueryContestById(contestId);
                if (entity != null)
                {
                    // 删除本地配置文件。
                    File.Delete(entity.ContestConfigurationFile);

                    // 从数据上下文中删除数据实体对象。
                    context.RemoveContest(entity);
                    context.SaveChanges();
                }
            });
        }

        /// <summary>
        /// 检查给定比赛 ID 所代表的比赛是否存在。
        /// </summary>
        /// <param name="contestId">比赛 ID 。</param>
        /// <returns>一个值，该值指示给定比赛 ID 所代表的比赛是否存在。</returns>
        public bool IsContestExist(int contestId)
        {
            return m_factory.WithContext(context => context.Contests.Find(contestId) != null);
        }
    }
}

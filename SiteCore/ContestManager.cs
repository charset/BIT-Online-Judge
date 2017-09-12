namespace BITOJ.Core
{
    using BITOJ.Common.Cache;
    using BITOJ.Common.Cache.Settings;
    using BITOJ.Core.Data.Queries;
    using BITOJ.Data;
    using BITOJ.Data.Entities;
    using System;
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

        private ContestDataContext m_context;

        /// <summary>
        /// 创建 ContestManager 类的新实例。
        /// </summary>
        private ContestManager()
        {
            m_context = new ContestDataContext();
        }

        ~ContestManager()
        {
            m_context.SaveChanges();
            m_context.Dispose();
        }

        /// <summary>
        /// 创建一个新的比赛并返回相应的比赛句柄。
        /// </summary>
        /// <returns></returns>
        public ContestHandle CreateContest()
        {
            // 创建新的比赛实体对象。
            ContestEntity entity = new ContestEntity();
            entity = m_context.AddContest(entity);

            // 为新创建的比赛分配配置文件。
            entity.ContestConfigurationFile = string.Concat(ContestDirectory, "\\", entity.Id);
            m_context.SaveChanges();

            return ContestHandle.FromContestEntity(entity);
        }

        /// <summary>
        /// 从给定的比赛 ID 创建 ContestHandle 类的新实例。
        /// </summary>
        /// <param name="contestId">比赛 ID。</param>
        /// <returns>从给定的比赛 ID 创建的 ContestHandle 类对象。若数据库中不存在这样的比赛实体对象，返回 null。</returns>
        public ContestHandle QueryContestById(int contestId)
        {
            ContestEntity entity = m_context.QueryContestById(contestId);
            if (entity == null)
            {
                return null;
            }

            return ContestHandle.FromContestEntity(entity);
        }

        /// <summary>
        /// 使用给定的查询参数查询比赛句柄。
        /// </summary>
        /// <param name="query">封装查询参数的对象。</param>
        /// <returns>一个查询结果对象，其中包含了所有查询到的比赛句柄。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        public IPageableQueryResult<ContestHandle> QueryContests(ContestQueryParameter query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (query.QueryByTitle && query.Title == null)
                throw new ArgumentNullException(nameof(query.Title));
            if (query.QueryByCreator && query.Creator == null)
                throw new ArgumentNullException(nameof(query.Creator));

            IQueryable<ContestEntity> set = m_context.QueryAllContests();
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

            // 对数据实体对象进行排序以准备随时执行分页操作并显示。
            set = set.OrderByDescending(entity => entity.CreationTime);

            PageableQueryResult<ContestEntity> originResult = new PageableQueryResult<ContestEntity>(set);
            return new MappedQueryResult<ContestEntity, ContestHandle>(originResult,
                entity => ContestHandle.FromContestEntity(entity));
        }

        /// <summary>
        /// 删除给定比赛 ID 所对应的比赛。
        /// </summary>
        /// <param name="contestId">比赛 ID。</param>
        public void RemoveContest(int contestId)
        {
            ContestEntity entity = m_context.QueryContestById(contestId);
            if (entity != null)
            {
                m_context.RemoveContest(entity);

                // 删除本地配置文件。
                File.Delete(entity.ContestConfigurationFile);
            }
        }

        /// <summary>
        /// 检查给定比赛 ID 所代表的比赛是否存在。
        /// </summary>
        /// <param name="contestId">比赛 ID 。</param>
        /// <returns>一个值，该值指示给定比赛 ID 所代表的比赛是否存在。</returns>
        public bool IsContestExist(int contestId)
        {
            return m_context.Contests.Find(contestId) != null;
        }

        /// <summary>
        /// 获取数据上下文对象。
        /// </summary>
        internal ContestDataContext Context
        {
            get
            {
                return m_context;
            }
        }
    }
}

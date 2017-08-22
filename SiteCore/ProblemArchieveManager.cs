namespace BITOJ.Core
{
    using BITOJ.Common.Cache;
    using BITOJ.Common.Cache.Settings;
    using BITOJ.Core.Data.Queries;
    using BITOJ.Data;
    using BITOJ.Data.Components;
    using BITOJ.Data.Entities;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// 对 BITOJ 主题目库提供管理、访问服务。
    /// </summary>
    public sealed class ProblemArchieveManager
    {
        private static ProblemArchieveManager ms_default;
        private static object ms_sync;   // 线程安全的单例模式的线程同步互斥锁对象。
        private static readonly string ms_archieveDirectory;
        private const string ArchieveDiretcorySettingName = "archieve_directory";

        /// <summary>
        /// 获取全局唯一的 ProblemArchieveManager 对象。
        /// </summary>
        public static ProblemArchieveManager Default
        {
            get
            {
                if (ms_default == null)
                {
                    lock (ms_sync)
                    {
                        if (ms_default == null)
                        {
                            ms_default = new ProblemArchieveManager();
                        }
                    }
                }
                return ms_default;
            }
        }

        static ProblemArchieveManager()
        {
            ms_default = null;
            ms_sync = new object();

            // 初始化本地主题目库根目录。
            FileSystemSettingProvider settings = new FileSystemSettingProvider();
            if (!settings.Contains(ArchieveDiretcorySettingName))
            {
                // 设置集中无根目录设置。设置为默认目录。
                ms_archieveDirectory = ApplicationDirectory.GetAppSubDirectory("ProblemArchieve");
            }
            else
            {
                ms_archieveDirectory = settings.Get<string>(ArchieveDiretcorySettingName);
            }
        }

        private ProblemArchieveDataContext m_context;       // 数据上下文

        /// <summary>
        /// 初始化 ProblemArchieveManager 类的新实例。
        /// </summary>
        private ProblemArchieveManager()
        {
            m_context = new ProblemArchieveDataContext();
        }

        ~ProblemArchieveManager()
        {
            m_context.Dispose();
        }

        /// <summary>
        /// 在主题目库中创建一道新题目并返回该题目的句柄。
        /// </summary>
        /// <returns></returns>
        public ProblemHandle CreateProblem()
        {
            ProblemEntity entity = new ProblemEntity();

            // 为题目创建文件系统目录。
            string directory = string.Concat(ms_archieveDirectory, "\\", entity.Id.ToString("D4"));
            Directory.CreateDirectory(directory);

            // 将题目实体对象添加至底层数据库中。
            m_context.AddProblemEntity(entity);

            // 创建句柄并返回。
            ProblemHandle handle = ProblemHandle.FromProblemEntity(entity);
            return handle;
        }

        /// <summary>
        /// 由给定的题目 ID 获取题目句柄对象。
        /// </summary>
        /// <param name="id">题目 ID。</param>
        /// <returns>具有给定题目 ID 的题目句柄对象。若主题库中不存在这样的题目，返回 null。</returns>
        public ProblemHandle GetProblemById(int id)
        {
            ProblemEntity entity = m_context.GetProblemEntityById(id);
            if (entity == null)
            {
                return null;
            }

            return ProblemHandle.FromProblemEntity(entity);
        }

        /// <summary>
        /// 使用指定的查询对象查询题目句柄。
        /// </summary>
        /// <param name="query">为查询提供参数。</param>
        /// <returns>一个列表，该列表中包含了所有的查询结果。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public IList<ProblemHandle> QueryProblems(ProblemArchieveQueryParameter query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (query.QueryByTitle && query.Title == null)
                throw new ArgumentNullException(nameof(query.Title));
            if (query.QueryBySource && query.Source == null)
                throw new ArgumentNullException(nameof(query.Source));
            if (query.QueryByAuthor && query.Author == null)
                throw new ArgumentNullException(nameof(query.Author));
            if (query.Page == null)
                throw new ArgumentNullException(nameof(query.Page));
            if (query.Page.Page <= 0 || query.Page.EntriesPerPage <= 0)
                throw new ArgumentOutOfRangeException(nameof(query.Page));
            
            IQueryable<ProblemEntity> set = m_context.GetAllProblemEntities();

            // 根据查询参数执行相应的查询，动态维护查询基础数据集。
            if (query.QueryByTitle)
            {
                set = ProblemArchieveDataContext.QueryProblemEntitiesByTitle(set, query.Title);
            }
            if (query.QueryBySource)
            {
                set = ProblemArchieveDataContext.QueryProblemEntitiesBySource(set, query.Source);
            }
            if (query.QueryByAuthor)
            {
                set = ProblemArchieveDataContext.QueryProblemEntitiesByAuthor(set, query.Author);
            }

            // 执行分页。
            set = set.Page(query.Page.Page, query.Page.EntriesPerPage);

            List<ProblemHandle> handleList = new List<ProblemHandle>();
            foreach (ProblemEntity entity in set)
            {
                handleList.Add(ProblemHandle.FromProblemEntity(entity));
            }

            return handleList;
        }

        /// <summary>
        /// 获取底层数据上下文对象。
        /// </summary>
        internal ProblemArchieveDataContext DataContext
        {
            get { return m_context; }
        }
    }
}

namespace BITOJ.Core
{
    using BITOJ.Common.Cache;
    using BITOJ.Common.Cache.Settings;
    using BITOJ.Core.Context;
    using BITOJ.Core.Data;
    using BITOJ.Core.Data.Queries;
    using BITOJ.Data;
    using BITOJ.Data.Entities;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using NativeOJSystem = BITOJ.Data.Entities.OJSystem;

    /// <summary>
    /// 对 BITOJ 题目库提供管理、访问服务。
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
            using (FileSystemSettingProvider settings = new FileSystemSettingProvider())
            {
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
        }

        private ProblemDataContextFactory m_factory;

        /// <summary>
        /// 初始化 ProblemArchieveManager 类的新实例。
        /// </summary>
        private ProblemArchieveManager()
        {
            m_factory = new ProblemDataContextFactory();
        }

        ~ProblemArchieveManager()
        {
        }

        /// <summary>
        /// 判断给定的题目是否已经存在于数据库中。
        /// </summary>
        /// <param name="problemId">题目编号。</param>
        /// <returns>一个值，该值指示题目是否已经存在于数据库中。</returns>
        /// <exception cref="ArgumentNullException"/>
        public bool IsProblemExist(string problemId)
        {
            if (problemId == null)
                throw new ArgumentNullException(nameof(problemId));

            return m_factory.WithContext(context =>
            {
                ProblemEntity entity = context.GetProblemEntityById(problemId);
                return entity != null;
            });
        }

        /// <summary>
        /// 在主题目库中创建一道新题目并返回该题目的句柄。
        /// </summary>
        /// <returns>创建的题目句柄。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ProblemAlreadyExistException"/>
        public ProblemHandle CreateProblem(string problemId)
        {
            if (problemId == null)
                throw new ArgumentNullException(nameof(problemId));
            if (IsProblemExist(problemId))
                throw new ProblemAlreadyExistException(new ProblemHandle(problemId));

            // 为题目创建文件系统目录。
            string directory = string.Concat(ms_archieveDirectory, "\\", problemId);
            Directory.CreateDirectory(directory);

            ProblemEntity entity = new ProblemEntity()
            {
                Id = problemId,
                ProblemDirectory = directory,
            };

            // 将题目实体对象添加至底层数据库中。
            m_factory.WithContext(context =>
            {
                context.AddProblemEntity(entity);
                context.SaveChanges();
            });

            // 创建句柄并返回。
            ProblemHandle handle = ProblemHandle.FromProblemEntity(entity);
            return handle;
        }

        /// <summary>
        /// 在题目库中使用给定的新题目 ID 创建已存在题目的精确副本。
        /// </summary>
        /// <param name="newProblemId">要创建的新题目 ID。</param>
        /// <param name="oldProblemId">已存在的题目 ID。</param>
        /// <returns>新题目句柄。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ProblemNotExistException"/>
        /// <exception cref="ProblemAlreadyExistException"/>
        public ProblemHandle CloneProblem(string newProblemId, string oldProblemId)
        {
            if (newProblemId == null)
                throw new ArgumentNullException(nameof(newProblemId));
            if (oldProblemId == null)
                throw new ArgumentNullException(nameof(oldProblemId));

            ProblemHandle oldHandle = new ProblemHandle(oldProblemId);
            ProblemHandle newHandle = CreateProblem(newProblemId);

            using (ProblemDataProvider newData = ProblemDataProvider.Create(newHandle, false))
            {
                using (ProblemDataProvider oldData = ProblemDataProvider.Create(oldHandle, true))
                {
                    oldData.CopyTo(newData);
                    
                    // 维护评测数据集源链接。
                    if (oldData.TestSuiteOrigin != null)
                    {
                        newData.TestSuiteOrigin = oldData.TestSuiteOrigin;
                    }
                    else
                    {
                        newData.TestSuiteOrigin = oldData.ProblemId;
                    }
                }
            }

            return newHandle;
        }

        /// <summary>
        /// 由给定的题目 ID 获取题目句柄对象。
        /// </summary>
        /// <param name="id">题目 ID。</param>
        /// <returns>具有给定题目 ID 的题目句柄对象。若主题库中不存在这样的题目，返回 null。</returns>
        /// <exception cref="ArgumentNullException"/>
        public ProblemHandle GetProblemById(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            return m_factory.WithContext(context =>
            {
                ProblemEntity entity = context.GetProblemEntityById(id);
                if (entity == null)
                {
                    return null;
                }
                else
                {
                    return ProblemHandle.FromProblemEntity(entity);
                }
            });
        }

        private static IQueryable<ProblemEntity> DoQuery(ProblemDataContext context, ProblemArchieveQueryParameter query)
        {
            IQueryable<ProblemEntity> set = context.GetAllProblemEntities();

            // 根据查询参数执行相应的查询，动态维护查询基础数据集。
            if (query.QueryByTitle)
            {
                set = ProblemDataContext.QueryProblemEntitiesByTitle(set, query.Title);
            }
            if (query.QueryBySource)
            {
                set = ProblemDataContext.QueryProblemEntitiesBySource(set, query.Source);
            }
            if (query.QueryByAuthor)
            {
                set = ProblemDataContext.QueryProblemEntitiesByAuthor(set, query.Author);
            }
            if (query.QueryByOrigin)
            {
                set = ProblemDataContext.QueryProblemEntitiesByOrigin(set, (NativeOJSystem)query.Origin);
            }
            if (query.QueryByContestId)
            {
                set = ProblemDataContext.QueryProblemEntitiesByContestId(set, query.ContestId);
            }

            return set;
        }

        /// <summary>
        /// 使用指定的查询对象查询题目句柄。
        /// </summary>
        /// <param name="query">为查询提供参数。</param>
        /// <returns>一个包含了所有的查询结果的查询结果对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public ReadOnlyCollection<ProblemHandle> QueryProblems(ProblemArchieveQueryParameter query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (query.QueryByTitle && query.Title == null)
                throw new ArgumentNullException(nameof(query.Title));
            if (query.QueryBySource && query.Source == null)
                throw new ArgumentNullException(nameof(query.Source));
            if (query.QueryByAuthor && query.Author == null)
                throw new ArgumentNullException(nameof(query.Author));
            
            return m_factory.WithContext(context =>
            {
                IQueryable<ProblemEntity> set = DoQuery(context, query);

                // 对数据集进行排序以准备随时执行分页。
                set = set.OrderBy(entity => entity.Id);

                if (query.EnablePageQuery)
                {
                    set = set.Page(query.PageQuery);
                }

                List<ProblemHandle> handles = new List<ProblemHandle>();
                foreach (ProblemEntity ent in set)
                {
                    handles.Add(ProblemHandle.FromProblemEntity(ent));
                }

                return new ReadOnlyCollection<ProblemHandle>(handles);
            });
        }

        /// <summary>
        /// 使用指定的查询对象计算满足条件的数据条目能填充多少页面。
        /// </summary>
        /// <param name="query">查询参数。</param>
        /// <returns>满足查询条件的数据条目能填充的页面数量。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public int QueryPages(ProblemArchieveQueryParameter query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (!query.EnablePageQuery)
                throw new ArgumentException("给定的查询参数未启用分页。");
            if (query.PageQuery.ItemsPerPage <= 0)
                throw new ArgumentOutOfRangeException(nameof(query.PageQuery.ItemsPerPage));

            return m_factory.WithContext(context =>
            {
                IQueryable<ProblemEntity> set = DoQuery(context, query);
                return set.GetPages(query.PageQuery.ItemsPerPage);
            });
        }

        /// <summary>
        /// 从题目库中删除给定的题目。
        /// </summary>
        /// <param name="problemId">要删除的题目的题目 ID。</param>
        /// <exception cref="ArgumentNullException"/>
        public void RemoveProblem(string problemId)
        {
            if (problemId == null)
                throw new ArgumentNullException(nameof(problemId));

            m_factory.WithContext(context =>
            {
                ProblemEntity entity = context.GetProblemEntityById(problemId);
                if (entity != null)
                {
                    // 删除本地文件系统文件。
                    Directory.Delete(entity.ProblemDirectory, true);
                    // 从数据库中移除。
                    context.RemoveProblemEntity(entity);
                }
            });
        }
    }
}

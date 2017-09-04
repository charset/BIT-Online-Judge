namespace BITOJ.Core
{
    using BITOJ.Common.Cache;
    using BITOJ.Common.Cache.Settings;
    using BITOJ.Core.Data.Queries;
    using BITOJ.Data;
    using BITOJ.Data.Components;
    using BITOJ.Data.Entities;
    using DatabaseVerdictStatus = BITOJ.Data.Entities.SubmissionVerdictStatus;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 对用户提交提供访问服务。
    /// </summary>
    public sealed class SubmissionManager
    {
        private static SubmissionManager ms_default;
        private static object ms_syncLock;
        private const string SubmissionCodeFileDirectorySettingName = "submission_code_file_directory";
        private static readonly string SubmissionCodeFilesDirectory;

        static SubmissionManager()
        {
            ms_default = null;
            ms_syncLock = new object();

            // 加载用户提交代码文件目录信息。
            FileSystemSettingProvider settings = new FileSystemSettingProvider();
            if (settings.Contains(SubmissionCodeFileDirectorySettingName))
            {
                SubmissionCodeFilesDirectory = settings.Get<string>(SubmissionCodeFileDirectorySettingName);
            }
            else
            {
                // 加载默认设置。
                SubmissionCodeFilesDirectory = ApplicationDirectory.GetAppSubDirectory("Submissions");
            }
        }

        /// <summary>
        /// 获取全局 SubmissionManager 对象。
        /// </summary>
        public static SubmissionManager Default
        {
            get
            {
                if (ms_default == null)
                {
                    lock (ms_syncLock)
                    {
                        if (ms_default == null)
                        {
                            ms_default = new SubmissionManager();
                        }
                    }
                }

                return ms_default;
            }
        }

        private SubmissionDataContext m_context;

        /// <summary>
        /// 创建 SubmissionManager 类的新实例。
        /// </summary>
        private SubmissionManager()
        {
            m_context = new SubmissionDataContext();
        }

        /// <summary>
        /// 在数据库中创建一个新的用户提交项目。
        /// </summary>
        /// <returns></returns>
        public SubmissionHandle CreateSubmission()
        {
            SubmissionEntity entity = new SubmissionEntity();
            entity = m_context.AddSubmissionEntity(entity);

            return new SubmissionHandle(entity.Id);
        }

        /// <summary>
        /// 使用给定的提交 ID 查询提交句柄。
        /// </summary>
        /// <param name="submissionId">提交 ID。</param>
        /// <returns>查询到的提交句柄。若给定的提交 ID 不存在，返回 null。</returns>
        public SubmissionHandle QuerySubmissionById(int submissionId)
        {
            SubmissionEntity entity = m_context.QuerySubmissionEntityById(submissionId);
            if (entity == null)
            {
                return null;
            }

            return SubmissionHandle.FromSubmissionEntity(entity);
        }

        /// <summary>
        /// 使用给定的查询参数查询用户提交。
        /// </summary>
        /// <param name="query">查询参数。</param>
        /// <returns>一个列表，该列表包含了所有的查询结果。</returns>
        /// <exception cref="ArgumentNullException"/>
        public IList<SubmissionHandle> QuerySubmissions(SubmissionQueryParameter query)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));
            if (query.QueryByProblemId && query.ProblemId == null)
                throw new ArgumentNullException(nameof(query.ProblemId));
            if (query.QueryByUsername && query.Username == null)
                throw new ArgumentNullException(nameof(query.Username));

            var set = m_context.QuerySubmissionEntities(query.GetQueryHandle());
            
            // 将所有实体对象按照创建时间排序。
            if (query.OrderByDescending)
            {
                set = set.OrderByDescending(item => item.CreationTimestamp);
            }
            else
            {
                set = set.OrderBy(item => item.CreationTimestamp);
            }
            // 然后执行分页。
            set = set.Page(query.Page.Page, query.Page.EntriesPerPage);

            List<SubmissionHandle> handles = new List<SubmissionHandle>();
            foreach (SubmissionEntity entity in set)
            {
                handles.Add(SubmissionHandle.FromSubmissionEntity(entity));
            }

            return handles;
        }

        /// <summary>
        /// 获取当前正处在等待评测状态的用户提交中创建时间最早的一条用户提交的句柄。
        /// </summary>
        /// <returns>当前正处在等待评测状态的用户提交中创建时间最早的一条用户提交的句柄。若不存在这样的用户提交，返回 null。</returns>
        public SubmissionHandle GetPendingListFront()
        {
            SubmissionQueryHandle query = new SubmissionQueryHandle()
            {
                VerdictStatus = DatabaseVerdictStatus.Pending,
                UseVerdictStatus = true
            };

            IList<SubmissionEntity> set = m_context.QuerySubmissionEntities(query)
                .OrderBy(entity => entity.CreationTimestamp).Take(1).ToList();

            if (set.Count == 0)
            {
                return null;
            }
            else
            {
                return SubmissionHandle.FromSubmissionEntity(set[0]);
            }
        }

        /// <summary>
        /// 检查某个用户提交是否存在。
        /// </summary>
        /// <param name="submissionId">用户提交 ID。</param>
        /// <returns>一个值，该值指示给定的用户提交是否存在。</returns>
        public bool IsSubmissionExist(int submissionId)
        {
            SubmissionEntity entity = m_context.QuerySubmissionEntityById(submissionId);
            return entity != null;
        }

        /// <summary>
        /// 获取底层数据上下文对象。
        /// </summary>
        internal SubmissionDataContext Context { get; }
    }
}

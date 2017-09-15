namespace BITOJ.Core.Data
{
    using BITOJ.Core;
    using BITOJ.Data.Entities;

    using CoreSubmissionLanguage = SubmissionLanguage;
    using CoreSubmissionVerdict = SubmissionVerdict;
    using CoreSubmissionVerdictStatus = SubmissionVerdictStatus;
    using DatabaseSubmissionLanguage = BITOJ.Data.Entities.SubmissionLanguage;
    using DatabaseSubmissionVerdict = BITOJ.Data.Entities.SubmissionVerdict;
    using DatabaseSubmissionVerdictStatus = BITOJ.Data.Entities.SubmissionVerdictStatus;

    using System;
    using System.IO;

    /// <summary>
    /// 提供对用户提交数据的访问。
    /// </summary>
    public sealed class SubmissionDataProvider : IDisposable
    {
        /// <summary>
        /// 从给定的提交句柄对象创建 SubmissionDataProvider 类的新实例。
        /// </summary>
        /// <param name="handle">提交句柄对象。</param>
        /// <param name="isReadonly">一个值，该值指示创建的对象是否为只读。</param>
        /// <returns>创建的 SubmissionDataProvider 类对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="SubmissionNotExistException"/>
        public static SubmissionDataProvider Create(SubmissionHandle handle, bool isReadonly)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            SubmissionEntity entity = SubmissionManager.Default.Context.QuerySubmissionEntityById(handle.SubmissionId);
            if (entity == null)
                throw new SubmissionNotExistException(handle);

            return new SubmissionDataProvider(entity, isReadonly);
        }

        private SubmissionEntity m_entity;
        private string m_code;
        private bool m_readonly;
        private bool m_dirty;
        private bool m_disposed;

        /// <summary>
        /// 使用给定的用户提交实体对象以及一个指示当前对象是否为只读对象的值创建 SubmissionDataProvider 类的新实例。
        /// </summary>
        /// <param name="entity">用户提交实体对象。</param>
        /// <param name="isReadonly">一个值，该值指示当前对象是否为只读对象。</param>
        /// <exception cref="ArgumentNullException"/>
        private SubmissionDataProvider(SubmissionEntity entity, bool isReadonly)
        {
            m_entity = entity ?? throw new ArgumentNullException(nameof(entity));
            m_code = null;
            m_readonly = isReadonly;
            m_dirty = false;
            m_disposed = false;
        }

        ~SubmissionDataProvider()
        {
            Dispose();
        }

        /// <summary>
        /// 检查当前对象是否具有写权限。
        /// </summary>
        private void CheckAccess()
        {
            if (m_disposed)
                throw new ObjectDisposedException(GetType().Name);
            if (m_readonly)
                throw new InvalidCastException("试图在只读对象上执行写操作。");
        }

        /// <summary>
        /// 获取当前用户提交的 ID。
        /// </summary>
        public int SubmissionId
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.Id;
        }

        /// <summary>
        /// 获取或设置远程 OJ 系统的提交 ID。
        /// </summary>
        public int RemoteSubmissionId
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.RemoteSubmissionId;
            set
            {
                CheckAccess();
                m_entity.RemoteSubmissionId = value;
            }
        }

        /// <summary>
        /// 获取或设置用户提交的题目 ID。
        /// </summary>
        public string ProblemId
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.ProblemId;
            set
            {
                CheckAccess();
                m_entity.ProblemId = value;
            }
        }

        /// <summary>
        /// 获取或设置用户提交的比赛 ID。若该提交不是比赛提交，该域为 -1。
        /// </summary>
        public int ContestId
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.ContestId;
            set
            {
                CheckAccess();
                m_entity.ContestId = value;
            }
        }

        /// <summary>
        /// 获取或设置用户提交的用户名。
        /// </summary>
        public string Username
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.Username;
            set
            {
                CheckAccess();
                m_entity.Username = value;
            }
        }

        /// <summary>
        /// 获取或设置用户提交的队伍 ID。
        /// </summary>
        public int TeamId
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.TeamId;
            set
            {
                CheckAccess();
                m_entity.TeamId = value;
            }
        }

        /// <summary>
        /// 获取或设置用户提交的创建时间戳。
        /// </summary>
        public DateTime CreationTimeStamp
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.CreationTimestamp;
            set
            {
                CheckAccess();
                m_entity.CreationTimestamp = value;
            }
        }

        /// <summary>
        /// 获取或设置用户提交被提交到判题器上的时间戳。
        /// </summary>
        public DateTime? VerdictTimeStamp
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.VerdictTimestamp;
            set
            {
                CheckAccess();
                m_entity.VerdictTimestamp = value;
            }
        }

        /// <summary>
        /// 获取或设置用户提交的代码。
        /// </summary>
        public string Code
        {
            get
            {
                if (m_disposed)
                    throw new ObjectDisposedException(GetType().Name);

                if (m_code == null)
                {
                    // 从代码文件中加载代码。
                    try
                    {
                        m_code = File.ReadAllText(m_entity.CodeFilename);
                    }
                    catch
                    {
                        return string.Empty;
                    }
                    finally
                    {
                        m_dirty = false;
                    }
                }

                return m_code;
            }
            set
            {
                CheckAccess();

                m_code = value;
                m_dirty = true;
            }
        }

        /// <summary>
        /// 获取或设置客户程序占用的 CPU 时间，以毫秒为单位。
        /// </summary>
        public int ExecutionTime
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.ExecutionTime;
            set
            {
                CheckAccess();
                m_entity.ExecutionTime = value;
            }
        }

        /// <summary>
        /// 获取或设置客户程序占用的峰值内存，以 KB 为单位。
        /// </summary>
        public int ExecutionMemory
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.ExecutionMemory;
            set
            {
                CheckAccess();
                m_entity.ExecutionMemory = value;
            }
        }

        /// <summary>
        /// 获取或设置用户提交所使用的语言。
        /// </summary>
        public CoreSubmissionLanguage Language
        {
            get => m_disposed 
                ? throw new ObjectDisposedException(GetType().Name) 
                : (CoreSubmissionLanguage)m_entity.Language;
            set
            {
                CheckAccess();
                m_entity.Language = (DatabaseSubmissionLanguage)value;
            }
        }

        /// <summary>
        /// 获取或设置用户提交的判题结果。
        /// </summary>
        public CoreSubmissionVerdict Verdict
        {
            get => m_disposed 
                ? throw new ObjectDisposedException(GetType().Name) 
                : (CoreSubmissionVerdict)m_entity.VerdictResult;
            set
            {
                CheckAccess();
                m_entity.VerdictResult = (DatabaseSubmissionVerdict)value;
            }
        }
        
        /// <summary>
        /// 获取或设置用户提交的判题状态。
        /// </summary>
        public CoreSubmissionVerdictStatus VerdictStatus
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : (CoreSubmissionVerdictStatus)m_entity.VerdictStatus;
            set
            {
                CheckAccess();
                m_entity.VerdictStatus = (DatabaseSubmissionVerdictStatus)value;
            }
        }

        /// <summary>
        /// 获取或设置用户提交的评测消息。
        /// </summary>
        public string VerdictMessage
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.VerdictMessage;
            set
            {
                CheckAccess();
                m_entity.VerdictMessage = value;
            }
        }

        /// <summary>
        /// 将任何挂起的更改写入数据库以及文件系统中。
        /// </summary>
        public void Save()
        {
            if (!m_disposed && !m_readonly)
            {
                SubmissionManager.Default.Context.SaveChanges();

                if (m_dirty)
                {
                    File.WriteAllText(m_entity.CodeFilename, m_code ?? string.Empty);
                    m_dirty = false;
                }
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
                m_disposed = true;
            }
        }
    }
}

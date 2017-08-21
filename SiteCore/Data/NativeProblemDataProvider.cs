namespace BITOJ.Core.Data
{
    using BITOJ.Core.Data;
    using BITOJ.Data;
    using BITOJ.Data.Entities;
    using CoreUserGroup = Authorization.UserGroup;
    using NativeUserGroup = BITOJ.Data.Entities.UserGroup;
    using System;

    /// <summary>
    /// 为 BITOJ 提供本地题目数据源。
    /// </summary>
    public class NativeProblemDataProvider : IProblemDataProvider
    {
        /// <summary>
        /// 从给定的题目句柄对象创建 NativeProblemDataProvider 对象。
        /// </summary>
        /// <param name="handle">题目句柄对象。</param>
        /// <param name="isReadonly">一个值，该值指示创建的 NativeProblemDataProvider 对象是否处于只读模式。</param>
        /// <returns>从给定题目句柄创建的 NativeProblemDataProvider 对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidProblemException"/>
        public static NativeProblemDataProvider Create(ProblemHandle handle, bool isReadonly)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));
            if (!handle.IsNativeProblem)
                throw new InvalidProblemException(handle, "给定的题目句柄不是 BITOJ 本地题目。");

            // 尝试从题目句柄中解析出题目ID。
            if (!int.TryParse(handle.ProblemId.Substring(3), out int id))
                throw new InvalidProblemException(handle, "给定的题目句柄非法。");

            // 从底层数据库中查询题目实体对象。
            ProblemEntity entity = ProblemArchieveManager.Default.DataContext.GetProblemEntityById(id);
            if (entity == null)
                throw new ProblemNotExistException(handle);

            // 创建底层题目数据访问器对象。
            ProblemEntryHandle entryHandle = new ProblemEntryHandle(entity.ProblemDirectory);
            return new NativeProblemDataProvider()
            {
                m_entity = entity,
                m_accessHandle = entryHandle,
                m_readonly = isReadonly
            };
        }

        private ProblemEntity m_entity;     // 底层数据库实体对象。
        private ProblemEntryHandle m_accessHandle;
        private bool m_readonly;
        private bool m_disposed;

        /// <summary>
        /// 初始化 NativeProblemDataProvider 类的新实例。
        /// </summary>
        private NativeProblemDataProvider()
        {
            m_entity = null;
            m_accessHandle = null;
            m_readonly = true;
            m_disposed = false;
        }

        /// <summary>
        /// 检查当前对象是否具有写权限。若没有写权限，该方法会抛出 InvalidOperationException 异常。
        /// </summary>
        private void CheckAccess()
        {
            if (m_disposed)
                throw new ObjectDisposedException(GetType().Name);
            if (m_readonly)
                throw new InvalidOperationException("尝试在只读对象上执行写操作。");
        }

        /// <summary>
        /// 获取或设置题目的标题。
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException"/>
        public string Title
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : m_entity.Title;
            set
            {
                CheckAccess();
                m_entity.Title = value;
                m_accessHandle.Title = value;
            }
        }

        /// <summary>
        /// 获取或设置题目的作者。
        /// </summary>
        public string Author
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : m_accessHandle.Author;
            set
            {
                CheckAccess();
                m_entity.Author = value;
                m_accessHandle.Author = value;
            }
        }

        /// <summary>
        /// 获取或设置题目的正文描述部分。
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException"/>
        public string Description
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : m_accessHandle.GetProblemPart(ProblemEntryParts.Description);
            set
            {
                CheckAccess();
                m_accessHandle.SetProblemPart(ProblemEntryParts.Description, value);
            }
        }

        /// <summary>
        /// 获取或设置题目的输入描述部分。
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException"/>
        public string InputDescription
        {
            get => m_disposed 
                ? throw new ObjectDisposedException(GetType().Name) 
                : m_accessHandle.GetProblemPart(ProblemEntryParts.InputDescription);
            set
            {
                CheckAccess();
                m_accessHandle.SetProblemPart(ProblemEntryParts.InputDescription, value);
            }
        }

        /// <summary>
        /// 获取或设置题目的输出描述部分。
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException"/>
        public string OutputDescription
        {
            get => m_disposed 
                ? throw new ObjectDisposedException(GetType().Name)
                : m_accessHandle.GetProblemPart(ProblemEntryParts.OutputDescription);
            set
            {
                CheckAccess();
                m_accessHandle.SetProblemPart(ProblemEntryParts.OutputDescription, value);
            }
        }

        /// <summary>
        /// 获取或设置题目的输入样例部分。
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException"/>
        public string InputSamples
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : m_accessHandle.GetProblemPart(ProblemEntryParts.InputSample);
            set
            {
                CheckAccess();
                m_accessHandle.SetProblemPart(ProblemEntryParts.InputSample, value);
            }
        }

        /// <summary>
        /// 获取或设置题目的输出样例部分。
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException"/>
        public string OutputSamples
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : m_accessHandle.GetProblemPart(ProblemEntryParts.OutputSample);
            set
            {
                CheckAccess();
                m_accessHandle.SetProblemPart(ProblemEntryParts.OutputSample, value);
            }
        }

        /// <summary>
        /// 获取或设置题目的来源部分。
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException"/>
        public string Source
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : m_accessHandle.GetProblemPart(ProblemEntryParts.Source);
            set
            {
                CheckAccess();
                m_entity.Source = value;
                m_accessHandle.SetProblemPart(ProblemEntryParts.Source, value);
            }
        }

        /// <summary>
        /// 获取或设置题目的提示部分。
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException"/>
        public string Hints
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : m_accessHandle.GetProblemPart(ProblemEntryParts.Hint);
            set
            {
                CheckAccess();
                m_accessHandle.SetProblemPart(ProblemEntryParts.Hint, value);
            }
        }

        /// <summary>
        /// 获取或设置可访问当前题目的用户权限组。
        /// </summary>
        public CoreUserGroup AuthorizationGroup
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : (CoreUserGroup)m_accessHandle.AuthorizationGroup;
            set
            {
                CheckAccess();
                m_accessHandle.AuthorizationGroup = (NativeUserGroup)value;
            }
        }

        /// <summary>
        /// 获取一个值，该值指示当前对象是否处于只读模式。
        /// </summary>
        public bool IsReadOnly { get => m_readonly; }

        /// <summary>
        /// 释放由当前对象占有的所有资源。
        /// </summary>
        public void Dispose()
        {
            if (!m_disposed)
            {
                if (!m_readonly)
                {
                    m_accessHandle.Save();      // 将挂起的更改写入底层文件系统中。
                }

                m_disposed = true;
            }
        }
    }
}

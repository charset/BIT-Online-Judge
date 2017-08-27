namespace BITOJ.Core.Data
{
    using BITOJ.Data;
    using BITOJ.Data.Entities;
    using System;
    using System.Collections.Generic;
    using CoreUserGroup = UserGroup;
    using NativeUserGroup = BITOJ.Data.Entities.UserGroup;

    /// <summary>
    /// 为 BITOJ 提供本地题目数据源。
    /// </summary>
    public class ProblemDataProvider : IDisposable
    {
        /// <summary>
        /// 从给定的题目句柄对象创建 NativeProblemDataProvider 对象。
        /// </summary>
        /// <param name="handle">题目句柄对象。</param>
        /// <param name="isReadonly">一个值，该值指示创建的 NativeProblemDataProvider 对象是否处于只读模式。</param>
        /// <returns>从给定题目句柄创建的 NativeProblemDataProvider 对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidProblemException"/>
        /// <exception cref="ProblemNotExistException"/>
        public static ProblemDataProvider Create(ProblemHandle handle, bool isReadonly)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));
            if (!handle.IsNativeProblem)
                throw new InvalidProblemException(handle, "给定的题目句柄不是 BITOJ 本地题目。");

            // 从底层数据库中查询题目实体对象。
            ProblemEntity entity = ProblemArchieveManager.Default.DataContext.GetProblemEntityById(handle.ProblemId);
            if (entity == null)
                throw new ProblemNotExistException(handle);

            // 创建底层题目数据访问器对象。
            ProblemAccessHandle entryHandle = new ProblemAccessHandle(entity);
            return new ProblemDataProvider()
            {
                m_entity = entity,
                m_accessHandle = entryHandle,
                m_readonly = isReadonly
            };
        }

        private ProblemEntity m_entity;
        private ProblemAccessHandle m_accessHandle;
        private bool m_readonly;
        private bool m_dirty;
        private bool m_disposed;

        /// <summary>
        /// 初始化 NativeProblemDataProvider 类的新实例。
        /// </summary>
        private ProblemDataProvider()
        {
            m_entity = null;
            m_accessHandle = null;
            m_readonly = true;
            m_dirty = false;
            m_disposed = false;
        }

        ~ProblemDataProvider()
        {
            Dispose();
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
                m_dirty = true;
            }
        }

        /// <summary>
        /// 获取或设置题目的作者。
        /// </summary>
        public string Author
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : m_entity.Author;
            set
            {
                CheckAccess();
                m_entity.Author = value;
                m_dirty = true;
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
                : m_accessHandle.GetProblemPart(ProblemParts.Description);
            set
            {
                CheckAccess();
                m_accessHandle.SetProblemPart(ProblemParts.Description, value);
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
                : m_accessHandle.GetProblemPart(ProblemParts.InputDescription);
            set
            {
                CheckAccess();
                m_accessHandle.SetProblemPart(ProblemParts.InputDescription, value);
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
                : m_accessHandle.GetProblemPart(ProblemParts.OutputDescription);
            set
            {
                CheckAccess();
                m_accessHandle.SetProblemPart(ProblemParts.OutputDescription, value);
            }
        }

        /// <summary>
        /// 获取或设置题目的输入样例部分。
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException"/>
        public string InputExample
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : m_accessHandle.GetProblemPart(ProblemParts.InputExample);
            set
            {
                CheckAccess();
                m_accessHandle.SetProblemPart(ProblemParts.InputExample, value);
            }
        }

        /// <summary>
        /// 获取或设置题目的输出样例部分。
        /// </summary>
        /// <exception cref="ObjectDisposedException"/>
        /// <exception cref="InvalidOperationException"/>
        public string OutputExample
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : m_accessHandle.GetProblemPart(ProblemParts.OutputExample);
            set
            {
                CheckAccess();
                m_accessHandle.SetProblemPart(ProblemParts.OutputExample, value);
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
                : m_entity.Source;
            set
            {
                CheckAccess();
                m_entity.Source = value;
                m_dirty = true;
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
                : m_accessHandle.GetProblemPart(ProblemParts.Hint);
            set
            {
                CheckAccess();
                m_accessHandle.SetProblemPart(ProblemParts.Hint, value);
            }
        }

        /// <summary>
        /// 获取或设置题目的 CPU 时间限制，以毫秒为单位。
        /// </summary>
        public int TimeLimit
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : m_entity.TimeLimit;
            set
            {
                CheckAccess();
                m_entity.TimeLimit = value;
                m_dirty = true;
            }
        }

        /// <summary>
        /// 获取或设置题目的峰值内存大小限制，以 KB 为单位。
        /// </summary>
        public int MemoryLimit
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : m_entity.MemoryLimit;
            set
            {
                CheckAccess();
                m_entity.MemoryLimit = value;
                m_dirty = true;
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示当前题目的判题过程是否需要用户提供的 Judge 程序。
        /// </summary>
        public bool IsSpecialJudge
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : m_entity.IsSpecialJudge;
            set
            {
                CheckAccess();
                m_entity.IsSpecialJudge = value;
                m_dirty = true;
            }
        }

        /// <summary>
        /// 获取或设置可访问当前题目的用户权限组。
        /// </summary>
        public CoreUserGroup AuthorizationGroup
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : (CoreUserGroup)m_entity.AuthorizationGroup;
            set
            {
                CheckAccess();
                m_entity.AuthorizationGroup = (NativeUserGroup)value;
                m_dirty = true;
            }
        }

        /// <summary>
        /// 获取或设置当前题目的总提交数。
        /// </summary>
        public int TotalSubmissions
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : m_entity.TotalSubmissions;
            set
            {
                CheckAccess();
                m_entity.TotalSubmissions = value;
                m_dirty = true;
            }
        }

        /// <summary>
        /// 获取或设置当前题目的 AC 提交数。
        /// </summary>
        public int AcceptedSubmissions
        {
            get => m_disposed
                ? throw new ObjectDisposedException(GetType().Name)
                : m_entity.AcceptedSubmissions;
            set
            {
                CheckAccess();
                m_entity.AcceptedSubmissions = value;
                m_dirty = true;
            }
        }

        /// <summary>
        /// 获取一个值，该值指示当前对象是否处于只读模式。
        /// </summary>
        public bool IsReadOnly { get => m_readonly; }

        /// <summary>
        /// 获取当前题目所处的题目类别。
        /// </summary>
        /// <returns>当前题目所处的题目类别集合。</returns>
        public ICollection<string> GetCategories()
        {
            List<string> collection = new List<string>();
            foreach (ProblemCategoryEntity cat in m_entity.Categories)
            {
                collection.Add(cat.Name);
            }

            return collection;
        }

        /// <summary>
        /// 将挂起的实体更改写入数据库中。
        /// </summary>
        public void Save()
        {
            if (!m_disposed && !m_readonly && m_dirty)
            {
                // 更新数据库上下文。
                ProblemArchieveManager.Default.DataContext.SaveChanges();
                m_dirty = false;
            }
        }

        /// <summary>
        /// 释放由当前对象占有的所有资源。
        /// </summary>
        public void Dispose()
        {
            if (!m_disposed)
            {
                if (!m_readonly)
                {
                    Save();
                    m_accessHandle.Save();      // 将挂起的更改写入底层文件系统中。
                }

                m_accessHandle.Dispose();
                m_disposed = true;
            }
        }
    }
}

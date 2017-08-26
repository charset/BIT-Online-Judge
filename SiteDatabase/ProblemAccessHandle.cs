namespace BITOJ.Data
{
    using BITOJ.Data.Entities;
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// 封装对本地题目库中一道题目的访问接口。
    /// </summary>
    public sealed class ProblemAccessHandle : IDisposable
    {
        private static readonly string ProblemDescriptionFileName = "description";
        private static readonly string ProblemInputDescriptionFileName = "input_description";
        private static readonly string ProblemOutputDescriptionFileName = "output_description";
        private static readonly string ProblemInputSampleFileName = "input_example";
        private static readonly string ProblemOutputSampleFileName = "output_example";
        private static readonly string ProblemHintFileName = "hint";

        private static readonly Dictionary<ProblemParts, string> PartFileName;

        static ProblemAccessHandle()
        {
            // 初始化 PartFileName 。
            PartFileName = new Dictionary<ProblemParts, string>
            {
                { ProblemParts.Description, ProblemDescriptionFileName },
                { ProblemParts.InputDescription, ProblemInputDescriptionFileName },
                { ProblemParts.OutputDescription, ProblemOutputDescriptionFileName },
                { ProblemParts.InputExample, ProblemInputSampleFileName },
                { ProblemParts.OutputExample, ProblemOutputSampleFileName },
                { ProblemParts.Hint, ProblemHintFileName },
            };
        }

        private ProblemEntity m_entity;
        private Dictionary<ProblemParts, string> m_parts;
        private bool m_disposed;
        private bool m_dirty;

        /// <summary>
        /// 使用给定的题目实体对象初始化 ProblemEntryHandle 类的新实例。
        /// </summary>
        /// <param name="entity">题目实体对象。</param>
        /// <exception cref="ArgumentNullException"/>
        public ProblemAccessHandle(ProblemEntity entity)
        {
            m_entity = entity ?? throw new ArgumentNullException(nameof(entity));
            m_parts = new Dictionary<ProblemParts, string>();
            m_disposed = false;
            m_dirty = false;
        }

        /// <summary>
        /// 获取或设置题目的标题。
        /// </summary>
        public string Title
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.Title;
            set
            {
                if (m_disposed)
                    throw new ObjectDisposedException(GetType().Name);

                m_entity.Title = value;
            }
        }

        /// <summary>
        /// 获取或设置题目的作者的用户名。
        /// </summary>
        public string Author
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.Author;
            set
            {
                if (m_disposed)
                    throw new ObjectDisposedException(GetType().Name);

                m_entity.Author = value;
            }
        }

        /// <summary>
        /// 获取或设置题目的来源。
        /// </summary>
        public string Source
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.Source;
            set
            {
                if (m_disposed)
                    throw new ObjectDisposedException(GetType().Name);

                m_entity.Source = value;
            }
        }

        /// <summary>
        /// 获取或设置题目的 CPU 时间限制，以毫秒为单位。
        /// </summary>
        public int TimeLimit
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.TimeLimit;
            set
            {
                if (m_disposed)
                    throw new ObjectDisposedException(GetType().Name);

                m_entity.TimeLimit = value;
            }
        }

        /// <summary>
        /// 获取或设置题目的峰值内存大小限制，以 KB 为单位。
        /// </summary>
        public int MemoryLimit
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.MemoryLimit;
            set
            {
                if (m_disposed)
                    throw new ObjectDisposedException(GetType().Name);

                m_entity.TimeLimit = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示当前题目的判题过程是否需要用户提供的 Judge 程序。
        /// </summary>
        public bool IsSpecialJudge
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.IsSpecialJudge;
            set
            {
                if (m_disposed)
                    throw new ObjectDisposedException(GetType().Name);

                m_entity.IsSpecialJudge = value;
            }
        }

        /// <summary>
        /// 获取或设置允许访问题目的用户组。
        /// </summary>
        public UserGroup AuthorizationGroup
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.AuthorizationGroup;
            set
            {
                if (m_disposed)
                    throw new ObjectDisposedException(GetType().Name);

                m_entity.AuthorizationGroup = value;
            }
        }

        /// <summary>
        /// 获取或设置当前题目的总提交数目。
        /// </summary>
        public int TotalSubmissions
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.TotalSubmissions;
            set
            {
                if (m_disposed)
                    throw new ObjectDisposedException(GetType().Name);

                m_entity.TotalSubmissions = value;
            }
        }

        /// <summary>
        /// 获取或设置当前题目的 AC 提交数目。
        /// </summary>
        public int AcceptedSubmissions
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.AcceptedSubmissions;
            set
            {
                if (m_disposed)
                    throw new ObjectDisposedException(GetType().Name);

                m_entity.AcceptedSubmissions = value;
            }
        }

        /// <summary>
        /// 获取当前题目所处的题目类别。
        /// </summary>
        public ICollection<ProblemCategoryEntity> Categories
        {
            get => m_disposed ? throw new ObjectDisposedException(GetType().Name) : m_entity.Categories;
        }

        /// <summary>
        /// 获取题目描述中给定部分的文件名。
        /// </summary>
        /// <param name="part">要查询的题目描述逻辑部分。</param>
        /// <returns>给定部分的文件名。</returns>
        private string GetProblemPartFilename(ProblemParts part)
        {
            return string.Concat(m_entity.ProblemDirectory, "\\", PartFileName[part]);
        }

        /// <summary>
        /// 获取题目描述中给定部分的 HTML 表示。
        /// </summary>
        /// <param name="part">要查询的题目描述逻辑部分。</param>
        /// <returns>给定部分的 HTML 表示。</returns>
        public string GetProblemPart(ProblemParts part)
        {
            if (m_parts.ContainsKey(part))
            {
                return m_parts[part];
            }

            // 缓存中未找到指定的逻辑部分。读取本地文件系统以查找。
            string filename = GetProblemPartFilename(part);
            if (File.Exists(filename))
            {
                // 将数据存入缓存中。
                string content = File.ReadAllText(filename);
                m_parts.Add(part, content);

                return content;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 设置题目描述中给定部分的 HTML 表示。
        /// </summary>
        /// <param name="part">要设置的题目描述逻辑部分。</param>
        /// <param name="html">给定部分的 HTML 表示。</param>
        /// <exception cref="ArgumentNullException"/>
        public void SetProblemPart(ProblemParts part, string html)
        {
            if (html == null)
                throw new ArgumentNullException(nameof(html));

            if (m_parts.ContainsKey(part))
            {
                m_parts[part] = html;
            }
            else
            {
                m_parts.Add(part, html);
            }

            m_dirty = true;
        }

        /// <summary>
        /// 将挂起的更改写入本地文件系统中。
        /// </summary>
        public void Save()
        {
            if (!m_disposed && m_dirty)
            {
                foreach (KeyValuePair<ProblemParts, string> item in m_parts)
                {
                    string filename = GetProblemPartFilename(item.Key);
                    File.WriteAllText(filename, item.Value);
                }

                m_dirty = false;
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

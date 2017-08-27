namespace BITOJ.Data
{
    using BITOJ.Data.Entities;
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// 封装对本地题目库中一道题目详细信息的访问接口。
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

        private string m_problemDirectory;
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
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            m_problemDirectory = entity.ProblemDirectory;
            m_parts = new Dictionary<ProblemParts, string>();
            m_disposed = false;
            m_dirty = false;
        }

        ~ProblemAccessHandle()
        {
            Dispose();
        }

        /// <summary>
        /// 获取题目描述中给定部分的文件名。
        /// </summary>
        /// <param name="part">要查询的题目描述逻辑部分。</param>
        /// <returns>给定部分的文件名。</returns>
        private string GetProblemPartFilename(ProblemParts part)
        {
            return string.Concat(m_problemDirectory, "\\", PartFileName[part]);
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

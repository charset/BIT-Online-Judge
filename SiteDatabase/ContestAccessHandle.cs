namespace BITOJ.Data
{
    using BITOJ.Data.Entities;
    using BITOJ.Data.Models;
    using Newtonsoft.Json;
    using System;
    using System.IO;

    /// <summary>
    /// 封装对比赛信息的一组访问接口。
    /// </summary>
    public sealed class ContestAccessHandle : IDisposable
    {
        private ContestEntity m_entity;
        private ContestConfigurationModel m_config;
        private bool m_dirty;
        private bool m_disposed;

        /// <summary>
        /// 使用指定的比赛实体对象创建 ContestEntryHandle 类的新实例。
        /// </summary>
        /// <param name="entity">比赛实体对象。</param>
        /// <exception cref="ArgumentNullException"/>
        public ContestAccessHandle(ContestEntity entity)
        {
            m_entity = entity ?? throw new ArgumentNullException(nameof(entity));
            m_config = null;
            m_dirty = false;
            m_disposed = false;
        }

        ~ContestAccessHandle()
        {
            Dispose();
        }

        /// <summary>
        /// 将配置信息从本地文件系统载入对象中。
        /// </summary>
        private void LoadConfigurationModel()
        {
            if (!File.Exists(m_entity.ContestConfigurationFile))
            {
                // 配置文件不存在。使用默认配置。
                m_config = new ContestConfigurationModel();
                m_dirty = true;
            }
            else
            {
                // 读取配置文件内容。
                m_config = JsonConvert.DeserializeObject<ContestConfigurationModel>(File.ReadAllText(
                    m_entity.ContestConfigurationFile));
                m_dirty = false;
            }
        }

        /// <summary>
        /// 将配置信息写入本地文件系统中。
        /// </summary>
        public void Save()
        {
            if (!m_disposed && m_dirty)
            {
                File.WriteAllText(m_entity.ContestConfigurationFile, JsonConvert.SerializeObject(m_config));
                m_dirty = false;
            }
        }

        /// <summary>
        /// 获取或设置比赛目录中包含的题目信息。
        /// </summary>
        /// <returns></returns>
        public string[] GetProblems()
        {
            string[] problems = new string[Configuration.Problems.Count];
            Configuration.Problems.CopyTo(problems, 0);

            return problems;
        }

        /// <summary>
        /// 将给定的题目添加到当前的比赛中。
        /// </summary>
        /// <param name="problemId">要添加的题目 ID。</param>
        /// <exception cref="ArgumentNullException"/>
        public void AddProblem(string problemId)
        {
            if (problemId == null)
                throw new ArgumentNullException(nameof(problemId));

            Configuration.Problems.Add(problemId);
            m_dirty = true;
        }

        /// <summary>
        /// 从当前比赛中删除给定的题目。
        /// </summary>
        /// <param name="problemId">要删除的题目 ID。</param>
        /// <exception cref="ArgumentNullException"/>
        public void RemoveProblem(string problemId)
        {
            if (problemId == null)
                throw new ArgumentNullException(nameof(problemId));

            foreach (string item in Configuration.Problems)
            {
                if (string.Compare(item, problemId, true) == 0)
                {
                    Configuration.Problems.Remove(item);
                }
            }

            m_dirty = true;
        }

        /// <summary>
        /// 获取比赛的配置信息。
        /// </summary>
        public ContestConfigurationModel Configuration
        {
            get
            {
                if (m_config == null)
                {
                    LoadConfigurationModel();
                }
                return m_config;
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

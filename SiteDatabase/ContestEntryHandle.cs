namespace BITOJ.Data
{
    using BITOJ.Data.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// 封装对比赛信息的一组访问接口。
    /// </summary>
    public sealed class ContestEntryHandle
    {
        private static readonly string ConfigurationFileName = "config.json";
        private static readonly string ProblemsDirectoryName = "Problems";

        private string m_contestDirectory;
        private ContestConfigurationModel m_config;
        private bool m_dirty;

        /// <summary>
        /// 使用指定的比赛目录创建 ContestEntryHandle 类的新实例。
        /// </summary>
        /// <param name="contestDirectory">比赛的目录。</param>
        /// <exception cref="ArgumentNullException"/>
        public ContestEntryHandle(string contestDirectory)
        {
            m_contestDirectory = contestDirectory ?? throw new ArgumentNullException(nameof(contestDirectory));
            LoadConfigurationModel();
            m_dirty = false;
        }

        /// <summary>
        /// 将配置信息从本地文件系统载入对象中。
        /// </summary>
        private void LoadConfigurationModel()
        {
            string configFileName = string.Concat(m_contestDirectory, "\\", ConfigurationFileName);
            if (!File.Exists(configFileName))
            {
                // 配置文件不存在。使用默认配置。
                m_config = new ContestConfigurationModel();
            }
            else
            {
                // 读取配置文件内容。
                m_config = JsonConvert.DeserializeObject<ContestConfigurationModel>(File.ReadAllText(configFileName));
            }
        }

        /// <summary>
        /// 将配置信息写入本地文件系统中。
        /// </summary>
        public void SaveConfigurationModel()
        {
            string configFileName = string.Concat(m_contestDirectory, "\\", ConfigurationFileName);
            File.WriteAllText(configFileName, JsonConvert.SerializeObject(m_config));
        }

        /// <summary>
        /// 在当前的比赛中创建一个新的题目并返回题目句柄。
        /// </summary>
        /// <returns></returns>
        public ProblemAccessHandle CreateProblem()
        {
            int existCount = 0;         // 已经存在的题目数量。
            string problemDirectory = string.Concat(m_contestDirectory, "\\", ProblemsDirectoryName);
            if (!Directory.Exists(problemDirectory))
            {
                existCount = 0;
                Directory.CreateDirectory(problemDirectory);
            }
            else
            {
                existCount = Directory.GetDirectories(problemDirectory).Length;
            }

            string problemHandleDirectory = string.Concat(problemDirectory, "\\", (existCount + 1).ToString("D2"));
            Directory.CreateDirectory(problemHandleDirectory);
            return new ProblemAccessHandle(problemHandleDirectory);
        }

        /// <summary>
        /// 获取或设置比赛目录中包含的题目信息。
        /// </summary>
        /// <returns></returns>
        public ProblemAccessHandle[] GetProblems()
        {
            string problemDirectory = string.Concat(m_contestDirectory, "\\", ProblemsDirectoryName);
            if (!Directory.Exists(problemDirectory))
            {
                // 题目目录不存在。
                return new ProblemAccessHandle[0];
            }
            else
            {
                List<ProblemAccessHandle> entries = new List<ProblemAccessHandle>();
                foreach (string problemHandleDirectory in Directory.GetDirectories(problemDirectory))
                {
                    entries.Add(new ProblemAccessHandle(problemHandleDirectory));
                }
                return entries.ToArray();
            }
        }

        /// <summary>
        /// 移除比赛中给定 ID 的题目。
        /// </summary>
        /// <param name="id">要移除的题目在当前比赛中的 ID 。该 ID 应该由 1 开始编码。</param>
        public void RemoveProblem(int id)
        {
            string problemHandleDirectory 
                = string.Concat(m_contestDirectory, "\\", ProblemsDirectoryName, "\\", id.ToString("D2"));
            if (Directory.Exists(problemHandleDirectory))
            {
                Directory.Delete(problemHandleDirectory);
            }
        }

        /// <summary>
        /// 获取或设置比赛的配置信息。
        /// </summary>
        public ContestConfigurationModel Configuration
        {
            get { return m_config; }
            set { m_config = value; }
        }
    }
}

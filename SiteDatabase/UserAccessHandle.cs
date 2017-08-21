namespace BITOJ.Data
{
    using BITOJ.Data.Models;
    using Newtonsoft.Json;
    using System;
    using System.IO;

    /// <summary>
    /// 提供对用户信息的访问支持。
    /// </summary>
    public sealed class UserAccessHandle
    {
        private string m_userProfileFile;
        private UserProfileModel m_model;
        private bool m_dirty;

        /// <summary>
        /// 使用给定的用户信息文件初始化 UserAccessHandle 类的新实例。
        /// </summary>
        /// <param name="userProfileFile">用户信息文件。</param>
        /// <exception cref="ArgumentNullException"/>
        public UserAccessHandle(string userProfileFile)
        {
            m_userProfileFile = userProfileFile ?? throw new ArgumentNullException(nameof(userProfileFile));
            m_model = null;
            m_dirty = false;
        }

        /// <summary>
        /// 从给定的文件中加载用户信息模型对象。
        /// </summary>
        private void LoadModel()
        {
            if (!File.Exists(m_userProfileFile))
            {
                // 信息文件不存在。创建默认信息文件并加载默认模型。
                m_model = new UserProfileModel();
                m_dirty = true;

                Save();
            }
            else
            {
                // 尝试读取所有内容并按照 JSON 格式解析。
                string json = File.ReadAllText(m_userProfileFile);
                m_model = JsonConvert.DeserializeObject<UserProfileModel>(json);

                m_dirty = false;
            }
        }

        /// <summary>
        /// 获取或设置用户信息模型对象。
        /// </summary>
        private UserProfileModel Model
        {
            get
            {
                if (m_model == null)
                {
                    LoadModel();
                }
                return m_model;
            }
            set => m_model = value;
        }

        /// <summary>
        /// 获取用户名。
        /// </summary>
        public string Username { get => Model.Username; }

        /// <summary>
        /// 获取或设置组织名称。
        /// </summary>
        public string Organization
        {
            get => Model.Organization;
            set
            {
                Model.Organization = value;
                m_dirty = true;
            }
        }

        /// <summary>
        /// 获取或设置用户性别。
        /// </summary>
        public UserSex Sex
        {
            get => Model.Sex;
            set
            {
                Model.Sex = value;
                m_dirty = true;
            }
        }

        /// <summary>
        /// 获取或设置用户头像文件文件名。
        /// </summary>
        public string ImagePath
        {
            get => Model.ImagePath;
            set
            {
                Model.ImagePath = value;
                m_dirty = true;
            }
        }

        /// <summary>
        /// 获取或设置用户的 Rating 值。
        /// </summary>
        public int Rating
        {
            get => Model.Rating;
            set
            {
                Model.Rating = value;
                m_dirty = true;
            }
        }

        /// <summary>
        /// 获取或设置用户的提交统计信息。
        /// </summary>
        public UserSubmissionStatisticsModel SubmissionsStatistics
        {
            get => Model.SubmissionStatistics;
            set
            {
                Model.SubmissionStatistics = value;
                m_dirty = true;
            }
        }

        /// <summary>
        /// 将挂起的更改写入底层文件系统中。
        /// </summary>
        public void Save()
        {
            if (m_dirty)
            {
                // 注意：若 m_dirty 为 true，则延迟加载过程必然已经进行，即成员 m_model 一定不为 null。
                string json = JsonConvert.SerializeObject(m_model);

                // 将 JSON 文本写入文件。
                File.WriteAllText(m_userProfileFile, json);

                m_dirty = false;
            }
        }
    }
}

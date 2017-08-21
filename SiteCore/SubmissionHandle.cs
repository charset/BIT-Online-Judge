namespace BITOJ.Core
{
    using BITOJ.Data.Entities;
    using DatabaseSubmissionLanguage = BITOJ.Data.Entities.SubmissionLanguage;
    using DatabaseSubmissionVerdictStatus = BITOJ.Data.Entities.SubmissionVerdictStatus;
    using DatabaseSubmissionVerdict = BITOJ.Data.Entities.SubmissionVerdict;
    using Newtonsoft.Json;
    using System;

    /// <summary>
    /// 封装一个用户提交的信息。
    /// </summary>
    public sealed class SubmissionHandle
    {
        private SubmissionEntity m_nativeEntity;        // 底层数据库实现对象

        /// <summary>
        /// 获取底层数据库实现对象。
        /// </summary>
        internal SubmissionEntity NativeEntity { get; }

        /// <summary>
        /// 初始化 SubmissionHandle 类的新实例。
        /// </summary>
        [JsonConstructor]
        public SubmissionHandle()
        {
            m_nativeEntity = new SubmissionEntity();
        }

        /// <summary>
        /// 获取当前的用户提交本地 ID。
        /// </summary>
        [JsonProperty("id")]
        public int SubmissionId
        {
            get => m_nativeEntity.Id;
        }

        /// <summary>
        /// 在 Virtual Judge 提交中，获取或设置远程 OJ 系统的提交 ID。
        /// </summary>
        [JsonProperty("remote_id")]
        public int RemoteSubmissionId
        {
            get => m_nativeEntity.RemoteSubmissionId;
            set => m_nativeEntity.RemoteSubmissionId = value;
        }

        /// <summary>
        /// 获取或设置当前用户提交的创建时间戳。
        /// </summary>
        [JsonProperty("creation_time")]
        public DateTime CreationTimestamp
        {
            get => m_nativeEntity.CreationTimestamp;
            set => m_nativeEntity.CreationTimestamp = value;
        }

        /// <summary>
        /// 获取或设置当前用户提交的判题时间戳。
        /// </summary>
        [JsonProperty("verdict_time")]
        public DateTime? VerdictTimestamp
        {
            get => m_nativeEntity.VerdictTimestamp;
            set => m_nativeEntity.VerdictTimestamp = value;
        }

        /// <summary>
        /// 获取或设置当前用户提交的用户名。
        /// </summary>
        [JsonProperty("username")]
        public string Username
        {
            get => m_nativeEntity.Username;
            set => m_nativeEntity.Username = value;
        }

        /// <summary>
        /// 获取或设置当前用户提交的队伍 ID 。
        /// </summary>
        [JsonProperty("team")]
        public int TeamId
        {
            get => m_nativeEntity.TeamId;
            set => m_nativeEntity.TeamId = value;
        }

        /// <summary>
        /// 获取或设置当前用户提交的代码文件名。
        /// </summary>
        [JsonProperty("code_file")]
        public string CodeFilename
        {
            get => m_nativeEntity.CodeFilename;
            set => m_nativeEntity.CodeFilename = value;
        }

        /// <summary>
        /// 获取或设置当前用户提交所使用的语言。
        /// </summary>
        [JsonProperty("language")]
        public SubmissionLanguage Language
        {
            get => (SubmissionLanguage)m_nativeEntity.Language;
            set => m_nativeEntity.Language = (DatabaseSubmissionLanguage)value;
        }

        /// <summary>
        /// 获取或设置当前用户提交的判题状态。
        /// </summary>
        [JsonProperty("verdict_status")]
        public SubmissionVerdictStatus VerdictStatus
        {
            get => (SubmissionVerdictStatus)m_nativeEntity.VerdictStatus;
            set => m_nativeEntity.VerdictStatus = (DatabaseSubmissionVerdictStatus)value;
        }

        /// <summary>
        /// 获取或设置当前用户提交的判题结果。
        /// </summary>
        [JsonProperty("verdict_result")]
        public SubmissionVerdict VerdictResult
        {
            get => (SubmissionVerdict)m_nativeEntity.VerdictResult;
            set => m_nativeEntity.VerdictResult = (DatabaseSubmissionVerdict)value;
        }

        /// <summary>
        /// 获取或设置判题器回送的判题消息。
        /// </summary>
        [JsonProperty("verdict_msg")]
        public string VerdictMessage
        {
            get => m_nativeEntity.VerdictMessage;
            set => m_nativeEntity.VerdictMessage = value;
        }
    }
}

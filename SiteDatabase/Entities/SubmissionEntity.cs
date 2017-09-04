namespace BITOJ.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 表示一个提交的实体对象。
    /// </summary>
    [Table("Submissions")]
    public sealed class SubmissionEntity
    {
        /// <summary>
        /// 获取或设置提交 ID 。不应在外部代码中手动修改此属性值。
        /// </summary>
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// 在一个 VJ 提交中，获取或设置在第三方 OJ 系统中的提交 ID。若当前提交不是 VJ 提交，该属性值为 0。
        /// </summary>
        public int RemoteSubmissionId { get; set; }

        /// <summary>
        /// 获取或设置当前提交的创建时间。
        /// </summary>
        public DateTime CreationTimestamp { get; set; }

        /// <summary>
        /// 获取或设置当前提交的 Judge 时间。
        /// </summary>
        public DateTime? VerdictTimestamp { get; set; }

        /// <summary>
        /// 获取或设置创建该提交的用户名称。
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// 获取或设置队伍 ID 。
        /// </summary>
        public int TeamId { get; set; }

        /// <summary>
        /// 获取或设置该提交所对应的题目 ID。
        /// </summary>
        public string ProblemId { get; set; }

        /// <summary>
        /// 获取或设置该提交所对应的代码文件名。
        /// </summary>
        public string CodeFilename { get; set; }

        /// <summary>
        /// 获取或设置用户程序运行的 CPU 时间，以毫秒为单位。
        /// </summary>
        public int ExecutionTime { get; set; }

        /// <summary>
        /// 获取或设置用户程序运行的峰值内存总量，以 KB 为单位。
        /// </summary>
        public int ExecutionMemory { get; set; }

        /// <summary>
        /// 获取或设置该提交所使用的程序设计语言或编译系统。
        /// </summary>
        public SubmissionLanguage Language { get; set; }

        /// <summary>
        /// 获取或设置该提交的 Judge 状态。
        /// </summary>
        public SubmissionVerdictStatus VerdictStatus { get; set; }

        /// <summary>
        /// 获取或设置该提交的 Judge 结果。
        /// </summary>
        public SubmissionVerdict VerdictResult { get; set; }

        /// <summary>
        /// 获取或设置 Judge 返回的额外信息。
        /// </summary>
        public string VerdictMessage { get; set; }

        /// <summary>
        /// 初始化 SubmissionEntity 类的新实例。
        /// </summary>
        public SubmissionEntity()
        {
            Id = 0;
            RemoteSubmissionId = 0;
            CreationTimestamp = DateTime.Now;
            VerdictTimestamp = null;
            Username = string.Empty;
            TeamId = 0;
            ProblemId = string.Empty;
            CodeFilename = string.Empty;
            ExecutionTime = 0;
            ExecutionMemory = 0;
            Language = SubmissionLanguage.GnuCPlusPlus;
            VerdictStatus = SubmissionVerdictStatus.Pending;
            VerdictResult = SubmissionVerdict.Unknown;
            VerdictMessage = string.Empty;
        }
    }
}

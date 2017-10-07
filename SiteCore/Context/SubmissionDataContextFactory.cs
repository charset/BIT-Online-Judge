namespace BITOJ.Core.Context
{
    using BITOJ.Data;

    /// <summary>
    /// 表示 SubmissionDataContext 工厂类。
    /// </summary>
    internal sealed class SubmissionDataContextFactory : DataContextFactoryBase<SubmissionDataContext>
    {
        /// <summary>
        /// 创建新的 SubmissionDataContext 对象。
        /// </summary>
        /// <returns>新创建的 SubmissionDataContext 对象。</returns>
        public override SubmissionDataContext CreateContext()
        {
            return new SubmissionDataContext();
        }

        /// <summary>
        /// 创建 SubmissionDataContextFactory 类的新实例。
        /// </summary>
        public SubmissionDataContextFactory()
        {
        }
    }
}

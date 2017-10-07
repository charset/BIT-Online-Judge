namespace BITOJ.Core.Context
{
    using BITOJ.Data;

    /// <summary>
    /// 表示 ContestDataContext 对象工厂类。
    /// </summary>
    internal sealed class ContestDataContextFactory : DataContextFactoryBase<ContestDataContext>
    {
        /// <summary>
        /// 创建新的 ContestDataContext 对象。
        /// </summary>
        /// <returns>新创建的 ContestDataContext 对象。</returns>
        public override ContestDataContext CreateContext()
        {
            return new ContestDataContext();
        }

        /// <summary>
        /// 创建新的 ContestDataContextFactory 对象。
        /// </summary>
        public ContestDataContextFactory()
        {
        }
    }
}

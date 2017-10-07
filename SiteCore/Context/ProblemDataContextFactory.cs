namespace BITOJ.Core.Context
{
    using BITOJ.Data;

    /// <summary>
    /// 表示题目数据上下文工厂类。
    /// </summary>
    internal sealed class ProblemDataContextFactory : DataContextFactoryBase<ProblemDataContext>
    {
        /// <summary>
        /// 创建新的 ProblemDataContext 对象。
        /// </summary>
        /// <returns>新创建的 ProblemDataContext 对象。</returns>
        public override ProblemDataContext CreateContext()
        {
            return new ProblemDataContext();
        }

        /// <summary>
        /// 创建新的 ProblemDataContextFactory 对象。
        /// </summary>
        public ProblemDataContextFactory()
        {
        }
    }
}

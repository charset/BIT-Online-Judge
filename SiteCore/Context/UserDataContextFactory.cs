namespace BITOJ.Core.Context
{
    using BITOJ.Data;

    /// <summary>
    /// 表示 UserDataContext 工厂类。
    /// </summary>
    internal sealed class UserDataContextFactory : DataContextFactoryBase<UserDataContext>
    {
        /// <summary>
        /// 创建 UserDataContext 类的新实例。
        /// </summary>
        /// <returns></returns>
        public override UserDataContext CreateContext()
        {
            return new UserDataContext();
        }

        /// <summary>
        /// 创建 UserDataContextFactory 类的新实例。
        /// </summary>
        public UserDataContextFactory()
        {
        }
    }
}

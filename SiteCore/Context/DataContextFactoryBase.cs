namespace BITOJ.Core.Context
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 为数据上下文工厂类提供抽象基类。
    /// </summary>
    /// <typeparam name="TContext">数据上下文对象类型。</typeparam>
    internal abstract class DataContextFactoryBase<TContext> where TContext: IDisposable
    {
        /// <summary>
        /// 创建一个新的数据上下文对象。
        /// </summary>
        /// <returns>创建的新的数据上下文对象。</returns>
        public abstract TContext CreateContext();

        /// <summary>
        /// 在数据上下文对象上同步执行传入的委托。
        /// </summary>
        /// <param name="action">要执行的委托。</param>
        /// <exception cref="ArgumentNullException"/>
        public void WithContext(Action<TContext> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            using (TContext context = CreateContext())
            {
                action(context);
            }
        }

        /// <summary>
        /// 在数据上下文对象上同步执行传入的委托并返回结果。
        /// </summary>
        /// <typeparam name="TResult">结果对象类型。</typeparam>
        /// <param name="func">要执行的委托。</param>
        /// <returns>执行结果。</returns>
        /// <exception cref="ArgumentNullException"/>
        public TResult WithContext<TResult>(Func<TContext, TResult> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            using (TContext context = CreateContext())
            {
                return func(context);
            }
        }

        /// <summary>
        /// 在数据上下文对象上异步执行传入的委托。
        /// </summary>
        /// <param name="action">要执行的委托。</param>
        /// <returns>一个标识异步任务的 Task 对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        public async Task WithContextAsync(Action<TContext> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            using (TContext context = CreateContext())
            {
                await Task.Run(() => action(context));
            }
        }

        /// <summary>
        /// 在数据上下文对象上异步执行传入的委托并返回结果。
        /// </summary>
        /// <typeparam name="TResult">结果对象类型。</typeparam>
        /// <param name="func">要执行的委托。</param>
        /// <returns>一个标识异步任务的 Task 对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        public async Task<TResult> WithContextAsync<TResult>(Func<TContext, TResult> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            using (TContext context = CreateContext())
            {
                return await Task.Run(() => func(context));
            }
        }
    }
}

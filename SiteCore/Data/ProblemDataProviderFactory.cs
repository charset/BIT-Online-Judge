namespace BITOJ.Core
{
    using BITOJ.Core.Data;
    using System;

    /// <summary>
    /// 为创建 ProblemDataProvider 对象提供静态工厂方法。
    /// </summary>
    public static class ProblemDataProviderFactory
    {
        /// <summary>
        /// 从给定的题目句柄创建 IProblemDataProvider 对象。
        /// </summary>
        /// <param name="handle">题目句柄对象。</param>
        /// <param name="isReadonly">一个值，该值指示创建的 IProblemDataProvider 对象是否应仅具有读权限。</param>
        /// <returns>创建的 IProblemDataProvider 对象。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static IProblemDataProvider CreateDataProvider(ProblemHandle handle, bool isReadonly)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            if (handle.IsNativeProblem)
            {
                // 本地题目。
                return NativeProblemDataProvider.Create(handle, isReadonly);
            }
            else
            {
                // 非本地题目。需要 VJ 技术支持。第一版本暂不支持。
                throw new NotSupportedException("暂不支持访问非本地平台题目。");
            }
        }
    }
}

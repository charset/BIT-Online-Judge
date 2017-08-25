namespace BITOJ.Core.Resolvers
{
    using System;

    /// <summary>
    /// 为题目源 OJ 解析提供接口。
    /// </summary>
    public interface IProblemUrlResolver
    {
        /// <summary>
        /// 解析给定题目句柄的 URL。
        /// </summary>
        /// <param name="handle">待解析的题目句柄。</param>
        /// <returns>给定题目的 URL。</returns>
        /// <exception cref="ArgumentNullException"/>
        string Resolve(ProblemHandle handle);
    }
}

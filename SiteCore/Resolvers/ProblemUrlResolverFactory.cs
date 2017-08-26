namespace BITOJ.Core.Resolvers
{
    using BITOJ.Core;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 表示 ProblemOriginResolver 类的工厂类。
    /// </summary>
    public static class ProblemUrlResolverFactory
    {
        private static readonly Dictionary<OJSystem, IProblemUrlResolver> ms_prototypes;

        static ProblemUrlResolverFactory()
        {
            // 向工厂类注册原型对象。
            ms_prototypes = new Dictionary<OJSystem, IProblemUrlResolver>()
            {
                // 在这里添加原型对象。
                { OJSystem.HDU, new HduUrlResolver() },
            };
        }

        /// <summary>
        /// 根据给定的题目句柄返回 IProblemUrlResolver 原型对象。
        /// </summary>
        /// <param name="handle">题目句柄。</param>
        /// <returns>题目 URL 解析器原型对象。若工厂类中没有注册对应的原型对象，返回 null。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static IProblemUrlResolver GetUrlResolverFor(ProblemHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            OJSystem oj = ProblemOriginResolver.Resolve(handle);
            if (ms_prototypes.ContainsKey(oj))
            {
                return ms_prototypes[oj];
            }
            else
            {
                return null;
            }
        }
    }
}

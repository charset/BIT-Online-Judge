namespace BITOJ.Core.Resolvers
{
    using BITOJ.Core;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 提供对题目源 OJ 的解析操作。
    /// </summary>
    public static class ProblemOriginResolver
    {
        private static readonly List<KeyValuePair<string, OJSystem>> ms_prefixes;

        static ProblemOriginResolver()
        {
            ms_prefixes = new List<KeyValuePair<string, OJSystem>>()
            {
                new KeyValuePair<string, OJSystem>("BIT", OJSystem.BIT),
                new KeyValuePair<string, OJSystem>("BZOJ", OJSystem.BZOJ),
                new KeyValuePair<string, OJSystem>("CF", OJSystem.CodeForces),
                new KeyValuePair<string, OJSystem>("GYM", OJSystem.Gym),
                new KeyValuePair<string, OJSystem>("HDU", OJSystem.HDU),
                new KeyValuePair<string, OJSystem>("POJ", OJSystem.POJ),
                new KeyValuePair<string, OJSystem>("UVA", OJSystem.UVa),
                new KeyValuePair<string, OJSystem>("LA", OJSystem.UVaLiveArchieve),
                new KeyValuePair<string, OJSystem>("ZOJ", OJSystem.ZOJ),
            };
        }

        /// <summary>
        /// 解析给定题目句柄的源 OJ 信息。
        /// </summary>
        /// <param name="handle">要解析的题目句柄。</param>
        /// <returns>一个 OJSystem 枚举值，表示给定题目句柄的源 OJ 信息。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static OJSystem Resolve(ProblemHandle handle)
        {
            if (handle == null)
                throw new ArgumentNullException(nameof(handle));

            foreach (KeyValuePair<string, OJSystem> map in ms_prefixes)
            {
                if (handle.ProblemId.StartsWith(map.Key, StringComparison.CurrentCultureIgnoreCase))
                {
                    return map.Value;
                }
            }
            return OJSystem.BIT;
        }
    }
}

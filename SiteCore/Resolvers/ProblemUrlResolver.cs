namespace BITOJ.Core.Resolvers
{
    using BITOJ.Common.Cache.Settings;
    using BITOJ.Core.Data;
    using System;

    /// <summary>
    /// 为 ProblemEntry 提供全局统一定位符（URL）解析服务。
    /// </summary>
    public abstract class ProblemUrlResolver
    {
        private static readonly string RulesSettingName = "prob_url_resolver_rules";

        private static ProblemUrlResolverRule[] ms_rules;
        private static object ms_syncLock;

        private static void InitializeRulesFromSettings()
        {
            FileSystemSettingProvider settings = new FileSystemSettingProvider();
            if (!settings.Contains(RulesSettingName))
            {
                // 不存在相应的规则设置。
                return;
            }
            else
            {
                ms_rules = settings.Get<ProblemUrlResolverRule[]>(RulesSettingName);
            }
        }

        /// <summary>
        /// 使用指定的题目编号查询解析规则。
        /// </summary>
        /// <param name="problemId">待查询的题目编号。</param>
        /// <returns>如果期望的解析规则存在，返回相应的解析规则；否则返回 null。</returns>
        /// <exception cref="ArgumentNullException"/>
        private static ProblemUrlResolverRule GetRule(string problemId)
        {
            if (problemId == null)
                throw new ArgumentNullException(nameof(problemId));

            foreach (ProblemUrlResolverRule rule in ms_rules)
            {
                if (problemId.StartsWith(rule.Prefix, StringComparison.CurrentCultureIgnoreCase))
                {
                    return rule;
                }
            }
            return null;
        }

        private static ProblemUrlResolver ms_default;

        /// <summary>
        /// 获取全局默认 ProblemUrlResolver 对象。
        /// </summary>
        public static ProblemUrlResolver Default { get => ms_default; }

        static ProblemUrlResolver()
        {
            ms_rules = null;
            ms_syncLock = new object();

            // TODO: 初始化全局默认 ProblemUrlResolver 对象。
            ms_default = null;
        }

        /// <summary>
        /// 初始化 ProblemUrlResolver 类的新实例。
        /// </summary>
        protected ProblemUrlResolver()
        { }

        /// <summary>
        /// 在派生类中重写时，使用给定的解析器规则解析题目 ID。
        /// </summary>
        /// <param name="id">要解析的题目 ID，已经去除 OJ 前缀。/</param>
        /// <param name="rule">要使用的解析器规则。</param>
        /// <returns>题目 ID 对应的 URL。</returns>
        protected abstract string ResolveRule(string id, ProblemUrlResolverRule rule);

        /// <summary>
        /// 解析指定 ProblemHandle 对象的全局统一定位符（URL）。
        /// </summary>
        /// <param name="entry">待解析的 ProblemHandle 对象。</param>
        /// <returns>给定 ProblemHandle 对象所对应的全局统一定位符（URL）。</returns>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidProblemException"/>
        public string Get(ProblemHandle entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            ProblemUrlResolverRule rule = GetRule(entry.ProblemId);
            if (rule == null)
                throw new InvalidProblemException(entry);

            string id = entry.ProblemId.Substring(rule.Prefix.Length);
            return ResolveRule(id, rule);
        }
    }
}

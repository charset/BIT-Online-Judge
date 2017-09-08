namespace BITOJ.SiteUI.Models.Validation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// 验证数据是否与给定的一系列数据中的一个相等。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EqualToAttribute : ValidationAttribute
    {
        private IEnumerable<object> m_protovalues;

        /// <summary>
        /// 创建 EqualToAttribute 类的新实例。
        /// </summary>
        /// <param name="protovalues">要比较的元数据对象。</param>
        public EqualToAttribute(params object[] protovalues)
        {
            m_protovalues = protovalues;
        }

        /// <summary>
        /// 检查给定的值是否有效。
        /// </summary>
        /// <param name="value">给定的数据值。</param>
        /// <returns>一个值，该值指示对给定数据对象的验证是否成功。</returns>
        public override bool IsValid(object value)
        {
            foreach (object item in m_protovalues)
            {
                if (Equals(value, item))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
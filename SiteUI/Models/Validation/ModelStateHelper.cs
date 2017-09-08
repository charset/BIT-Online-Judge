namespace BITOJ.SiteUI.Models.Validation
{
    using System;
    using System.Web.Mvc;

    /// <summary>
    /// 对数据模型验证结果提取提供帮助方法。
    /// </summary>
    public static class ModelStateHelper
    {
        /// <summary>
        /// 返回数据模型上给定属性的第一个数据验证错误消息。
        /// </summary>
        /// <param name="state">数据模型验证状态字典。</param>
        /// <param name="propertyName">数据模型属性名称。</param>
        /// <returns>给定属性的第一个数据验证错误消息。若给定属性没有错误消息，返回 null。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static string GetFirstError(ModelStateDictionary state, string propertyName)
        {
            if (state == null)
                throw new ArgumentNullException(nameof(state));
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            if (state.ContainsKey(propertyName) && state[propertyName].Errors.Count > 0)
            {
                return state[propertyName].Errors[0].ErrorMessage;
            }
            else
            {
                return null;
            }
        }
    }
}
namespace BITOJ.SiteUI.Controllers.Extensions
{
    using Newtonsoft.Json;
    using System.Web.Mvc;

    /// <summary>
    /// 为控制器方法提供 JSON.Net 帮助方法。
    /// </summary>
    internal static class JsonDotNet
    {
        /// <summary>
        /// 使用 JSON.Net 组件对对象进行 JSON 序列化并返回操作结果对象。
        /// </summary>
        /// <param name="controller">控制器对象。</param>
        /// <param name="obj">要序列化的对象。</param>
        /// <returns>序列化产生的操作结果。</returns>
        public static ActionResult NewtonsoftJson(this Controller controller, object obj)
        {
            return new ContentResult()
            {
                Content = JsonConvert.SerializeObject(obj)
            };
        }
    }
}
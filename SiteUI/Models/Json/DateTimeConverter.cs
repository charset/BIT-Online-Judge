namespace BITOJ.SiteUI.Models.Json
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// 为数据模型的日期数据提供 JSON 转换操作。
    /// </summary>
    public class DateTimeConverter : DateTimeConverterBase
    {
        /// <summary>
        /// Not implemented yet.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="objectType"></param>
        /// <param name="existingValue"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将给定的数据写入 JSON 流中。
        /// </summary>
        /// <param name="writer">JSON 写对象。</param>
        /// <param name="value">要写入的 DateTime 值。</param>
        /// <param name="serializer">JSON 格式化器。</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (writer == null)
                throw new ArgumentNullException(nameof(writer));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));
            if (!(value is DateTime))
                throw new ArgumentException("value 的类型不是 DateTime");

            DateTime dt = (DateTime)value;
            writer.WriteValue(dt.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        /// <summary>
        /// 创建 DateTimeConverter 类的新实例。
        /// </summary>
        public DateTimeConverter()
        {
        }
    }
}
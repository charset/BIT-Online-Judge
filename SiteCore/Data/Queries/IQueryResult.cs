namespace BITOJ.Core.Data.Queries
{
    using System.Collections.Generic;

    /// <summary>
    /// 为数据查询结果提供接口。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQueryResult<out T> : IEnumerable<T> where T: class
    {
    }
}

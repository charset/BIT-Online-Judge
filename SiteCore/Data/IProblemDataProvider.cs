namespace BITOJ.Core.Data
{
    using BITOJ.Core.Authorization;
    using System;

    /// <summary>
    /// 为题目数据提供方提供接口。
    /// </summary>
    /// <remarks>
    /// 此处的题目数据不是指判题器使用的题目测试数据，而是诸如题目标题、题目正文描述等这一类数据。
    /// </remarks>
    public interface IProblemDataProvider : IDisposable
    {
        /// <summary>
        /// 获取或设置题目的标题。
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// 获取或设置题目的正文描述。
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// 获取或设置题目的输入数据描述。
        /// </summary>
        string InputDescription { get; set; }

        /// <summary>
        /// 获取或设置题目的输出数据描述。
        /// </summary>
        string OutputDescription { get; set; }

        /// <summary>
        /// 获取或设置题目的样例输入。
        /// </summary>
        string InputSamples { get; set; }

        /// <summary>
        /// 获取或设置题目的样例输出。
        /// </summary>
        string OutputSamples { get; set; }

        /// <summary>
        /// 获取或设置题目的来源。
        /// </summary>
        string Source { get; set; }

        /// <summary>
        /// 获取或设置题目的提示信息。
        /// </summary>
        string Hints { get; set; }

        /// <summary>
        /// 获取或设置题目的作者。
        /// </summary>
        string Author { get; set; }

        /// <summary>
        /// 获取或设置可访问当前题目数据的用户权限组。
        /// </summary>
        UserGroup AuthorizationGroup { get; set; }

        /// <summary>
        /// 获取一个值，该值指示当前的数据源是否为只读模式。
        /// </summary>
        bool IsReadOnly { get; }
    }
}

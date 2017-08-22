namespace BITOJ.Common.Cache
{
    using System;
    using System.IO;

    /// <summary>
    /// 为应用程序提供应用级别根目录。
    /// </summary>
    public static class ApplicationDirectory
    {
        private static readonly string ms_name = "BITOJ";

        /// <summary>
        /// 获取应用程序根目录。
        /// </summary>
        public static string AppDirectory
        {
            get
            {
                string physical = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "\\", ms_name);
                if (!Directory.Exists(physical))
                {
                    // 目录不存在。创建目录。
                    Directory.CreateDirectory(physical);
                }

                return physical;
            }
        }

        /// <summary>
        /// 获取应用程序子目录完全限定路径。
        /// </summary>
        /// <param name="name">子目录名称。</param>
        /// <returns>子目录的完全限定路径。</returns>
        /// <exception cref="ArgumentNullException"/>
        public static string GetAppSubDirectory(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return string.Concat(AppDirectory, "\\", name);
        }
    }
}

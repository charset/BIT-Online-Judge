namespace BITOJ.Core
{
    /// <summary>
    /// 编码用户提交所使用的语言。
    /// </summary>
    public enum SubmissionLanguage : uint
    {
        /// <summary>
        /// GNU C 编译器。
        /// </summary>
        GnuC = 0x00010001,

        /// <summary>
        /// GNU C++ 编译器。
        /// </summary>
        GnuCPlusPlus = 0x00020001,

        /// <summary>
        /// GNU C++ 编译器，支持 C++11 标准。
        /// </summary>
        GnuCPlusPlus11 = 0x00020002,

        /// <summary>
        /// GNU C++ 编译器，支持 C++14 标准。
        /// </summary>
        GnuCPlusPlus14 = 0x00020003,

        /// <summary>
        /// GNU C++ 编译器，支持 C++17 标准。
        /// </summary>
        GnuCPlusPlus17 = 0x00020004,

        /// <summary>
        /// Microsoft C++ 优化编译器。
        /// </summary>
        MicrosoftCPlusPlus = 0x00020005,

        /// <summary>
        /// Java 编译环境。
        /// </summary>
        Java = 0x00030000,

        /// <summary>
        /// Pascal 编译器。
        /// </summary>
        Pascal = 0x00040000,

        /// <summary>
        /// Python2 执行环境。
        /// </summary>
        Python2 = 0x00050001,

        /// <summary>
        /// Python3 执行环境。
        /// </summary>
        Python3 = 0x00050002,
    }
}

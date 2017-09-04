namespace BITOJ.Core.Authorization
{
    /// <summary>
    /// 对一个缓冲区提供操作。
    /// </summary>
    public static class Buffer
    {
        /// <summary>
        /// 检查两缓冲区中的数据是否相同。
        /// </summary>
        /// <param name="array1">第一个缓冲区。</param>
        /// <param name="array2">第二个缓冲区。</param>
        /// <returns>一个值，该值指示两个缓冲区是否相等。</returns>
        public static bool IsByteArraysEqual(byte[] array1, byte[] array2)
        {
            if (array1 == null && array2 == null)
            {
                return true;
            }
            else if (array1 == null || array2 == null)
            {
                return false;
            }
            else if (array1.Length != array2.Length)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < array1.Length; ++i)
                {
                    if (array1[i] != array2[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}

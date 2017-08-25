namespace BITOJ.Common.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// 封装一个映射集合。该集合实际存储 TFrom 类型集合对象，通过映射函数对外部呈现 TTarget 类型集合接口。
    /// </summary>
    /// <typeparam name="TFrom">源对象类型。</typeparam>
    /// <typeparam name="TTarget">接口对象类型。</typeparam>
    public class MappedCollection<TFrom, TTarget> : ICollection<TTarget>
    {
        private sealed class DefaultComparer<T> : IEqualityComparer<T>
        {
            public bool Equals(T x, T y)
            {
                return Equals(x, y);
            }

            public int GetHashCode(T obj)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class Enumerator : IEnumerator<TTarget>
        {
            private IEnumerator<TFrom> m_from;
            private Func<TFrom, TTarget> m_map;

            public TTarget Current => m_map(m_from.Current);

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                m_from.Dispose();
            }

            public bool MoveNext()
            {
                return m_from.MoveNext();
            }

            public void Reset()
            {
                m_from.Reset();
            }

            public Enumerator(IEnumerator<TFrom> from, Func<TFrom, TTarget> map)
            {
                m_from = from ?? throw new ArgumentNullException(nameof(from));
                m_map = map ?? throw new ArgumentNullException(nameof(map));
            }

            ~Enumerator()
            {
                Dispose();
            }
        }

        private ICollection<TFrom> m_from;
        private Func<TFrom, TTarget> m_forwardFunc;
        private Func<TTarget, TFrom> m_backwardFunc;
        private IEqualityComparer<TFrom> m_compare;

        /// <summary>
        /// 获取或设置集合内的元素数量。
        /// </summary>
        public int Count => m_from.Count;

        /// <summary>
        /// 获取或设置一个值，该值指示当前集合是否为只读。
        /// </summary>
        public bool IsReadOnly => m_from.IsReadOnly;

        /// <summary>
        /// 创建 MappedCollection 类的新实例。
        /// </summary>
        /// <param name="from">源集合对象。</param>
        /// <param name="forward">前向映射函数。该函数将源映射到外部接口。</param>
        /// <param name="backward">后向映射函数。该函数将外部接口映射到源。</param>
        /// <param name="compare">源对象比较器。</param>
        /// <exception cref="ArgumentNullException"/>
        public MappedCollection(ICollection<TFrom> from, Func<TFrom, TTarget> forward, 
            Func<TTarget, TFrom> backward, IEqualityComparer<TFrom> compare)
        {
            m_from = from ?? throw new ArgumentNullException(nameof(from));
            m_forwardFunc = forward ?? throw new ArgumentNullException(nameof(forward));
            m_backwardFunc = backward ?? throw new ArgumentNullException(nameof(backward));
            m_compare = compare ?? new DefaultComparer<TFrom>();
        }

        /// <summary>
        /// 将给定的项目添加至当前集合。
        /// </summary>
        /// <param name="item">要添加的项目。</param>
        public void Add(TTarget item)
        {
            m_from.Add(m_backwardFunc(item));
        }

        /// <summary>
        /// 清空当前集合。
        /// </summary>
        public void Clear()
        {
            m_from.Clear();
        }

        /// <summary>
        /// 判断当前集合是否存在一个特定的项目。
        /// </summary>
        /// <param name="item">待检查存在性的项目。</param>
        /// <returns>一个值，指示给定的对象是否存在于当前集合中。</returns>
        public bool Contains(TTarget item)
        {
            return m_from.Contains(m_backwardFunc(item));
        }

        /// <summary>
        /// 将当前集合中的元素复制到目标数组中。
        /// </summary>
        /// <param name="array">复制目标。</param>
        /// <param name="arrayIndex">目标数组中开始接收元素的位置。</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentException"/>
        public void CopyTo(TTarget[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < Count)
                throw new ArgumentException("目标数组空间太小。");

            int i = arrayIndex;
            foreach (TFrom item in m_from)
            {
                array[i++] = m_forwardFunc(item);
            }
        }

        /// <summary>
        /// 从当前集合中移除给定的元素。
        /// </summary>
        /// <param name="item">要移除的元素。</param>
        /// <returns>一个值，指示移除是否成功。</returns>
        public bool Remove(TTarget item)
        {
            return m_from.Remove(m_backwardFunc(item));
        }

        /// <summary>
        /// 获取当前集合对象的枚举器。
        /// </summary>
        /// <returns>当前集合对象的枚举器。</returns>
        public IEnumerator<TTarget> GetEnumerator()
        {
            return new Enumerator(m_from.GetEnumerator(), m_forwardFunc);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

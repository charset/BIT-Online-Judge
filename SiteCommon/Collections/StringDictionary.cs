namespace BITOJ.Common.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 表示键值类型为字符串的字典类实现。通常情况下，该类类似于维护一棵字典树结构。
    /// </summary>
    public class StringDictionary<TValue> : IDictionary<string, TValue>
    {
        private sealed class Trie : ICollection<string>
        {
            private const int Root = 0;

            private sealed class TrieNode
            {
                public char Character { get; set; }

                /// <summary>
                /// 当当前节点为一字符串的最后一个字符时，获取或设置该字符串的编号值；否则此属性应始终保持 -1。
                /// </summary>
                public int StringId { get; set; }

                public int Parent { get; set; }

                public Dictionary<char, int> Children { get; }

                public TrieNode(char ch)
                {
                    Character = ch;
                    StringId = -1;
                    Parent = -1;
                    Children = new Dictionary<char, int>();
                }
            }
            
            private List<TrieNode> m_nodesCache;
            private int m_stringCount;

            public Trie()
            {
                m_nodesCache = new List<TrieNode>();
                m_stringCount = 0;

                AllocateNewNode('\0');      // Allocate root node.
            }

            private int AllocateNewNode(char ch, int parent = -1)
            {
                m_nodesCache.Add(new TrieNode(ch) { Parent = parent });
                return m_nodesCache.Count - 1;
            }

            private string FetchString(int nodeId)
            {
                StringBuilder builder = new StringBuilder();
                while (nodeId != Root)
                {
                    TrieNode node = m_nodesCache[nodeId];

                    builder.Insert(0, node.Character);
                    nodeId = node.Parent;
                }

                return builder.ToString();
            }

            public int Count => m_stringCount;

            public bool IsReadOnly => false;

            public void Add(string item)
            {
                if (item == null)
                    throw new ArgumentNullException(nameof(item));

                int nodePtr = Root;
                foreach (char current in item)
                {
                    TrieNode currentNode = m_nodesCache[nodePtr];
                    if (!currentNode.Children.ContainsKey(current))
                    {
                        currentNode.Children.Add(current, AllocateNewNode(current, nodePtr));
                    }
                    nodePtr = currentNode.Children[current];
                }

                TrieNode endNode = m_nodesCache[nodePtr];
                if (endNode.StringId >= 0)
                {
                    throw new InvalidOperationException("提供的字符串已经存在于字典树中。");
                }

                endNode.StringId = m_stringCount++;
            }

            public void Clear()
            {
                m_nodesCache.Clear();
                m_stringCount = 0;

                AllocateNewNode('\0');      // Allocate root node.
            }

            public bool Contains(string item)
            {
                if (item == null)
                {
                    return false;
                }

                return GetStringId(item) >= 0;
            }

            public int GetStringId(string str)
            {
                if (str == null)
                    throw new ArgumentNullException(nameof(str));
                
                int nodePtr = Root;
                foreach (char current in str)
                {
                    TrieNode currentNode = m_nodesCache[nodePtr];
                    if (!currentNode.Children.ContainsKey(current))
                    {
                        return -1;
                    }

                    nodePtr = currentNode.Children[current];
                }

                return m_nodesCache[nodePtr].StringId;
            }

            public void CopyTo(string[] array, int arrayIndex)
            {
                if (array == null)
                    throw new ArgumentNullException(nameof(array));
                if (arrayIndex < 0)
                    throw new ArgumentOutOfRangeException(nameof(arrayIndex));
                if (array.Length - arrayIndex < m_stringCount)
                    throw new ArgumentException("数组容量不足以容纳当前字典树中的所有字符串。");

                foreach (string item in this)
                {
                    array[arrayIndex++] = string.Copy(item);
                }
            }

            public IEnumerator<string> GetEnumerator()
            {
                // 在字典树中深度优先搜索寻找字符串。
                Stack<int> dfsStack = new Stack<int>();
                dfsStack.Push(Root);

                while (dfsStack.Count > 0)
                {
                    int current = dfsStack.Pop();
                    TrieNode currentNode = m_nodesCache[current];

                    if (currentNode.StringId >= 0)
                    {
                        yield return FetchString(current);
                    }

                    foreach (int child in currentNode.Children.Values)
                    {
                        dfsStack.Push(child);
                    }
                }

                yield break;
            }

            public bool Remove(string item)
            {
                throw new InvalidOperationException("字典树不支持删除操作。");
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private List<TValue> m_values;
        private Trie m_trie;

        /// <summary>
        /// 创建 StringDictionary 类的新实例。
        /// </summary>
        public StringDictionary()
        {
            m_values = new List<TValue>();
            m_trie = new Trie();
        }

        public ICollection<string> Keys => m_trie;

        public ICollection<TValue> Values => m_values;

        public int Count => m_trie.Count;

        public bool IsReadOnly => false;

        public TValue this[string key]
        {
            get
            {
                TryGetValue(key, out TValue ret);
                return ret;
            }
            set
            {
                int id = m_trie.GetStringId(key);
                if (id < 0)
                    throw new KeyNotFoundException();

                m_values[id] = value;
            }
        }

        public bool ContainsKey(string key)
        {
            return m_trie.Contains(key);
        }

        public void Add(string key, TValue value)
        {
            m_trie.Add(key);
            m_values[m_trie.GetStringId(key)] = value;
        }

        public bool Remove(string key)
        {
            throw new NotSupportedException("字典树不支持删除操作。");
        }

        public bool TryGetValue(string key, out TValue value)
        {
            int id = m_trie.GetStringId(key);
            if (id < 0)
            {
                value = default(TValue);
                return false;
            }
            else
            {
                value = m_values[id];
                return true;
            }
        }

        public void Add(KeyValuePair<string, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            m_trie.Clear();
            m_values.Clear();
        }

        public bool Contains(KeyValuePair<string, TValue> item)
        {
            if (ContainsKey(item.Key))
            {
                return Equals(item.Value, m_values[m_trie.GetStringId(item.Key)]);
            }
            else
            {
                return false;
            }
        }

        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, TValue> item)
        {
            throw new NotSupportedException("字典树不支持删除操作。");
        }

        public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}

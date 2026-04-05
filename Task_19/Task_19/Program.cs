using System;
using System.Collections;
using System.Collections.Generic;

namespace MyCollections
{
    // Цвет узла красно-черного дерева
    public enum Color
    {
        Red,
        Black
    }

    // Узел красно-черного дерева
    public class TreeNode<K, V> where K : IComparable<K>
    {
        public K Key { get; set; }
        public V Value { get; set; }
        public Color Color { get; set; }
        public TreeNode<K, V> Left { get; set; }
        public TreeNode<K, V> Right { get; set; }
        public TreeNode<K, V> Parent { get; set; }

        public TreeNode(K key, V value, Color color)
        {
            Key = key;
            Value = value;
            Color = color;
            Left = null;
            Right = null;
            Parent = null;
        }
    }

    // Реализация красно-черного дерева
    public class MyTreeMap<K, V> where K : IComparable<K>
    {
        private TreeNode<K, V> root;
        private IComparer<K> comparator;
        private int size;

        public MyTreeMap()
        {
            comparator = Comparer<K>.Default;
            root = null;
            size = 0;
        }

        public MyTreeMap(IComparer<K> comparator)
        {
            this.comparator = comparator ?? Comparer<K>.Default;
            root = null;
            size = 0;
        }

        public int Size()
        {
            return size;
        }

        public bool IsEmpty()
        {
            return size == 0;
        }

        public void Clear()
        {
            root = null;
            size = 0;
        }

        public bool ContainsKey(K key)
        {
            return GetNode(key) != null;
        }

        public V Get(K key)
        {
            TreeNode<K, V> node = GetNode(key);
            return node != null ? node.Value : default(V);
        }

        public V Put(K key, V value)
        {
            if (root == null)
            {
                root = new TreeNode<K, V>(key, value, Color.Black);
                size++;
                return default(V);
            }

            TreeNode<K, V> parent = null;
            TreeNode<K, V> current = root;
            int cmp = 0;

            while (current != null)
            {
                parent = current;
                cmp = Compare(key, current.Key);

                if (cmp < 0)
                    current = current.Left;
                else if (cmp > 0)
                    current = current.Right;
                else
                {
                    V oldValue = current.Value;
                    current.Value = value;
                    return oldValue;
                }
            }

            TreeNode<K, V> newNode = new TreeNode<K, V>(key, value, Color.Red);
            newNode.Parent = parent;

            if (cmp < 0)
                parent.Left = newNode;
            else
                parent.Right = newNode;

            FixAfterInsertion(newNode);
            size++;
            return default(V);
        }

        public V Remove(K key)
        {
            TreeNode<K, V> node = GetNode(key);
            if (node == null)
                return default(V);

            V oldValue = node.Value;
            DeleteNode(node);
            return oldValue;
        }

        public TreeNode<K, V> GetFirstNode()
        {
            if (root == null)
                return null;

            TreeNode<K, V> current = root;
            while (current.Left != null)
                current = current.Left;

            return current;
        }

        public TreeNode<K, V> GetLastNode()
        {
            if (root == null)
                return null;

            TreeNode<K, V> current = root;
            while (current.Right != null)
                current = current.Right;

            return current;
        }

        public TreeNode<K, V> GetCeilingNode(K key)
        {
            TreeNode<K, V> current = root;
            TreeNode<K, V> result = null;

            while (current != null)
            {
                int cmp = Compare(key, current.Key);

                if (cmp <= 0)
                {
                    result = current;
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }

            return result;
        }

        public TreeNode<K, V> GetFloorNode(K key)
        {
            TreeNode<K, V> current = root;
            TreeNode<K, V> result = null;

            while (current != null)
            {
                int cmp = Compare(key, current.Key);

                if (cmp >= 0)
                {
                    result = current;
                    current = current.Right;
                }
                else
                {
                    current = current.Left;
                }
            }

            return result;
        }

        public TreeNode<K, V> GetHigherNode(K key)
        {
            TreeNode<K, V> current = root;
            TreeNode<K, V> result = null;

            while (current != null)
            {
                int cmp = Compare(key, current.Key);

                if (cmp < 0)
                {
                    result = current;
                    current = current.Left;
                }
                else
                {
                    current = current.Right;
                }
            }

            return result;
        }

        public TreeNode<K, V> GetLowerNode(K key)
        {
            TreeNode<K, V> current = root;
            TreeNode<K, V> result = null;

            while (current != null)
            {
                int cmp = Compare(key, current.Key);

                if (cmp > 0)
                {
                    result = current;
                    current = current.Right;
                }
                else
                {
                    current = current.Left;
                }
            }

            return result;
        }

        public List<K> GetKeysInOrder()
        {
            List<K> keys = new List<K>();
            InOrderTraversal(root, keys);
            return keys;
        }

        public List<K> GetKeysInOrder(TreeNode<K, V> startNode)
        {
            List<K> keys = new List<K>();
            InOrderTraversal(startNode, keys);
            return keys;
        }

        public List<K> GetKeysInRange(K from, bool fromInclusive, K to, bool toInclusive)
        {
            List<K> keys = new List<K>();
            InOrderTraversalRange(root, from, fromInclusive, to, toInclusive, keys);
            return keys;
        }

        public List<K> GetKeysHead(K to, bool inclusive)
        {
            List<K> keys = new List<K>();
            InOrderTraversalHead(root, to, inclusive, keys);
            return keys;
        }

        public List<K> GetKeysTail(K from, bool inclusive)
        {
            List<K> keys = new List<K>();
            InOrderTraversalTail(root, from, inclusive, keys);
            return keys;
        }

        public TreeNode<K, V> GetRoot()
        {
            return root;
        }

        private int Compare(K key1, K key2)
        {
            return comparator.Compare(key1, key2);
        }

        private TreeNode<K, V> GetNode(K key)
        {
            TreeNode<K, V> current = root;

            while (current != null)
            {
                int cmp = Compare(key, current.Key);

                if (cmp < 0)
                    current = current.Left;
                else if (cmp > 0)
                    current = current.Right;
                else
                    return current;
            }

            return null;
        }

        private void FixAfterInsertion(TreeNode<K, V> node)
        {
            while (node != root && node.Parent.Color == Color.Red)
            {
                if (node.Parent == node.Parent.Parent.Left)
                {
                    TreeNode<K, V> uncle = node.Parent.Parent.Right;

                    if (uncle != null && uncle.Color == Color.Red)
                    {
                        node.Parent.Color = Color.Black;
                        uncle.Color = Color.Black;
                        node.Parent.Parent.Color = Color.Red;
                        node = node.Parent.Parent;
                    }
                    else
                    {
                        if (node == node.Parent.Right)
                        {
                            node = node.Parent;
                            RotateLeft(node);
                        }

                        node.Parent.Color = Color.Black;
                        node.Parent.Parent.Color = Color.Red;
                        RotateRight(node.Parent.Parent);
                    }
                }
                else
                {
                    TreeNode<K, V> uncle = node.Parent.Parent.Left;

                    if (uncle != null && uncle.Color == Color.Red)
                    {
                        node.Parent.Color = Color.Black;
                        uncle.Color = Color.Black;
                        node.Parent.Parent.Color = Color.Red;
                        node = node.Parent.Parent;
                    }
                    else
                    {
                        if (node == node.Parent.Left)
                        {
                            node = node.Parent;
                            RotateRight(node);
                        }

                        node.Parent.Color = Color.Black;
                        node.Parent.Parent.Color = Color.Red;
                        RotateLeft(node.Parent.Parent);
                    }
                }
            }

            root.Color = Color.Black;
        }

        private void DeleteNode(TreeNode<K, V> node)
        {
            TreeNode<K, V> replacement;
            TreeNode<K, V> child;

            if (node.Left != null && node.Right != null)
            {
                TreeNode<K, V> successor = GetSuccessor(node);
                node.Key = successor.Key;
                node.Value = successor.Value;
                node = successor;
            }

            child = node.Left != null ? node.Left : node.Right;

            if (child != null)
            {
                replacement = child;
                replacement.Parent = node.Parent;

                if (node.Parent == null)
                    root = replacement;
                else if (node == node.Parent.Left)
                    node.Parent.Left = replacement;
                else
                    node.Parent.Right = replacement;

                node.Left = node.Right = node.Parent = null;

                if (node.Color == Color.Black)
                    FixAfterDeletion(replacement);
            }
            else if (node.Parent == null)
            {
                root = null;
            }
            else
            {
                if (node.Color == Color.Black)
                    FixAfterDeletion(node);

                if (node.Parent != null)
                {
                    if (node == node.Parent.Left)
                        node.Parent.Left = null;
                    else if (node == node.Parent.Right)
                        node.Parent.Right = null;

                    node.Parent = null;
                }
            }

            size--;
        }

        private void FixAfterDeletion(TreeNode<K, V> node)
        {
            while (node != root && GetColor(node) == Color.Black)
            {
                if (node == node.Parent.Left)
                {
                    TreeNode<K, V> sibling = node.Parent.Right;

                    if (GetColor(sibling) == Color.Red)
                    {
                        sibling.Color = Color.Black;
                        node.Parent.Color = Color.Red;
                        RotateLeft(node.Parent);
                        sibling = node.Parent.Right;
                    }

                    if (GetColor(sibling.Left) == Color.Black &&
                        GetColor(sibling.Right) == Color.Black)
                    {
                        sibling.Color = Color.Red;
                        node = node.Parent;
                    }
                    else
                    {
                        if (GetColor(sibling.Right) == Color.Black)
                        {
                            sibling.Left.Color = Color.Black;
                            sibling.Color = Color.Red;
                            RotateRight(sibling);
                            sibling = node.Parent.Right;
                        }

                        sibling.Color = node.Parent.Color;
                        node.Parent.Color = Color.Black;
                        sibling.Right.Color = Color.Black;
                        RotateLeft(node.Parent);
                        node = root;
                    }
                }
                else
                {
                    TreeNode<K, V> sibling = node.Parent.Left;

                    if (GetColor(sibling) == Color.Red)
                    {
                        sibling.Color = Color.Black;
                        node.Parent.Color = Color.Red;
                        RotateRight(node.Parent);
                        sibling = node.Parent.Left;
                    }

                    if (GetColor(sibling.Right) == Color.Black &&
                        GetColor(sibling.Left) == Color.Black)
                    {
                        sibling.Color = Color.Red;
                        node = node.Parent;
                    }
                    else
                    {
                        if (GetColor(sibling.Left) == Color.Black)
                        {
                            sibling.Right.Color = Color.Black;
                            sibling.Color = Color.Red;
                            RotateLeft(sibling);
                            sibling = node.Parent.Left;
                        }

                        sibling.Color = node.Parent.Color;
                        node.Parent.Color = Color.Black;
                        sibling.Left.Color = Color.Black;
                        RotateRight(node.Parent);
                        node = root;
                    }
                }
            }

            node.Color = Color.Black;
        }

        private Color GetColor(TreeNode<K, V> node)
        {
            return node == null ? Color.Black : node.Color;
        }

        private void RotateLeft(TreeNode<K, V> node)
        {
            TreeNode<K, V> rightChild = node.Right;
            node.Right = rightChild.Left;

            if (rightChild.Left != null)
                rightChild.Left.Parent = node;

            rightChild.Parent = node.Parent;

            if (node.Parent == null)
                root = rightChild;
            else if (node == node.Parent.Left)
                node.Parent.Left = rightChild;
            else
                node.Parent.Right = rightChild;

            rightChild.Left = node;
            node.Parent = rightChild;
        }

        private void RotateRight(TreeNode<K, V> node)
        {
            TreeNode<K, V> leftChild = node.Left;
            node.Left = leftChild.Right;

            if (leftChild.Right != null)
                leftChild.Right.Parent = node;

            leftChild.Parent = node.Parent;

            if (node.Parent == null)
                root = leftChild;
            else if (node == node.Parent.Right)
                node.Parent.Right = leftChild;
            else
                node.Parent.Left = leftChild;

            leftChild.Right = node;
            node.Parent = leftChild;
        }

        private TreeNode<K, V> GetSuccessor(TreeNode<K, V> node)
        {
            if (node.Right != null)
            {
                TreeNode<K, V> current = node.Right;
                while (current.Left != null)
                    current = current.Left;
                return current;
            }

            TreeNode<K, V> parent = node.Parent;
            while (parent != null && node == parent.Right)
            {
                node = parent;
                parent = parent.Parent;
            }

            return parent;
        }

        private void InOrderTraversal(TreeNode<K, V> node, List<K> keys)
        {
            if (node == null)
                return;

            InOrderTraversal(node.Left, keys);
            keys.Add(node.Key);
            InOrderTraversal(node.Right, keys);
        }

        private void InOrderTraversalRange(TreeNode<K, V> node, K from, bool fromInclusive,
                                           K to, bool toInclusive, List<K> keys)
        {
            if (node == null)
                return;

            int cmpFrom = Compare(node.Key, from);
            int cmpTo = Compare(node.Key, to);

            if (cmpFrom > 0 || (fromInclusive && cmpFrom == 0))
                InOrderTraversalRange(node.Left, from, fromInclusive, to, toInclusive, keys);

            bool include = false;
            if (cmpFrom > 0 || (fromInclusive && cmpFrom == 0))
            {
                if (cmpTo < 0 || (toInclusive && cmpTo == 0))
                    include = true;
            }

            if (include)
                keys.Add(node.Key);

            if (cmpTo < 0 || (toInclusive && cmpTo == 0))
                InOrderTraversalRange(node.Right, from, fromInclusive, to, toInclusive, keys);
        }

        private void InOrderTraversalHead(TreeNode<K, V> node, K to, bool inclusive, List<K> keys)
        {
            if (node == null)
                return;

            int cmp = Compare(node.Key, to);

            if (cmp < 0)
            {
                InOrderTraversalHead(node.Left, to, inclusive, keys);
                keys.Add(node.Key);
                InOrderTraversalHead(node.Right, to, inclusive, keys);
            }
            else if (inclusive && cmp == 0)
            {
                InOrderTraversalHead(node.Left, to, inclusive, keys);
                keys.Add(node.Key);
            }
            else
            {
                InOrderTraversalHead(node.Left, to, inclusive, keys);
            }
        }

        private void InOrderTraversalTail(TreeNode<K, V> node, K from, bool inclusive, List<K> keys)
        {
            if (node == null)
                return;

            int cmp = Compare(node.Key, from);

            if (cmp > 0)
            {
                InOrderTraversalTail(node.Left, from, inclusive, keys);
                keys.Add(node.Key);
                InOrderTraversalTail(node.Right, from, inclusive, keys);
            }
            else if (inclusive && cmp == 0)
            {
                InOrderTraversalTail(node.Left, from, inclusive, keys);
                keys.Add(node.Key);
                InOrderTraversalTail(node.Right, from, inclusive, keys);
            }
            else
            {
                InOrderTraversalTail(node.Right, from, inclusive, keys);
            }
        }
    }

    // Множество на основе красно-черного дерева
    public class MyTreeSet<E> : IEnumerable<E> where E : IComparable<E>
    {
        private MyTreeMap<E, object> m;
        private static readonly object PRESENT = new object();

        // 1) Конструктор для создания пустого множества с естественным порядком
        public MyTreeSet()
        {
            m = new MyTreeMap<E, object>();
        }

        // 2) Конструктор с указанным объектом MyTreeMap
        public MyTreeSet(MyTreeMap<E, object> map)
        {
            if (map == null)
                throw new ArgumentNullException("map");
            m = map;
        }

        // 3) Конструктор с компаратором
        public MyTreeSet(IComparer<E> comparator)
        {
            m = new MyTreeMap<E, object>(comparator);
        }

        // 4) Конструктор из массива
        public MyTreeSet(E[] a)
        {
            m = new MyTreeMap<E, object>();
            if (a != null)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    Add(a[i]);
                }
            }
        }

        // 5) Конструктор из сортированного множества
        public MyTreeSet(SortedSet<E> s)
        {
            m = new MyTreeMap<E, object>();
            if (s != null)
            {
                foreach (E item in s)
                {
                    Add(item);
                }
            }
        }

        // 6) Добавление элемента
        public bool Add(E e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            object oldValue = m.Put(e, PRESENT);
            return oldValue == null;
        }

        // 7) Добавление элементов из массива
        public bool AddAll(E[] a)
        {
            if (a == null)
                throw new ArgumentNullException("a");

            bool modified = false;
            for (int i = 0; i < a.Length; i++)
            {
                if (Add(a[i]))
                    modified = true;
            }
            return modified;
        }

        // 8) Очистка множества
        public void Clear()
        {
            m.Clear();
        }

        // 9) Проверка наличия объекта
        public bool Contains(object o)
        {
            if (o == null)
                return false;

            try
            {
                E key = (E)o;
                return m.ContainsKey(key);
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        // 10) Проверка наличия всех объектов из массива
        public bool ContainsAll(E[] a)
        {
            if (a == null)
                throw new ArgumentNullException("a");

            for (int i = 0; i < a.Length; i++)
            {
                if (!Contains(a[i]))
                    return false;
            }
            return true;
        }

        // 11) Проверка на пустоту
        public bool IsEmpty()
        {
            return m.IsEmpty();
        }

        // 12) Удаление объекта
        public bool Remove(object o)
        {
            if (o == null)
                return false;

            try
            {
                E key = (E)o;
                object oldValue = m.Remove(key);
                return oldValue != null;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        // 13) Удаление всех объектов из массива
        public bool RemoveAll(E[] a)
        {
            if (a == null)
                throw new ArgumentNullException("a");

            bool modified = false;
            for (int i = 0; i < a.Length; i++)
            {
                if (Remove(a[i]))
                    modified = true;
            }
            return modified;
        }

        // 14) Оставить только указанные объекты
        public bool RetainAll(E[] a)
        {
            if (a == null)
                throw new ArgumentNullException("a");

            MyTreeSet<E> toRetain = new MyTreeSet<E>(a);
            bool modified = false;

            E[] allElements = ToArray();
            for (int i = 0; i < allElements.Length; i++)
            {
                if (!toRetain.Contains(allElements[i]))
                {
                    Remove(allElements[i]);
                    modified = true;
                }
            }

            return modified;
        }

        // 15) Размер множества
        public int Size()
        {
            return m.Size();
        }

        // 16) Преобразование в массив объектов
        public E[] ToArray()
        {
            List<E> keys = m.GetKeysInOrder();
            E[] result = new E[keys.Count];
            for (int i = 0; i < keys.Count; i++)
            {
                result[i] = keys[i];
            }
            return result;
        }

        // 17) Преобразование в массив с указанным типом
        public E[] ToArray(E[] a)
        {
            if (a == null)
            {
                return ToArray();
            }

            List<E> keys = m.GetKeysInOrder();

            if (a.Length < keys.Count)
            {
                a = new E[keys.Count];
            }

            for (int i = 0; i < keys.Count; i++)
            {
                a[i] = keys[i];
            }

            if (a.Length > keys.Count)
            {
                a[keys.Count] = default(E);
            }

            return a;
        }

        // 18) Первый (наименьший) элемент
        public E First()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Set is empty");

            TreeNode<E, object> node = m.GetFirstNode();
            return node.Key;
        }

        // 19) Последний (наибольший) элемент
        public E Last()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Set is empty");

            TreeNode<E, object> node = m.GetLastNode();
            return node.Key;
        }

        // 20) Подмножество из диапазона [fromElement; toElement)
        public MyTreeSet<E> SubSet(E fromElement, E toElement)
        {
            if (fromElement == null || toElement == null)
                throw new ArgumentNullException();

            if (fromElement.CompareTo(toElement) > 0)
                throw new ArgumentException("fromElement больше, чем toElement");

            MyTreeSet<E> result = new MyTreeSet<E>();
            List<E> keys = m.GetKeysInRange(fromElement, true, toElement, false);

            for (int i = 0; i < keys.Count; i++)
            {
                result.Add(keys[i]);
            }

            return result;
        }

        // 21) Множество элементов, меньших чем указанный
        public MyTreeSet<E> HeadSet(E toElement)
        {
            if (toElement == null)
                throw new ArgumentNullException();

            MyTreeSet<E> result = new MyTreeSet<E>();
            List<E> keys = m.GetKeysHead(toElement, false);

            for (int i = 0; i < keys.Count; i++)
            {
                result.Add(keys[i]);
            }

            return result;
        }

        // 22) Множество элементов, больших или равных указанному
        public MyTreeSet<E> TailSet(E fromElement)
        {
            if (fromElement == null)
                throw new ArgumentNullException();

            MyTreeSet<E> result = new MyTreeSet<E>();
            List<E> keys = m.GetKeysTail(fromElement, true);

            for (int i = 0; i < keys.Count; i++)
            {
                result.Add(keys[i]);
            }

            return result;
        }

        // 23) Наименьший элемент e >= obj
        public E Ceiling(E obj)
        {
            if (obj == null)
                throw new ArgumentNullException();

            TreeNode<E, object> node = m.GetCeilingNode(obj);
            return node != null ? node.Key : default(E);
        }

        // 24) Наибольший элемент e <= obj
        public E Floor(E obj)
        {
            if (obj == null)
                throw new ArgumentNullException();

            TreeNode<E, object> node = m.GetFloorNode(obj);
            return node != null ? node.Key : default(E);
        }

        // 25) Наименьший элемент e > obj
        public E Higher(E obj)
        {
            if (obj == null)
                throw new ArgumentNullException();

            TreeNode<E, object> node = m.GetHigherNode(obj);
            return node != null ? node.Key : default(E);
        }

        // 26) Наибольший элемент e < obj
        public E Lower(E obj)
        {
            if (obj == null)
                throw new ArgumentNullException();

            TreeNode<E, object> node = m.GetLowerNode(obj);
            return node != null ? node.Key : default(E);
        }

        // 27) Множество элементов, меньших upperBound
        public MyTreeSet<E> HeadSet(E upperBound, bool incl)
        {
            if (upperBound == null)
                throw new ArgumentNullException();

            MyTreeSet<E> result = new MyTreeSet<E>();
            List<E> keys = m.GetKeysHead(upperBound, incl);

            for (int i = 0; i < keys.Count; i++)
            {
                result.Add(keys[i]);
            }

            return result;
        }

        // 28) Множество элементов из диапазона
        public MyTreeSet<E> SubSet(E lowerBound, bool lowIncl, E upperBound, bool highIncl)
        {
            if (lowerBound == null || upperBound == null)
                throw new ArgumentNullException();

            if (lowerBound.CompareTo(upperBound) > 0)
                throw new ArgumentException("lowerBound больше, чем upperBound");

            MyTreeSet<E> result = new MyTreeSet<E>();
            List<E> keys = m.GetKeysInRange(lowerBound, lowIncl, upperBound, highIncl);

            for (int i = 0; i < keys.Count; i++)
            {
                result.Add(keys[i]);
            }

            return result;
        }

        // 29) Множество элементов, больших или равных fromElement
        public MyTreeSet<E> TailSet(E fromElement, bool inclusive)
        {
            if (fromElement == null)
                throw new ArgumentNullException();

            MyTreeSet<E> result = new MyTreeSet<E>();
            List<E> keys = m.GetKeysTail(fromElement, inclusive);

            for (int i = 0; i < keys.Count; i++)
            {
                result.Add(keys[i]);
            }

            return result;
        }

        // 30) Возврат и удаление последнего элемента
        public E PollLast()
        {
            if (IsEmpty())
                return default(E);

            TreeNode<E, object> node = m.GetLastNode();
            E key = node.Key;
            m.Remove(key);
            return key;
        }

        // 31) Возврат и удаление первого элемента
        public E PollFirst()
        {
            if (IsEmpty())
                return default(E);

            TreeNode<E, object> node = m.GetFirstNode();
            E key = node.Key;
            m.Remove(key);
            return key;
        }

        // 32) Обратный итератор
        public IEnumerator<E> DescendingIterator()
        {
            E[] elements = ToArray();
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                yield return elements[i];
            }
        }

        // 33) Обратное множество
        public MyTreeSet<E> DescendingSet()
        {
            MyTreeSet<E> result = new MyTreeSet<E>();
            E[] elements = ToArray();

            for (int i = elements.Length - 1; i >= 0; i--)
            {
                result.Add(elements[i]);
            }

            return result;
        }

        // Реализация IEnumerable
        public IEnumerator<E> GetEnumerator()
        {
            E[] elements = ToArray();
            for (int i = 0; i < elements.Length; i++)
            {
                yield return elements[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                // Создание пустого множества и добавление элементов
                MyTreeSet<int> set1 = new MyTreeSet<int>();
                set1.Add(5);
                set1.Add(3);
                set1.Add(7);
                set1.Add(1);
                set1.Add(9);

                Console.Write("Множество: ");
                foreach (int item in set1)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine("\nРазмер: " + set1.Size());

                // Проверка наличия элементов
                Console.WriteLine("Содержит 3: " + set1.Contains(3));
                Console.WriteLine("Содержит 10: " + set1.Contains(10));

                // Удаление элементов
                set1.Remove(3);
                Console.Write("После удаления 3: ");
                foreach (int item in set1)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine();

                // Первый и последний элементы
                Console.WriteLine("Первый: " + set1.First());
                Console.WriteLine("Последний: " + set1.Last());

                // Ceiling, floor, higher, lower
                Console.WriteLine("Ceiling(4): " + set1.Ceiling(4));
                Console.WriteLine("Floor(4): " + set1.Floor(4));
                Console.WriteLine("Higher(5): " + set1.Higher(5));
                Console.WriteLine("Lower(5): " + set1.Lower(5));

                // Подмножества
                MyTreeSet<int> subSet = set1.SubSet(2, 8);
                Console.Write("SubSet(2, 8): ");
                foreach (int item in subSet)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine();

                MyTreeSet<int> headSet = set1.HeadSet(7);
                Console.Write("HeadSet(7): ");
                foreach (int item in headSet)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine();

                MyTreeSet<int> tailSet = set1.TailSet(5);
                Console.Write("TailSet(5): ");
                foreach (int item in tailSet)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine();

                // pollFirst и pollLast
                Console.WriteLine("PollFirst: " + set1.PollFirst());
                Console.WriteLine("PollLast: " + set1.PollLast());
                Console.Write("После удаления: ");
                foreach (int item in set1)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine();

                // Обратный итератор
                Console.Write("\nНовый массив: 10, 20, 30, 40\n");
                MyTreeSet<int> set2 = new MyTreeSet<int>();
                set2.Add(10);
                set2.Add(20);
                set2.Add(30);
                set2.Add(40);

                Console.Write("Обратный итератор: ");
                IEnumerator<int> descIter = set2.DescendingIterator();
                while (descIter.MoveNext())
                {
                    Console.Write(descIter.Current + " ");
                }
                Console.WriteLine();

                MyTreeSet<int> descSet = set2.DescendingSet();
                Console.Write("Обратное множество: ");
                foreach (int item in descSet)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine();

                // Работа с массивами
                int[] arr = new int[] { 100, 200, 300 };
                set2.AddAll(arr);
                Console.Write("После добавления массива: ");
                foreach (int item in set2)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine();

                int[] toArray = set2.ToArray();
                Console.Write("ToArray: ");
                foreach (int item in toArray)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine();

                // Очистка
                Console.WriteLine("\nТест 10: Очистка");
                set2.Clear();
                Console.WriteLine("IsEmpty: " + set2.IsEmpty());
                Console.WriteLine("Size: " + set2.Size());

                Console.WriteLine("\nВсе тесты выполнены успешно!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }
    }
}
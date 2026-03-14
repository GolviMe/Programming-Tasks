using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_19
{
    public class MyTreeMap<K, V>
    {
        private class Node
        {
            public K Key { get; set; }
            public V Value { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public Node Parent { get; set; }

            public Node(K key, V value, Node parent)
            {
                Key = key;
                Value = value;
                Parent = parent;
            }
        }

        // Поля класса
        private IComparer<K> comparator;
        private Node root;
        private int size;
        public IComparer<K> Comparator
        {
            get { return comparator; }
        }

        // 1) Конструктор по умолчанию (естественный порядок)
        public MyTreeMap()
        {
            comparator = Comparer<K>.Default;
            root = null;
            size = 0;
        }

        // 2) Конструктор с компаратором
        public MyTreeMap(IComparer<K> comp)
        {
            comparator = comp ?? Comparer<K>.Default; // Присвоить comparator null при comp == null
            root = null;
            size = 0;
        }

        // 3) Очистка отображения
        public void Clear()
        {
            root = null;
            size = 0;
        }

        // 4) Проверка наличия ключа
        public bool ContainsKey(object key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            return GetNode((K)key) != null;
        }

        // 5) Проверка наличия значения
        public bool ContainsValue(object value)
        {
            return ContainsValue(root, value);
        }

        private bool ContainsValue(Node node, object value)
        {
            if (node == null)
                return false;

            if (value == null)
            {
                if (node.Value == null)
                    return true;
            }
            else
            {
                if (value.Equals(node.Value))
                    return true;
            }

            return ContainsValue(node.Left, value) || ContainsValue(node.Right, value);
        }

        // 6) Возврат всех пар ключ-значение
        public List<KeyValuePair<K, V>> EntrySet()
        {
            List<KeyValuePair<K, V>> result = new List<KeyValuePair<K, V>>();
            InOrderCollectEntries(root, result);
            return result;
        }

        private void InOrderCollectEntries(Node node, List<KeyValuePair<K, V>> list)
        {
            if (node == null)
                return;

            InOrderCollectEntries(node.Left, list);
            list.Add(new KeyValuePair<K, V>(node.Key, node.Value));
            InOrderCollectEntries(node.Right, list);
        }

        // 7) Получение значения по ключу
        public V Get(object key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            Node node = GetNode((K)key);
            return node == null ? default(V) : node.Value;
        }

        private Node GetNode(K key)
        {
            Node current = root;
            while (current != null)
            {
                int cmp = comparator.Compare(key, current.Key);
                if (cmp < 0)
                    current = current.Left;
                else if (cmp > 0)
                    current = current.Right;
                else
                    return current;
            }
            return null;
        }

        // 8) Проверка на пустоту
        public bool IsEmpty()
        {
            return size == 0;
        }

        // 9) Возврат всех ключей
        public List<K> KeySet()
        {
            List<K> result = new List<K>();
            InOrderCollectKeys(root, result);
            return result;
        }

        private void InOrderCollectKeys(Node node, List<K> list)
        {
            if (node == null)
                return;

            InOrderCollectKeys(node.Left, list);
            list.Add(node.Key);
            InOrderCollectKeys(node.Right, list);
        }

        // 10) Добавление пары ключ-значение
        public V Put(K key, V value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (root == null)
            {
                root = new Node(key, value, null);
                size++;
                return default(V);
            }

            Node current = root;
            Node parent = null;
            int cmp = 0;

            while (current != null)
            {
                parent = current;
                cmp = comparator.Compare(key, current.Key);

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

            Node newNode = new Node(key, value, parent);
            if (cmp < 0)
                parent.Left = newNode;
            else
                parent.Right = newNode;

            size++;
            return default(V);
        }

        // 11) Удаление по ключу
        public V Remove(object key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            Node node = GetNode((K)key);
            if (node == null)
                return default(V);

            V oldValue = node.Value;
            DeleteNode(node);
            return oldValue;
        }

        private void DeleteNode(Node node)
        {
            if (node.Left == null && node.Right == null)
            {
                // Узел - лист
                if (node.Parent == null)
                    root = null;
                else if (node == node.Parent.Left)
                    node.Parent.Left = null;
                else
                    node.Parent.Right = null;
            }
            else if (node.Left == null)
            {
                // Только правый потомок
                if (node.Parent == null)
                {
                    root = node.Right;
                    root.Parent = null;
                }
                else if (node == node.Parent.Left)
                {
                    node.Parent.Left = node.Right;
                    node.Right.Parent = node.Parent;
                }
                else
                {
                    node.Parent.Right = node.Right;
                    node.Right.Parent = node.Parent;
                }
            }
            else if (node.Right == null)
            {
                // Только левый потомок
                if (node.Parent == null)
                {
                    root = node.Left;
                    root.Parent = null;
                }
                else if (node == node.Parent.Left)
                {
                    node.Parent.Left = node.Left;
                    node.Left.Parent = node.Parent;
                }
                else
                {
                    node.Parent.Right = node.Left;
                    node.Left.Parent = node.Parent;
                }
            }
            else
            {
                // Два потомка - ищем минимальный в правом поддереве
                Node successor = FindMin(node.Right);
                node.Key = successor.Key;
                node.Value = successor.Value;
                DeleteNode(successor);
                return;
            }

            size--;
        }

        private Node FindMin(Node node)
        {
            while (node.Left != null)
                node = node.Left;
            return node;
        }

        private Node FindMax(Node node)
        {
            while (node.Right != null)
                node = node.Right;
            return node;
        }

        // 12) Размер отображения
        public int Size()
        {
            return size;
        }

        // 13) Первый ключ
        public K FirstKey()
        {
            if (root == null)
                throw new InvalidOperationException("Отображение пусто");

            return FindMin(root).Key;
        }

        // 14) Последний ключ
        public K LastKey()
        {
            if (root == null)
                throw new InvalidOperationException("Отображение пусто");

            return FindMax(root).Key;
        }

        // 15) headMap - элементы с ключом меньше end
        public MyTreeMap<K, V> HeadMap(K end)
        {
            MyTreeMap<K, V> result = new MyTreeMap<K, V>(comparator);
            AddToMap(root, result, key => comparator.Compare(key, end) < 0);
            return result;
        }

        // 16) subMap - элементы с ключом от start (включительно) до end (исключительно)
        public MyTreeMap<K, V> SubMap(K start, K end)
        {
            if (comparator.Compare(start, end) > 0)
                throw new ArgumentException("start должен быть меньше end");

            MyTreeMap<K, V> result = new MyTreeMap<K, V>(comparator);
            AddToMap(root, result, key =>
                comparator.Compare(key, start) >= 0 &&
                comparator.Compare(key, end) < 0);
            return result;
        }

        // 17) tailMap - элементы с ключом больше или равно start
        public MyTreeMap<K, V> TailMap(K start)
        {
            MyTreeMap<K, V> result = new MyTreeMap<K, V>(comparator);
            AddToMap(root, result, key => comparator.Compare(key, start) >= 0);
            return result;
        }

        private void AddToMap(Node node, MyTreeMap<K, V> map, Predicate<K> condition)
        {
            if (node == null)
                return;

            if (condition(node.Key))
                map.Put(node.Key, node.Value);

            AddToMap(node.Left, map, condition);
            AddToMap(node.Right, map, condition);
        }

        // 18) lowerEntry - пара с ключом строго меньше заданного
        public KeyValuePair<K, V>? LowerEntry(K key)
        {
            Node node = LowerNode(root, key);
            return node == null ? (KeyValuePair<K, V>?)null :
                new KeyValuePair<K, V>(node.Key, node.Value);
        }

        private Node LowerNode(Node node, K key)
        {
            Node result = null;
            while (node != null)
            {
                int cmp = comparator.Compare(node.Key, key);
                if (cmp < 0)
                {
                    result = node;
                    node = node.Right;
                }
                else
                    node = node.Left;
            }
            return result;
        }

        // 19) floorEntry - пара с ключом меньше или равно
        public KeyValuePair<K, V>? FloorEntry(K key)
        {
            Node node = FloorNode(root, key);
            return node == null ? (KeyValuePair<K, V>?)null :
                new KeyValuePair<K, V>(node.Key, node.Value);
        }

        private Node FloorNode(Node node, K key)
        {
            Node result = null;
            while (node != null)
            {
                int cmp = comparator.Compare(node.Key, key);
                if (cmp <= 0)
                {
                    result = node;
                    node = node.Right;
                }
                else
                    node = node.Left;
            }
            return result;
        }

        // 20) higherEntry - пара с ключом строго больше
        public KeyValuePair<K, V>? HigherEntry(K key)
        {
            Node node = HigherNode(root, key);
            return node == null ? (KeyValuePair<K, V>?)null :
                new KeyValuePair<K, V>(node.Key, node.Value);
        }

        private Node HigherNode(Node node, K key)
        {
            Node result = null;
            while (node != null)
            {
                int cmp = comparator.Compare(node.Key, key);
                if (cmp > 0)
                {
                    result = node;
                    node = node.Left;
                }
                else
                    node = node.Right;
            }
            return result;
        }

        // 21) ceilingEntry - пара с ключом больше или равно
        public KeyValuePair<K, V>? CeilingEntry(K key)
        {
            Node node = CeilingNode(root, key);
            return node == null ? (KeyValuePair<K, V>?)null :
                new KeyValuePair<K, V>(node.Key, node.Value);
        }

        private Node CeilingNode(Node node, K key)
        {
            Node result = null;
            while (node != null)
            {
                int cmp = comparator.Compare(node.Key, key);
                if (cmp >= 0)
                {
                    result = node;
                    node = node.Left;
                }
                else
                    node = node.Right;
            }
            return result;
        }

        // 22) lowerKey - ключ строго меньше заданного
        public K LowerKey(K key)
        {
            Node node = LowerNode(root, key);
            return node == null ? default(K) : node.Key;
        }

        // 23) floorKey - ключ меньше или равно
        public K FloorKey(K key)
        {
            Node node = FloorNode(root, key);
            return node == null ? default(K) : node.Key;
        }

        // 24) higherKey - ключ строго больше
        public K HigherKey(K key)
        {
            Node node = HigherNode(root, key);
            return node == null ? default(K) : node.Key;
        }

        // 25) ceilingKey - ключ больше или равно
        public K CeilingKey(K key)
        {
            Node node = CeilingNode(root, key);
            return node == null ? default(K) : node.Key;
        }

        // 26) pollFirstEntry - удалить и вернуть первый элемент
        public KeyValuePair<K, V>? PollFirstEntry()
        {
            if (root == null)
                return null;

            Node min = FindMin(root);
            KeyValuePair<K, V> result = new KeyValuePair<K, V>(min.Key, min.Value);
            DeleteNode(min);
            return result;
        }

        // 27) pollLastEntry - удалить и вернуть последний элемент
        public KeyValuePair<K, V>? PollLastEntry()
        {
            if (root == null)
                return null;

            Node max = FindMax(root);
            KeyValuePair<K, V> result = new KeyValuePair<K, V>(max.Key, max.Value);
            DeleteNode(max);
            return result;
        }

        // 28) firstEntry - вернуть первый элемент без удаления
        public KeyValuePair<K, V>? FirstEntry()
        {
            if (root == null)
                return null;

            Node min = FindMin(root);
            return new KeyValuePair<K, V>(min.Key, min.Value);
        }

        // 29) lastEntry - вернуть последний элемент без удаления
        public KeyValuePair<K, V>? LastEntry()
        {
            if (root == null)
                return null;

            Node max = FindMax(root);
            return new KeyValuePair<K, V>(max.Key, max.Value);
        }
    }
}


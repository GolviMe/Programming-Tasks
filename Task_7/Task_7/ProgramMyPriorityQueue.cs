using System;
using System.Collections;
using System.Collections.Generic;

public delegate int PriorityQueueComparer<T>(T x, T y);

public class MyPriorityQueue<T> : IEnumerable<T>
{
    private T[] queue;
    private int size;
    private PriorityQueueComparer<T> comparator;
    private const int DEFAULT_CAPACITY = 11;

    // 1) Конструктор по умолчанию
    public MyPriorityQueue()
    {
        queue = new T[DEFAULT_CAPACITY];
        size = 0;
        comparator = DefaultComparer;
    }

    // 2) Конструктор из массива
    public MyPriorityQueue(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));

        queue = new T[Math.Max(a.Length, DEFAULT_CAPACITY)];
        size = a.Length;
        comparator = DefaultComparer;

        Array.Copy(a, queue, a.Length);
        BuildHeap();
    }

    // 3) Конструктор с начальной ёмкостью
    public MyPriorityQueue(int initialCapacity)
    {
        if (initialCapacity < 1) throw new ArgumentException("Ёмкость должна быть положительной");

        queue = new T[initialCapacity];
        size = 0;
        comparator = DefaultComparer;
    }

    // 4) Конструктор с ёмкостью и компаратором
    public MyPriorityQueue(int initialCapacity, PriorityQueueComparer<T> comparator) : this(initialCapacity)
    {
        this.comparator = comparator ?? DefaultComparer;
    }

    // 5) Конструктор копирования
    public MyPriorityQueue(MyPriorityQueue<T> other)
    {
        if (other == null) throw new ArgumentNullException(nameof(other));

        queue = new T[other.queue.Length];
        size = other.size;
        comparator = other.comparator;
        Array.Copy(other.queue, queue, other.queue.Length);
    }

    // 6) Добавление элемента
    public void Add(T e)
    {
        if (e == null) throw new ArgumentNullException(nameof(e));

        EnsureCapacity(size + 1);
        queue[size] = e;
        SiftUp(size);
        size++;
    }

    // 7) Добавление всех элементов массива
    public void AddAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));

        foreach (var item in a)
        {
            Add(item);
        }
    }

    // 8) Очистка
    public void Clear()
    {
        Array.Clear(queue, 0, size);
        size = 0;
    }

    // 9) Проверка наличия элемента
    public bool Contains(object o)
    {
        if (o == null) return false;
        if (!(o is T)) return false;

        T item = (T)o;
        for (int i = 0; i < size; i++)
        {
            if (EqualityComparer<T>.Default.Equals(queue[i], item))
                return true;
        }
        return false;
    }

    // 10) Проверка наличия всех элементов
    public bool ContainsAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));

        foreach (var item in a)
        {
            if (!Contains(item)) return false;
        }
        return true;
    }

    // 11) Проверка на пустоту
    public bool IsEmpty() => size == 0;

    // 12) Удаление элемента
    public bool Remove(object o)
    {
        if (o == null) return false;
        if (!(o is T)) return false;

        T item = (T)o;
        int index = -1;

        // Находим индекс
        for (int i = 0; i < size; i++)
        {
            if (EqualityComparer<T>.Default.Equals(queue[i], item))
            {
                index = i;
                break;
            }
        }

        if (index == -1) return false;

        // Удаляем элемент
        size--;
        queue[index] = queue[size];
        queue[size] = default(T);

        // Восстанавливаем кучу
        if (index < size)
        {
            SiftDown(index);
            SiftUp(index);
        }

        return true;
    }

    // 13) Удаление всех элементов массива
    public bool RemoveAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));

        bool modified = false;
        foreach (var item in a)
        {
            if (Remove(item)) modified = true;
        }
        return modified;
    }

    // 14) Оставить только указанные элементы
    public bool RetainAll(T[] a)
    {
        if (a == null) throw new ArgumentNullException(nameof(a));

        var set = new HashSet<T>(a);
        var newQueue = new List<T>();

        for (int i = 0; i < size; i++)
        {
            if (set.Contains(queue[i]))
                newQueue.Add(queue[i]);
        }

        bool modified = newQueue.Count != size;

        // Перестраиваем очередь
        Clear();
        foreach (var item in newQueue)
        {
            Add(item);
        }

        return modified;
    }

    // 15) Получение размера
    public int Size() => size;

    // 16) В массив объектов
    public object[] ToArray()
    {
        object[] result = new object[size];
        Array.Copy(queue, result, size);
        return result;
    }

    // 17) В массив типа T
    public T[] ToArray(T[] a)
    {
        if (a == null) a = new T[size];
        if (a.Length < size) a = new T[size];

        Array.Copy(queue, a, size);
        if (a.Length > size) a[size] = default(T);

        return a;
    }

    // 18) Элемент из верхушки (без удаления)
    public T Element()
    {
        if (size == 0) throw new InvalidOperationException("Очередь пуста");
        return queue[0];
    }

    // 19) Попытка добавления
    public bool Offer(T obj)
    {
        try
        {
            Add(obj);
            return true;
        }
        catch
        {
            return false;
        }
    }

    // 20) Просмотр верхушки
    public T Peek() => size == 0 ? default(T) : queue[0];

    // 21) Извлечение из верхушки
    public T Poll()
    {
        if (size == 0) return default(T);

        T result = queue[0];
        size--;
        queue[0] = queue[size];
        queue[size] = default(T);

        if (size > 0) SiftDown(0);

        return result;
    }

    // Вспомогательные методы
    private int DefaultComparer(T x, T y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        if (x is IComparable<T> comparableT)
            return comparableT.CompareTo(y);

        if (x is IComparable comparable)
            return comparable.CompareTo(y);

        throw new InvalidOperationException("Тип не реализует IComparable");
    }

    private void EnsureCapacity(int minCapacity)
    {
        if (minCapacity > queue.Length)
        {
            int newCapacity;
            if (queue.Length < 64)
                newCapacity = queue.Length * 2;
            else
                newCapacity = queue.Length + (queue.Length >> 1);

            if (newCapacity < minCapacity)
                newCapacity = minCapacity;

            Array.Resize(ref queue, newCapacity);
        }
    }

    private void BuildHeap()
    {
        for (int i = size / 2 - 1; i >= 0; i--)
        {
            SiftDown(i);
        }
    }

    private void SiftUp(int index)
    {
        while (index > 0)
        {
            int parent = (index - 1) / 2;
            if (comparator(queue[parent], queue[index]) <= 0)
                break;

            Swap(parent, index);
            index = parent;
        }
    }

    private void SiftDown(int index)
    {
        while (true)
        {
            int left = 2 * index + 1;
            int right = 2 * index + 2;
            int smallest = index;

            if (left < size && comparator(queue[left], queue[smallest]) < 0)
                smallest = left;

            if (right < size && comparator(queue[right], queue[smallest]) < 0)
                smallest = right;

            if (smallest == index) break;

            Swap(index, smallest);
            index = smallest;
        }
    }

    private void Swap(int i, int j)
    {
        T temp = queue[i];
        queue[i] = queue[j];
        queue[j] = temp;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < size; i++)
            yield return queue[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

//class Program
//{
//    static int CompareInt(int x, int y) => x.CompareTo(y);
//
//    static void Main()
//    {
//        Console.WriteLine("Тестирование");
//
//        // 1. Тест конструктора по умолчанию
//        Console.WriteLine("\n1. Тест конструктора по умолчанию:");
//        var queue1 = new MyPriorityQueue<int>();
//        Console.WriteLine($"Размер: {queue1.Size()}, Проверка на пустоту: {queue1.IsEmpty()}");
//
//        // 2. Тест добавления элементов
//        Console.WriteLine("\n2. Тест добавления элементов:");
//        queue1.Add(10);
//        queue1.Add(5);
//        queue1.Add(20);
//        queue1.Add(3);
//        queue1.Add(15);
//        Console.WriteLine($"Размер: {queue1.Size()}, Верхушка: {queue1.Peek()}");
//
//        // 3. Тест Peek и Poll
//        Console.WriteLine("\n3. Тест Peek и Poll:");
//        Console.WriteLine($"Peek: {queue1.Peek()}");
//        Console.WriteLine($"Poll: {queue1.Poll()}");
//        Console.WriteLine($"После Poll Peek: {queue1.Peek()}");
//        Console.WriteLine($"Размер: {queue1.Size()}");
//
//        // 4. Тест конструктора из массива
//        Console.WriteLine("\n4. Тест конструктора из массива:");
//        int[] array = { 30, 10, 40, 5, 25 };
//        var queue2 = new MyPriorityQueue<int>(array);
//        Console.WriteLine($"Размер: {queue2.Size()}, Верхушка: {queue2.Peek()}");
//
//        // 5. Тест Contains
//        Console.WriteLine("\n5. Тест Contains:");
//        Console.WriteLine($"Содержит 10: {queue2.Contains(10)}");
//        Console.WriteLine($"Содержит 99: {queue2.Contains(99)}");
//
//        // 6. Тест Remove
//        Console.WriteLine("\n6. Тест Remove:");
//        Console.WriteLine($"Удалить 10: {queue2.Remove(10)}");
//        Console.WriteLine($"Содержит 10 после удаления: {queue2.Contains(10)}");
//        Console.WriteLine($"Размер после удаления: {queue2.Size()}");
//
//        // 7. Тест ToArray
//        Console.WriteLine("\n7. Тест ToArray:");
//        int[] resultArray = queue2.ToArray(new int[0]);
//        Console.Write("Элементы в массиве: ");
//        foreach (var item in resultArray)
//            Console.Write($"{item} ");
//        Console.WriteLine();
//
//        // 8. Тест конструктора с компаратором
//        Console.WriteLine("\n8. Тест конструктора с компаратором:");
//        var queue3 = new MyPriorityQueue<int>(10, CompareInt);
//        queue3.AddAll(new int[] { 100, 50, 75, 25 });
//        Console.WriteLine($"Размер: {queue3.Size()}, Верхушка: {queue3.Peek()}");
//
//        // 9. Тест конструктора копирования
//        Console.WriteLine("\n9. Тест конструктора копирования:");
//        var queue4 = new MyPriorityQueue<int>(queue3);
//        Console.WriteLine($"Размер оригинала: {queue3.Size()}, Размер копии: {queue4.Size()}");
//        Console.WriteLine($"Верхушка оригинала: {queue3.Peek()}, Верхушка копии: {queue4.Peek()}");
//
//        // 10. Тест Element() и исключения
//        Console.WriteLine("\n10. Тест Element() и исключения:");
//        try
//        {
//            var emptyQueue = new MyPriorityQueue<int>();
//            Console.WriteLine($"Element на пустой очереди: {emptyQueue.Element()}");
//        }
//        catch (InvalidOperationException ex)
//        {
//            Console.WriteLine($"Ожидаемое исключение: {ex.Message}");
//        }
//
//        // 11. Тест Offer
//        Console.WriteLine("\n11. Тест Offer:");
//        Console.WriteLine($"Offer(100): {queue1.Offer(100)}");
//        Console.WriteLine($"Offer(null) на очереди с ссылочным типом:");
//
//        var stringQueue = new MyPriorityQueue<string>();
//        try
//        {
//            stringQueue.Add(null);
//        }
//        catch (ArgumentNullException ex)
//        {
//            Console.WriteLine($"Ожидаемое исключение при Add(null): {ex.ParamName}");
//        }
//        Console.WriteLine($"Offer(null): {stringQueue.Offer(null)}");
//
//        // 12. Тест Clear
//        Console.WriteLine("\n12. Тест Clear:");
//        Console.WriteLine($"Размер до Clear: {queue2.Size()}");
//        queue2.Clear();
//        Console.WriteLine($"Размер после Clear: {queue2.Size()}, Проверка на пустоту: {queue2.IsEmpty()}");
//
//        // 13. Тест ContainsAll
//        Console.WriteLine("\n13. Тест ContainsAll:");
//        var queue5 = new MyPriorityQueue<int>(new int[] { 1, 2, 3, 4, 5 });
//        Console.WriteLine($"Содержит [2,3,4]: {queue5.ContainsAll(new int[] { 2, 3, 4 })}");
//        Console.WriteLine($"Содержит [2,3,6]: {queue5.ContainsAll(new int[] { 2, 3, 6 })}");
//
//        // 14. Тест RemoveAll
//        Console.WriteLine("\n14. Тест RemoveAll:");
//        Console.WriteLine($"Удалить [2,4,6]: {queue5.RemoveAll(new int[] { 2, 4, 6 })}");
//        Console.WriteLine($"Размер после RemoveAll: {queue5.Size()}");
//
//        // 15. Тест RetainAll
//        Console.WriteLine("\n15. Тест RetainAll:");
//        var queue6 = new MyPriorityQueue<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
//        Console.WriteLine($"Оставить только [3,6,9]: {queue6.RetainAll(new int[] { 3, 6, 9 })}");
//        Console.WriteLine($"Размер после RetainAll: {queue6.Size()}");
//        Console.Write("Оставшиеся элементы: ");
//        foreach (var item in queue6)
//            Console.Write($"{item} ");
//        Console.WriteLine();
//
//        // 16. Тест увеличения емкости
//        Console.WriteLine("\n16. Тест увеличения емкости (добавление 100 элементов):");
//        var queue7 = new MyPriorityQueue<int>();
//        for (int i = 0; i < 100; i++)
//        {
//            queue7.Add(i);
//        }
//        Console.WriteLine($"Размер: {queue7.Size()}");
//
//        // 17. Тест с пользовательским типом
//        Console.WriteLine("\n17. Тест с пользовательским типом:");
//        var personQueue = new MyPriorityQueue<Person>();
//        personQueue.Add(new Person("Абв", 30));
//        personQueue.Add(new Person("Где", 25));
//        personQueue.Add(new Person("Ёжз", 35));
//
//        Console.WriteLine($"Первый по возрасту: {personQueue.Peek()}");
//
//        // 18. Тест перечисления
//        Console.WriteLine("\n18. Тест перечисления (foreach):");
//        Console.Write("Все элементы queue1: ");
//        foreach (var item in queue1)
//            Console.Write($"{item} ");
//        Console.WriteLine();
//
//        Console.WriteLine("\nВсе тесты завершены");
//    }
//
//    class Person : IComparable<Person>
//    {
//        public string Name { get; }
//        public int Age { get; }
//
//        public Person(string name, int age)
//        {
//            Name = name;
//            Age = age;
//        }
//
//        public int CompareTo(Person other)
//        {
//            if (other == null) return 1;
//            return Age.CompareTo(other.Age);
//        }
//
//        public override string ToString() => $"{Name} ({Age})";
//    }
//}
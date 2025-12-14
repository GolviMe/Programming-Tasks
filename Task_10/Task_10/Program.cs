using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_10
{
    public class MyVector<T>
    {
        private T[] elementData;         // внутренний массив для хранения элементов
        private int elementCount;        // текущее количество элементов (size)
        private int capacityIncrement;   // на сколько увеличивать ёмкость при нехватке
        private const int DEFAULT_CAPACITY = 10;

        // 1. Конструктор для создания пустого вектора с начальной ёмкостью initialCapacity и значением приращения ёмкости capacityIncrement.
        public MyVector(int initialCapacity, int capacityIncrement)
        {
            if (initialCapacity < 0)
                throw new ArgumentException("Начальная размерность не может быть отрицательной");
            if (capacityIncrement < 0)
                throw new ArgumentException("Прирост количества элементов не может быть отрицательным");

            elementData = new T[initialCapacity];
            elementCount = 0;
            this.capacityIncrement = capacityIncrement;
        }

        // 2. Конструктор для создания пустого вектора с начальной ёмкостью initialCapacity и значением приращения ёмкости по умолчанию(0).
        public MyVector(int initialCapacity) : this(initialCapacity, 0)
        {
        }

        // 3. Конструктор для создания пустого вектора с начальной ёмкостью по умолчанию(10) и значением приращения ёмкости по умолчанию(0). 
        public MyVector() : this(DEFAULT_CAPACITY, 0)
        {
        }

        // 4. Конструктор для создания вектора и заполнения его элементами из передаваемого массива a.
        public MyVector(T[] a)
            : this(a != null ? a.Length : DEFAULT_CAPACITY, 0)
        {
            if (a != null)
            {
                Array.Copy(a, elementData, a.Length);
                elementCount = a.Length;
            }
        }

        // Метод увеличения ёмкости
        private void EnsureCapacity(int minCapacity)
        {
            if (minCapacity > elementData.Length)
            {
                int newCapacity;
                if (capacityIncrement > 0)
                {
                    newCapacity = elementData.Length + capacityIncrement;
                }
                else
                {
                    newCapacity = elementData.Length * 2;  // удвоение
                    if (newCapacity < minCapacity)
                        newCapacity = minCapacity;
                }

                if (newCapacity < minCapacity)
                    newCapacity = minCapacity;

                T[] newArray = new T[newCapacity];
                Array.Copy(elementData, newArray, elementCount);
                elementData = newArray;
            }
        }

        // 5. Добавление в конец
        public void Add(T e)
        {
            EnsureCapacity(elementCount + 1);
            elementData[elementCount] = e;
            elementCount++;
        }

        // 6. Добавление всех элементов из массива
        public void AddAll(T[] a)
        {
            if (a == null) return;
            EnsureCapacity(elementCount + a.Length);
            Array.Copy(a, 0, elementData, elementCount, a.Length);
            elementCount += a.Length;
        }

        // 7. Удаление всех элементов из массива
        public void Clear()
        {
            for (int i = 0; i < elementCount; i++)
                elementData[i] = default(T);
            elementCount = 0;
        }

        // 8. Проверка нахождения элемента
        public bool Contains(object o)
        {
            return IndexOf(o) >= 0;
        }

        // 9. Проверка нахождения элементов
        public bool ContainsAll(T[] a)
        {
            if (a == null) return true;
            for (int i = 0; i < a.Length; i++)
                if (!Contains(a[i])) return false;
            return true;
        }

        // 10. Проверка на пустоту
        public bool IsEmpty()
        {
            return elementCount == 0;
        }

        // 11. Удаление указанного объекта
        public bool Remove(object o)
        {
            int index = IndexOf(o);
            if (index >= 0)
            {
                Remove(index);
                return true;
            }
            return false;
        }

        // 12. Удаление всех указанных объектов
        public bool RemoveAll(T[] a)
        {
            if (a == null) return false;
            bool changed = false;
            for (int i = 0; i < a.Length; i++)
            {
                while (Remove(a[i]))
                    changed = true;
            }
            return changed;
        }

        // 13. Оставление в векторе только указанных объектов.
        public bool RetainAll(T[] a)
        {
            bool changed = false;

            if (a == null)
            {
                changed = elementCount > 0;
                Clear();
                return changed;
            }

            
            for (int i = elementCount - 1; i >= 0; i--)
            {
                bool found = false;
                for (int j = 0; j < a.Length; j++)
                {
                    if (object.Equals(elementData[i], a[j]))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Remove(i);
                    changed = true;
                }
            }
            return changed;
        }

        // 14. Возвращение размера вектора
        public int Size()
        {
            return elementCount;
        }

        // 15. Возвращение массива объектов, содержащего все элементы вектора
        public T[] ToArray()
        {
            T[] result = new T[elementCount];
            Array.Copy(elementData, result, elementCount);
            return result;
        }

        // 16. Метод для возвращения массива объектов, содержащего все элементы вектора.Если аргумент a равен null, то создаётся новый массив, в который копируются элементы.
        public T[] ToArray(T[] a)
        {
            if (a == null || a.Length < elementCount)
                a = new T[elementCount];

            Array.Copy(elementData, a, elementCount);
            if (a.Length > elementCount)
                a[elementCount] = default(T);
            return a;
        }

        // 17. Метод для добавления элемента в указанную позицию.
        public void Add(int index, T e)
        {
            if (index < 0 || index > elementCount)
                throw new IndexOutOfRangeException();

            EnsureCapacity(elementCount + 1);
            Array.Copy(elementData, index, elementData, index + 1, elementCount - index);
            elementData[index] = e;
            elementCount++;
        }

        // 18. Метод для добавления элемента в указанную позицию.
        public void AddAll(int index, T[] a)
        {
            if (index < 0 || index > elementCount)
                throw new IndexOutOfRangeException();
            if (a == null) return;

            EnsureCapacity(elementCount + a.Length);
            Array.Copy(elementData, index, elementData, index + a.Length, elementCount - index);
            Array.Copy(a, 0, elementData, index, a.Length);
            elementCount += a.Length;
        }

        // 19. Метод для возвращения элемента в указанной позиции.
        public T Get(int index)
        {
            if (index < 0 || index >= elementCount)
                throw new IndexOutOfRangeException();
            return elementData[index];
        }

        // 20. Метод для возвращения индекса указанного объекта
        public int IndexOf(object o)
        {
            for (int i = 0; i < elementCount; i++)
                if (object.Equals(elementData[i], o))
                    return i;
            return -1;
        }

        // 21. Метод для нахождения последнего вхождения указанного объекта
        public int LastIndexOf(object o)
        {
            for (int i = elementCount - 1; i >= 0; i--)
                if (object.Equals(elementData[i], o))
                    return i;
            return -1;
        }

        // 22. Метод  для удаления и возвращения элемента в указанной позиции.
        public T Remove(int index)
        {
            if (index < 0 || index >= elementCount)
                throw new IndexOutOfRangeException();

            T old = elementData[index];
            int moved = elementCount - index - 1;
            if (moved > 0)
                Array.Copy(elementData, index + 1, elementData, index, moved);
            elementCount--;
            elementData[elementCount] = default(T);
            return old;
        }

        // 23. Замена элемента в указанной позиции новым элементом.
        public T Set(int index, T e)
        {
            if (index < 0 || index >= elementCount)
                throw new IndexOutOfRangeException();

            T old = elementData[index];
            elementData[index] = e;
            return old;
        }

        // 24. Метод для возвращения части вектора, т.е.элементов в диапазоне[fromIndex; toIndex).
        public MyVector<T> SubList(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || toIndex > elementCount || fromIndex > toIndex)
                throw new ArgumentException("Недопустимые индексы");

            MyVector<T> sub = new MyVector<T>(toIndex - fromIndex, capacityIncrement);
            for (int i = fromIndex; i < toIndex; i++)
                sub.Add(elementData[i]);
            return sub;
        }

        // 25. Метод для обращения к первому элементу вектора.
        public T FirstElement()
        {
            if (elementCount == 0)
                throw new InvalidOperationException("Вектор пуст");
            return elementData[0];
        }

        // 26. Метод для обращения к последнему элементу вектора.
        public T LastElement()
        {
            if (elementCount == 0)
                throw new InvalidOperationException("Вектор пуст");
            return elementData[elementCount - 1];
        }

        // 27. Метод для удаления элемента в заданной позиции.
        public void RemoveElementAt(int pos)
        {
            Remove(pos);
        }

        // 28. Метод для удаления нескольких подряд идущих элементов.
        public void RemoveRange(int begin, int end)
        {
            if (begin < 0 || end > elementCount || begin > end)
                throw new ArgumentException("Недопустимый диапазон");

            int count = end - begin;
            if (count > 0)
            {
                Array.Copy(elementData, end, elementData, begin, elementCount - end);
                elementCount -= count;
                for (int i = elementCount; i < elementCount + count; i++)
                    elementData[i] = default(T);
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            MyVector<string> cars = new MyVector<string>(5, 0); // начальная ёмкость 5, increment = 0 (удвоение)

            // Проверка на пустоту
            Console.WriteLine("\nВектор пуст? " + cars.IsEmpty() + '\n');

            // Добавление автомобилей
            cars.Add("Toyota");
            cars.Add("Honda");
            cars.Add("BMW");
            cars.Add("Mercedes");
            cars.Add("Audi");
            cars.Add("Ford");
            cars.Add("Volkswagen");
            cars.Add("Tesla");
            cars.Add("Honda"); // дубликат для теста

            Console.WriteLine("После добавления автомобилей:");
            PrintVector(cars);
            Console.WriteLine("Размер вектора: " + cars.Size());

            // Удаление некоторых автомобилей
            cars.Remove("Audi");
            cars.Remove("Volkswagen");
            Console.WriteLine("\nПосле удаления 'Audi' и 'Volkswagen':");
            PrintVector(cars);

            // Добавление в конец нового автомобиля
            cars.Add("Porsche");
            Console.WriteLine("\nПосле добавления 'Porsche' в конец:");
            PrintVector(cars);

            // Поиск элемента
            Console.WriteLine("\nИндекс 'Honda': " + cars.IndexOf("Honda"));
            Console.WriteLine("Содержит ли 'Tesla'? " + cars.Contains("Tesla"));

            // Получение и замена по индексу
            Console.WriteLine("\nЭлемент на позиции 2: " + cars.Get(2));
            string old = cars.Set(2, "Lamborghini");
            Console.WriteLine("Старое значение на позиции 2 было: " + old);
            Console.WriteLine("После замены:");
            PrintVector(cars);

            // Оставить только указанные автомобили
            string[] keep = { "Honda", "Toyota", "Porsche", "Lamborghini", "Tesla" };
            cars.RetainAll(keep);
            Console.WriteLine("\nПосле RetainAll (оставить только Honda, Toyota, Porsche, Lamborghini, Tesla):");
            PrintVector(cars);
            Console.WriteLine("Размер: " + cars.Size());

            Console.WriteLine("\nПервый элемент: " + cars.FirstElement());
            Console.WriteLine("Последний элемент: " + cars.LastElement());

            // Проверка на пустоту
            Console.WriteLine("\nВектор пуст? " + cars.IsEmpty());


            Console.ReadKey();
        }

        static void PrintVector(MyVector<string> vector)
        {
            if (vector.IsEmpty())
            {
                Console.WriteLine("Вектор пуст");
                return;
            }

            for (int i = 0; i < vector.Size(); i++)
            {
                Console.Write(vector.Get(i));
                if (i < vector.Size() - 1) Console.Write(", ");
            }
            Console.WriteLine();
            Console.WriteLine("Size: " + vector.Size());
        }
    }
}

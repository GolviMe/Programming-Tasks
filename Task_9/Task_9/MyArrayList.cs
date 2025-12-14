using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_9
{
    public class MyArrayList<T>
    {
        private T[] elementData;
        private int size;
        private const int DEFAULT_CAPACITY = 10;

        // 1. Конструктор для создания пустого динамического массива
        public MyArrayList()
        {
            elementData = new T[DEFAULT_CAPACITY];
            size = 0;
        }

        // 2. Конструктор для создания динамического массива и заполнения его элементами из передаваемого массива a.
        public MyArrayList(T[] a)
        {
            if (a == null)
            {
                elementData = new T[DEFAULT_CAPACITY];
                size = 0;
            }
            else
            {
                elementData = new T[a.Length];
                Array.Copy(a, elementData, a.Length);
                size = a.Length;
            }
        }

        // 3. Конструктор для создания пустого динамического массива с внутренним массивом, размер которого буде равен значению параметра capacity.
        public MyArrayList(int capacity)
        {
            if (capacity < 0) throw new ArgumentException("Ёмкость не может быть отрицательной");

            elementData = new T[capacity];
            size = 0;
        }

        // Метод увеличения ёмкости
        private void EnsureCapacity(int minCapacity)
        {
            if (minCapacity > elementData.Length)
            {
                int newCapacity = elementData.Length + (elementData.Length >> 1) + 1; // увеличениче в полтора раза + 1
                if (newCapacity < minCapacity) newCapacity = minCapacity;

                T[] newArray = new T[newCapacity];
                Array.Copy(elementData, newArray, size);
                elementData = newArray;
            }
        }

        // 4. Добавление элемента в конец
        public void Add(T e)
        {
            EnsureCapacity(size + 1);
            elementData[size] = e;
            size++;
        }

        // 5. Добавление всех элементов из массива в конец
        public void AddAll(T[] a)
        {
            if (a == null) return;

            EnsureCapacity(size + a.Length);
            Array.Copy(a, 0, elementData, size, a.Length);
            size += a.Length;
        }

        // 6. Удаление всех элементов
        public void Clear()
        {
            for (int i = 0; i < size; i++)
            {
                elementData[i] = default(T);
            }
            size = 0;
        }

        // 7. Проверка наличия объекта
        public bool Contains(object o)
        {
            return IndexOf(o) >= 0;
        }

        // 8. Проверка наличия всех объектов из массива
        public bool ContainsAll(T[] a)
        {
            if (a == null) return true;

            for (int i = 0; i < a.Length; i++)
            {
                if (!Contains(a[i])) return false;
            }
            return true;
        }

        // 9. Пустой ли список
        public bool IsEmpty()
        {
            return size == 0;
        }

        // 10. Удаление первого вхождения объекта
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

        // 11. Удаление всех указанных объектов
        public bool RemoveAll(T[] a)
        {
            if (a == null) return false;

            bool changed = false;
            for (int i = 0; i < a.Length; i++)
            {
                while (Remove(a[i]))
                {
                    changed = true;
                }
            }
            return changed;
        }

        // 12. Оставить только указанные объекты
        public bool RetainAll(T[] a)
        {
            bool changed = false;
            if (a == null)
            {
                changed = size > 0;
                Clear();
                return changed;
            }

            for (int i = size - 1; i >= 0; i--)
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

        // 13. Размер списка
        public int Size()
        {
            return size;
        }

        // 14. Возврат нового массива со всеми элементами
        public T[] ToArray()
        {
            T[] result = new T[size];
            Array.Copy(elementData, result, size);
            return result;
        }

        // 15. Возврат массива в переданный (или новый)
        public T[] ToArray(T[] a)
        {
            if (a == null || a.Length < size)
            {
                a = new T[size];
            }
            Array.Copy(elementData, a, size);
            if (a.Length > size) a[size] = default(T);
            return a;
        }

        // 16. Вставка по индексу
        public void Add(int index, T e)
        {
            if (index < 0 || index > size) throw new IndexOutOfRangeException();

            EnsureCapacity(size + 1);
            Array.Copy(elementData, index, elementData, index + 1, size - index);
            elementData[index] = e;
            size++;
        }

        // 17. Вставка массива по индексу
        public void AddAll(int index, T[] a)
        {
            if (index < 0 || index > size) throw new IndexOutOfRangeException();
            if (a == null) return;

            EnsureCapacity(size + a.Length);
            Array.Copy(elementData, index, elementData, index + a.Length, size - index);
            Array.Copy(a, 0, elementData, index, a.Length);
            size += a.Length;
        }

        // 18. Получение элемента по индексу
        public T Get(int index)
        {
            if (index < 0 || index >= size) throw new IndexOutOfRangeException();

            return elementData[index];
        }

        // 19. Первый индекс объекта
        public int IndexOf(object o)
        {
            for (int i = 0; i < size; i++)
            {
                if (object.Equals(elementData[i], o)) return i;
            }
            return -1;
        }

        // 20. Последний индекс объекта
        public int LastIndexOf(object o)
        {
            for (int i = size - 1; i >= 0; i--)
            {
                if (object.Equals(elementData[i], o)) return i;
            }
            return -1;
        }

        // 21. Удаление по индексу с возвратом элемента
        public T Remove(int index)
        {
            if (index < 0 || index >= size) throw new IndexOutOfRangeException();

            T oldValue = elementData[index];
            int numMoved = size - index - 1;
            if (numMoved > 0)
            {
                Array.Copy(elementData, index + 1, elementData, index, numMoved);
            }
            size--;
            elementData[size] = default(T);
            return oldValue;
        }

        // 22. Замена элемента по индексу
        public T Set(int index, T e)
        {
            if (index < 0 || index >= size) throw new IndexOutOfRangeException();

            T oldValue = elementData[index];
            elementData[index] = e;
            return oldValue;
        }

        // 23. Подсписок (новый MyArrayList с элементами [fromIndex, toIndex))
        public MyArrayList<T> SubList(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || toIndex > size || fromIndex > toIndex) throw new ArgumentException("Недопустимые индексы");

            MyArrayList<T> sub = new MyArrayList<T>(toIndex - fromIndex);
            for (int i = fromIndex; i < toIndex; i++)
            {
                sub.Add(elementData[i]);
            }
            return sub;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Task_12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyStack<int> stack = new MyStack<int>();

            List<int> list = new List<int>();

            Console.WriteLine($"Проверка на путоту: {stack.Empty()}");

            stack.Push(1);
            stack.Push(3);
            stack.Push(8);

            Console.WriteLine($"Проверка на путоту: {stack.Empty()}");

            Console.WriteLine($"Верхушка: {stack.Peek()}");
            Console.WriteLine($"Поиск (глубина) элемента 3: {stack.Search(3)}");

            Console.WriteLine($"Извлечен: {stack.Pop()}");
            Console.WriteLine($"Извлечен: {stack.Pop()}");
            Console.WriteLine($"Проверка на путоту: {stack.Empty()}");
            Console.WriteLine($"Извлечен: {stack.Pop()}");
            Console.WriteLine($"Проверка на путоту: {stack.Empty()}");

            Console.ReadKey();
        }
    }

    public class MyVector<T>
    {
        protected List<T> miniVector;

        public MyVector()
        {
            miniVector = new List<T>();
        }

        public void Add(T item)
        {
            miniVector.Add(item);
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= miniVector.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            miniVector.RemoveAt(index);
        }

        public T this[int index]
        {
            get //возврат
            {
                if (index < 0 || index >= miniVector.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
                return miniVector[index];
            }
            set //установка
            {
                if (index < 0 || index >= miniVector.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                miniVector[index] = value;
            }
        }

        public int Count => miniVector.Count;

        public bool Contains(T item)
        {
            return miniVector.Contains(item);
        }

        public int IndexOf(T item)
        {
            return miniVector.IndexOf(item);
        }
    }

    public class MyStack<T> : MyVector<T>
    {
        public void Push(T item) //пуш
        {
            miniVector.Add(item);
        }

        
        public T Pop() //извлечение верхушки
        {
            if (Empty())
            {
                throw new InvalidOperationException("Стек пуст");
            }

            T item = miniVector[miniVector.Count - 1];
            miniVector.RemoveAt(miniVector.Count - 1);
            return item;
        }

        
        public T Peek() //чек верхнего элемента
        {
            if (Empty())
                throw new InvalidOperationException("Стек пуст");

            return miniVector[miniVector.Count - 1];
        }

        
        public bool Empty() //чек на пустоту
        {
            if(miniVector.Count == 0)
            {
                return true;
            }
            return false;
        }

        
        public int Search(T item) //нахождение глубины
        {
            for (int i = miniVector.Count - 1; i >= 0; i--)
            {
                if (miniVector[i].Equals(item))
                {
                    return miniVector.Count - i;
                }
            }
            return -1;
        }
    }
}
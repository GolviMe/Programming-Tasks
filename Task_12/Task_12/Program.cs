using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Task_12
{
    public class MyStack<T> : MyVector<T>
    {
        public MyStack()
            : base()
        {
        }

        public MyStack(int initialCapacity)
            : base(initialCapacity)
        {
        }

        public void Push(T item)
        {
            Add(item);
        }

        public T Pop()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Стек пуст.");
            }

            T top = LastElement();
            Remove(Size() - 1);
            return top;
        }

        public T Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Стек пуст.");
            }

            return LastElement();
        }

        public bool Empty()
        {
            return IsEmpty();
        }

        public int Search(T item)
        {
            for (int i = Size() - 1; i >= 0; i--)
            {
                if (object.Equals(Get(i), item))
                {
                    return Size() - i;
                }
            }
            return -1;
        }
    }


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
}
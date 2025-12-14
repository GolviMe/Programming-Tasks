using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_13
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

        // Помещение элемента на верхушку стека
        public void Push(T item)
        {
            Add(item);
        }

        // Извлечение верхнего элемента из верхушки стека
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

        // Возвращение верхнего элемента из верхушки стека
        public T Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("Стек пуст.");
            }

            return LastElement();
        }

        // Проверка на пустоту
        public bool Empty()
        {
            return IsEmpty();
        }

        // Поиск глубины объекта
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
}

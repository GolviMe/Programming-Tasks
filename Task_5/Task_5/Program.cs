using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_5
{
    internal class Program
    {
        static void Main(string[] args)
        {

            int[] numbers = { 19, 36, 17, 3, 25, 1, 2, 7, 100 };
            MaxHeap heap = new MaxHeap(numbers);

            heap.Print();
            Console.WriteLine("Максимум: " + heap.FindMaximum());

            Console.WriteLine("Извлечение максимума: " + heap.ExtractMaximum());
            Console.WriteLine("Новый максимум: " + heap.FindMaximum());
            heap.Print();

            heap.Insert(50);
            Console.WriteLine("После вставки 50: ");
            heap.Print();

            // Тест слияния
            Console.WriteLine("\nСлияние двух куч:");
            MaxHeap heap1 = new MaxHeap(new int[] { 10, 40, 30 });
            MaxHeap heap2 = new MaxHeap(new int[] { 35, 20, 45 });

            Console.Write("Куча 1: "); heap1.Print();
            Console.Write("Куча 2: "); heap2.Print();

            heap1.Merge(heap2);
            Console.Write("После слияния: "); heap1.Print();
            Console.WriteLine("Максимум: " + heap1.FindMaximum());
            Console.ReadKey();
        }
    }


    internal class MaxHeap
    {
        private List<int> array;

        // 1. Конструктор
        public MaxHeap(int[] elements)
        {
            array = new List<int>(elements);
            BuildHeap();
        }

        private void BuildHeap()
        {
            for (int i = array.Count / 2 - 1; i >= 0; i--)
            {
                SiftDown(i);
            }
        }

        // Опускание элемента вниз
        private void SiftDown(int index)
        {
            int size = array.Count;

            while (true)
            {
                int left = 2 * index + 1;
                int right = 2 * index + 2;
                int largest = index;

                if (left < size && array[left] > array[largest])
                    largest = left;
                if (right < size && array[right] > array[largest])
                    largest = right;

                if (largest == index)
                    break;

                int temp = array[index];
                array[index] = array[largest];
                array[largest] = temp;
                index = largest;
            }
        }

        public MaxHeap()
        {
            array = new List<int>();
        }

        public int Count // кол-во эл-тов
        {
            get { return array.Count; }
        }

        // 2. Поиск максимума
        public int FindMaximum()
        {
            if (array.Count == 0)
            {
                throw new InvalidOperationException("Куча пустая");
            }
            return array[0];
        }

        // 3. Удаление максимального элемента
        public int ExtractMaximum()
        {
            if (array.Count == 0)
            {
                throw new InvalidOperationException("Куча пустая");
            }

            int max = array[0];
            array[0] = array[array.Count - 1];
            array.RemoveAt(array.Count - 1);

            if (array.Count > 0)
            {
                SiftDown(0);
            }

            return max;
        }

        // 4. Увеличение ключа
        private void SiftUp(int index)
        {
            while (index > 0)
            {
                int parent = (index - 1) / 2;
                if (array[index] > array[parent])
                {
                    int temp = array[index];
                    array[index] = array[parent];
                    array[parent] = temp;
                    index = parent;
                }
                else
                {
                    break;
                }
            }
        }


        // 5. Добавление нового элемента
        public void Insert(int value)
        {
            array.Add(value);
            SiftUp(array.Count - 1);
        }

        // 6. Слияние с другой кучей
        public void Merge(MaxHeap SecondHeap)
        {
            for (int i = 0; i < SecondHeap.array.Count; i++)
            {
                Insert(SecondHeap.array[i]);
            }
        }

       

        // Вывод кучи
        public void Print()
        {
            Console.Write("Куча: ");
            for (int i = 0; i < array.Count; i++)
                Console.Write(array[i] + " ");
            Console.WriteLine();
        }
    }

    
}

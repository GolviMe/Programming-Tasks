using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_19
{
    using System;
    using System.Collections.Generic;

    public class MyTreeSet<T> : IEnumerable<T>
    {
        // Фиктивный объект для значений в карте
        private static readonly object PRESENT = new object();

        // Внутренняя карта для хранения элементов (ключи — элементы множества)
        private MyTreeMap<T, object> map;

        // 1) Конструктор по умолчанию (естественный порядок)
        public MyTreeSet()
        {
            map = new MyTreeMap<T, object>();
        }

        // 2) Конструктор с готовой картой
        public MyTreeSet(MyTreeMap<T, object> m)
        {
            map = m ?? throw new ArgumentNullException(nameof(m));
        }

        // 3) Конструктор с компаратором
        public MyTreeSet(IComparer<T> comparator)
        {
            map = new MyTreeMap<T, object>(comparator);
        }

        // 4) Конструктор из массива
        public MyTreeSet(T[] a) : this()
        {
            if (a != null)
                AddAll(a);
        }

        // 5) Конструктор из другого сортированного множества (любого, реализующего IEnumerable)
        public MyTreeSet(IEnumerable<T> s) : this()
        {
            if (s != null)
            {
                foreach (var item in s)
                    Add(item);
            }
        }

        // 6) Добавление элемента
        public bool Add(T e)
        {
            if (e == null)
                throw new ArgumentNullException(nameof(e));
            // Если ключ уже был, Put вернёт старое значение (не null) – значит элемент уже существовал
            return map.Put(e, PRESENT) == null;
        }

        // 7) Добавление всех элементов из массива
        public void AddAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            foreach (var item in a)
                Add(item);
        }

        // 8) Очистка множества
        public void Clear()
        {
            map.Clear();
        }

        // 9) Проверка наличия элемента
        public bool Contains(object o)
        {
            if (o == null)
                return false;
            return map.ContainsKey(o);
        }

        // 10) Проверка наличия всех элементов из массива
        public bool ContainsAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            foreach (var item in a)
            {
                if (!Contains(item))
                    return false;
            }
            return true;
        }

        // 11) Проверка на пустоту
        public bool IsEmpty()
        {
            return map.IsEmpty();
        }

        // 12) Удаление одного элемента
        public bool Remove(object o)
        {
            if (o == null)
                return false;
            return map.Remove(o) != null;
        }

        // 13) Удаление всех указанных элементов
        public bool RemoveAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            bool modified = false;
            foreach (var item in a)
            {
                if (Remove(item))
                    modified = true;
            }
            return modified;
        }

        // 14) Оставить только указанные элементы
        public bool RetainAll(T[] a)
        {
            if (a == null)
                throw new ArgumentNullException(nameof(a));
            // Создаём множество из a для быстрой проверки
            var set = new HashSet<T>(a);
            var toRemove = new List<T>();
            foreach (var key in map.KeySet())
            {
                if (!set.Contains(key))
                    toRemove.Add(key);
            }
            foreach (var key in toRemove)
                map.Remove(key);
            return toRemove.Count > 0;
        }

        // 15) Размер множества
        public int Size()
        {
            return map.Size();
        }

        // 16) Преобразование в массив (без типа)
        public object[] ToArray()
        {
            var keys = map.KeySet();
            object[] result = new object[keys.Count];
            for (int i = 0; i < keys.Count; i++)
                result[i] = keys[i];
            return result;
        }

        // 17) Преобразование в массив указанного типа
        public T[] ToArray(T[] a)
        {
            var keys = map.KeySet();
            if (a == null)
            {
                // Создаём новый массив нужного размера
                T[] result = new T[keys.Count];
                for (int i = 0; i < keys.Count; i++)
                    result[i] = keys[i];
                return result;
            }
            else
            {
                if (a.Length < keys.Count)
                {
                    // Если переданный массив слишком мал, создаём новый
                    T[] result = new T[keys.Count];
                    for (int i = 0; i < keys.Count; i++)
                        result[i] = keys[i];
                    return result;
                }
                else
                {
                    for (int i = 0; i < keys.Count; i++)
                        a[i] = keys[i];
                    if (a.Length > keys.Count)
                        a[keys.Count] = default(T);
                    return a;
                }
            }
        }

        // 18) Первый (наименьший) элемент
        public T First()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Множество пусто");
            return map.FirstKey();
        }

        // 19) Последний (наибольший) элемент
        public T Last()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Множество пусто");
            return map.LastKey();
        }

        // 20) subSet – элементы от fromElement (включительно) до toElement (исключительно)
        public MyTreeSet<T> SubSet(T fromElement, T toElement)
        {
            if (fromElement == null || toElement == null)
                throw new ArgumentNullException();
            var subMap = map.SubMap(fromElement, toElement);
            return new MyTreeSet<T>(subMap);
        }

        // 21) headSet – элементы, меньшие toElement (исключительно)
        public MyTreeSet<T> HeadSet(T toElement)
        {
            if (toElement == null)
                throw new ArgumentNullException();
            var headMap = map.HeadMap(toElement);
            return new MyTreeSet<T>(headMap);
        }

        // 22) tailSet – элементы, большие или равные fromElement
        public MyTreeSet<T> TailSet(T fromElement)
        {
            if (fromElement == null)
                throw new ArgumentNullException();
            var tailMap = map.TailMap(fromElement);
            return new MyTreeSet<T>(tailMap);
        }

        // 23) ceiling – наименьший элемент e >= obj
        public T Ceiling(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            return map.CeilingKey(obj);
        }

        // 24) floor – наибольший элемент e <= obj
        public T Floor(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            return map.FloorKey(obj);
        }

        // 25) higher – наименьший элемент e > obj
        public T Higher(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            return map.HigherKey(obj);
        }

        // 26) lower – наибольший элемент e < obj
        public T Lower(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            return map.LowerKey(obj);
        }

        // 27) headSet с указанием, включать ли upperBound
        public MyTreeSet<T> HeadSet(T upperBound, bool inclusive)
        {
            if (upperBound == null)
                throw new ArgumentNullException(nameof(upperBound));
            if (inclusive)
            {
                var result = new MyTreeSet<T>(map.Comparator);
                foreach (var key in map.KeySet())
                {
                    if (map.Comparator.Compare(key, upperBound) <= 0)
                        result.Add(key);
                }
                return result;
            }
            else
            {
                return HeadSet(upperBound);
            }
        }

        // 28) subSet с указанием включения границ
        public MyTreeSet<T> SubSet(T lowerBound, bool lowInclusive, T upperBound, bool highInclusive)
        {
            if (lowerBound == null || upperBound == null)
                throw new ArgumentNullException();
            var result = new MyTreeSet<T>(map.Comparator);
            foreach (var key in map.KeySet())
            {
                int cmpLow = map.Comparator.Compare(key, lowerBound);
                int cmpHigh = map.Comparator.Compare(key, upperBound);
                bool lowOk = lowInclusive ? cmpLow >= 0 : cmpLow > 0;
                bool highOk = highInclusive ? cmpHigh <= 0 : cmpHigh < 0;
                if (lowOk && highOk)
                    result.Add(key);
            }
            return result;
        }

        // 29) tailSet с указанием включения нижней границы
        public MyTreeSet<T> TailSet(T fromElement, bool inclusive)
        {
            if (fromElement == null)
                throw new ArgumentNullException(nameof(fromElement));
            if (inclusive)
            {
                return TailSet(fromElement);
            }
            else
            {
                var result = new MyTreeSet<T>(map.Comparator);
                foreach (var key in map.KeySet())
                {
                    if (map.Comparator.Compare(key, fromElement) > 0)
                        result.Add(key);
                }
                return result;
            }
        }

        // 30) pollLast – удалить и вернуть последний (наибольший) элемент
        public T PollLast()
        {
            if (IsEmpty())
                return default(T);
            var entry = map.PollLastEntry();
            return entry.HasValue ? entry.Value.Key : default(T);
        }

        // 31) pollFirst – удалить и вернуть первый (наименьший) элемент
        public T PollFirst()
        {
            if (IsEmpty())
                return default(T);
            var entry = map.PollFirstEntry();
            return entry.HasValue ? entry.Value.Key : default(T);
        }

        // 32) descendingIterator – возвращает итератор в обратном порядке
        public IEnumerator<T> DescendingIterator()
        {
            var list = map.KeySet();
            for (int i = list.Count - 1; i >= 0; i--)
                yield return list[i];
        }

        // 33) descendingSet – возвращает множество в обратном порядке (копия с обратным компаратором)
        public MyTreeSet<T> DescendingSet()
        {
            // Создаём компаратор, обратный текущему
            IComparer<T> reverseComp;
            if (map.Comparator == Comparer<T>.Default)
            {
                // Для естественного порядка создаём обратный через Comparer<T>.Create
                reverseComp = Comparer<T>.Create((x, y) => map.Comparator.Compare(y, x));
            }
            else
            {
                reverseComp = Comparer<T>.Create((x, y) => map.Comparator.Compare(y, x));
            }
            var result = new MyTreeSet<T>(reverseComp);
            // Добавляем все элементы в обратном порядке (они автоматически упорядочатся по новому компаратору)
            foreach (var key in map.KeySet())
                result.Add(key);
            return result;
        }
        

        public IEnumerator<T> GetEnumerator()
        {
            return map.KeySet().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    internal class Program
    {
        static void Main()
        {
            MyTreeSet<string> set = new MyTreeSet<string>();

            set.Add("яблоко");
            set.Add("банан");
            set.Add("вишня");
            set.Add("арбуз");
            set.Add("дыня");

            Console.WriteLine($"Размер: {set.Size()}");

            Console.WriteLine("Все элементы: " + string.Join(", ", set));

            Console.WriteLine($"Первый: {set.First()}");
            Console.WriteLine($"Последний: {set.Last()}");

            Console.WriteLine($"Содержит 'банан': {set.Contains("банан")}");
            Console.WriteLine($"Содержит 'апельсин': {set.Contains("апельсин")}");

            Console.WriteLine($"Меньше 'вишня': {set.Lower("вишня")}");
            Console.WriteLine($"Больше 'вишня': {set.Higher("вишня")}");
            Console.WriteLine($"Не больше 'вишня': {set.Floor("вишня")}");
            Console.WriteLine($"Не меньше 'вишня': {set.Ceiling("вишня")}");

            set.Remove("банан");
            Console.WriteLine($"После удаления 'банан': {string.Join(", ", set)}");
        }

    }
}
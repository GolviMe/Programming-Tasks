using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_10;

namespace Task_11
{
    internal class Program
    {
        // Метод проверки на корректность IP
        static bool IsValidIPv4(string s)
        {
            string[] parts = s.Split('.');
            if (parts.Length != 4) return false;

            foreach (string part in parts)
            {
                // Проверка на пустую часть или символы-не-цифры
                if (part.Length == 0 || part.Length > 3) return false;
                if (part[0] == '0' && part.Length > 1) return false; // нули перед числами запрещены

                int num;
                if (!int.TryParse(part, out num)) return false;
                if (num < 0 || num > 255) return false;
            }
            return true;
        }

        // Метод поиска всех подстрок-IP в одной строке
        static void ExtractIPsFromLine(string line, MyVector<string> allIPs, MyVector<string> uniqueIPs)
        {
            if (string.IsNullOrEmpty(line)) return;

            string current = "";
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '.' || char.IsDigit(c))
                {
                    current += c;
                }
                else
                {
                    // Если накопилась потенциальная IP-подстрока — проверка
                    if (current.Length > 0)
                    {
                        if (IsValidIPv4(current))
                        {
                            allIPs.Add(current);
                            if (!uniqueIPs.Contains(current))
                            {
                                uniqueIPs.Add(current);
                            }
                        }
                        current = "";
                    }
                }
            }

            // Проверка в конце строки
            if (current.Length > 0)
            {
                if (IsValidIPv4(current))
                {
                    allIPs.Add(current);
                    if (!uniqueIPs.Contains(current))
                    {
                        uniqueIPs.Add(current);
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            MyVector<string> uniqueIPs = new MyVector<string>(); // уникальные IP
            MyVector<string> allIPs = new MyVector<string>(); // все найденные IP (с повторениями)

            try
            {
                string[] lines = File.ReadAllLines("input.txt");

                foreach (string line in lines)
                {
                    ExtractIPsFromLine(line.Trim(), allIPs, uniqueIPs);
                }

                // Запись в output.txt
                using (StreamWriter writer = new StreamWriter("output.txt"))
                {
                    writer.WriteLine("Уникальные IP-адреса:");
                    for (int i = 0; i < uniqueIPs.Size(); i++)
                    {
                        writer.WriteLine(uniqueIPs.Get(i));
                    }

                    writer.WriteLine("\nВсе найденные IP-адреса:");
                    for (int i = 0; i < allIPs.Size(); i++)
                    {
                        writer.WriteLine(allIPs.Get(i));
                    }

                    writer.WriteLine("\nКоличество уникальных IP: " + uniqueIPs.Size());
                    writer.WriteLine("Общее количество найденных IP: " + allIPs.Size());
                }

                Console.WriteLine("Обработка завершена. Результат записан в output.txt");
                Console.WriteLine("Найдено уникальных IP: " + uniqueIPs.Size());
                Console.WriteLine("Всего найдено IP: " + allIPs.Size());
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл input.txt не найден.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}

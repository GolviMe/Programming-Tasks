using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MyArrayList<string> uniqueTags = new MyArrayList<string>();

            try
            {
                // Считываем все строки из файла input.txt
                string[] lines = File.ReadAllLines("input.txt");

                foreach (string line in lines)
                {
                    string currentLine = line.Trim();
                    if (string.IsNullOrEmpty(currentLine)) continue;

                    int pos = 0;
                    while (pos < currentLine.Length)
                    {
                        // Ищем начало тега: символ '<'
                        int start = currentLine.IndexOf('<', pos);
                        if (start == -1) break; // больше тегов нет

                        // Ищем конец тега: символ '>'
                        int end = currentLine.IndexOf('>', start + 1);
                        if (end == -1) break; // некорректный тег, пропускаем

                        string tag = currentLine.Substring(start + 1, end - start - 1); // содержимое между < и >

                        // Приведение тега к нормальному виду:
                        // 1. Удаляется '/' в начале, если есть
                        if (tag.StartsWith("/")) tag = tag.Substring(1);

                        // 2. Приводится к нижнему регистру
                        tag = tag.ToLower();

                        // 3. Проверяется правильность условий тега: первый символ — буква
                        if (tag.Length == 0 || !char.IsLetter(tag[0]))
                        {
                            pos = end + 1;
                            continue; 
                        }

                        // 4. Проверяется, что остальные символы — буквы или цифры
                        bool valid = true;
                        for (int i = 1; i < tag.Length; i++)
                        {
                            char c = tag[i];
                            if (!char.IsLetterOrDigit(c))
                            {
                                valid = false;
                                break;
                            }
                        }
                        if (!valid)
                        {
                            pos = end + 1;
                            continue;
                        }

                        // Добавляется тег в список только если такого ещё нет
                        if (!uniqueTags.Contains(tag))
                        {
                            uniqueTags.Add(tag);
                        }

                        // Переход к следующей позиции после тега
                        pos = end + 1;
                    }
                }

                // Вывод результата
                Console.WriteLine("Уникальные теги:");
                for (int i = 0; i < uniqueTags.Size(); i++)
                {
                    Console.WriteLine(uniqueTags.Get(i));
                }
                Console.WriteLine("\nВсего уникальных тегов: " + uniqueTags.Size());
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл input.txt не найден.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }

            Console.ReadKey();
        }
    }
}

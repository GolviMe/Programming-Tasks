using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_2
{
    public class ComplexNumber
    {
        public double Real { get; set; }
        public double Imaginary { get; set; }

        public ComplexNumber(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        
        
        public static ComplexNumber Plus(ref ComplexNumber a, ref ComplexNumber b) //плюс
        {
            return new ComplexNumber(a.Real + b.Real, a.Imaginary + b.Imaginary);
        }

        public static ComplexNumber Minus(ref ComplexNumber a, ref ComplexNumber b) //минус
        {
            return new ComplexNumber(a.Real - b.Real, a.Imaginary - b.Imaginary);
        }

        public static ComplexNumber Multiply(ref ComplexNumber a, ref ComplexNumber b) //умножить
        {
            double real = a.Real * b.Real - a.Imaginary * b.Imaginary;
            double imaginary = a.Real * b.Imaginary + a.Imaginary * b.Real;
            return new ComplexNumber(real, imaginary);
        }

        public static ComplexNumber Divide(ref ComplexNumber a, ref ComplexNumber b) //делить
        {
            double denominator = b.Real * b.Real + b.Imaginary * b.Imaginary;
            if (denominator == 0)
            {
                Console.WriteLine("Деление на ноль невозможно");
                return new ComplexNumber(0, 0);
            }

            double real = (a.Real * b.Real + a.Imaginary * b.Imaginary) / denominator;
            double imaginary = (a.Imaginary * b.Real - a.Real * b.Imaginary) / denominator;
            return new ComplexNumber(real, imaginary);
        }

        public double Modul() //модуль
        {
            return Math.Sqrt(Real * Real + Imaginary * Imaginary);
        }

        public double Argument() //нахождение аргумента
        {
            if (Real == 0 && Imaginary == 0)
                return 0;
            return Math.Atan2(Imaginary, Real);
        }


        //вывод в строку алгебраической формы
        public override string ToString()
        {
            if (Imaginary == 0)
                return $"{Real}";
            else if (Real == 0)
                return $"{Imaginary}i";
            else if (Imaginary > 0)
                return $"{Real} + {Imaginary}i";
            else
                return $"{Real} - {Math.Abs(Imaginary)}i";
        }





        //вывод тригонометрической формы
        public string TrigonometricForm()
        {
            double modul = Modul();
            double argument = Argument();
            return $"{modul} * (cos({argument}) + i*sin({argument}))";
        }
    }
    class Program
    {
        static ComplexNumber currentNumber = new ComplexNumber(0, 0);

        static void Main()
        {
            Console.WriteLine("Комплексные числа");
            ShowMenu();

            while (true)
            {
                Console.Write("\nВведите команду: ");
                string input = Console.ReadLine();


                switch (input)
                {
                    case "1":
                        InputNumber();
                        break;

                    case "2":
                        Operation("сложение");
                        break;

                    case "3":
                        Operation("вычитание");
                        break;

                    case "4":
                        Operation("умножение");
                        break;

                    case "5":
                        Operation("деление");
                        break;

                    case "6":
                        ShowModul();
                        break;

                    case "7":
                        ShowArgument();
                        break;

                    case "8":
                        ShowReal();
                        break;

                    case "9":
                        ShowImag();
                        break;

                    case "S":
                        DisplayCurrentNumber();
                        break;

                    case "s":
                        DisplayCurrentNumber();
                        break;

                    case "q":
                        Console.WriteLine("Пока");
                        return;

                    case "Q":
                        Console.WriteLine("Пока");
                        return;

                    default:
                        Console.WriteLine("Ошибка");
                        ShowMenu();
                        break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("\nВыберите желаемый пункт:");
            Console.WriteLine("1 - Ввод нового комплексного числа");
            Console.WriteLine("2 - Сложение");
            Console.WriteLine("3 - Вычитание");
            Console.WriteLine("4 - Умножение");
            Console.WriteLine("5 - Деление");
            Console.WriteLine("6 - Модуль");
            Console.WriteLine("7 - Аргумент");
            Console.WriteLine("8 - Вещественная часть");
            Console.WriteLine("9 - Мнимая часть");
            Console.WriteLine("s - Показать число");
            Console.WriteLine("q - Выход");
            Console.WriteLine();
        }

        static void InputNumber()
        {
            try
            {
                Console.Write("Введите вещественную часть: ");
                double real = double.Parse(Console.ReadLine());

                Console.Write("Введите мнимую часть: ");
                double imaginary = double.Parse(Console.ReadLine());

                currentNumber = new ComplexNumber(real, imaginary);
                Console.WriteLine("Новое число успешно введено!");
            }
            catch
            {
                Console.WriteLine("Ошибка");
            }
        }

        static void Operation(string operationName)
        {
            try
            {
                Console.WriteLine($"\nВыбранная операция: {operationName}");
                Console.Write("Введите вещественную часть второго числа: ");
                double real2 = double.Parse(Console.ReadLine());

                Console.Write("Введите мнимую часть второго числа: ");
                double imaginary2 = double.Parse(Console.ReadLine());

                ComplexNumber secondNumber = new ComplexNumber(real2, imaginary2);
                ComplexNumber result;

                switch (operationName)
                {
                    case "сложение":
                        result = ComplexNumber.Plus(ref currentNumber, ref secondNumber);
                        break;
                    case "вычитание":
                        result = ComplexNumber.Minus(ref currentNumber, ref secondNumber);
                        break;
                    case "умножение":
                        result = ComplexNumber.Multiply(ref currentNumber, ref secondNumber);
                        break;
                    case "деление":
                        result = ComplexNumber.Divide(ref currentNumber, ref secondNumber);
                        break;
                    default:
                        Console.WriteLine("Неизвестная операция");
                        return;
                }

                Console.WriteLine($"Результат: {result}");
            }
            catch
            {
                Console.WriteLine("Ошибка");
            }
        }

        static void ShowModul()
        {
            double modul = currentNumber.Modul();
            Console.WriteLine($"Модуль числа {currentNumber}: {modul}");
        }

        static void ShowArgument()
        {
            double argument = currentNumber.Argument();
            double degrees = argument * 180 / Math.PI;
            Console.WriteLine($"Аргумент числа {currentNumber}: {argument} радиан ({degrees} градусов)");
        }

        static void ShowReal()
        {
            Console.WriteLine($"Вещественная часть числа {currentNumber}: {currentNumber.Real}");
        }

        static void ShowImag()
        {
            Console.WriteLine($"Мнимая часть числа {currentNumber}: {currentNumber.Imaginary}");
        }

        static void DisplayCurrentNumber()
        {
            Console.WriteLine($"Текущее число: {currentNumber}");
            Console.WriteLine($"Тригонометрическая форма: {currentNumber.TrigonometricForm()}");
            Console.WriteLine($"Модуль: {currentNumber.Modul()}");
            Console.WriteLine($"Аргумент: {currentNumber.Argument()} радиан");
        }
    }
}

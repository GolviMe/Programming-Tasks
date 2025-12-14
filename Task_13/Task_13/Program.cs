using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_13
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите математическое выражение:");
            string infix = Console.ReadLine().Trim();

            Console.WriteLine("Введите переменные и их значения (например, a=5 b=10), или пустую строку, если переменных нет:");
            string varsLine = Console.ReadLine().Trim();

            try
            {
                // Словарь переменных
                System.Collections.Generic.Dictionary<string, double> variables = new System.Collections.Generic.Dictionary<string, double>();

                if (!string.IsNullOrEmpty(varsLine))
                {
                    string[] pairs = varsLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string pair in pairs)
                    {
                        string[] kv = pair.Split('=');
                        if (kv.Length != 2 || !double.TryParse(kv[1], out double value))
                            throw new Exception("Некорректный формат переменной: " + pair);
                        variables[kv[0].Trim()] = value;
                    }
                }

                // Преобразование в обратную польскую нотацию
                MyStack<string> rpnStack = InfixToRPN(infix, variables);

                // Вычисление обратной польской записи
                double result = EvaluateRPN(rpnStack);

                Console.WriteLine("Результат: " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }

            Console.ReadKey();
        }

        // Приоритеты операций
        private static int Priority(string op)
        {
            switch (op)
            {
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                case "%":
                    return 2;
                case "^":
                    return 3;
                case "sqrt":
                case "sin":
                case "cos":
                case "tan":
                case "ln":
                case "log":
                case "exp":
                case "floor":
                case "sign":
                    return 4;
                default:
                    return 0;
            }
        }

        // Является ли строка унарной функцией
        private static bool IsUnaryFunction(string token)
        {
            return token == "sqrt" || token == "sin" || token == "cos" || token == "tan" ||
                   token == "ln" || token == "log" || token == "exp" || token == "floor" || token == "sign";
        }

        // Преобразование инфиксной нотации в обратную польскую
        private static MyStack<string> InfixToRPN(string infix, System.Collections.Generic.Dictionary<string, double> variables)
        {
            MyStack<string> output = new MyStack<string>();
            MyStack<string> operators = new MyStack<string>();

            string[] tokens = infix.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];

                if (double.TryParse(token, out double num))
                {
                    output.Push(token);
                }
                else if (variables.ContainsKey(token))
                {
                    output.Push(variables[token].ToString());
                }
                else if (IsUnaryFunction(token))
                {
                    operators.Push(token);
                }
                else if (token == "(")
                {
                    operators.Push(token);
                }
                else if (token == ")")
                {
                    while (!operators.Empty() && operators.Peek() != "(")
                    {
                        output.Push(operators.Pop());
                    }
                    if (operators.Empty())
                        throw new Exception("Несбалансированные скобки");
                    operators.Pop(); // удаление '('

                    if (!operators.Empty() && IsUnaryFunction(operators.Peek()))
                    {
                        output.Push(operators.Pop());
                    }
                }
                else // бинарная операция
                {
                    while (!operators.Empty() && Priority(operators.Peek()) >= Priority(token))
                    {
                        output.Push(operators.Pop());
                    }
                    operators.Push(token);
                }
            }

            while (!operators.Empty())
            {
                if (operators.Peek() == "(" || operators.Peek() == ")")
                    throw new Exception("Несбалансированные скобки");
                output.Push(operators.Pop());
            }

            return output;
        }

        // Вычисление обратной польской записи с использованием стека
        private static double EvaluateRPN(MyStack<string> rpn)
        {
            MyStack<double> stack = new MyStack<double>();

            MyStack<string> temp = new MyStack<string>();
            while (!rpn.Empty())
            {
                temp.Push(rpn.Pop());
            }
            while (!temp.Empty())
            {
                string token = temp.Pop();

                if (double.TryParse(token, out double num))
                {
                    stack.Push(num);
                }
                else
                {
                    if (IsUnaryFunction(token))
                    {
                        if (stack.Empty())
                            throw new Exception("Недостаточно операндов для " + token);
                        double a = stack.Pop();

                        switch (token)
                        {
                            case "sqrt": stack.Push(Math.Sqrt(a)); break;
                            case "sin": stack.Push(Math.Sin(a)); break;
                            case "cos": stack.Push(Math.Cos(a)); break;
                            case "tan": stack.Push(Math.Tan(a)); break;
                            case "ln":
                                if (a <= 0) throw new Exception("ln от неположительного числа");
                                stack.Push(Math.Log(a)); break;
                            case "log":
                                if (a <= 0) throw new Exception("log от неположительного числа");
                                stack.Push(Math.Log10(a)); break;
                            case "exp": stack.Push(Math.Exp(a)); break;
                            case "floor": stack.Push(Math.Floor(a)); break;
                            case "sign": stack.Push(Math.Sign(a)); break;
                            default: throw new Exception("Неизвестная функция: " + token);
                        }
                    }
                    else
                    {
                        if (stack.Size() < 2)
                            throw new Exception("Недостаточно операндов для " + token);
                        double b = stack.Pop();
                        double a = stack.Pop();

                        switch (token)
                        {
                            case "+": stack.Push(a + b); break;
                            case "-": stack.Push(a - b); break;
                            case "*": stack.Push(a * b); break;
                            case "/":
                                if (b == 0) throw new Exception("Деление на ноль");
                                stack.Push(a / b); break;
                            case "^": stack.Push(Math.Pow(a, b)); break;
                            case "%": stack.Push(a % b); break;
                            default: throw new Exception("Неизвестная операция: " + token);
                        }
                    }
                }
            }

            if (stack.Size() != 1)
                throw new Exception("Некорректное выражение");

            return stack.Pop();
        }
    }
}

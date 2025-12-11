using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Request : IComparable<Request>
{
    public int Priority { get; set; }
    public int RequestNumber { get; set; }
    public int StepAdded { get; set; }
    public int? StepRemoved { get; set; }

    public Request(int priority, int requestNumber, int stepAdded)
    {
        Priority = priority;
        RequestNumber = requestNumber;
        StepAdded = stepAdded;
        StepRemoved = null;
    }

    //сравн. по приоритету
    public int CompareTo(Request other)
    {
        if (other == null) return 1;

        int priorityCompare = other.Priority.CompareTo(this.Priority);
        if (priorityCompare != 0) return priorityCompare;

        int stepCompare = this.StepAdded.CompareTo(other.StepAdded);
        if (stepCompare != 0) return stepCompare;

        return this.RequestNumber.CompareTo(other.RequestNumber);
    }

    public int GetWaitingTime()
    {
        if (StepRemoved.HasValue)
        {
            return StepRemoved.Value - StepAdded;
        }
        return 0;
    }

    public override string ToString()
    {
        return $"Заявка {RequestNumber}: приоритет {Priority}, добавлена на шаге {StepAdded}, " + $"удалена на шаге {(StepRemoved.HasValue ? StepRemoved.Value.ToString() : "ожидает")}, " + $"время ожидания: {GetWaitingTime()}";
    }
}

class Program
{
    static void Main()
    {
        Console.Write("Введите количество шагов N: ");
        int N = int.Parse(Console.ReadLine());

        var queue = new MyPriorityQueue<Request>();

        using (StreamWriter logFile = new StreamWriter("log.txt"))
        {
            Random random = new Random();
            int requestCounter = 1;
            Request maxWaitingRequest = null;
            int maxWaitingTime = 0;

            for (int step = 1; step <= N; step++)
            {
                Console.WriteLine($"\nШаг {step}:");

                int requestsToGenerate = random.Next(1, 11);
                Console.WriteLine($"Генерируется {requestsToGenerate} заявок:");

                for (int i = 0; i < requestsToGenerate; i++)
                {
                    int priority = random.Next(1, 6);
                    var request = new Request(priority, requestCounter, step);
                    queue.Add(request);

                    //лог
                    string logEntry = $"ADD {requestCounter} {priority} {step}";
                    logFile.WriteLine(logEntry);
                    Console.WriteLine($"  {logEntry}");

                    requestCounter++;
                }

                //удаление заявки с наивысшим приоритетом
                if (!queue.IsEmpty())
                {
                    var removedRequest = queue.Poll();
                    removedRequest.StepRemoved = step;

                    int waitingTime = removedRequest.GetWaitingTime();

                    if (waitingTime > maxWaitingTime)
                    {
                        maxWaitingTime = waitingTime;
                        maxWaitingRequest = removedRequest;
                    }

                    string logEntry = $"REMOVE {removedRequest.RequestNumber} {removedRequest.Priority} {step}";
                    logFile.WriteLine(logEntry);
                    Console.WriteLine($"Удалена заявка: {logEntry}");
                    Console.WriteLine($"Время ожидания: {waitingTime} шагов");
                }
                else
                {
                    Console.WriteLine("Очередь пуста");
                }

                Console.WriteLine($"Заявок в очереди: {queue.Size()}");
            }

            Console.WriteLine($"\nЗавершение генерации заявок");
            Console.WriteLine($"Осталось заявок в очереди: {queue.Size()}");

            int emptyStep = N + 1;
            while (!queue.IsEmpty())
            {
                Console.WriteLine($"\nШаг {emptyStep} (на удаление)");

                var removedRequest = queue.Poll();
                removedRequest.StepRemoved = emptyStep;

                int waitingTime = removedRequest.GetWaitingTime();

                if (waitingTime > maxWaitingTime)
                {
                    maxWaitingTime = waitingTime;
                    maxWaitingRequest = removedRequest;
                }

                string logEntry = $"REMOVE {removedRequest.RequestNumber} {removedRequest.Priority} {emptyStep}";
                logFile.WriteLine(logEntry);
                Console.WriteLine($"Удалена заявка: {logEntry}");
                Console.WriteLine($"Время ожидания: {waitingTime} шагов");
                Console.WriteLine($"Осталось заявок: {queue.Size()}");

                emptyStep++;
            }

            Console.WriteLine("РЕЗУЛЬТАТ:");

            if (maxWaitingRequest != null)
            {
                Console.WriteLine($"Максимальное время ожидания: {maxWaitingTime} шагов");
                Console.WriteLine($"Информация о заявке с максимальным временем ожидания:");
                Console.WriteLine($"Номер заявки: {maxWaitingRequest.RequestNumber}");
                Console.WriteLine($"Приоритет: {maxWaitingRequest.Priority}");
                Console.WriteLine($"Добавлена на шаге: {maxWaitingRequest.StepAdded}");
                Console.WriteLine($"Удалена на шаге: {maxWaitingRequest.StepRemoved}");
                Console.WriteLine($"Время ожидания: {maxWaitingRequest.GetWaitingTime()} шагов");
            }

            Console.WriteLine($"\nЛог операций сохранен в файл: log.txt");
        }

        Console.ReadKey();
    }
}
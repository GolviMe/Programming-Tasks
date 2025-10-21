using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Task_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Высчитывание длины вектора в N-мерном пространстве:");

            try
            {
                var (sizing, matrixG, vectorX) = ReadFile("input.txt");

                //проверка симметрии матрицы
                if (!IsSimmetric(matrixG, sizing))
                {
                    Console.WriteLine("Матрица G не является симметричной!");
                    Console.ReadKey();
                    return;
                }

                double length = CalculateLengthOfVector(vectorX, matrixG, sizing);

                Console.WriteLine($"Длина вектора: {length}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }

        static (int sizing, double[,] matrixG, double[] vectorX) ReadFile(string filename)
        {
            string[] lines = File.ReadAllLines(filename);

            int sizing = int.Parse(lines[0]); //размерность

            double[,] matrixG = new double[sizing, sizing]; //матрица G
            double[] vectorX = new double[sizing]; //вектор x

            for (int i = 0; i < sizing; i++) //заполнение G
            {
                string[] elements = lines[i + 1].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < sizing; j++)
                {
                    matrixG[i, j] = Convert.ToDouble(elements[j]);
                }
            }

            string[] vectorElements = lines[sizing + 1].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries); //ввод вектора

            for (int i = 0; i < sizing; i++)
            {
                vectorX[i] = double.Parse(vectorElements[i]);
            }

            return (sizing, matrixG, vectorX);
        }

        static bool IsSimmetric(double[,] matrix, int dimension)
        {
            for (int i = 0; i < dimension; i++)
            {
                for (int j = i + 1; j < dimension; j++)
                {
                    if (Math.Abs(matrix[i, j]) != Math.Abs(matrix[j, i]))
                    {
                        Console.WriteLine("Матрица НЕсимметрична");
                        return false;
                    }
                }
            }
            return true;
        }




        static double CalculateLengthOfVector(double[] vector, double[,] matrix, int sizing)
        {
            double[] tempRes = new double[sizing];


            for (int j = 0; j < sizing; j++) //Умножение вектора на матрицу
            {
                double sum = 0;
                for (int i = 0; i < sizing; i++)
                {
                    sum += vector[i] * matrix[i, j];
                }
                tempRes[j] = sum;
            }

            //умножение (вектора на матрицу) на транспонированную матрицу
            double tempPoint = 0;
            for (int i = 0; i < sizing; i++)
            {
                tempPoint += tempRes[i] * vector[i];
            }

            return Math.Sqrt(tempPoint);
        }
    }
}

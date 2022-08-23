using System;

namespace ConsoleApp
{
    internal class Program
    {
        static readonly string fileName = "data.csv";

        static void Main(string[] args)
        {
            var reader = new DataReader();
            reader.ImportAndPrintData(fileName);

            Console.ReadLine();
        }
    }
}

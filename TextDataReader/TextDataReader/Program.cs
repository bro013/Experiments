using System;
using System.IO;

namespace TextDataReader
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = Path.Combine(AppContext.BaseDirectory, "TextFile.txt");
            TextTableReader reader = new TextTableReader(fileName);
            while (reader.Read())
            {
                Console.WriteLine(reader["col2"]);
            }
            Console.ReadKey();
        }
    }
}

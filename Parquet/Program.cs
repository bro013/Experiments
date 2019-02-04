using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Parquet.Data;

namespace Parquet
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "/home/bjorn/Documents/test.parquet";
            var idColumn = new DataColumn(
            new DataField<int>("id"),
            new int[] { 1, 2 });

            var cityColumn = new DataColumn(
            new DataField<string>("city"),
            new string[] { "London", "Derby" });

            // create file schema
            var schema = new Schema(idColumn.Field, cityColumn.Field);
            using (Stream fileStream = System.IO.File.OpenWrite(path))
            {
                using (var parquetWriter = new ParquetWriter(schema, fileStream))
                {
                    // create a new row group in the file
                    using (ParquetRowGroupWriter groupWriter = parquetWriter.CreateRowGroup())
                    {
                        groupWriter.WriteColumn(idColumn);
                        groupWriter.WriteColumn(cityColumn);
                    }
                }
            }
        }
    }
}

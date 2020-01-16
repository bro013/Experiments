using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;

namespace TextDataReader
{
    public class TextTableReader : IDataReader
    {
        protected StreamReader Stream { get; set; }
        protected object[] Values;
        protected bool Eof { get; set; }
        protected string CurrentLine { get; set; }
        protected int CurrentIndex { get; set; }

        public TextTableReader(string path)
        {
            Stream = File.OpenText(path);
        }
        public bool Read()
        {
            CurrentLine = Stream.ReadLine();
            Eof = string.IsNullOrEmpty(CurrentLine);
            if (!Eof)
            {
                Values = CurrentLine.Split(",");
            }
            else
            {
                
                Close();
            }
            return !Eof;
        }

        private void Fill(object[] values)
        {
            //To simplify the implementation, lets assume here that the table have just 3         
            //columns: the primary key, and 2 string columns. And the file is fixed column formatted 
            //and have 2 columns: the first with width 12 and the second with width 40. Said that, we can do as follows

            values[0] = null;
            values[1] = null;
            values[2] = null;

            // by default, the first position of the array hold the value that will be  
            // inserted at the first column of the table, and so on
            // lets assume here that the primary key is auto-generated
            // if the file is xml we could parse the nodes instead of Substring operations
        }

        public int RecordsAffected
        {
            get { return -1; }
        }

        public int FieldCount
        {
            get
            {
                return 3;//assuming the table has 3 columns }
            }
        }

        private object GetValue(string name)
        {
            return name switch
            {
                "col1" => Values[0],
                "col2" => Values[1],
                "col3" => Values[2],
                _ => throw new InvalidDataException("Invalid column name"),
            };
        }

        public int Depth =>  0;

        public bool IsClosed => !Stream.EndOfStream;

        public object this[string name] => GetValue(name);

        public IDataReader GetData(int i)
        {
            if (i == 0)
                return this;

            return null;
        }

        public string GetDataTypeName(int i)
        {
            return "String";
        }

        public string GetName(int i)
        {
            return Values[i].ToString();
        }

        public string GetString(int i)
        {
            return Values[i].ToString();
        }

        public object GetValue(int i)
        {
            return Values[i];
        }

        public int GetValues(object[] values)
        {
            Fill(values);

            Array.Copy(values, Values, this.FieldCount);

            return this.FieldCount;
        }

        public void Close()
        {
            Stream.Close();
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Stream.Dispose();
        }

        public object this[int i]
        {
            get { return Values[i]; }
        }
    }
}

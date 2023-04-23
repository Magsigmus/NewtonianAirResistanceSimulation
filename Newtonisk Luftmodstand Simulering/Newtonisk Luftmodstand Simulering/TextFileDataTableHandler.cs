using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace DataFiles
{
    internal static class TextFileDataTableHandler
    {
        public static DataTable ReadDataTableFromTextFile(string path)
        {
            StreamReader sr = new StreamReader(path);
            DataTable dt = new DataTable();
            dt.Clear();

            string firstRow = sr.ReadLine();
            string[] Columns = firstRow.Split('\t');
            for(int i = 0; i < Columns.Length; i++)
            {
                dt.Columns.Add(Columns[i]);
            }

            while (!sr.EndOfStream)
            {
                string[] vals = sr.ReadLine().Split('\t');
                DataRow row = dt.NewRow();
                for(int i = 0; i < vals.Length; i++)
                {
                    row[Columns[i]] = vals[i];
                }
                dt.Rows.Add(row);
            }

            return dt;
        }

        public static void WriteDataTableToTextfile(string path, DataTable table)
        {
            string formattedTable = string.Join("\n", table.Rows.Cast<DataRow>().Select(
                x => string.Join("\t", x.ItemArray.Select(y => y.ToString()).ToArray())));
            WriteString(path, formattedTable);
        }

        public static void WriteString(string path, string str)
        {
            //Write some text to the test.txt file
            StreamWriter writer = new StreamWriter(path, true);
            writer.WriteLine(str);
            writer.Close();
        }

        public static string ReadString(string path)
        {
            //Read the text from directly from the test.txt file
            StreamReader reader = new StreamReader(path);

            string result = reader.ReadToEnd();
            reader.Close();
            return result;
        }

        public static string CreateTextFile(string path)
        {
            string orgPath = path;
            int val = 1;
            while (File.Exists(path))
            {
                val++;
                path = orgPath.Insert(orgPath.Length - 4, val.ToString());
            }

            // Create a file to write to.
            StreamWriter sw = File.CreateText(path);

            return path;
        }
    }
}

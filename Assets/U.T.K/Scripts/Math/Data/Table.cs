using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UTK.Math.Data
{
    public class Table
    {
        Dictionary<string, TableColumn> tableData = new Dictionary<string, TableColumn>();

        public void CreateColumn(string dataName)
        {
            if(!tableData.ContainsKey(dataName))
            {
                tableData.Add(dataName, new TableColumn(dataName));
            }
        }

        public void AppendData(string dataName, int entry)
        {
            tableData[dataName].AppendData(entry.ToString());
        }

        public void AppendData(string dataName, double entry)
        {
            tableData[dataName].AppendData(entry.ToString());
        }

        public void AppendData(string dataName, float entry)
        {
            tableData[dataName].AppendData(entry.ToString());
        }

        public void AppendData(string dataName, string entry)
        {
            tableData[dataName].AppendData(entry.ToString());
        }

        public void AppendRange(string dataName, List<string> entries)
        {
            tableData[dataName].AppendRange(entries);
        }

        public void AppendRange(string dataName, List<double> entries)
        {
            foreach (double val in entries)
            {
                tableData[dataName].AppendData(val.ToString());
            }
        }

        public void AppendRange(string dataName, List<float> entries)
        {
            foreach (float val in entries)
            {
                tableData[dataName].AppendData(val.ToString());
            }
        }

        public void AppendRange(string dataName, List<int> entries)
        {
            foreach (int val in entries)
            {
                tableData[dataName].AppendData(val.ToString());
            }
        }

        public void AppendTable(string dataName, ITabulate entry)
        {
            List<List<string>> tableData = entry.GetTableData();
            int index = 0;

            foreach(string label in entry.GetLabels())
            {
                CreateColumn(dataName + ":" + label);

                AppendRange(label, tableData[index++]);
            }
        }

        void BuildTitleRow(StringBuilder builder)
        {
            string currentTitle = "";
            string previousTitle = "";
            foreach(string title in tableData.Keys)
            {
                string [] titleLevels = title.Split(new char[] { ':' });

                if(titleLevels.Length > 1)
                {
                    currentTitle = titleLevels[0];
                }

                if(currentTitle != previousTitle)
                {
                    builder.Append(currentTitle);
                }
                else
                {
                    builder.Append(' ');
                }
                builder.Append(',');
            }

            builder.AppendLine();
        }

        void BuildLabels(StringBuilder builder)
        {
            foreach (string label in tableData.Keys)
            {
                string[] labelLevels = label.Split(new char[] { ':' });
                string currentLabel;

                if (labelLevels.Length > 1)
                {
                    currentLabel = labelLevels[1];
                }

                builder.Append(label + ",");
            }

            builder.AppendLine();
        }

        void BuildRow(StringBuilder builder, int index)
        {
            foreach (string label in tableData.Keys)
            {
                if (tableData[label].Length > index)
                {
                    builder.Append("\"" + tableData[label][index] + "\"" + ",");
                }
                else
                {
                    builder.Append(" ,");
                }
            }
            builder.AppendLine();
        }

        public void SaveToCSV(string destination, bool relativePath = false, bool appendTimeStamp = false)
        {
            StringBuilder builder = new StringBuilder();

            BuildTitleRow(builder);

            BuildLabels(builder);
            int maxTableLength = 0;

            foreach(string label in tableData.Keys)
            {
                if (tableData[label].Length > maxTableLength)
                    maxTableLength = tableData[label].Length;
            }

            for (int i = 0; i < maxTableLength; i++)
            {
                BuildRow(builder, i);
            }

            string fileName = System.IO.Path.GetFileNameWithoutExtension(destination);
            string filePath = System.IO.Path.GetDirectoryName(destination);

            string finalPath = "";

            if (relativePath)
            {
                finalPath = System.IO.Directory.GetCurrentDirectory() +"/" + filePath + "/";
            }
            else
            {
                finalPath = filePath;
            }

            System.IO.Directory.CreateDirectory(finalPath);

            if (appendTimeStamp)
            {
                finalPath += "/" + fileName + DateTime.Now.ToString("dd_MM_yyyy_HH_mm") + ".csv";
            }
            else
            {
                finalPath += "/" + fileName + ".csv";
            }

            System.IO.File.WriteAllText(finalPath, builder.ToString());
        }
    }

    public interface ITabulate
    {
        List<List<string>> GetTableData();

        List<string> GetLabels();

        List<int> GetSortedOrder();
    }

    internal class TableColumn
    {
        List<string> stringVector = new List<string>();

        public string label = "";

        public string this[int index]
        {
            get
            {
                return stringVector[index];
            }
        }
        public int Length
        {
            get
            {
                return stringVector.Count;
            }
        }

        public TableColumn(string label)
        {
            this.label = label;

        }

        public void AppendData(string entry)
        {
            stringVector.Add(entry.ToString());
        }

        public void AppendRange(List<string> entries)
        {
            stringVector.AddRange(entries);
        }
    }
}

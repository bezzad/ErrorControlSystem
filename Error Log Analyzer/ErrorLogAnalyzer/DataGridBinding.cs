using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ErrorLogAnalyzer
{
    public static class DataGridBinding
    {
        public static void CreateColumns(this DataGridView dataGrid, Type propertyType)
        {
            foreach (var property in propertyType.GetProperties())
            {
                dataGrid.Columns.Add(property.Name, GetHeaderNameFromColName(property.Name));
            }
        }

        public static void AddRow(this DataGridView dataGrid, Object row)
        {
            dataGrid.Rows.Add();

            foreach (DataGridViewColumn col in dataGrid.Columns)
            {
                dataGrid.Rows[dataGrid.Rows.Count - 2].Cells[col.Name].Value =
                     row.GetType().GetProperty(col.Name).GetValue(row) ?? "";
            }
        }

        public static void RemoveRow(this DataGridView dataGrid, Object rowObj)
        {
            foreach (var row in dataGrid.Rows.Cast<DataGridViewRow>())
            {
                var foundFlag = true;
                foreach (var col in dataGrid.Columns.Cast<DataGridViewColumn>())
                {
                    if (row.Cells[col.Name].Value == null || rowObj.GetType().GetProperty(col.Name).GetValue(rowObj) == null)
                    {
                        if (!ReferenceEquals(row.Cells[col.Name].Value, rowObj.GetType().GetProperty(col.Name).GetValue(rowObj)))
                        {
                            foundFlag = false;
                            break;
                        }
                    }

                    else if (row.Cells[col.Name].Value.ToString() != rowObj.GetType().GetProperty(col.Name).GetValue(rowObj).ToString())
                    {
                        foundFlag = false;
                        break;
                    }
                }
                if (foundFlag)
                {
                    dataGrid.Rows.Remove(row);
                    break;
                }
            }

            dataGrid.Refresh();
        }

        internal static string GetHeaderNameFromColName(string columnName)
        {
            var header = Regex.Replace(columnName, "([a-z])([A-Z])", "$1 $2");

            return header;
        }
    }
}


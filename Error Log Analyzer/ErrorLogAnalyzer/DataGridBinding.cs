using System;
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

        internal static string GetHeaderNameFromColName(string columnName)
        {
            var header = Regex.Replace(columnName, "([a-z])([A-Z])", "$1 $2");

            return header;
        }
    }
}


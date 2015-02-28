﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ErrorLogAnalyzer
{
    public static class DataGridBinding
    {
        static readonly Dictionary<DataGridView, List<Object>> History = new Dictionary<DataGridView, List<object>>();

        public static void CreateColumns(this DataGridView dataGrid, Type propertyType)
        {
            if (!History.ContainsKey(dataGrid))
                History.Add(dataGrid, new List<object>());

            foreach (var property in propertyType.GetProperties())
            {
                dataGrid.Columns.Add(property.Name, GetHeaderNameFromColName(property.Name));
            }
        }

        public static void AddRow(this DataGridView dataGrid, Object obj)
        {
            if (History.ContainsKey(dataGrid))
            {
                History[dataGrid].Add(obj);

                //dataGrid.Rows.Clear();

                //foreach (var row in History[dataGrid])
                //{
                var row = obj;
                dataGrid.Rows.Add();

                foreach (var property in row.GetType().GetProperties())
                {
                    dataGrid.Rows[dataGrid.Rows.Count - 2].Cells[property.Name].Value =
                        row.GetType().GetProperty(property.Name).GetValue(row) ?? "";
                }
                //}
            }
        }

        internal static string GetHeaderNameFromColName(string columnName)
        {
            var header = Regex.Replace(columnName, "([a-z])([A-Z])", "$1 $2");

            return header;
        }
    }
}


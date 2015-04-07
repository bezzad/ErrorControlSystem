using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ErrorLogAnalyzer
{
    public class DynamicDataGridView<T> : DataGridView
    {
        public new List<T> DataSource { get; set; }

        public DynamicDataGridView()
        {
            CreateColumns(typeof(T));
        }

        public void CreateColumns(Type propertyType)
        {
            foreach (var property in propertyType.GetProperties())
            {
                this.Columns.Add(property.Name, GetHeaderNameFromColName(property.Name));
            }
        }

        public void AddRow(Object row)
        {
            this.Rows.Add();

            foreach (DataGridViewColumn col in this.Columns)
            {
                this.Rows[this.Rows.Count - 1].Cells[col.Name].Value =
                     row.GetType().GetProperty(col.Name).GetValue(row) ?? "";
            }
        }

        public void RemoveRow(Object rowObj)
        {
            foreach (var row in this.Rows.Cast<DataGridViewRow>())
            {
                var foundFlag = true;
                foreach (var col in this.Columns.Cast<DataGridViewColumn>())
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
                    this.Rows.Remove(row);
                    break;
                }
            }

            this.Refresh();
        }

        public void RemoveRowByCondition(Object rowObj, Func<DataGridViewRow, object, bool> comparison)
        {
            foreach (var row in this.Rows.Cast<DataGridViewRow>())
            {
                if (comparison(row, rowObj))
                {
                    this.Rows.Remove(row);
                }
            }
        }

        public void UpdateRow(Object rowObj, Func<DataGridViewRow, object, bool> comparison)
        {
            foreach (DataGridViewRow row in this.Rows)
            {
                if (comparison(row, rowObj))
                {
                    foreach (DataGridViewColumn col in this.Columns)
                    {
                        row.Cells[col.Name].Value = rowObj.GetType().GetProperty(col.Name).GetValue(rowObj) ?? "";
                    }
                }
            }
        }


        internal string GetHeaderNameFromColName(string columnName)
        {
            var header = Regex.Replace(columnName, "([a-z])([A-Z])", "$1 $2");

            return header;
        }
    }
}
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ErrorLogAnalyzer
{
    internal static class DataGridViewHelper<TCol, TRow>
        where TCol : class, IEquatable<TCol>
        where TRow : class, IEquatable<TCol>
    {

        public static void CreateColumns(DataGridView dgv)
        {

            foreach (var property in typeof(TCol).GetProperties())
            {
                dgv.Columns[dgv.Columns.Add(property.Name, GetHeaderNameFromColName(property.Name))].DataPropertyName
                    =
                    property.Name;
            }
        }

        public static void AddRow(DataGridView dgv, TRow row)
        {
            var index = dgv.Rows.Add();

            foreach (DataGridViewColumn col in dgv.Columns)
            {
                dgv.Rows[index].Cells[col.Name].Value = row.GetType().GetProperty(col.Name).GetValue(row) ?? "";
            }
        }

        public static void RemoveRow(DataGridView dgv, TRow rowObj)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (GetRowataBoundItem(row, typeof(TRow)).Equals(rowObj))
                {
                    dgv.Rows.Remove(row);
                    break;
                }
            }
            dgv.Refresh();
        }

        public static void UpdateRow(DataGridView dgv, TRow rowObj)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (GetRowataBoundItem(row, typeof(TRow)).Equals(rowObj))
                {
                    foreach (DataGridViewColumn col in dgv.Columns)
                    {
                        row.Cells[col.Name].Value = rowObj.GetType().GetProperty(col.Name).GetValue(rowObj) ?? "";
                    }
                }
            }
        }

        public static TRow GetCurrentRow(DataGridView dgv)
        {
            if (dgv.CurrentRow != null)
                return ((TRow)GetRowataBoundItem(dgv.CurrentRow, typeof(TRow)));

            return null;
        }


        public static object GetRowataBoundItem(DataGridViewRow row, Type T)
        {
            var tInstance = Activator.CreateInstance(T);

            foreach (var cell in row.Cells.Cast<DataGridViewCell>())
            {
                T.GetProperty(cell.OwningColumn.DataPropertyName).SetValue(tInstance, cell.Value);
            }

            return tInstance;
        }

        private static string GetHeaderNameFromColName(string columnName)
        {
            var header = Regex.Replace(columnName, "([a-z])([A-Z])", "$1 $2");

            return header;
        }
    }
}

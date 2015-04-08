using System.Windows.Forms;
using ErrorControlSystem.Shared;

namespace ErrorLogAnalyzer
{
    public class ErrorDataGridView : DataGridView
    {
        public ErrorDataGridView()
        {
            CreateColumns();
        }

        public void CreateColumns()
        {
            DataGridViewHelper<IError, ProxyError>.CreateColumns(this);
        }

        public void AddRow(ProxyError row)
        {
            DataGridViewHelper<IError, ProxyError>.AddRow(this, row);
        }

        public void RemoveRow(ProxyError rowObj)
        {
            DataGridViewHelper<IError, ProxyError>.RemoveRow(this, rowObj);
        }

        public void UpdateRow(ProxyError rowObj)
        {
            DataGridViewHelper<IError, ProxyError>.UpdateRow(this, rowObj);
        }

        public ProxyError GetCurrentRow()
        {
            return DataGridViewHelper<IError, ProxyError>.GetCurrentRow(this);
        }
    }
}
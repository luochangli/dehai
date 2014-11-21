using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using FormUI.OperationLayer;
using TomorrowSoft.BLL;
using TomorrowSoft.Model;

namespace FormUI.ManagerForms
{
    public partial class HistoryForm : Form
    {
        private readonly TomorrowSoft.BLL.HistoryRecordService _history = new HistoryRecordService();
        DataGridViewPrinter MyDataGridViewPrinter;
        private BindingSource bs = new BindingSource();
   
        public HistoryForm()
        {
            
            InitializeComponent();
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.AppendFormat("datetime([HandlerTime]) >= datetime('{0} 00:00:00') ",DateTime .Now.ToString("yyyy-MM-dd"));
            sbQuery.Append(" AND ");
            sbQuery.AppendFormat("datetime([HandlerTime]) <= datetime('{0}') ", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            List<HistoryRecord> terminals = _history.GetModelList(sbQuery.ToString());
            bs.DataSource = terminals;
            GetHistoryList(bs);
            MyDataGridViewPrinter = new DataGridViewPrinter();

        } 
      
        private void GetHistoryList(BindingSource bs)
        {
            
            dataGridView1.DataSource = bs;
            bindingNavigator1.BindingSource = bs;
            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.AutoResizeColumn(4, DataGridViewAutoSizeColumnMode.AllCells);
     
           
        }
        private void btQuery_Click(object sender, EventArgs e)
        {
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.AppendFormat("datetime([HandlerTime]) >= datetime('{0} 00:00:00') ", dtpBegin.Value.ToString("yyyy-MM-dd"));
            sbQuery.Append(" AND ");
            sbQuery.AppendFormat("datetime([HandlerTime]) <= datetime('{0}') ", dtpEnd.Value.ToString("yyyy-MM-dd HH:mm:ss"));
            List<HistoryRecord> terminals = _history.GetModelList(sbQuery.ToString());
            bs.DataSource = terminals;
            GetHistoryList(bs);
        }

        
        private void btPrint_Click(object sender, EventArgs e)
        {
            MyDataGridViewPrinter.PrintView(dataGridView1);
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                if (MessageBox.Show(string.Format(@"您确定要清除所有历史记录？"), "提示",
                                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {

                    _history.DeleteList("");
                    
                    MessageBox.Show("清除成功！");
                    GetHistoryList(bs);
                }
            }
            else
            {
                if(MessageBox .Show(string.Format( "您确定要删除{0}记录?",dataGridView1 .SelectedRows .Count ),"提示",
                MessageBoxButtons .OKCancel ,MessageBoxIcon.Question ) == DialogResult .OK )
                {
                    for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                    {
                        _history.Delete(Convert.ToInt32(dataGridView1.SelectedRows[i].Cells[0].Value));
                    }
                    MessageBox.Show("删除成功！");
                    GetHistoryList(bs);
                }
            }

        }

        private void HistoryForm_Load(object sender, EventArgs e)
        {
            if (LoginForm.Level == "管理员")
            {
                btClear.Visible  = true;
            }
        }
      
        
      
     
    }
}

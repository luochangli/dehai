using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace FormUI.OperationLayer
{
    /// <summary>
    ///     实现DataGridView的打印 类
    /// </summary>
    public class Printer
    {
        //打印文檔 
        private static readonly PrintDocument _pdDocument = new PrintDocument();

        //打印格式設置頁面 
        private static readonly PageSetupDialog _dlgPageSetup = new PageSetupDialog();

        //打印頁面 
        private static readonly PrintDialog _dlgPrint = new PrintDialog();
        //實例化打印預覽 
        private static readonly PrintPreviewDialog _dlgPrintPreview = new PrintPreviewDialog();

        private static readonly List<DataGridViewCellPrint> CellPrintList = new List<DataGridViewCellPrint>();

        /// <summary>
        ///     打印的行数
        /// </summary>
        private static int printRowCount;

        /// <summary>
        ///     是否要打印
        /// </summary>
        private static bool IsPrint = true;

        /// <summary>
        ///     设置的起始位置是否大于默认打印的边框
        /// </summary>
        private static bool IsRole = true;

        /// <summary>
        ///     X坐标
        /// </summary>
        private static int PoXTmp;

        /// <summary>
        ///     Y坐标
        /// </summary>
        private static int PoYTmp;

        /// <summary>
        ///     列间距
        /// </summary>
        private static int WidthTmp;

        /// <summary>
        ///     行间距
        /// </summary>
        private static int HeightTmp;

        /// <summary>
        ///     列数
        /// </summary>
        private static int RowIndex;

        /// <summary>
        ///     DataGridView1数据绑定
        /// </summary>
        public static DataGridView DataGridViewList;

        /// <summary>
        ///     构造函数
        /// </summary>
        public Printer()
        {
            _pdDocument.PrintPage += OnPrintPage;
            //頁面設置的打印文檔設置為需要打印的文檔 
            _dlgPageSetup.Document = _pdDocument;

            //打印界面的打印文檔設置為被打印文檔 
            _dlgPrint.Document = _pdDocument;

            //打印預覽的打印文檔設置為被打印文檔 
            _dlgPrintPreview.Document = _pdDocument;
        }

        /// <summary>
        ///     //顯示打印預覽界面 ，此处需要添加一个打印预览的按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void PrintView()
        {
            _dlgPrintPreview.ShowDialog();
        }

        /// <summary>
        ///     打印设置，此处需要添加一个打印设置的按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void PrintSetup()
        {
//            _dlgPageSetup.ShowDialog();
            _dlgPrint.ShowDialog();
        }

        /// <summary>
        ///     printDocument的PrintPage事件 ，实现打印功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPrintPage(object sender, PrintPageEventArgs e)
        {
            int iX = 30;
            int iY = 40;
            Print(DataGridViewList, true, e, ref iX, ref iY);
        }

        /// <summary>
        ///     打印
        /// </summary>
        public static void Printting()
        {
            _pdDocument.Print();
        }

        /// 打印DataGridView控件 
        ///  
        /// DataGridView控件 
        /// 是否包括列标题 
        /// 为 System.Drawing.Printing.PrintDocument.PrintPage 事件提供数据。 
        /// 起始X坐标 
        /// 起始Y坐标
        private static void Print(DataGridView dataGridView, bool includeColumnText, PrintPageEventArgs eValue,
                                  ref int PoX, ref int PoY)
        {
            try
            {
                if (IsPrint)
                {
                    printRowCount = 0;
                    IsPrint = false;
                    DataGridViewCellVsList(dataGridView, includeColumnText); //获取要打印的数据
                    if (0 == CellPrintList.Count)
                        return;
                    if (PoX > eValue.MarginBounds.Left) //如果设置的起始位置大于默认打印的边框，IsRole为true
                        IsRole = true;
                    else
                        IsRole = false;
                    PoXTmp = PoX;
                    PoYTmp = PoY;
                    RowIndex = 0;
                    WidthTmp = 0;
                    HeightTmp = 0;
                }
                if (0 != printRowCount) //换页后确定打印的初始位置
                {
                    if (IsRole) //如果设置的起始位置大于默认打印的边框,起始位置为默认打印边框
                    {
                        PoX = PoXTmp = eValue.MarginBounds.Left;
                        PoY = PoYTmp = eValue.MarginBounds.Top;
                    }
                    else
                    {
                        PoX = PoXTmp;
                        PoY = PoYTmp;
                    }
                }
                while (printRowCount < CellPrintList.Count)
                {
                    DataGridViewCellPrint CellPrint = CellPrintList[printRowCount];
                    if (RowIndex == CellPrint.RowIndex) //一行一行打印，CellPrint.RowIndex为datagridview1的行号
                        PoX = PoX + WidthTmp; //如果数据在一行，x坐标右移
                    else //换行后Y坐标下移，X坐标回到初始位置
                    {
                        PoX = PoXTmp;
                        PoY = PoY + HeightTmp;
                        if (PoY + HeightTmp > eValue.MarginBounds.Bottom) //分页
                        {
                            HeightTmp = 0;
                            eValue.HasMorePages = true;
                            return; //重新触发OnPrintPage事件
                        }
                    }

                    using (var solidBrush = new SolidBrush(CellPrint.BackColor))
                    {
                        var rectF1 = new RectangleF(PoX, PoY, CellPrint.Width, CellPrint.Height);
                        eValue.Graphics.FillRectangle(solidBrush, rectF1);
                        using (var pen = new Pen(Color.Black, 1))
                            eValue.Graphics.DrawRectangle(pen, Rectangle.Round(rectF1)); //画出单个数据的方框格子
                        solidBrush.Color = CellPrint.ForeColor;
                        eValue.Graphics.DrawString(CellPrint.FormattedValue, CellPrint.Font, solidBrush,
                                                   new Point(PoX + 2, PoY + 3)); //在方框中画出数据
                    }
                    WidthTmp = CellPrint.Width;
                    HeightTmp = CellPrint.Height;
                    RowIndex = CellPrint.RowIndex;
                    printRowCount++;
                }
                PoY = PoY + HeightTmp; //全部打印完后不再分页
                eValue.HasMorePages = false;
                IsPrint = true;
            }
            catch
            {
                eValue.HasMorePages = false;
                IsPrint = true;
                throw; //抛出异常
            }
        }

        /// 将DataGridView控件内容转变到 CellPrintList 
        ///  
        /// DataGridView控件 
        /// 是否包括列标题
        private static void DataGridViewCellVsList(DataGridView dataGridView, bool includeColumnText)
        {
            CellPrintList.Clear();
            try
            {
                int rowsCount = dataGridView.Rows.Count;
                int colsCount = dataGridView.Columns.Count;

                //最后一行是供输入的行时，不用读数据。 
                if (dataGridView.Rows[rowsCount - 1].IsNewRow)
                    rowsCount--;
                //包括列标题 
                if (includeColumnText)
                {
                    for (int columnsIndex = 0; columnsIndex < colsCount; columnsIndex++)
                    {
                        if (dataGridView.Columns[columnsIndex].Visible)
                        {
                            var CellPrint = new DataGridViewCellPrint();
                            CellPrint.FormattedValue = dataGridView.Columns[columnsIndex].HeaderText;
                            CellPrint.RowIndex = 0;
                            CellPrint.ColumnIndex = columnsIndex;
                            CellPrint.Font = dataGridView.Columns[columnsIndex].HeaderCell.Style.Font;
                            CellPrint.BackColor = dataGridView.ColumnHeadersDefaultCellStyle.BackColor;
                            CellPrint.ForeColor = dataGridView.ColumnHeadersDefaultCellStyle.ForeColor;
                            CellPrint.Width = dataGridView.Columns[columnsIndex].Width;
                            CellPrint.Height = dataGridView.ColumnHeadersHeight;
                            CellPrintList.Add(CellPrint); //add:每次添加一个数据
                        }
                    }
                }
                //读取单元格数据 
                for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
                {
                    for (int columnsIndex = 0; columnsIndex < colsCount; columnsIndex++)
                    {
                        if (dataGridView.Columns[columnsIndex].Visible)
                        {
                            var CellPrint = new DataGridViewCellPrint();
                            CellPrint.FormattedValue =
                                dataGridView.Rows[rowIndex].Cells[columnsIndex].FormattedValue.ToString();
                            if (includeColumnText)
                                CellPrint.RowIndex = rowIndex + 1; //假如包括列标题则从行号1开始 
                            else
                                CellPrint.RowIndex = rowIndex;
                            CellPrint.ColumnIndex = columnsIndex;
                            CellPrint.Font = dataGridView.Rows[rowIndex].Cells[columnsIndex].Style.Font;
                            Color TmpColor = Color.Empty;
                            if (Color.Empty !=
                                dataGridView.Rows[rowIndex].Cells[columnsIndex].Style.BackColor)
                                TmpColor = dataGridView.Rows[rowIndex].Cells[columnsIndex].Style.BackColor;
                            else if (Color.Empty != dataGridView.Rows[rowIndex].DefaultCellStyle.BackColor)
                                TmpColor = dataGridView.Rows[rowIndex].DefaultCellStyle.BackColor;
                            else
                                TmpColor = dataGridView.DefaultCellStyle.BackColor;
                            CellPrint.BackColor = TmpColor;
                            TmpColor = Color.Empty;
                            if (Color.Empty !=
                                dataGridView.Rows[rowIndex].Cells[columnsIndex].Style.ForeColor)
                                TmpColor = dataGridView.Rows[rowIndex].Cells[columnsIndex].Style.ForeColor;
                            else if (Color.Empty != dataGridView.Rows[rowIndex].DefaultCellStyle.ForeColor)
                                TmpColor = dataGridView.Rows[rowIndex].DefaultCellStyle.ForeColor;
                            else
                                TmpColor = dataGridView.DefaultCellStyle.ForeColor;
                            CellPrint.ForeColor = TmpColor;
                            CellPrint.Width = dataGridView.Columns[columnsIndex].Width;
                            CellPrint.Height = dataGridView.Rows[rowIndex].Height;
                            CellPrintList.Add(CellPrint);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private class DataGridViewCellPrint
        {
            private Color _BackColor = Color.White;
            private int _ColumnIndex = -1;
            private Font _Font = new Font("宋体", 9F, FontStyle.Regular, GraphicsUnit.Point, ((134)));
            private Color _ForeColor = Color.Black;

            /// <summary>
            ///     格式化的单元格的值
            /// </summary>
            private string _FormattedValue = "";

            private int _Height = 30;
            private int _RowIndex = -1;
            private int _Width = 100;

            /// <summary>
            ///     获取或设置单元格的字体。
            ///     <summary>
            public Font Font
            {
                set { if (null != value) _Font = value; }
                get { return _Font; }
            }

            /// <summary>
            ///     获取为显示进行格式化的单元格的值。
            ///     <summary>
            public string FormattedValue
            {
                set { _FormattedValue = value; }
                get { return _FormattedValue; }
            }

            /// <summary>
            ///     获取或设置列的当前宽度 （以像素为单位）。默认值为 100。
            ///     <summary>
            public int Width
            {
                set { _Width = value; }
                get { return _Width; }
            }

            /// <summary>
            ///     获取或设置列标题行的高度（以像素为单位）。默认值为 30。
            ///     <summary>
            public int Height
            {
                set { _Height = value; }
                get { return _Height; }
            }

            /// <summary>
            ///     获取或设置行号。
            ///     <summary>
            public int RowIndex
            {
                set { _RowIndex = value; }
                get { return _RowIndex; }
            }

            /// <summary>
            ///     获取或设置列号。
            ///     <summary>
            public int ColumnIndex
            {
                set { _ColumnIndex = value; }
                get { return _ColumnIndex; }
            }

            /// <summary>
            ///     获取或设置前景色。
            ///     <summary>
            public Color ForeColor
            {
                set { _ForeColor = value; }
                get { return _ForeColor; }
            }

            /// <summary>
            ///     获取或设置背景色。
            ///     <summary>
            public Color BackColor
            {
                set { _BackColor = value; } //只写
                get { return _BackColor; } //只读
            }
        }
    }
}
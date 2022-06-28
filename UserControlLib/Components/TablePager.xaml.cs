using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UserControlLib.Components
{
    public interface ITablePager
    {
        IEnumerable RefreshData();

        DataGrid DataTable();
    }

    /// <summary>
    /// TablePager.xaml 的交互逻辑
    /// 表格分页器
    /// </summary>
    public partial class TablePager : UserControl
    {
        private ITablePager itablePager = null;
        private DataGrid dgrWrapper = null;

        public DataGrid DgrWrapper 
        {
            get { return dgrWrapper; }
        }

        /// <summary>
        /// 当前页
        /// </summary>
        public int pageIndex = 1;

        /// <summary>
        /// 每页数据总数
        /// </summary>
        public int pageSize = 30;

        /// <summary>
        /// 总数据记录
        /// </summary>
        public int totalCount = 0;

        /// <summary>
        /// 共几页
        /// </summary>
        public int totalPage
        {
            get
            {
                if (totalCount > 0)
                {
                    int pages = totalCount / pageSize;
                    return totalCount % pageSize == 0 ? pages : pages + 1;
                }
                return 0;
            }
        }
 
        public TablePager()
        {
            InitializeComponent();
        }

        public void InitDataGrid(ITablePager _itablePager)
        {
            if (_itablePager == null)
                return;
            itablePager = _itablePager;

            dgrWrapper = itablePager.DataTable();
            if (dgrWrapper == null)
                return;

            dgrWrapper.Margin=new Thickness(5,5,5,0);
            dgrWrapper.AlternationCount=2;
            dgrWrapper.AutoGenerateColumns=false;
            dgrWrapper.CanUserAddRows=false;
            dgrWrapper.CanUserReorderColumns=false;
            dgrWrapper.CanUserResizeColumns=true;
            dgrWrapper.CanUserResizeRows=false;
            dgrWrapper.CanUserSortColumns=true;
            dgrWrapper.IsEnabled=true;
            dgrWrapper.IsReadOnly=true;
            dgrWrapper.RowHeaderWidth = 0;
            dgrWrapper.SelectionMode = DataGridSelectionMode.Single;
            tableWrapper.Children.Add(dgrWrapper);
            dgrWrapper.SetValue(Grid.RowProperty, 0);
            dgrWrapper.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Auto);
        }

        public void InitPageStatus()
        {
            pageIndex = 1;
            itablePager.DataTable().ItemsSource = itablePager.RefreshData();
            RefreshPageStatus();
        }

        public void ClearData()
        {
            this.dgrWrapper.ItemsSource = null;
            this.tbkCurrentPage.Text = "";
            this.tbkTotalPage.Text = "";
            this.tbxCurrentPage.Text = "";
            this.tbkTotalCount.Text = "";
        }

        private void RefreshPageStatus()
        {
            this.tbkCurrentPage.Text = String.Format("第{0}页", pageIndex);
            this.tbkTotalPage.Text = String.Format("共{0}页", totalPage);
            this.tbxCurrentPage.Text = String.Format("{0}", pageIndex);
            this.tbkTotalCount.Text = String.Format("共{0}条", totalCount);
        }

        private void firstPage_MouseDown(object sender, MouseButtonEventArgs e)
        {//第一页
            if (pageIndex != 1)
            {
                pageIndex = 1;
                itablePager.DataTable().ItemsSource = itablePager.RefreshData();
                RefreshPageStatus();
            }
        }

        private void previousPage_MouseDown(object sender, MouseButtonEventArgs e)
        {//上一页
            if (pageIndex > 1)
            {
                pageIndex--;
                itablePager.DataTable().ItemsSource = itablePager.RefreshData();
                RefreshPageStatus();
            }
        }

        private void nextPage_MouseDown(object sender, MouseButtonEventArgs e)
        {//下一页
            if (pageIndex < totalPage)
            {
                pageIndex++;
                this.dgrWrapper.ItemsSource = itablePager.RefreshData();
                RefreshPageStatus();
            }
        }

        private void finalPage_MouseDown(object sender, MouseButtonEventArgs e)
        {//最后一页
            if (pageIndex != totalPage)
            {
                pageIndex = totalPage;
                this.dgrWrapper.ItemsSource = itablePager.RefreshData();
                RefreshPageStatus();
            }
        }

        private void tbxCurrentPage_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {//限制仅能输入整数
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }

        private void tbxCurrentPage_KeyDown(object sender, KeyEventArgs e)
        {//回车跳转
            if (e.Key == Key.Enter)
            {
                if (this.tbxCurrentPage.Text.Replace(" ", "") == "" || int.Parse(this.tbxCurrentPage.Text) <= 0 || int.Parse(this.tbxCurrentPage.Text) > totalPage)
                {
                    pageIndex = 1;
                    this.dgrWrapper.ItemsSource = itablePager.RefreshData();
                    RefreshPageStatus();
                }

                pageIndex = int.Parse(this.tbxCurrentPage.Text);
                this.dgrWrapper.ItemsSource = itablePager.RefreshData();
                RefreshPageStatus();
            }
        }
    }
}

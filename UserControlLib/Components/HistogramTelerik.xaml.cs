using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls.ChartView;
using UserControlLib.Modes;

namespace UserControlLib.Components
{
    public delegate void ChartSelectEvent(object sender, ChartSelectionChangedEventArgs e);
    /// <summary>
    /// HistogramTelerik.xaml 的交互逻辑
    /// 直方图控件
    /// </summary>
    public partial class HistogramTelerik : UserControl
    {
        /// <summary>
        /// 图表名称
        /// </summary>
        public string ChartTitle { get;set; }

        public List<Legend> Legends { get;set; }
        
        /// <summary>
        /// 数据绑定容器
        /// </summary>
        public object DataWrapper { get; set; }
        
        public HistogramTelerik()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void Init()
        {
            if (DataWrapper == null) return;

            if(DataWrapper is DataTable)
            {
                DataTable dt=DataWrapper as DataTable;
                barSeriesError.DataContext = dt.Rows;
                barSeriesCount.DataContext = dt.Rows;

                this.barSeriesError.ValueBinding = new GenericDataPointBinding<DataRow, int>()
                {
                    ValueSelector = row => (int)row["Error"]
                };
                this.barSeriesError.CategoryBinding = new GenericDataPointBinding<DataRow, string>()
                {
                    ValueSelector = row => row["Title"].ToString()
                };
                this.barSeriesCount.ValueBinding = new GenericDataPointBinding<DataRow, int>()
                {
                    ValueSelector = row => (int)row["Count"]
                };
                this.barSeriesCount.CategoryBinding = new GenericDataPointBinding<DataRow, string>()
                {
                    ValueSelector = row => row["Title"].ToString()
                };
            }
        }

        /// <summary>
        /// 设置图表样式
        /// </summary>
        /// <param name="key"></param>
        public void SetPalette(string key)
        {
            chartView.Palette= Resources[key] as ChartPalette;
        }

        /// <summary>
        /// 图表选中事件
        /// </summary>
        public event ChartSelectEvent Chart_Selected;

        /// <summary>
        /// 选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartSelectionBehavior_SelectionChanged(object sender, ChartSelectionChangedEventArgs e)
        {
            if(Chart_Selected != null)
            {
                Chart_Selected(sender, e);
            }
        }
    }
}

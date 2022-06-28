using System.Collections.Generic;
using System.Data;
using System.Windows.Controls;
using Telerik.Windows.Controls.ChartView;
using UserControlLib.Modes;

namespace UserControlLib.Components
{
    /// <summary>
    /// HistogramSingle.xaml 的交互逻辑
    /// </summary>
    public partial class HistogramSingle : UserControl
    {
        /// <summary>
        /// 图表名称
        /// </summary>
        public string ChartTitle { get; set; }

        public List<Legend> Legends { get; set; }

        /// <summary>
        /// 数据绑定容器
        /// </summary>
        public object DataWrapper { get; set; }
        public HistogramSingle()
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

            if (DataWrapper is DataTable)
            {
                DataTable dt = DataWrapper as DataTable;
                barSeriesCount.DataContext = dt.Rows;
                this.barSeriesCount.ValueBinding = new GenericDataPointBinding<DataRow, int>()
                {
                    ValueSelector = row => (int)row["Count"]
                };
                this.barSeriesCount.CategoryBinding = new GenericDataPointBinding<DataRow, string>()
                {
                    ValueSelector = row => row["Title"].ToString()
                };
            }
            this.DataContext = null;
            this.DataContext=this;
        }

        /// <summary>
        /// 设置图表样式
        /// </summary>
        /// <param name="key"></param>
        public void SetPalette(string key)
        {
            chartView.Palette = Resources[key] as ChartPalette;
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
            if (Chart_Selected != null)
            {
                Chart_Selected(sender, e);
            }
        }

        /// <summary>
        /// 设置坐标轴最大值
        /// </summary>
        /// <param name="max"></param>
        public void SetAxisMax(int max)
        {
            if (max < 0) return;
            linearAxis.Maximum = max;
        }
    }
}

using System.Data;
using System.Windows.Controls;
using Telerik.Windows.Controls.ChartView;

namespace UserControlLib.Components
{
    /// <summary>
    /// CirqueTelerik.xaml 的交互逻辑
    /// 扇环组件
    /// </summary>
    public partial class CirqueTelerik : UserControl
    {
        /// <summary>
        /// 标段名称
        /// </summary>
        public string BidTitle { get; set; }

        /// <summary>
        /// 设备总数
        /// </summary>
        public int BidCount { get; set; }

        /// <summary>
        /// 数据绑定容器
        /// </summary>
        public object DataWrapper { get; set; }

        public CirqueTelerik()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public void Init()
        {
            if (DataWrapper == null) return;

            if (DataWrapper is DataTable)
            {
                DataTable dt = DataWrapper as DataTable;
                runStateSeries.DataContext = dt.Rows;

                runStateSeries.ValueBinding = new GenericDataPointBinding<DataRow, int>()
                {
                    ValueSelector = row => (int)row["Count"],
                };
            }
        }

        public void SetPalette(string key)
        {
            runState.Palette = Resources[key] as ChartPalette;
        }
    }
}

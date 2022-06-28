using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using CR_UtilityWPF.Dialog;

namespace UserControlLib.Components
{
    /// <summary>
    /// PictureWrapper.xaml 的交互逻辑
    /// </summary>
    public partial class PictureWrapper : BaseWindow
    {
        private static PictureWrapper instance;
        public static PictureWrapper Instance
        {
            get
            {
                if(instance == null)
                    instance = new PictureWrapper();
                return instance;
            }
        }
        public PictureWrapper()
        {
            InitializeComponent();
            Init(mainBorder, mainGrid, titleGrid);
            SetTitle("图片浏览", HorizontalAlignment.Left);
            SetScript("图片浏览");
            Closing += PictureWrapper_Closing;
        }

        private void PictureWrapper_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            e.Cancel = true;
        }

        public void UpdatePic(string src)
        {
            if (String.IsNullOrEmpty(src)) return;
            picSrc.Source = new BitmapImage(new Uri(src));
            if(this.Visibility==Visibility.Collapsed)
                this.Visibility = Visibility.Visible;
        }
    }
}

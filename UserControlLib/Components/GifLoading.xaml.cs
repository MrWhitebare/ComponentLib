using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace UserControlLib.Components
{
    /// <summary>
    /// GifLoading.xaml 的交互逻辑
    /// Gif图片加载控件
    /// </summary>
    public partial class GifLoading : UserControl
    {
        private Storyboard board = null;
        public GifLoading()
        {
            InitializeComponent();
            ShowGifByAnimate("pack://siteoforigin:,,,/CRConfig/Resources/Images/chartloading.gif");
        }

        /// <summary>
        /// GIF动图
        /// </summary>
        /// <param name="filePath">文件路径</param>
        private void ShowGifByAnimate(string filePath)
        {
            this.Dispatcher.Invoke(() => { 
                List<BitmapFrame> frameList=new List<BitmapFrame>();

                GifBitmapDecoder decoder=new GifBitmapDecoder(new Uri(filePath,UriKind.RelativeOrAbsolute),BitmapCreateOptions.PreservePixelFormat,BitmapCacheOption.Default);

                if (decoder != null && decoder.Frames != null)
                {
                    frameList.AddRange(decoder.Frames);
                    ObjectAnimationUsingKeyFrames objKeyAnimate = new ObjectAnimationUsingKeyFrames();
                    objKeyAnimate.Duration = new Duration(TimeSpan.FromSeconds(1));
                    foreach (var item in frameList)
                    {
                        DiscreteObjectKeyFrame k1_img1 = new DiscreteObjectKeyFrame(item);
                        objKeyAnimate.KeyFrames.Add(k1_img1);
                    }
                    imgGifWrapper.Source = frameList[0];

                    board = new Storyboard();
                    board.RepeatBehavior = RepeatBehavior.Forever;
                    board.FillBehavior = FillBehavior.HoldEnd;
                    board.Children.Add(objKeyAnimate);
                    Storyboard.SetTarget(objKeyAnimate,imgGifWrapper);
                    Storyboard.SetTargetProperty(objKeyAnimate, new PropertyPath("(Image.Source)"));
                    board.Begin();
                }
            });
        }
    }
}

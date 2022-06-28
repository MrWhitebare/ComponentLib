# ComponentLib
WPF编写的用户控件仓库

### 1. 文件夹说明

├─Components 组件
├─Modes 实体类
├─Properties
└─Style WPF样式

### 2. 项目说明

该程序是一个WPF 的Dll控件程序，无法直接运行，但可以查看源码和逻辑。

#### 2.1 图片浏览器组件逻辑

```C#
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Input;

namespace UserControlLib.Components
{
    public delegate void PathOnClose(object sender, MouseButtonEventArgs e);
    /// <summary>
    /// PictureViewer.xaml 的交互逻辑
    /// 用于图片浏览查看
    /// </summary>
    public partial class PictureViewer : UserControl
    {
        /// <summary>
        /// 当前选中图片Index
        /// </summary>
        private int cur_Index;

        /// <summary>
        /// 当前资源列表Index
        /// </summary>
        private int sourceIndex;

        /// <summary>
        /// 资源上限
        /// </summary>
        private int nextLength;

        /// <summary>
        /// 图片资源地址
        /// </summary>
        private List<string> PicSrcList;

        public PictureViewer(List<string> picList)
        {
            InitializeComponent();
            RefreshControls(picList);
        }

        public void RefreshControls(List<string> picList)
        {
            PicSrcList = picList;
            InitPage();
        }

        private void InitPage()
        {
            if (PicSrcList!=null&&PicSrcList.Count > 0)
            {
                List<string> subList=new List<string>();
                leftMove.Visibility = Visibility.Visible;
                commonBorder.Visibility = Visibility.Visible;
                rightMove.Visibility = Visibility.Visible;

                if(PicSrcList.Count >= 5)
                {
                    nextLength = 4;
                    subList = PicSrcList.GetRange(sourceIndex, 5);
                }
                else
                {
                    nextLength=PicSrcList.Count-1;
                    subList = PicSrcList;
                }
                RenderPhoto(subList.ToArray(), 0);                
            }
            else
                return;
        }

        /// <summary>
        /// 渲染图片
        /// </summary>
        /// <param name="photoArray">图片资源列表</param>
        /// <param name="selectIndex">当前选中项</param>
        private void RenderPhoto(string[] photoArray, int selectIndex)
        {
            if (photoArray.Length <= 0) return;
            this.picWrapper.Children.Clear();
            int index = 0;
            foreach (string photo in photoArray)
            {
                Image image = new Image();
                image.Stretch = Stretch.Fill;
                image.Margin = new Thickness(3);
                image.Source = new BitmapImage(new Uri(photo));
                Border border = new Border();
                border.Background = Brushes.Transparent;
                border.BorderBrush = Brushes.Transparent;
                border.BorderThickness = new Thickness(2);
                border.Child = image;
                border.Cursor = Cursors.Hand;
                if (index == selectIndex)
                {
                    border.BorderBrush = Brushes.Red;
                    //this.photoWrapper.Source = image.Source;
                }
                this.picWrapper.Children.Add(border);
                border.SetValue(Grid.RowProperty, index);
                index++;
            }
        }

        private void leftMove_MouseDown(object sender, MouseButtonEventArgs e)
        {//左移
            if (cur_Index == 0 && sourceIndex > 0)
            {
                sourceIndex--;
                List<string> subList = PicSrcList.GetRange(sourceIndex, 5);
                RenderPhoto(subList.ToArray(), cur_Index);
                if (PictureWrapper.Instance.Visibility == Visibility.Visible)
                    PictureWrapper.Instance.UpdatePic(subList[cur_Index]);
            }
            else if (cur_Index <= 0) return;
            else
            {
                ((Border)this.picWrapper.Children[cur_Index]).BorderBrush = Brushes.Transparent;
                Border curBorder = this.picWrapper.Children[cur_Index - 1] as Border;
                curBorder.BorderBrush = Brushes.Red;
                Image curImage = curBorder.Child as Image;
                if (PictureWrapper.Instance.Visibility == Visibility.Visible)
                    PictureWrapper.Instance.UpdatePic(curImage.Source.ToString());
                cur_Index--;
            }
        }

        private void rightMove_MouseDown(object sender, MouseButtonEventArgs e)
        {//右移
            if(cur_Index >= nextLength)
            {
                if (sourceIndex + nextLength >= PicSrcList.Count - 1) return;
                sourceIndex++;
                List<string> subList = PicSrcList.GetRange(sourceIndex, 5);
                RenderPhoto(subList.ToArray(), cur_Index);
                if (PictureWrapper.Instance.Visibility == Visibility.Visible)
                    PictureWrapper.Instance.UpdatePic(subList[cur_Index]);
            }
            else
            {
                ((Border)this.picWrapper.Children[cur_Index]).BorderBrush = Brushes.Transparent;
                Border curBorder = this.picWrapper.Children[cur_Index+ 1] as Border;
                curBorder.BorderBrush = Brushes.Red;
                Image curImage = curBorder.Child as Image;
                if (PictureWrapper.Instance.Visibility == Visibility.Visible)
                    PictureWrapper.Instance.UpdatePic(curImage.Source.ToString());
                cur_Index++;
            }
        }

        private void picWrapper_MouseDown(object sender, MouseButtonEventArgs e)
        {//选中
            Image img = e.OriginalSource as Image;
            switch (e.ClickCount)
            {
                case 1:
                    if (picWrapper.Children.Count > 0 && img != null)
                    {
                        if(PictureWrapper.Instance.Visibility == Visibility.Visible)
                            PictureWrapper.Instance.UpdatePic(img.Source.ToString());
                        int preSelectI = FindSelected(picWrapper.Children);
                        int curSelectI = FindSelected(picWrapper.Children, img);
                        if (preSelectI > -1 && curSelectI > -1 && preSelectI != curSelectI)
                        {
                            ((Border)picWrapper.Children[preSelectI]).BorderBrush = Brushes.Transparent;
                            Border curBorder = picWrapper.Children[curSelectI] as Border;
                            curBorder.BorderBrush = Brushes.Red;
                            cur_Index = curSelectI;
                        }
                    }
                    break;
                case 2:
                    if(picWrapper.Children.Count > 0 &&img != null)
                    {
                        PictureWrapper.Instance.UpdatePic(img.Source.ToString());
                    }
                    break;
            }
           
        }

        /// <summary>
        /// 找出选中项
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private int FindSelected(UIElementCollection collection)
        {
            int i = 0;
            foreach (Border item in collection)
            {
                if (item.BorderBrush == Brushes.Red) return i;
                i++;
            }
            return -1;
        }

        /// <summary>
        /// 找出选中项
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        private int FindSelected(UIElementCollection collection, Image image)
        {
            int i = 0;
            foreach (Border item in collection)
            {
                Image tempImg = item.Child as Image;
                if (tempImg.Source.ToString() == image.Source.ToString()) return i;
                i++;
            }
            return -1;
        }

        /// <summary>
        /// 关闭控件
        /// </summary>
        public event PathOnClose OnClosing;

        /// <summary>
        /// 关闭控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pathClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(OnClosing != null)
            {
                if(PictureWrapper.Instance.Visibility == Visibility.Visible) PictureWrapper.Instance.Close();
                OnClosing(this, e);
            }
        }
    }
}
```

#### 2.2 图片浏览器页面组成

```xaml
<UserControl
    x:Class="UserControlLib.Components.PictureViewer"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    d:DesignHeight="450"
    d:DesignWidth="800"
    mc:Ignorable="d">
    <Grid Name="parent">
        <Grid.RowDefinitions>
            <RowDefinition Height="1*" />
            <RowDefinition Height="8*" />
            <RowDefinition Height="1*" />
        </Grid.RowDefinitions>

        <Path
            Name="pathClose"
            Grid.Row="0"
            Margin="0,0,10,0"
            HorizontalAlignment="Right"
            VerticalAlignment="Center"
            Cursor="Hand"
            Data="M0,0 L20,20 M20,0 0,20"
            Fill="#18EdF3"
            MouseDown="pathClose_MouseDown"
            Stroke="#18EdF3"
            StrokeThickness="4"
            ToolTip="关闭图片浏览器" />
        <Path
            Name="leftMove"
            Grid.Row="0"
            HorizontalAlignment="Center"
            VerticalAlignment="Center"
            Cursor="Hand"
            Data="M0,0 L0,20 L20,0 Z"
            Fill="#18EdF3"
            MouseDown="leftMove_MouseDown"
            Stroke="Black"
            ToolTip="上一张"
            Visibility="Hidden">
            <Path.RenderTransform>
                <RotateTransform Angle="45" CenterX="5" CenterY="5" />
            </Path.RenderTransform>
        </Path>


        <Border
            Name="commonBorder"
            Grid.Row="1"
            Margin="0,1,4,8"
            Background="Transparent"
            BorderBrush="Black"
            BorderThickness="1"
            Visibility="Hidden">
            <Grid Name="picWrapper" MouseDown="picWrapper_MouseDown">
                <Grid.RowDefinitions>
                    <RowDefinition Height="*" />
                    <RowDefinition Height="*" />
                    <RowDefinition Height="*" />
                    <RowDefinition Height="*" />
                    <RowDefinition Height="*" />
                </Grid.RowDefinitions>
            </Grid>
        </Border>


        <Path
            Name="rightMove"
            Grid.Row="2"
            HorizontalAlignment="Center"
            VerticalAlignment="Center"
            Cursor="Hand"
            Data="M0,0 L0,20 L20,0 Z"
            Fill="#18EdF3"
            MouseDown="rightMove_MouseDown"
            Stroke="Black"
            ToolTip="下一张"
            Visibility="Hidden">
            <Path.RenderTransform>
                <RotateTransform Angle="-135" CenterX="5" CenterY="5" />
            </Path.RenderTransform>
        </Path>

    </Grid>
</UserControl>
```

> 请给UP主^(*￣(oo)￣)^ 一键三连 [doge]

//*****************************************************************//
//  Copyright (C) HamGuy 2013 All rights reserved.
//
//  The information contained herein is confidential, proprietary
//  to HamGuy. Use of this information by anyone other than 
//  authorized employees of HamGuy is granted only under a 
//  written non-disclosure agreement, expressly prescribing the 
//  scope and manner of such use.
//
//****************************************************************//
//  MainPage Create by HamGuy at 2013/4/2 21:49:00
//  Version 1.0
//  wangrui15@gmail.com
//  http://www.hamguy.info
//****************************************************************//

using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace AwesomeMenuForWindowsPhone
{
    public partial class MainPage : PhoneApplicationPage
    {
        bool closeByTapItem = false;
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            InitPathMenu();
            //ContentPanel.MouseLeftButtonUp += ContentPanel_MouseLeftButtonUp;
        }

        void ContentPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Rect rc = new Rect(0, LayoutRoot.ActualHeight - TitlePanel.ActualHeight, ContentPanel.ActualWidth, ContentPanel.ActualHeight);
            Point pt = e.GetPosition(ContentPanel);
            var items = new List<AwesomMenuItem>();

            items.Add(new AwesomMenuItem("Images/icon-star.png", "Images/bg-menuitem.png"));
            items.Add(new AwesomMenuItem("Images/icon-star.png", "Images/bg-menuitem.png"));
            items.Add(new AwesomMenuItem("Images/icon-star.png", "Images/bg-menuitem.png"));
            items.Add(new AwesomMenuItem("Images/icon-star.png", "Images/bg-menuitem.png")); 
            
            var menu = new AwesomeMenu(rc, items, "Images/icon-plus.png", "Images/bg-addbutton.png", pt);
            menu.TapToDissmissItem = false;
            menu.Margin = ContentPanel.Margin;
            Grid.SetRow(menu, 1);
            //menu.Background = new SolidColorBrush(Colors.Cyan);
            //ContentPanel.Children.Add(menu); //不能加在这里面，否则会出现异常
            LayoutRoot.Children.Add(menu);
            menu.ActionExpened += () =>
            {
            };
            menu.ActionClosed += (item) =>
            {
                Dispatcher.BeginInvoke(delegate
                {
                    this.LayoutRoot.Children.Remove(menu);
                    if(menu!=null)
                    menu.Children.Clear();
                    menu = null;
                    ProcessItem(item);
                });
            };
        }

        private void ProcessItem(AwesomMenuItem item)
        {
            if (item != null)
            {
                
                if (item != null && !item.Tag.Equals(999))
                {
                    int index = Convert.ToInt32(item.Tag);
                    MessageBox.Show(string.Format("Item {0} Clicked!", index));
                    //switch (index)
                    //{
                    //    case 0:
                    //        MessageBox.Show(string.Format("Item {0} Clicked!", index));
                    //        break;
                    //    case 1:
                    //        MessageBox.Show(string.Format("Item {0} Clicked!", index));
                    //        break;
                    //    case 2:
                    //        break;
                    //}
                }
            }

        }

        void InitPathMenu()
        {
            Rect rc = new Rect { Width =480, Height = 600 };
            var items = new List<AwesomMenuItem>();

            items.Add(new AwesomMenuItem("Images/icon-star.png", "Images/bg-menuitem.png"));
            items.Add(new AwesomMenuItem("Images/icon-star.png", "Images/bg-menuitem.png"));
            items.Add(new AwesomMenuItem("Images/icon-star.png", "Images/bg-menuitem.png"));
            items.Add(new AwesomMenuItem("Images/icon-star.png", "Images/bg-menuitem.png"));
            //items.Add(new AwesomMenuItem("Images/icon-star.png", "Images/bg-menuitem.png"));
            //items.Add(new AwesomMenuItem("Images/icon-star.png", "Images/bg-menuitem.png"));
            //items.Add(new AwesomMenuItem("Images/icon-star.png", "Images/bg-menuitem.png"));

            //构造的时候可以设置指定方法也可以通过方法来设置，都可以
            var menu = new AwesomeMenu(rc, items, "Images/icon-plus.png", "Images/bg-addbutton.png", AwesomeMenuType.AwesomeMenuTypeUpAndRight);
            //var menu = new AwesomeMenu(rc, items, "Images/icon-plus.png", "Images/bg-addbutton.png", new Point(20, 30));
            //menu.Background = new SolidColorBrush(Colors.Cyan);
            //menu.SetType(AwesomeMenuType.AwesomeMenuTypeUpAndLeft);
            //menu.SetStartPoint(new Point(0, 150));

            menu.TapToDissmissItem = true;
            menu.AwesomeMenuRadianType = AwesomeMenuRadianType.AwesomeMenuRadianNone;
            menu.MenuItemSpacing = 0;
            ContentPanel.Children.Add(menu);
            menu.ActionClosed += (item) =>
            {
                Dispatcher.BeginInvoke(delegate
                {
                    ProcessItem(item);
                });
            };
        }
    }
}
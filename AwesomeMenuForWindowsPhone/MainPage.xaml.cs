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
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            InitPathMenu();
        }

        void InitPathMenu()
        {
            Rect rc = new Rect { Width = 400, Height = 400 };
            var items = new List<AwesomMenuItem>();

            items.Add(new AwesomMenuItem("Images/icon-star.png", "Images/bg-menuitem.png"));
            items.Add(new AwesomMenuItem("Images/icon-star.png", "Images/bg-menuitem.png"));
            items.Add(new AwesomMenuItem("Images/icon-star.png", "Images/bg-menuitem.png"));
            items.Add(new AwesomMenuItem("Images/icon-star.png", "Images/bg-menuitem.png"));
            items.Add(new AwesomMenuItem("Images/icon-star.png", "Images/bg-menuitem.png"));
            items.Add(new AwesomMenuItem("Images/icon-star.png", "Images/bg-menuitem.png"));
            items.Add(new AwesomMenuItem("Images/icon-star.png", "Images/bg-menuitem.png"));

            //构造的时候可以设置指定方法也可以通过方法来设置，都可以
            var menu = new AwesomeMenu(rc, items, "Images/icon-plus.png", "Images/bg-addbutton.png", AwesomeMenuType.AwesomeMenuTypeDownAndLeft);
                        
            menu.SetType(AwesomeMenuType.AwesomeMenuTypeUpAndLeft);
            //menu.SetStartPoint(new Point(150, 50));
            //menu.AwesomeMenuRadianType = AwesomeMenuRadianType.AwesomeMenuRadian180;
            //menu.MenuItemSpacing = 50;
            ContentPanel.Children.Add(menu);
        }
    }
}
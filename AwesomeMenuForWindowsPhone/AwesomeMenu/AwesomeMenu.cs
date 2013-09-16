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
//  AwesomeMenu Create by HamGuy at 2013/4/2 22:05:32
//  Version 1.0
//  wangrui15@gmail.com
//  http://www.hamguy.info
//****************************************************************//

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace AwesomeMenuForWindowsPhone
{
    public class AwesomeMenu : Grid
    {
        #region Private Fileds
        private List<AwesomMenuItem> MenuItems;
        private int _flag = 0;
        protected AwesomeMenuType Type
        {
            get { return _type; }
            set
            {
                _type = value;
            }
        }
        private Point _startPoint;
        private AwesomeMenuType _type = AwesomeMenuType.AwesomeMenuTypeDefault;
        private Point START_POINT = new Point(0, 0);
        private const double NEAR_RADIUS = 130d;
        private const double END_RADIUS = 140d;
        private const double FAR_RADIUS = 160d;
        private const double BETWEEN_RADIUS = 50d;
        private TimeSpan TIMEOFFSET = TimeSpan.FromSeconds(0.05);
        private DispatcherTimer _timer;
        private AwesomMenuItem _addButton;
        private string _addUri;
        private string _addUriHighlited;
        private bool _tapToDismissItem = true;
        private bool _isExpanding;
        private AwesomeMenuRadianType awesomeMenuRadianType = AwesomeMenuRadianType.AwesomeMenuRadian90;
        private double menuItemSpacing = 0.0;
        private bool _closedByTapMenu = false;
        /// <summary>
        /// Indicator whether dismiss the menuitem when tapped
        /// </summary>
        public bool TapToDissmissItem
        {
            get { return _tapToDismissItem; }
            set { _tapToDismissItem = value; }
        }
        #endregion

        #region public Attribute
        /// <summary>
        /// 控制面板展开与关闭
        /// </summary>
        public bool IsExpanding
        {
            get { return _isExpanding; }
            set
            {
                _isExpanding = value;
                SetExpanding(_isExpanding);
            }
        }

        /// <summary>
        /// 设置按钮以多少度展现
        /// </summary>
        public AwesomeMenuRadianType AwesomeMenuRadianType
        {
            get
            {
                return awesomeMenuRadianType;
            }
            set
            {
                awesomeMenuRadianType = value;
                ConversionMenuItemSpacing();
                SetType(Type);
            }
        }

        /// <summary>
        /// 每个Item的间距
        /// </summary>
        public double MenuItemSpacing
        {
            get
            {
                return menuItemSpacing;
            }
            set
            {
                menuItemSpacing = value;
                if (this.AwesomeMenuRadianType == AwesomeMenuForWindowsPhone.AwesomeMenuRadianType.AwesomeMenuRadianNone)
                {
                    ConversionMenuItemSpacing();
                    SetType(Type);
                }
            }
        }
        #endregion

        #region Public Methods
        public void SetType(AwesomeMenuType menuType = AwesomeMenuType.AwesomeMenuTypeDefault)
        {
            Type = menuType;
            int dx = 1;
            int dy = 1;
            bool isTwoDirections = true;
            if (MenuItems != null)
            {
                switch (Type)
                {
                    case AwesomeMenuType.AwesomeMenuTypeUpAndRight:
                        dx = 1;
                        dy = -1;
                        break;
                    case AwesomeMenuType.AwesomeMenuTypeUpAndLeft:
                        dx = -1;
                        dy = -1;
                        break;
                    case AwesomeMenuType.AwesomeMenuTypeDownAndRight:
                        dy = 1;
                        dx = 1;
                        break;
                    case AwesomeMenuType.AwesomeMenuTypeDownAndLeft:
                        dy = 1;
                        dx = -1;
                        break;
                    case AwesomeMenuType.AwesomeMenuTypeUp:
                        isTwoDirections = false;
                        dx = 0;
                        dy = -1;
                        break;
                    case AwesomeMenuType.AwesomeMenuTypeDown:
                        isTwoDirections = false;
                        dx = 0;
                        dy = 1;
                        break;
                    case AwesomeMenuType.AwesomeMenuTypeLeft:
                        isTwoDirections = false;
                        dx = -1;
                        dy = 0;
                        break;
                    case AwesomeMenuType.AwesomeMenuTypeRight:
                        isTwoDirections = false; dx = 1; dy = 0;
                        break;
                    default:
                        break;
                }

                int nCount = MenuItems.Count;
                for (int i = 0; i < nCount; i++)
                {
                    var item = MenuItems[i];
                    item.StratPoint = _startPoint;
                    if (isTwoDirections)
                    {
                        item.EndPoint = CalulateDynamaticPoint(i, END_RADIUS, nCount, dx, dy);
                        item.NearPoint = CalulateDynamaticPoint(i, NEAR_RADIUS, nCount, dx, dy);
                        item.FarPoint = CalulateDynamaticPoint(i, FAR_RADIUS, nCount, dx, dy);
                    }
                    else
                    {
                        var j = i + 1;
                        item.EndPoint = new Point(_startPoint.X + dx * j * BETWEEN_RADIUS, _startPoint.Y + dy * j * BETWEEN_RADIUS);
                        item.NearPoint = new Point(_startPoint.X + dx * j * (BETWEEN_RADIUS - 15), _startPoint.Y + dy * j * (BETWEEN_RADIUS - 15));
                        item.FarPoint = new Point(_startPoint.X + dx * j * (BETWEEN_RADIUS + 20), _startPoint.Y + dy * j * (BETWEEN_RADIUS + 20));
                    }
                    item.ItemTransfrom.TranslateX = item.StratPoint.X;
                    item.ItemTransfrom.TranslateY = item.StratPoint.Y;
                }

            }
        }

        public void SetStartPoint(Point pt)
        {
            this.Children.Clear();
            _startPoint = START_POINT = pt;
            InitAddButton();
            _addButton.ItemTransfrom.TranslateX = _startPoint.X;
            _addButton.ItemTransfrom.TranslateY = _startPoint.Y;
            //_addButton.ItemTransfrom.CenterX = _addButton.ItemTransfrom.CenterY = 0.5;
            InitMenuItem();
            SetType(Type);
        }
        #endregion

        #region Actions
        //public Action<AwesomeMenu, int> ActionDisMiss;
        public Action ActionExpened;
        public Action<AwesomMenuItem> ActionClosed;
        #endregion

        #region Structure
        public AwesomeMenu(Rect rect, List<AwesomMenuItem> menuItems, string addUri, string addUriHigtlighted, AwesomeMenuType menuType = AwesomeMenuType.AwesomeMenuTypeDefault)
        {
            this.Width = rect.Width;
            this.Height = rect.Height;
            this.MenuItems = menuItems;
            this._addUri = addUri;
            this._addUriHighlited = addUriHigtlighted;
            //this.MouseLeftButtonUp += Menu_MouseLeftButtonUp;
            this.Background = new SolidColorBrush(Colors.Transparent);
            this.Tap -= AwesomeMenu_Tap;
            this.Tap += AwesomeMenu_Tap;
            InitAddButton();
            InitMenuItem();
            //原来没有叫设置按钮角度属性
            //SetType(menuType);
            //现在有了更改一下代码
            Type = menuType;
            this.AwesomeMenuRadianType = AwesomeMenuForWindowsPhone.AwesomeMenuRadianType.AwesomeMenuRadian90;
        }

        public AwesomeMenu(Rect rect, List<AwesomMenuItem> menuItems, string addUri, string addUriHigtlighted, Point startPoint)
        {
            this.Width = rect.Width;
            this.Height = rect.Height;
            this.MenuItems = menuItems;
            this._addUri = addUri;
            this._addUriHighlited = addUriHigtlighted;
            this.Background = new SolidColorBrush(Colors.Transparent);
            this.Tap -= AwesomeMenu_Tap;
            this.Tap += AwesomeMenu_Tap;
            this.VerticalAlignment = VerticalAlignment.Top;
            this.HorizontalAlignment = HorizontalAlignment.Left;
            Type = InitTypeByStartPoint(rect,startPoint);
            _startPoint = START_POINT = startPoint;
            this.AwesomeMenuRadianType = AwesomeMenuForWindowsPhone.AwesomeMenuRadianType.AwesomeMenuRadian90;
            InitAddButton(true); //加个条件，设置对齐方式为左对齐
            InitMenuItem(true);
            SetType(Type);
            this.IsExpanding = true;
        }
        #endregion

        #region Private methods
        private void InitMenuItem(bool byPopint = false)
        {
            if (MenuItems != null || MenuItems.Count > 0)
            {
                int nCount = MenuItems.Count;
                for (int i = 0; i < nCount; i++)
                {
                    var item = MenuItems[i];
                    item.StratPoint = START_POINT;
                    item.EndPoint = new Point(0, 0);//CalulateInitPoint(i, END_RADIUS, nCount);
                    item.NearPoint = new Point(0, 0);//CalulateInitPoint(i, NEAR_RADIUS, nCount);
                    item.FarPoint = new Point(0, 0);//CalulateInitPoint(i, FAR_RADIUS, nCount);
                    item.Tag = i;
                    item.ClickMenuItem -= Item_ClickMenuItem;
                    item.ClickMenuItem += Item_ClickMenuItem;
                    if (byPopint)
                    {
                        item.VerticalAlignment = VerticalAlignment.Top;
                        item.HorizontalAlignment = HorizontalAlignment.Left;
                    }
                    this.Children.Add(item);
                }
            }
        }

        private void InitAddButton(bool byPopint = false)
        {
            _addButton = new AwesomMenuItem(_addUri, _addUriHighlited);
            _addButton.ItemTransfrom.TranslateX = _startPoint.X;
            _addButton.ItemTransfrom.TranslateY = _startPoint.Y;
            _addButton.Tag = 999;
            if (byPopint)
            {
                _addButton.VerticalAlignment = VerticalAlignment.Top;
                _addButton.HorizontalAlignment = HorizontalAlignment.Left;
            }
            Canvas.SetZIndex(_addButton, 10);
            _addButton.ClickMenuItem -= Item_ClickMenuItem;
            _addButton.ClickMenuItem += Item_ClickMenuItem;
            this.Children.Add(_addButton);
        }

        private void Expand()
        {
            if (_flag >= MenuItems.Count)
            {
                if (_timer.IsEnabled)
                {
                    _timer.Stop();
                    _timer = null;
                    return;
                }
            }

            int tag = _flag;
            if (tag >= MenuItems.Count || tag < 0)
                return;
            var item = MenuItems[tag];
            Duration duration = TimeSpan.FromSeconds(0.5);

            var sb = new Storyboard();
            sb.Duration = duration;

            var da = new DoubleAnimationUsingKeyFrames();
            da.Duration = duration;

            var keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = item.ItemTransfrom.Rotation;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.0);
            da.KeyFrames.Add(keyframe);

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = 90;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.15);
            da.KeyFrames.Add(keyframe);

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = 180;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.3);
            da.KeyFrames.Add(keyframe);

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = 0;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.5);
            da.KeyFrames.Add(keyframe);

            Storyboard.SetTarget(da, item.ItemTransfrom);
            Storyboard.SetTargetProperty(da, new PropertyPath(CompositeTransform.RotationProperty));
            sb.Children.Add(da);

            da = new DoubleAnimationUsingKeyFrames();
            da.Duration = duration;

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = item.StratPoint.X;
            keyframe.KeyTime = TimeSpan.FromSeconds(0);
            da.KeyFrames.Add(keyframe);

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = item.FarPoint.X;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.15);
            da.KeyFrames.Add(keyframe);

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = item.NearPoint.X;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.3);
            da.KeyFrames.Add(keyframe);

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = item.EndPoint.X;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.5);
            da.KeyFrames.Add(keyframe);


            Storyboard.SetTarget(da, item.ItemTransfrom);
            Storyboard.SetTargetProperty(da, new PropertyPath(CompositeTransform.TranslateXProperty));
            sb.Children.Add(da);

            da = new DoubleAnimationUsingKeyFrames();
            da.Duration = duration;

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = item.StratPoint.Y;
            keyframe.KeyTime = TimeSpan.FromSeconds(0);
            da.KeyFrames.Add(keyframe);

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = item.FarPoint.Y;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.15);
            da.KeyFrames.Add(keyframe);

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = item.NearPoint.Y;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.3);
            da.KeyFrames.Add(keyframe);

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = item.EndPoint.Y;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.5);
            da.KeyFrames.Add(keyframe);

            Storyboard.SetTarget(da, item.ItemTransfrom);
            Storyboard.SetTargetProperty(da, new PropertyPath(CompositeTransform.TranslateYProperty));
            sb.Children.Add(da);

            sb.Begin();
            sb.Completed += (o, a) =>
            {
                //System.Diagnostics.Debug.WriteLine(item.EndPoint.X)
                item.ItemTransfrom.TranslateX = item.EndPoint.X;
                item.ItemTransfrom.TranslateY = item.EndPoint.Y;
                item.ItemTransfrom.Rotation = 0;
                sb.Stop();
                sb.Children.Clear();
                da = null;
                sb = null;
                if (ActionExpened != null)
                    ActionExpened();
            };
            _flag++;
        }

        private void Close()
        {
            if (_flag < 0)
            {
                _timer.Stop();
                _timer = null;
                return;
            }

            int tag = _flag;
            AwesomMenuItem item = null;
            //if (tag >= MenuItems.Count)
            //{
            //    item = _addButton;
            //}
            //else
            //item = MenuItems[tag];

            if (tag - 1 < 0)
            {
                return;
            }
            item = MenuItems[tag - 1];

            Duration duration = TimeSpan.FromSeconds(0.5);

            var sb = new Storyboard();
            sb.Duration = duration;

            var da = new DoubleAnimationUsingKeyFrames();
            da.Duration = duration;

            var keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = 0;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.0);
            da.KeyFrames.Add(keyframe);

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = 180;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.25);
            da.KeyFrames.Add(keyframe);

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = 0;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.5);
            da.KeyFrames.Add(keyframe);

            Storyboard.SetTarget(da, item.ItemTransfrom);
            Storyboard.SetTargetProperty(da, new PropertyPath(CompositeTransform.RotationProperty));
            sb.Children.Add(da);

            da = new DoubleAnimationUsingKeyFrames();
            da.Duration = duration;

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = item.EndPoint.X;
            keyframe.KeyTime = TimeSpan.FromSeconds(0);
            da.KeyFrames.Add(keyframe);

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = item.FarPoint.X;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.25);
            da.KeyFrames.Add(keyframe);

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = item.StratPoint.X;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.5);
            da.KeyFrames.Add(keyframe);

            Storyboard.SetTarget(da, item.ItemTransfrom);
            Storyboard.SetTargetProperty(da, new PropertyPath(CompositeTransform.TranslateXProperty));
            sb.Children.Add(da);

            da = new DoubleAnimationUsingKeyFrames();
            da.Duration = duration;

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = item.EndPoint.Y;
            keyframe.KeyTime = TimeSpan.FromSeconds(0);
            da.KeyFrames.Add(keyframe);

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = item.FarPoint.Y;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.25);
            da.KeyFrames.Add(keyframe);

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = item.StratPoint.Y;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.5);
            da.KeyFrames.Add(keyframe);

            Storyboard.SetTarget(da, item.ItemTransfrom);
            Storyboard.SetTargetProperty(da, new PropertyPath(CompositeTransform.TranslateYProperty));
            sb.Children.Add(da);

            sb.Begin();
            sb.Completed += (o, a) =>
            {
                item.ItemTransfrom.TranslateX = item.StratPoint.X;
                item.ItemTransfrom.TranslateY = item.StratPoint.Y;
                item.ItemTransfrom.Rotation = 0;
                sb.Stop();
                sb.Children.Clear();
                da = null;
                sb = null;
                if (ActionClosed != null)
                    if (_closedByTapMenu == true)
                    {
                        _closedByTapMenu = false;
                        ActionClosed(null);
                    }
                    else
                        ActionClosed(_addButton);
            };
            _flag--;
        }

        private void SetExpanding(bool isExpanding)
        {
            _isExpanding = isExpanding;

            //rotate add button //弧度
            double angle = this.IsExpanding ? -Math.PI / 4 : 0.0d;
            //角度
            angle = angle * 180;
            var duration = TimeSpan.FromSeconds(0.5);
            var sb = new Storyboard();
            sb.Duration = duration;
            _addButton.ItemTransfrom.CenterX = _addButton.ItemTransfrom.CenterY = 0.5;
            var da = GetDoubleAnimation(duration, _addButton.ItemTransfrom.Rotation, angle);
            Storyboard.SetTarget(da, _addButton.ItemTransfrom);
            Storyboard.SetTargetProperty(da, new PropertyPath(CompositeTransform.RotationProperty));
            sb.Children.Add(da);
            sb.Begin();
            sb.Completed += (o, a) =>
            {
                _addButton.ItemTransfrom.Rotation = angle;
                sb.Stop();
                sb.Children.Clear();
                da = null;
                sb = null;
            };

            //Init and Start Timer
            if (_timer == null)
            {
                _flag = this.IsExpanding ? 0 : MenuItems.Count;
                _timer = new DispatcherTimer();
                _timer.Interval = TIMEOFFSET;
                _timer.Tick += (o, a) =>
                {
                    if (IsExpanding)
                    {
                        Expand();
                    }
                    else
                    {
                        Close();
                    }
                };
                _timer.Start();
            }
        }

        /// <summary>
        /// 计算角度
        /// </summary>
        private void ConversionMenuItemSpacing()
        {
            switch (this.AwesomeMenuRadianType)
            {
                case AwesomeMenuRadianType.AwesomeMenuRadian90:
                    {
                        this.menuItemSpacing = (90 / (MenuItems.Count - 1));
                    }
                    break;
                case AwesomeMenuRadianType.AwesomeMenuRadian180:
                    {
                        this.menuItemSpacing = (180 / (MenuItems.Count - 1));
                    }
                    break;
                case AwesomeMenuRadianType.AwesomeMenuRadian360:
                    {
                        this.menuItemSpacing = (360 / (MenuItems.Count - 1));
                    }
                    break;
                default:
                    if (this.MenuItemSpacing == 0.0 || double.IsNaN(this.MenuItemSpacing))
                    {
                        this.menuItemSpacing = (90 / (MenuItems.Count - 1));
                    }
                    break;
            }
        }

        private AwesomeMenuType InitTypeByStartPoint(Rect rc, Point pt)
        {
            Rect rcLU = new Rect (0, 0, END_RADIUS, END_RADIUS );
            Rect rcRU = new Rect(rc.Width-END_RADIUS, 0, END_RADIUS, END_RADIUS);
            Rect rcLD = new Rect(0, rc.Height - END_RADIUS, END_RADIUS, END_RADIUS);
            Rect rcRD = new Rect(rc.Width - END_RADIUS, rc.Height - END_RADIUS, END_RADIUS, END_RADIUS);
            
            Rect rcUp = new Rect(END_RADIUS, 0, rc.Width - 2 * END_RADIUS, END_RADIUS);
            Rect rcLeft = new Rect(0, END_RADIUS,  END_RADIUS,rc.Height-2*END_RADIUS);
            Rect rcCenter = new Rect(END_RADIUS, END_RADIUS, rc.Width - 2 * END_RADIUS, rc.Height - 2 * END_RADIUS);
            Rect rcRight = new Rect(rc.Width - END_RADIUS, END_RADIUS, END_RADIUS, rc.Height - 2 * END_RADIUS);
            Rect rcDown = new Rect(END_RADIUS, rc.Height-END_RADIUS, rc.Width - 2 * END_RADIUS,  END_RADIUS);

            if (rcLU.Contains(pt))
                return AwesomeMenuType.AwesomeMenuTypeDownAndRight;
            else if (rcRU.Contains(pt))
                return AwesomeMenuType.AwesomeMenuTypeDownAndLeft;
            else if (rcLD.Contains(pt))
                return AwesomeMenuType.AwesomeMenuTypeUpAndRight;
            else if (rcRD.Contains(pt))
                return AwesomeMenuType.AwesomeMenuTypeUpAndLeft;
            else if (rcUp.Contains(pt))
                return AwesomeMenuType.AwesomeMenuTypeDown;
            else if (rcLeft.Contains(pt))
                return AwesomeMenuType.AwesomeMenuTypeRight;
            else if (rcRight.Contains(pt))
                return AwesomeMenuType.AwesomeMenuTypeLeft;
            else if (rcDown.Contains(pt))
                return AwesomeMenuType.AwesomeMenuTypeUp;
            else
                return AwesomeMenuType.AwesomeMenuTypeUpAndRight;

        }

        #endregion

        #region Event Handler

        // Unexpanded the menu while expanded
        void AwesomeMenu_Tap(object sender, GestureEventArgs e)
        {
            // Confilct with the menuitem T_T
            //if (_isExpanding)
            //{
            //    _isExpanding = false;
            //}
            _closedByTapMenu = true;
            if (this.IsExpanding)
            {
                this.IsExpanding = false;
            }
        }

        private void Item_ClickMenuItem(AwesomMenuItem item)
        {
            if (item.Tag.Equals(_addButton.Tag))
            {
                this.IsExpanding = !this.IsExpanding;
                return;
            }

            if (TapToDissmissItem)
            {
                //blowup current button
                Point pt = new Point(item.ItemTransfrom.TranslateX, item.ItemTransfrom.TranslateY);
                var blowUpStory = GetBlowUpAnimation(item, pt);
                blowUpStory.Begin();
                blowUpStory.Completed += (o, a) =>
                {
                    item.ItemTransfrom.TranslateX = _startPoint.X;
                    item.ItemTransfrom.TranslateY = _startPoint.Y;
                    item.ItemTransfrom.ScaleX = 1;
                    item.ItemTransfrom.ScaleY = 1;
                    item.Opacity = 1;
                    blowUpStory.Stop();
                    blowUpStory.Children.Clear();
                    blowUpStory = null;
                };

                foreach (var otherItem in MenuItems)
                {
                    if (item != otherItem)
                    {
                        Point p = new Point(otherItem.ItemTransfrom.TranslateX, otherItem.ItemTransfrom.TranslateY);
                        var shrinkStory = GetShrinkAnimation(otherItem, p);
                        shrinkStory.Begin();
                        shrinkStory.Completed += (o, a) =>
                        {
                            otherItem.ItemTransfrom.TranslateX = _startPoint.X;
                            otherItem.ItemTransfrom.TranslateY = _startPoint.Y;
                            otherItem.ItemTransfrom.ScaleX = 1;
                            otherItem.ItemTransfrom.ScaleY = 1;
                            otherItem.Opacity = 1;
                            shrinkStory.Stop();
                            shrinkStory.Children.Clear();
                            shrinkStory = null;
                        };
                    }
                }
                _isExpanding = false;

                //收起做的动画
                //弧度
                double angle = this.IsExpanding ? -Math.PI / 4 : 0;
                //角度
                angle = angle * 180;
                Duration duration = TimeSpan.FromSeconds(0.5);
                _addButton.RenderTransformOrigin = new Point(0.5, 0.5);
                var da = GetDoubleAnimation(duration, _addButton.ItemTransfrom.Rotation, angle);
                var sb = new Storyboard();
                //sb.Duration = duration;
                Storyboard.SetTarget(da, _addButton.ItemTransfrom);
                Storyboard.SetTargetProperty(da, new PropertyPath(CompositeTransform.RotationProperty));
                sb.Children.Add(da);
                sb.Begin();
                sb.Completed += (o, a) =>
                {
                    sb.Stop();
                    _addButton.ItemTransfrom.Rotation = angle;
                    sb.Children.Clear();
                    sb = null;
                };
            }

            if (ActionClosed != null)
                ActionClosed(item);


            //if (ActionDisMiss != null)
            //    ActionDisMiss(this, Convert.ToInt32(item.Tag));

        }
        #endregion

        #region Helper Methods
        Point CalulateInitPoint(int index, double radius, int nCount)
        {
            return new Point(
                        _startPoint.X + radius * Math.Sin(index * Math.PI / 2 / (nCount - 1)),
                        _startPoint.Y - radius * Math.Cos(index * Math.PI / 2 / (nCount - 1))
                        );
        }

        Point CalulateDynamaticPoint(int index, double radius, int nCount, int dx, int dy)
        {
            //return new Point(
            //            _startPoint.X + dx * radius * Math.Sin(index * Math.PI / 2 / (nCount - 1)),
            //            _startPoint.Y + dy * radius * Math.Cos(index * Math.PI / 2 / (nCount - 1))
            //            );
            return new Point(
                        _startPoint.X + dx * radius * Math.Sin(index * Math.PI * MenuItemSpacing / 180),
                        _startPoint.Y + dy * radius * Math.Cos(index * Math.PI * MenuItemSpacing / 180)
                        );
        }

        Storyboard GetBlowUpAnimation(AwesomMenuItem item, Point pt)
        {
            Duration duration = TimeSpan.FromSeconds(0.3);
            var sb = new Storyboard();
            sb.Duration = duration;

            var keyFrams = new DoubleAnimationUsingKeyFrames();
            keyFrams.Duration = duration;

            var keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = pt.X;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.3);
            keyFrams.KeyFrames.Add(keyframe);

            Storyboard.SetTarget(keyFrams, item.ItemTransfrom);
            Storyboard.SetTargetProperty(keyFrams, new PropertyPath(CompositeTransform.TranslateXProperty));
            sb.Children.Add(keyFrams);

            keyFrams = new DoubleAnimationUsingKeyFrames();
            keyFrams.Duration = duration;

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = pt.Y;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.3);
            keyFrams.KeyFrames.Add(keyframe);

            Storyboard.SetTarget(keyFrams, item.ItemTransfrom);
            Storyboard.SetTargetProperty(keyFrams, new PropertyPath(CompositeTransform.TranslateYProperty));
            sb.Children.Add(keyFrams);


            var da = GetDoubleAnimation(duration, 3, 1);
            Storyboard.SetTarget(da, item.ItemTransfrom);
            Storyboard.SetTargetProperty(da, new PropertyPath(CompositeTransform.ScaleXProperty));
            sb.Children.Add(da);

            da = GetDoubleAnimation(duration, 3, 1);
            Storyboard.SetTarget(da, item.ItemTransfrom);
            Storyboard.SetTargetProperty(da, new PropertyPath(CompositeTransform.ScaleYProperty));
            sb.Children.Add(da);

            da = GetDoubleAnimation(duration, 1, 0);
            Storyboard.SetTarget(da, item);
            Storyboard.SetTargetProperty(da, new PropertyPath(UIElement.OpacityProperty));
            sb.Children.Add(da);

            return sb;
        }

        Storyboard GetShrinkAnimation(AwesomMenuItem item, Point pt)
        {
            Duration duration = TimeSpan.FromSeconds(0.3);
            var sb = new Storyboard();
            sb.Duration = duration;

            var keyFrams = new DoubleAnimationUsingKeyFrames();
            keyFrams.Duration = duration;

            var keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = pt.X;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.3);
            keyFrams.KeyFrames.Add(keyframe);

            Storyboard.SetTarget(keyFrams, item.ItemTransfrom);
            Storyboard.SetTargetProperty(keyFrams, new PropertyPath(CompositeTransform.TranslateXProperty));
            sb.Children.Add(keyFrams);

            keyFrams = new DoubleAnimationUsingKeyFrames();
            keyFrams.Duration = duration;

            keyframe = new EasingDoubleKeyFrame();
            keyframe.Value = pt.Y;
            keyframe.KeyTime = TimeSpan.FromSeconds(0.3);
            keyFrams.KeyFrames.Add(keyframe);

            Storyboard.SetTarget(keyFrams, item.ItemTransfrom);
            Storyboard.SetTargetProperty(keyFrams, new PropertyPath(CompositeTransform.TranslateYProperty));
            sb.Children.Add(keyFrams);


            var da = GetDoubleAnimation(duration, 0.01, 1);
            Storyboard.SetTarget(da, item.ItemTransfrom);
            Storyboard.SetTargetProperty(da, new PropertyPath(CompositeTransform.ScaleXProperty));
            sb.Children.Add(da);

            da = GetDoubleAnimation(duration, 0.01, 1);
            Storyboard.SetTarget(da, item.ItemTransfrom);
            Storyboard.SetTargetProperty(da, new PropertyPath(CompositeTransform.ScaleYProperty));
            sb.Children.Add(da);

            da = GetDoubleAnimation(duration, 1, 0);
            Storyboard.SetTarget(da, item);
            Storyboard.SetTargetProperty(da, new PropertyPath(UIElement.OpacityProperty));
            sb.Children.Add(da);

            return sb;
        }

        DoubleAnimation GetDoubleAnimation(Duration duration, double from, double to)
        {
            var da = new DoubleAnimation();
            da.Duration = duration;
            da.From = from;
            da.To = to;
            da.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut };
            return da;
        }
        #endregion
    }
}

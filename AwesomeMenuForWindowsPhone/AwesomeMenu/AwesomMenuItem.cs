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
//  AwesomMenuItem Create by HamGuy at 2013/4/2 21:50:43
//  Version 1.0
//  wangrui15@gmail.com
//  http://www.hamguy.info
//****************************************************************//

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AwesomeMenuForWindowsPhone
{
    public class AwesomMenuItem : Grid
    {
        #region Public Property
        public Point StratPoint { get; set; }
        public Point EndPoint { get; set; }
        public Point NearPoint { get; set; }
        public Point FarPoint { get; set; }
        public CompositeTransform ItemTransfrom { get; set; } 
        #endregion

        #region Private Fileds
        private Image _contentImage;
        private WriteableBitmap _bitmap;
        private string _backgroundUri;
        private string _contentImageUri;
        #endregion

        #region Clik Event Handle
        public delegate void TouchItemEnd(AwesomMenuItem item);
        public event TouchItemEnd Click; 
        #endregion

        public AwesomMenuItem(string imgUri, string backgrounUrl)
        {
            this._contentImageUri = imgUri;
            this._backgroundUri = backgrounUrl;
            IntiLayout(_contentImageUri);
        }

        #region Private Methods
        private void IntiLayout(string strImageUri)
        {
            SetImage(strImageUri);
            this.Width = 50;
            this.Height = 50;
            this.Children.Add(_contentImage);
            _bitmap = new WriteableBitmap(0, 0).FromContent(_backgroundUri);
            this.Background = new ImageBrush { ImageSource = _bitmap };
            _bitmap = null;
            this.Tap += AwesomMenuItem_Tap;

            ItemTransfrom = new CompositeTransform();
            this.RenderTransform = ItemTransfrom;
            this.RenderTransformOrigin = new Point(0.5, 0.5);
        }

        void AwesomMenuItem_Tap(object sender, GestureEventArgs e)
        {
            //SetImage(_imageUri);
            if (Click != null)
                Click.Invoke(this);
        }

        private static Rect ScaleRect(Rect rect, float n)
        {
            return new Rect((rect.Width - rect.Width * n) / 2, (rect.Height - rect.Height * n) / 2, rect.Width * n, rect.Height * n);
        }

        private void SetImage(string imgUri)
        {
            if (_bitmap != null)
            {
                _bitmap = null;
            }

            _bitmap = new WriteableBitmap(0, 0).FromContent(imgUri);
            if (_contentImage != null)
                _contentImage.Source = null;
            else
                _contentImage = new Image();
            _contentImage.Stretch = Stretch.None;
            _contentImage.Source = _bitmap;
        }

        void Item_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Click != null)
                Click.Invoke(this);
        }
        #endregion

    }
}

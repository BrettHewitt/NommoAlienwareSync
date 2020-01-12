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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ViewLibrary.Controls
{
    /// <summary>
    /// Interaction logic for MonitorDragSelection.xaml
    /// </summary>
    public partial class MonitorDragSelection : UserControl
    {
        private SolidColorBrush ErrorBorderBrush = Brushes.Red;
        private SolidColorBrush NoErrorBorderBrush = Brushes.Gray;

        private int MonitorHeight
        { 
            get;
            set;
        }

        private int MonitorWidth
        {
            get;
            set;
        }

        private double AspectRatio
        {
            get;
            set;
        }

        private bool Drag
        {
            get;
            set;
        }

        private Point StartPoint
        {
            get;
            set;
        }

        private bool SkipValidation
        {
            get;
            set;
        }

        public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register("IsError", typeof(bool), typeof(MonitorDragSelection), new PropertyMetadata(false));
        public bool IsError
        {
            get
            {
                return (bool)GetValue(IsErrorProperty);
            }
            set
            {
                SetValue(IsErrorProperty, value);
            }
        }

        public static readonly DependencyProperty XMinProperty = DependencyProperty.Register("XMin", typeof(int), typeof(MonitorDragSelection), new PropertyMetadata(0, OnScaleChanged));
        public int XMin
        {
            get
            {
                return (int)GetValue(XMinProperty);
            }
            set
            {
                SetValue(XMinProperty, value);
            }
        }

        public static readonly DependencyProperty XMaxProperty = DependencyProperty.Register("XMax", typeof(int), typeof(MonitorDragSelection), new PropertyMetadata(0, OnScaleChanged));
        public int XMax
        {
            get
            {
                return (int)GetValue(XMaxProperty);
            }
            set
            {
                SetValue(XMaxProperty, value);
            }
        }

        public static readonly DependencyProperty YMinProperty = DependencyProperty.Register("YMin", typeof(int), typeof(MonitorDragSelection), new PropertyMetadata(0, OnScaleChanged));
        public int YMin
        {
            get
            {
                return (int)GetValue(YMinProperty);
            }
            set
            {
                SetValue(YMinProperty, value);
            }
        }

        public static readonly DependencyProperty YMaxProperty = DependencyProperty.Register("YMax", typeof(int), typeof(MonitorDragSelection), new PropertyMetadata(0, OnScaleChanged));
        public int YMax
        {
            get
            {
                return (int)GetValue(YMaxProperty);
            }
            set
            {
                SetValue(YMaxProperty, value);
            }
        }

        public static readonly DependencyProperty MonitorRectProperty = DependencyProperty.Register("MonitorRect", typeof(Rect), typeof(MonitorDragSelection), new PropertyMetadata(Rect.Empty, OnRectChanged));
        public Rect MonitorRect
        {
            get
            {
                return (Rect)GetValue(MonitorRectProperty);
            }
            set
            {
                SetValue(MonitorRectProperty, value);
            }
        }

        public MonitorDragSelection()
        {
            InitializeComponent();
        }

        private static void OnScaleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MonitorDragSelection monitor = sender as MonitorDragSelection;
            if (monitor != null)
            {
                monitor.OnScaleChanged(e);
            }
        }

        private void OnScaleChanged(DependencyPropertyChangedEventArgs e)
        {
            MonitorWidth = XMax - XMin;
            MonitorHeight = YMax - YMin;
            AspectRatio = (double)MonitorWidth / MonitorHeight;
            HorizontalSizeText.Text = MonitorWidth.ToString();
            VerticalSizeText.Text = MonitorHeight.ToString();
        }

        private void OnSizeChanged()
        {
            Height = ActualWidth / AspectRatio;
            UpdateSizeArrows();
            UpdateRectangleFromCoords();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            OnSizeChanged();
        }

        private void UpdateSizeArrows()
        {
            double verticalHeight = TopRow.ActualHeight;
            double verticalXPos = VerticalSizeText.ActualHeight/2;
            VerticalSizePath.Data = Geometry.Parse($"M {verticalXPos},0 L {verticalXPos},{verticalHeight} M {verticalXPos - 5},5 L {verticalXPos},0 L {verticalXPos + 5},5 M {verticalXPos - 5},{verticalHeight - 5} L {verticalXPos},{verticalHeight} L {verticalXPos + 5},{verticalHeight - 5}");

            double horiztonalWidth = RightColumn.ActualWidth;
            double horizontalYPos = HorizontalSizeText.ActualHeight / 2;
            HorizontalSizePath.Data = Geometry.Parse($"M 0,{horizontalYPos} L {horiztonalWidth},{horizontalYPos} M 5,{horizontalYPos - 5} L 0,{horizontalYPos} L 5,{horizontalYPos + 5} M {horiztonalWidth - 5},{horizontalYPos - 5} L {horiztonalWidth},{horizontalYPos} L {horiztonalWidth - 5},{horizontalYPos + 5}");
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Drag)
            {
                return;
            }

            Point dragPoint = Mouse.GetPosition(MonitorCanvas);

            double width = dragPoint.X - StartPoint.X;
            double height = dragPoint.Y - StartPoint.Y;
            double x = StartPoint.X;
            double y = StartPoint.Y;

            if (width < 0)
            {
                width = Math.Abs(width);
                x = StartPoint.X - width;
            }

            if (height < 0)
            {
                height = Math.Abs(height);
                y = StartPoint.Y - height;
            }

            MonitorRectangle.Width = width;
            MonitorRectangle.Height = height;
            Canvas.SetLeft(MonitorRectangle, x);
            Canvas.SetTop(MonitorRectangle, y);

            UpdateRect();            
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            StartPoint = Mouse.GetPosition(MonitorCanvas);
            Drag = true;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Drag = false;
        }

        private static void OnRectChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MonitorDragSelection monitor = sender as MonitorDragSelection;
            if (monitor != null)
            {
                monitor.OnRectChanged(e);
            }
        }

        private void OnRectChanged(DependencyPropertyChangedEventArgs e)
        {
            UpdateRectangleFromCoords();
        }

        private void UpdateTextBoxes()
        {
            XPosTb.Text = ((int)MonitorRect.X).ToString();
            YPosTb.Text = ((int)MonitorRect.Y).ToString();
            WidthTb.Text = ((int)MonitorRect.Width).ToString();
            HeightTb.Text = ((int)MonitorRect.Height).ToString();
        }

        private void UpdateRect()
        {
            double width = MonitorCanvas.ActualWidth;
            double height = MonitorCanvas.ActualHeight;

            double deltaX = XMax - XMin;
            double deltaY = YMax - YMin;

            double xRatio = deltaX / width;
            double yRatio = deltaY / height;

            double xPos = Canvas.GetLeft(MonitorRectangle) * xRatio;
            double yPos = Canvas.GetTop(MonitorRectangle) * yRatio;
            double rectWidth = MonitorRectangle.Width * xRatio;
            double rectHeight = MonitorRectangle.Height * yRatio;

            MonitorRect = new Rect(xPos, yPos, rectWidth, rectHeight);
            UpdateTextBoxes();
        }

        private void UpdateRectangleFromCoords()
        {
            if (MonitorRect.IsEmpty)
            {
                MonitorRectangle.Width = 0;
                MonitorRectangle.Height = 0;
                Canvas.SetLeft(MonitorRectangle, 0);
                Canvas.SetTop(MonitorRectangle, 0);
                UpdateTextBoxes();
                return;            
            }

            double width = MonitorCanvas.ActualWidth;
            double height = MonitorCanvas.ActualHeight;

            double deltaX = XMax - XMin;
            double deltaY = YMax - YMin;

            double xRatio = deltaX / width;
            double yRatio = deltaY / height;

            double xPos = MonitorRect.X / xRatio;
            double yPos = MonitorRect.Y / yRatio;
            double rectWidth = MonitorRect.Width / xRatio;
            double rectHeight = MonitorRect.Height / yRatio;

            MonitorRectangle.Width = rectWidth;
            MonitorRectangle.Height = rectHeight;
            Canvas.SetLeft(MonitorRectangle, xPos);
            Canvas.SetTop(MonitorRectangle, yPos);
            SkipValidation = true;
            UpdateTextBoxes();
            SkipValidation = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MonitorRect = new Rect(XMin, YMin, XMax, YMax);
        }

        private void RectTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb == null)
            {
                return;
            }

            bool isError = false;
            int xPos, yPos, width, height;
            if (!int.TryParse(XPosTb.Text, out xPos) || xPos > XMax)
            {
                SetTextBoxError(XPosTb);
                isError = true;
            }
            else
            {
                ClearTextBoxError(XPosTb);
            }

            if (!int.TryParse(YPosTb.Text, out yPos) || yPos > YMax)
            {
                SetTextBoxError(YPosTb);
                isError = true;
            }
            else
            {
                ClearTextBoxError(YPosTb);
            }

            if (!int.TryParse(WidthTb.Text, out width) || width + xPos > XMax)
            {
                SetTextBoxError(WidthTb);
                isError = true;
            }
            else
            {
                ClearTextBoxError(WidthTb);
            }

            if (!int.TryParse(HeightTb.Text, out height) || height + yPos > YMax)
            {
                SetTextBoxError(HeightTb);
                isError = true;
            }
            else
            {
                ClearTextBoxError(HeightTb);
            }

            IsError = isError;
            if (IsError)
            {
                return;
            }

            if (SkipValidation)
            {
                return;
            }

            MonitorRect = new Rect(xPos, yPos, width, height);
        }

        private void SetTextBoxError(TextBox textBox)
        {
            textBox.BorderBrush = ErrorBorderBrush;
        }

        private void ClearTextBoxError(TextBox textBox)
        {
            textBox.BorderBrush = NoErrorBorderBrush;
        }
    }
}

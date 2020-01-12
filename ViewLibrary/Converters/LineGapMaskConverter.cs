using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ViewLibrary.Converters
{
    public class LineGapMaskConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Type doubleType = typeof(double);

            if (parameter == null ||
                values == null ||
                values.Length != 3 ||
                values[0] == null ||
                values[1] == null ||
                values[2] == null ||
                !doubleType.IsAssignableFrom(values[0].GetType()) ||
                !doubleType.IsAssignableFrom(values[1].GetType()) ||
                !doubleType.IsAssignableFrom(values[2].GetType()))
            {
                return DependencyProperty.UnsetValue;
            }

            Type paramType = parameter.GetType();
            if (!typeof(string).IsAssignableFrom(paramType))
            {
                return DependencyProperty.UnsetValue;
            }

            //
            // Conversion
            //

            double headerWidth = (double)values[0];
            double headerHeight = (double)values[1];
            double lineSize = (double)values[2];

            // Doesn't make sense to have a Grid
            // with 0 as width or height
            if (lineSize == 0 || headerWidth == 0 || headerHeight == 0)
            {
                return null;
            }

            // Width of the line to the left of the header
            // to be used to set the width of the first column of the Grid
            bool horizontal;
            if (parameter is string)
            {
                horizontal = ((string)parameter).ToLower() == "horizontal";
            }
            else
            {
                return null;
            }

            Grid grid = new Grid();
            Rectangle rect1 = new Rectangle();
            Rectangle rect3 = new Rectangle();
            rect1.Fill = Brushes.Black;
            rect3.Fill = Brushes.Black;
            if (horizontal)
            {
                grid.Width = lineSize;
                grid.Height = headerHeight;
                ColumnDefinition colDef1 = new ColumnDefinition();
                ColumnDefinition colDef2 = new ColumnDefinition();
                ColumnDefinition colDef3 = new ColumnDefinition();
                colDef1.Width = new GridLength(1, GridUnitType.Star);
                colDef2.Width = new GridLength(headerWidth);
                colDef3.Width = new GridLength(1, GridUnitType.Star);
                grid.ColumnDefinitions.Add(colDef1);
                grid.ColumnDefinitions.Add(colDef2);
                grid.ColumnDefinitions.Add(colDef3);
                Grid.SetColumn(rect1, 0);
                Grid.SetColumn(rect3, 2);
            }
            else
            {
                grid.Width = headerHeight;
                grid.Height = lineSize;
                RowDefinition rowDef1 = new RowDefinition();
                RowDefinition rowDef2 = new RowDefinition();
                RowDefinition rowDef3 = new RowDefinition();
                rowDef1.Height = new GridLength(1, GridUnitType.Star);
                rowDef2.Height = new GridLength(headerWidth);
                rowDef3.Height = new GridLength(1, GridUnitType.Star);
                grid.RowDefinitions.Add(rowDef1);
                grid.RowDefinitions.Add(rowDef2);
                grid.RowDefinitions.Add(rowDef3);
                Grid.SetRow(rect1, 0);
                Grid.SetRow(rect3, 2);
            }
                        
            grid.Children.Add(rect1);
            grid.Children.Add(rect3);

            return (new VisualBrush(grid));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

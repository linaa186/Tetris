using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows.Shapes;
using System.ComponentModel;
using static System.Formats.Asn1.AsnWriter;
using System.Windows.Data;

namespace Tetris;

public class Cube : INotifyPropertyChanged
{
    int cubePosX;
    public int CubePosX
    {
        get { return cubePosX; }
        set
        {
            cubePosX = value;
            OnPropertyChanged(nameof(CubePosX));
        }
    }

    int cubePosY;
    public int CubePosY
    {
        get { return cubePosY; }
        set
        {
            cubePosY = value;
            OnPropertyChanged(nameof(CubePosY));
        }
    }
    public string Type { get; set; }
    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
    public event PropertyChangedEventHandler PropertyChanged;
    Dictionary<string, SolidColorBrush> colors = new Dictionary<string, SolidColorBrush> 
    {
        { "rightL", Brushes.Red },
        { "leftL", Brushes.Yellow },
        { "rightZ", Brushes.HotPink },
        { "leftZ", Brushes.DarkViolet },
        { "1x4", Brushes.Orange },
        { "2x2", Brushes.Blue },
        { "T", Brushes.Green }
    };


    public Cube(string type)
    {
        Type = type;
    }

    public void CreateCube()
    {
        var cube = new System.Windows.Shapes.Rectangle
        {
            Width = 20,
            Height = 20
        };

        if(colors.TryGetValue(Type, out SolidColorBrush color))
        {
            cube.Fill = color;
        }
        BindPositionToCanvas(cube);
        mainWindow.blocks.Children.Add(cube);
    }

    void BindPositionToCanvas(System.Windows.Shapes.Rectangle rect)
    {
        var leftBinding = new Binding(nameof(CubePosX))
        {
            Source = this,
            Converter = new PositionToCanvasConverter(),
            Mode = BindingMode.TwoWay
        };
        rect.SetBinding(Canvas.LeftProperty, leftBinding);

        var bottomBinding = new Binding(nameof(CubePosY))
        {
            Source = this,
            Converter = new PositionToCanvasConverter(),
            Mode = BindingMode.TwoWay
        };
        rect.SetBinding(Canvas.BottomProperty, bottomBinding);
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

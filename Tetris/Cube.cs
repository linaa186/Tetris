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

        switch (Type)
        {
            case "rightL":
                cube.Fill = Brushes.Red;
                break;
            case "leftL":
                cube.Fill = Brushes.Yellow;
                break;
            case "rightZ":
                cube.Fill = Brushes.HotPink;
                break;
            case "leftZ":
                cube.Fill = Brushes.DarkViolet;
                break;
            case "1x4":
                cube.Fill = Brushes.Orange;
                break;
            case "2x2":
                cube.Fill = Brushes.Blue;
                break;
            case "T":
                cube.Fill = Brushes.Green;
                break;
            default:
                cube.Fill = Brushes.Black;
                break;
        }
        BindPositionToCanvas(cube, mainWindow.blocks);
        mainWindow.blocks.Children.Add(cube);
    }

    void BindPositionToCanvas(System.Windows.Shapes.Rectangle rect, Canvas canvas)
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

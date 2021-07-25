using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MapScale
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private double _scale = 1;

        public double Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                OnPropertyChanged();
            }
        }

        private readonly double _zoomStep = 0.1;

        private void Grid_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Matrix matrix = ((MatrixTransform)MapContainer.RenderTransform).Matrix;

            Point position = e.MouseDevice.GetPosition(MainContainer);

            double scale = 0.0;

            (double X, double Y) = (matrix.M11, matrix.M22);

            if (e.Delta > 0)
            {
                scale = 1.0 + _zoomStep;
            }
            else if (e.Delta < 0)
            {
                scale = 1 / (1 + _zoomStep);

                if (scale < _zoomStep)
                {
                    scale = _zoomStep;
                }
            }

            matrix.ScaleAt(scale, scale, position.X, position.Y);

            (double width, double height) = (MainContainer.ActualWidth - MapContainer.ActualWidth * matrix.M11,
                MainContainer.ActualHeight - MapContainer.ActualHeight * matrix.M22);

            matrix.OffsetX = matrix.M11 < 1
                ? width / 2
                : X switch
                {
                    < 1 => 0,
                    _ => matrix.OffsetX
                };

            matrix.OffsetY = matrix.M22 < 1
                ? height / 2
                : Y switch
                {
                    < 1 => 0,
                    _ => matrix.OffsetY
                };

            MapContainer.RenderTransform = new MatrixTransform(matrix);

            Scale = matrix.M11;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

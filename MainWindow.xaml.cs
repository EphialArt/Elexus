using System.Linq;
using System.Windows;
using System.Windows.Input;
using HelixToolkit.Wpf.SharpDX;

namespace Elexus
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetViewportBackground();
            View1.MouseDown3D += View1_MouseDown3DHandler;
        }

        private void SetViewportBackground()
        {
            var color = new System.Windows.Media.Color
            {
                A = 255,
                R = 30,
                G = 30,
                B = 30
            };
            View1.Background = new System.Windows.Media.SolidColorBrush(color);
        }

        private void View1_MouseDown3DHandler(object sender, MouseDown3DEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;
            viewModel?.OnMouseDown3DHandler(sender, e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Any initialization logic can be added here if needed
        }

        private void Viewport3DX_CameraChanged(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;
            viewModel?.UpdateGrid(viewModel.Camera.Position);
        }
    }
}

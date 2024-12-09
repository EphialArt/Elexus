using System.Windows;
using HelixToolkit.Wpf.SharpDX;

namespace Elexus
{
    public partial class MainWindow : Window
    {
        private GeometryModel3D selectedModel;

        public MainWindow()
        {
            InitializeComponent();
            view1.MouseDown3D += View1_MouseDown3DHandler;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            view1.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        private void View1_MouseDown3DHandler(object sender, RoutedEventArgs e)
        {
            if (e is MouseDown3DEventArgs args && args.HitTestResult != null)
            {
                // Clear existing effect
                if (selectedModel != null)
                {
                    selectedModel.PostEffects = null;
                }

                selectedModel = args.HitTestResult.ModelHit as GeometryModel3D;

                // Ensure only specific models get highlighted
                if (selectedModel != null && selectedModel.Name != "grid" && selectedModel.Name != "axisX" && selectedModel.Name != "axisY")
                {
                    selectedModel.PostEffects = "highlight";
                }
            }
        }
    }
}

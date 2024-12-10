using System;
using System.Windows;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;

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
            if (e is MouseDown3DEventArgs args && args.HitTestResult != null && args.HitTestResult.ModelHit is MeshGeometryModel3D)
            {
                // Check if the hit object is not part of the manipulator
                var hitModel = args.HitTestResult.ModelHit as GeometryModel3D;
                if (hitModel != null && hitModel.Name != "" && hitModel != manipulator.Target)
                {   Console.WriteLine(hitModel.Name);
                    // Clear existing effect
                    if (selectedModel != null)
                    {
                        selectedModel.PostEffects = null;
                    }

                    selectedModel = hitModel;
                    

                    // Ensure only specific models get highlighted
                    if (selectedModel.Name != "grid" && selectedModel.Name != "axisX" && selectedModel.Name != "axisY" && selectedModel.Name != "") 
                    {
                        selectedModel.PostEffects = "highlight";

                        var viewModel = DataContext as MainViewModel;
                        if (viewModel != null)
                        {
                            viewModel.Target = null;
                            viewModel.CenterOffset = selectedModel.Geometry.Bound.Center; // Must update this before updating target
                            viewModel.Target = selectedModel;
                        }
                    }
                }
            }
        }
        private void ToggleManipulatorHitTest(bool enable) { manipulator.IsHitTestVisible = enable; }




    }
}

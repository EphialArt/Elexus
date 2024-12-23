using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using HelixToolkit.Wpf.SharpDX;
using System.Windows.Input;
using SharpDX;
using System.Windows.Controls;

namespace Elexus
{
    public partial class MainWindow : Window
    {
        private GeometryModel3D selectedModel;
        private Popup popup;
        private ListView listView;
        public TransformManipulator3D Manipulator { get; private set; }
        public int Index { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Manipulator = manipulator;
            popup = AddMeshPopup;
            listView = objectview;
            view1.MouseDown3D += View1_MouseDown3DHandler;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            view1.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
        }

        public void View1_MouseDown3DHandler(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;

            if (e is MouseDown3DEventArgs args && args.HitTestResult != null && args.HitTestResult.ModelHit is MeshGeometryModel3D hitModel && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                // Check if the hit object is not part of the manipulator
                if (hitModel != null && hitModel.Name != "" && hitModel != manipulator.Target)
                {
                    // Clear existing effect
                    if (selectedModel != null)
                    {
                        selectedModel.PostEffects = null;
                    }
                    selectedModel = hitModel;
                    manipulator.IsRendering = true;
                    AddMeshPopup.IsOpen = false;
                    // Ensure only specific models get highlighted
                    if (selectedModel.Name != "grid" && selectedModel.Name != "axisX" && selectedModel.Name != "axisY" && selectedModel.Name != "")
                    {
                        selectedModel.PostEffects = "highlight";

                        if (viewModel != null)
                        {
                            viewModel.Target = null;
                            viewModel.CenterOffset = selectedModel.Geometry.Bound.Center; // Must update this before updating target
                            viewModel.Target = selectedModel;

                            // Set SelectedPart and log index
                            viewModel.SelectedPart = hitModel;

                            // Get and log the index of the selected part
                            int index = viewModel.PartsCollection.IndexOf(viewModel.SelectedPart);
                            Index = index;
                            objectview.SelectedItem = viewModel.PartsCollection[index];
                        }
                    }
                    else
                    {
                        selectedModel.PostEffects = null;
                        manipulator.IsRendering = false;
                    }
                }
            }
            else if (e is MouseDown3DEventArgs args2 && args2.HitTestResult == null && Mouse.LeftButton == MouseButtonState.Pressed )
            {
                AddMeshPopup.IsOpen = false;
                // Clear existing effect when clicking on empty space
                if (selectedModel != null)
                {
                    selectedModel.PostEffects = null;
                    manipulator.IsRendering = false;
                }

                if (viewModel != null)
                {
                    viewModel.Target = null;
                    viewModel.SelectedPart = null;
                    var mainWindow = (MainWindow)Application.Current.MainWindow;
                }
            }
            else
            {
                AddMeshPopup.IsOpen = false;
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (objectview.SelectedItem is MeshGeometryModel3D selectedMesh)
            {
                var viewModel = DataContext as MainViewModel;
                // Set the selected mesh in the 3D view
                selectedModel = selectedMesh;
                manipulator.IsRendering = true;

                // Apply highlight effect
                selectedModel.PostEffects = "highlight";

                // Update 3D view and manipulator
                viewModel.SelectedPart = selectedMesh;

                // Set the target, etc.
                viewModel.CenterOffset = selectedModel.Geometry.Bound.Center;
                viewModel.Target = selectedModel;
            }
        }


        private void ShowAddMeshPopup(object sender, MouseButtonEventArgs e) 
        { 
            if (e.RightButton == MouseButtonState.Pressed)
            { 
                System.Windows.Point mousePosition = e.GetPosition(this); 
                var viewModel = DataContext as MainViewModel; 
                viewModel?.ShowAddMeshPopupCommand.Execute(mousePosition); 
            } 
        }
    }
}

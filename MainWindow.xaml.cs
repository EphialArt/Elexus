using SharpDX;
using System.Windows;
using HelixToolkit.Wpf.SharpDX;
using HelixToolkit.Wpf.SharpDX.Cameras;
using HelixToolkit.Wpf.SharpDX.Extensions;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System;

namespace Elexus
{
    public partial class MainWindow : Window
    {
        private HelixToolkit.Wpf.SharpDX.GeometryModel3D selectedModel;
        private Popup popup;
        private ListView listView;
        public TransformManipulator3D Manipulator { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Manipulator = manipulator;
            popup = AddMeshPopup;
            listView = objectview;
            view1.MouseDown3D += View1_MouseDown3DHandler;

            this.Loaded += MainWindow_Loaded; // Add Loaded event handler
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Perform initialization tasks after components are loaded
        }

        public void View1_MouseDown3DHandler(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;

            if (e is MouseDown3DEventArgs args && args.HitTestResult?.ModelHit is MeshGeometryModel3D hitModel && Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (hitModel.Name != "" && hitModel != manipulator.Target)
                {
                    HighlightModel(viewModel, hitModel);

                    if (viewModel?.PartsCollection != null)
                    {
                        objectview.SelectedItem = viewModel.PartsCollection.FirstOrDefault(part =>
                            part is MeshGeometryModel3D meshPart &&
                            meshPart.Geometry.Equals(hitModel.Geometry) &&
                            meshPart.Material.Equals(hitModel.Material));
                    }
                }
            }
            else if (Mouse.LeftButton == MouseButtonState.Pressed) // Clicked empty space
            {
                ClearSelection(viewModel);
            }
        }

        private void HighlightModel(MainViewModel viewModel, MeshGeometryModel3D model)
        {
            if (selectedModel != null)
            {
                selectedModel.PostEffects = null;
            }

            selectedModel = model;
            selectedModel.PostEffects = "highlight";
            manipulator.IsRendering = true;

            if (viewModel != null)
            {
                viewModel.Target = null;
                viewModel.Target = selectedModel;
                viewModel.CenterOffset = selectedModel.Geometry.Bound.Center;
                viewModel.SelectedPart = selectedModel as MeshGeometryModel3D;
            }
        }

        private void ClearSelection(MainViewModel viewModel)
        {
            if (selectedModel != null)
            {
                selectedModel.PostEffects = null;
                selectedModel = null;
            }

            manipulator.IsRendering = false;

            if (viewModel != null)
            {
                viewModel.Target = null;
                viewModel.SelectedPart = null;
            }

            objectview.SelectedItem = null;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedMesh = objectview.SelectedItem as MeshGeometryModel3D;
            if (selectedMesh != null)
            {
                // Simulate click on the selected mesh
                SimulateClickOnMesh(selectedMesh);
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

        private void SimulateClickOnMesh(MeshGeometryModel3D mesh)
        {
            // Convert world coordinates to screen position
            var screenPosition = WorldToScreen(mesh.Bounds.Center);

            // Perform hit testing at the screen position
            var hitResult = HitTestAtScreenPosition(screenPosition);

            if (hitResult?.ModelHit is MeshGeometryModel3D hitModel)
            {
                View1_MouseDown3DHandler(this, new MouseDown3DEventArgs(
                    this,
                    hitResult,
                    screenPosition,
                    view1,
                    new MouseEventArgs(Mouse.PrimaryDevice, 0)
                ));
            }
        }

        private System.Windows.Point WorldToScreen(Vector3 point)
        {
            var viewport = view1;
            var camera = viewport.Camera as ProjectionCamera;

            if (camera == null)
            {
                throw new InvalidOperationException("Camera is not initialized.");
            }

            // Get the view and projection matrices from the camera using CameraExtensions
            var viewMatrix = camera.CreateViewMatrix();
            var projectionMatrix = camera.CreateProjectionMatrix((float)viewport.ActualWidth / (float)viewport.ActualHeight);

            // Transform the point to screen coordinates
            var worldPos = new Vector4(point, 1.0f);
            var viewPos = Vector4.Transform(worldPos, viewMatrix);
            var screenPos = Vector4.Transform(viewPos, projectionMatrix);

            if (screenPos.W != 0)
            {
                screenPos /= screenPos.W;
            }

            var x = (screenPos.X + 1) / 2 * viewport.ActualWidth;
            var y = (1 - screenPos.Y) / 2 * viewport.ActualHeight;

            return new System.Windows.Point(x, y);
        }

        private HelixToolkit.Wpf.SharpDX.HitTestResult HitTestAtScreenPosition(System.Windows.Point screenPosition)
        {
            // Use HelixToolkit hit testing
            var hitTestResult = view1.FindHits(screenPosition).FirstOrDefault();
            return hitTestResult;
        }
    }
}

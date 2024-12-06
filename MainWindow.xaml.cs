using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Elexus
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddPartButton_Click(object sender, RoutedEventArgs e)
        {
            // Create the cube's geometry
            var cube = new MeshGeometry3D();

            // Define cube vertices
            cube.Positions = new Point3DCollection
            {
                new Point3D(0, 0, 0), // Bottom face
                new Point3D(1, 0, 0),
                new Point3D(1, 1, 0),
                new Point3D(0, 1, 0),
                new Point3D(0, 0, 1), // Top face
                new Point3D(1, 0, 1),
                new Point3D(1, 1, 1),
                new Point3D(0, 1, 1)
            };

            // Define cube triangles (indices)
            cube.TriangleIndices = new Int32Collection
            {
                0, 1, 2, 0, 2, 3, // Bottom
                4, 5, 6, 4, 6, 7, // Top
                0, 4, 7, 0, 7, 3, // Left
                1, 5, 6, 1, 6, 2, // Right
                3, 7, 6, 3, 6, 2, // Front
                0, 4, 5, 0, 5, 1  // Back
            };

            // Create material for the cube
            var material = new MaterialGroup();
            material.Children.Add(new DiffuseMaterial(new SolidColorBrush(Colors.Gray)));
            material.Children.Add(new SpecularMaterial(new SolidColorBrush(Colors.White), 30));
            var cubeModel = new GeometryModel3D(cube, material);

            // Add the cube to the viewport
            var modelVisual = new ModelVisual3D { Content = cubeModel };
            HelixView.Children.Add(modelVisual);
        }

        private void SaveModelButton_Click(object sender, RoutedEventArgs e)
        {
            // Code to save the model (e.g., serialize to a file, etc.)
            MessageBox.Show("Model saved successfully.");
        }
    }
}
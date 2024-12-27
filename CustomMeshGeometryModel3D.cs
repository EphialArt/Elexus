using HelixToolkit.Wpf.SharpDX;
using System.Windows;

namespace Elexus
{
    public class CustomMeshGeometryModel3D : MeshGeometryModel3D
    {
        // Define the DependencyProperty for MeshName
        public static readonly DependencyProperty MeshNameProperty = DependencyProperty.Register(
            "MeshName", typeof(string), typeof(CustomMeshGeometryModel3D), new PropertyMetadata(string.Empty));

        // CLR property wrapper for MeshName
        public string MeshName
        {
            get { return (string)GetValue(MeshNameProperty); }
            set { SetValue(MeshNameProperty, value); }
        }
    }
}

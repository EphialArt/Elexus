using System.Windows;
using System.Windows.Controls;
using HelixToolkit.Wpf.SharpDX;
using System.Windows.Media.Media3D;
using Material = HelixToolkit.Wpf.SharpDX.Material;
using Geometry3D = HelixToolkit.Wpf.SharpDX.Geometry3D;

namespace Elexus
{
    public partial class MeshGeometryView : UserControl
    {
        public MeshGeometryView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty GeometryProperty =
            DependencyProperty.Register(nameof(Geometry), typeof(Geometry3D), typeof(MeshGeometryView), new PropertyMetadata(null));

        public Geometry3D Geometry
        {
            get { return (Geometry3D)GetValue(GeometryProperty); }
            set { SetValue(GeometryProperty, value); }
        }

        public static readonly DependencyProperty MaterialProperty =
            DependencyProperty.Register(nameof(Material), typeof(Material), typeof(MeshGeometryView), new PropertyMetadata(null));

        public Material Material
        {
            get { return (Material)GetValue(MaterialProperty); }
            set { SetValue(MaterialProperty, value); }
        }
    }
}

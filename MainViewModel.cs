using System.Linq;
using System.Windows.Input;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using MvvmHelpers;
using MeshGeometry3D = HelixToolkit.Wpf.SharpDX.MeshGeometry3D;
using PerspectiveCamera = HelixToolkit.Wpf.SharpDX.PerspectiveCamera;
using Point3D = System.Windows.Media.Media3D.Point3D;
using Vector3D = System.Windows.Media.Media3D.Vector3D;
using Color = System.Windows.Media.Color;
using Colors = System.Windows.Media.Colors;
using Media3D = System.Windows.Media.Media3D;
using MvvmHelpers.Commands;
using System.Windows.Media.Media3D; // Ensure this is included

namespace Elexus
{
    public class MainViewModel : BaseViewModel
    {
        public string Title { get; private set; }
        public string SubTitle { get; private set; }
        public MeshGeometry3D Model { get; private set; }
        public MeshGeometry3D SelectedModel { get; private set; }
        public LineGeometry3D Grid { get; private set; }
        public LineGeometry3D AxisX { get; private set; }
        public LineGeometry3D AxisY { get; private set; }
        public SharpDX.Color GridColor { get; private set; }
        public Color DirectionalLightColor { get; private set; }
        public Color AmbientLightColor { get; private set; }
        public BillboardText3D Text3D { get; set; } = new BillboardText3D { IsDynamic = true };
        public PerspectiveCamera Camera { get; private set; }
        public DefaultEffectsManager EffectsManager { get; private set; }
        public Media3D.Transform3D GridTransform { get; private set; }
        public Transform3D SelectedModelTransform { get; private set; }
        public ICommand AddPartCommand { get; private set; }

        public MainViewModel()
        {
            // Titles
            this.Title = "Elexus 3D CAD Application";
            this.SubTitle = "3D CAD application using HelixToolkit and SharpDX";

            // Camera setup
            this.Camera = new PerspectiveCamera
            {
                Position = new Point3D(4.4, 2.2, -4.4),
                LookDirection = new Vector3D(0, -4, 10),
                UpDirection = new Vector3D(0, 1, 0)
            };

            // Effects Manager
            this.EffectsManager = new DefaultEffectsManager();

            // Lighting setup
            this.AmbientLightColor = Colors.Black;
            this.DirectionalLightColor = Colors.White;

            // Floor plane grid
            this.Grid = GenerateGrid(this.Camera.Position);
            this.GridColor = SharpDX.Color.Gray;
            this.GridTransform = Media3D.Transform3D.Identity;

            // Axis lines
            this.AxisX = GenerateAxis(new Vector3(-50, (float)0.01, 0), new Vector3(50, (float)0.01, 0));
            this.AxisY = GenerateAxis(new Vector3(0, (float)0.01, -50), new Vector3(0, (float)0.01, 50));

            // Billboard text
            float scale = 0.8f;
            Text3D.TextInfo.Add(new TextInfo("Origin", new Vector3(0, 0, 0)) { Foreground = SharpDX.Color.Red, Scale = scale * 2 });

            // Command setup
            this.AddPartCommand = new Command(AddPart);
        }

        private LineGeometry3D GenerateGrid(Point3D cameraPosition)
        {
            var meshBuilder = new LineBuilder();

            int gridRange = 100; // Adjust this range as needed
            int minX = (int)(cameraPosition.X - gridRange);
            int maxX = (int)(cameraPosition.X + gridRange);
            int minZ = (int)(cameraPosition.Z - gridRange);
            int maxZ = (int)(cameraPosition.Z + gridRange);

            for (int i = minX; i <= maxX; i++)
            {
                meshBuilder.AddLine(new Vector3(i, 0, minZ), new Vector3(i, 0, maxZ));
            }
            for (int j = minZ; j <= maxZ; j++)
            {
                meshBuilder.AddLine(new Vector3(minX, 0, j), new Vector3(maxX, 0, j));
            }

            return meshBuilder.ToLineGeometry3D();
        }

        private LineGeometry3D GenerateAxis(Vector3 start, Vector3 end)
        {
            var meshBuilder = new LineBuilder();
            meshBuilder.AddLine(start, end);
            return meshBuilder.ToLineGeometry3D();
        }

        private void AddPart()
        {
            // Logic to add a part
            var meshBuilder = new MeshBuilder();
            meshBuilder.AddBox(new Vector3(0, 0, 0), 1, 1, 1);
            var newMesh = meshBuilder.ToMeshGeometry3D();
            newMesh.Colors = new Color4Collection(newMesh.TextureCoordinates.ToList().ConvertAll(x => x.ToColor4()));

            this.Model = newMesh;
            OnPropertyChanged(nameof(Model));
        }

        public void UpdateGrid(Point3D cameraPosition)
        {
            this.Grid = GenerateGrid(cameraPosition);
            OnPropertyChanged(nameof(Grid));
        }

        public void OnMouseDown3DHandler(object sender, MouseDown3DEventArgs e)
        {
            if (e.HitTestResult != null && e.HitTestResult.ModelHit is MeshGeometryModel3D m && m.Geometry == Model)
            {
                SelectedModel = Model; // Logic to highlight the model goes here
                OnPropertyChanged(nameof(SelectedModel));
            }
        }
    }
}

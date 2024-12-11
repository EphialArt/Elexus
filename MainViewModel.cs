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
using System.Windows.Media.Media3D;
using System.Collections.ObjectModel;
using GeometryModel3D = HelixToolkit.Wpf.SharpDX.GeometryModel3D;
using System.Windows.Threading;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using MessageBox = System.Windows.MessageBox;
using Application = System.Windows.Application;

namespace Elexus
{
    public class MainViewModel : BaseViewModel
    {
        public string Title { get; private set; }
        public string SubTitle { get; private set; }
        public MeshGeometry3D Model { get; private set; }
        public LineGeometry3D Grid { get; private set; }
        public LineGeometry3D AxisX { get; private set; }
        public LineGeometry3D AxisY { get; private set; }
        public SharpDX.Color GridColor { get; private set; }
        public Transform3D ModelTransform { get; set; }
        public Color DirectionalLightColor { get; private set; }
        public Color AmbientLightColor { get; private set; }
        public BillboardText3D Text3D { get; set; } = new BillboardText3D { IsDynamic = true };
        public PerspectiveCamera Camera { get; private set; }
        private List<MeshGeometryModel3D> clipboard = new List<MeshGeometryModel3D>();
        public DefaultEffectsManager EffectsManager { get; private set; }
        public ObservableCollection<Element3D> PartsCollection { get; private set; }

        public Media3D.Transform3D GridTransform { get; private set; }
        public ICommand AddPartCommand { get; private set; }
        public ICommand AddBoxCommand { get; private set; }
        public ICommand AddSphereCommand { get; private set; }
        public ICommand AddCylinderCommand { get; private set; }
        public ICommand CopyCommand { get; }
        public ICommand PasteCommand { get; }
        public ICommand DuplicateCommand { get; }
        public ICommand ShowAddMeshPopupCommand { get; private set; }
        public ICommand EnableTranslateCommand { get; private set; }
        public ICommand EnableScaleCommand { get; private set; }
        public ICommand EnableRotateCommand { get; private set; }
        public ICommand DeleteMeshCommand { get; private set; }

        private OutlineMode drawMode = OutlineMode.Merged;
        public OutlineMode DrawMode
        {
            get => drawMode;
            set => SetProperty(ref drawMode, value);
        }
        private Element3D target; private Vector3 centerOffset;
        public Element3D Target
        {
            get => target; set => SetProperty(ref target, value);
        }
        public Vector3 CenterOffset
        {
            get => centerOffset; set => SetProperty(ref centerOffset, value);
        }
        private MeshGeometryModel3D selectedPart;
        public MeshGeometryModel3D SelectedPart
        {
            get => selectedPart; set => SetProperty(ref selectedPart, value);
        }

        private bool highlightSeparated = false;
        public bool HighlightSeparated
        {
            get => highlightSeparated;
            set
            {
                if (SetProperty(ref highlightSeparated, value))
                {
                    DrawMode = value ? OutlineMode.Separated : OutlineMode.Merged;
                }
            }
        }
        private Brush background;
        public Brush Background
        {
            get => background;
            set
            {
                SetProperty(ref background, value);
            }
        }

        private bool isAddMeshPopupOpen;
        public bool IsAddMeshPopupOpen
        {
            get => isAddMeshPopupOpen;
            set => SetProperty(ref isAddMeshPopupOpen, value);
        }

        public MainViewModel()
        {
            // Titles
            this.Title = "Elexus 3D CAD Application";
            this.SubTitle = "3D CAD application using HelixToolkit and SharpDX";

            // Camera setup
            this.Camera = new PerspectiveCamera
            {
                Position = new Point3D(0, 0, -4.4),
                LookDirection = new Vector3D(0, -4, 10),
                UpDirection = new Vector3D(0, 1, 0)
            };

            // Parts Collection
            this.PartsCollection = new ObservableCollection<Element3D>();

            // Commands
            AddBoxCommand = new Command(AddBox);
            AddSphereCommand = new Command(AddSphere);
            AddCylinderCommand = new Command(AddCylinder);
            ShowAddMeshPopupCommand = new Command(ShowAddMeshPopup);

            // Effects Manager
            this.EffectsManager = new DefaultEffectsManager();

            // Transforms
            this.ModelTransform = new TranslateTransform3D(0, 0, 0);

            // Lighting setup
            this.AmbientLightColor = Colors.Black;
            this.DirectionalLightColor = Colors.White;

            // Floor plane grid
            this.Grid = GenerateGrid(this.Camera.Position);
            this.GridColor = SharpDX.Color.LightGray;
            this.GridTransform = Media3D.Transform3D.Identity;

            // Axis lines
            this.AxisX = GenerateAxis(new Vector3(-50, (float)0.01, 0), new Vector3(50, (float)0.01, 0));
            this.AxisY = GenerateAxis(new Vector3(0, (float)0.01, -50), new Vector3(0, (float)0.01, 50));

            // Billboard text
            float scale = 0.8f;
            Text3D.TextInfo.Add(new TextInfo("Origin", new Vector3(0, 0, 0)) { Foreground = SharpDX.Color.Red, Scale = scale * 2 });
            // Background
            Background = Brushes.LightBlue;

            // Command setup
            this.EnableTranslateCommand = new RelayCommand(_ => EnableTranslate());
            this.EnableScaleCommand = new RelayCommand(_ => EnableScale());
            this.EnableRotateCommand = new RelayCommand(_ => EnableRotate());
            this.DeleteMeshCommand = new RelayCommand(_ => DeleteSelectedMesh(), _ => SelectedPart != null);
            this.CopyCommand = new RelayCommand(_ => CopySelectedMesh());
            this.PasteCommand = new RelayCommand(_ => PasteCopiedMeshes());
            this.DuplicateCommand = new RelayCommand(_ => DuplicateSelectedMesh());
        }



        private LineGeometry3D GenerateGrid(Point3D cameraPosition)
        {
            var meshBuilder = new LineBuilder();

            int gridRange = 80; // Adjust this range as needed
            int minX = (int)(cameraPosition.X - gridRange);
            int maxX = (int)(cameraPosition.X + gridRange);
            int minZ = (int)(cameraPosition.Z - gridRange);
            int maxZ = (int)(cameraPosition.Z + gridRange);

            for (int i = minX; i <= maxX; i++)
            {
                meshBuilder.AddLine(new Vector3(i, 0, minZ), new Vector3(i, 0, maxZ));
            }
            for (int j = minZ; j <= maxX; j++)
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

        public void UpdateGrid(Point3D cameraPosition)
        {
            this.Grid = GenerateGrid(cameraPosition);
            OnPropertyChanged(nameof(Grid));
        }
        private void AddBox()
        {
            var meshBuilder = new MeshBuilder();
            meshBuilder.AddBox(new Vector3(0, 0, 0), 1, 1, 1);
            AddMeshToCollection(meshBuilder.ToMeshGeometry3D(), PhongMaterials.Chrome);
        }

        private void AddSphere()
        {
            var meshBuilder = new MeshBuilder();
            meshBuilder.AddSphere(new Vector3(0, 0, 0), 1);
            AddMeshToCollection(meshBuilder.ToMeshGeometry3D(), PhongMaterials.Chrome);
        }

        private void AddCylinder()
        {
            var meshBuilder = new MeshBuilder();
            meshBuilder.AddCylinder(new Vector3(0, 0, 0), new Vector3(0, 6, 0), 1, 20);
            AddMeshToCollection(meshBuilder.ToMeshGeometry3D(), PhongMaterials.Chrome);
        }

        private void AddMeshToCollection(MeshGeometry3D mesh, PhongMaterial material)
        {
            var newPart = new MeshGeometryModel3D
            {
                Geometry = mesh,
                Material = material,
                IsHitTestVisible = true
            };

            PartsCollection.Add(newPart);
            OnPropertyChanged(nameof(PartsCollection));
            IsAddMeshPopupOpen = false;
        }

        private void EnableTranslate()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            if (mainWindow != null && mainWindow.manipulator != null)
            {
                mainWindow.manipulator.EnableTranslation = true;
                mainWindow.manipulator.EnableRotation = false;
                mainWindow.manipulator.EnableScaling = false;
            }
        }
        private void EnableScale()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            if (mainWindow != null && mainWindow.manipulator != null)
            {
                mainWindow.manipulator.EnableTranslation = false;
                mainWindow.manipulator.EnableRotation = false;
                mainWindow.manipulator.EnableScaling = true;
            }
        }
        private void EnableRotate()
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            if (mainWindow != null && mainWindow.manipulator != null)
            {
                mainWindow.manipulator.EnableTranslation = false;
                mainWindow.manipulator.EnableRotation = true;
                mainWindow.manipulator.EnableScaling = false;
            }
        }

        private void ShowAddMeshPopup()
        {
            IsAddMeshPopupOpen = true;
        }
        private void DeleteSelectedMesh()
        {
            if (SelectedPart != null)
            {
                Element3D partToRemove = null;
                foreach (var part in PartsCollection)
                {
                    if (part is MeshGeometryModel3D meshPart && meshPart.Geometry.Equals(SelectedPart.Geometry) &&
                        meshPart.Material.Equals(SelectedPart.Material))
                    {
                        partToRemove = part;
                        break;
                    }
                }

                if (partToRemove != null)
                {
                    PartsCollection.Remove(partToRemove);

                    // Ensure it's removed from the scene graph
                    var mainWindow = (MainWindow)System.Windows.Application.Current.MainWindow;
                    if (mainWindow != null && mainWindow.view1.Items.Contains(partToRemove))
                    {
                        mainWindow.view1.Items.Remove(partToRemove);
                    }

                    // Clear the selection
                    SelectedPart = null;
                    mainWindow.Manipulator.IsRendering = false;
                }
                else
                {
                    MessageBox.Show("Part not found in PartsCollection.");
                }
            }
        }
        private void CopySelectedMesh()
        {
            if (SelectedPart != null)
            {
                clipboard.Clear();
                clipboard.Add(SelectedPart);
            }
        }
        private void PasteCopiedMeshes()
        {
            foreach (var mesh in clipboard)
            {
                var clonedMesh = new MeshGeometryModel3D
                {
                    Geometry = mesh.Geometry,
                    Material = mesh.Material,
                    Transform = mesh.Transform.Clone(),
                    IsHitTestVisible = true
                };
                PartsCollection.Add(clonedMesh);
            }
            clipboard.Clear();
        }
        private void DuplicateSelectedMesh()
        {
            if (SelectedPart != null)
            {
                var clonedMesh = new MeshGeometryModel3D
                {
                    Geometry = SelectedPart.Geometry,
                    Material = SelectedPart.Material,
                    Transform = SelectedPart.Transform.Clone(),
                    IsHitTestVisible = true
                };
                PartsCollection.Add(clonedMesh);
            }
        }
    }
}

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
using Matrix = SharpDX.Matrix;
using Quaternion = SharpDX.Quaternion;
using Point = SharpDX.Point;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Markup;
using System.Text.RegularExpressions;


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
        private List<int> clipboard_index = new();
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
        public ICommand RenameMeshCommand { get; }
        public ICommand SelectMeshCommand { get; }
        public ICommand ApplyTransformationsCommand { get; }
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
        private MeshGeometryModel3D _selectedPart;
        public MeshGeometryModel3D SelectedPart
        {
            get => _selectedPart; set
            {
                _selectedPart = value;
                OnPropertyChanged(nameof(SelectedPart));
                UpdateTransformationPanel();
            }
        }
        private double _selectedMeshPositionX;
        private double _selectedMeshPositionY;
        private double _selectedMeshPositionZ;
        private double _selectedMeshScaleX = 1;
        private double _selectedMeshScaleY = 1;
        private double _selectedMeshScaleZ = 1;
        private double _selectedMeshRotationX;
        private double _selectedMeshRotationY;
        private double _selectedMeshRotationZ;
        private bool _isTransformationPanelVisible;
        private bool highlightSeparated = false;
        private string _newMeshName;
        public double SelectedMeshPositionX { get => _selectedMeshPositionX; set { _selectedMeshPositionX = value; OnPropertyChanged(nameof(SelectedMeshPositionX)); } }
        public double SelectedMeshPositionY { get => _selectedMeshPositionY; set { _selectedMeshPositionY = value; OnPropertyChanged(nameof(SelectedMeshPositionY)); } }
        public double SelectedMeshPositionZ { get => _selectedMeshPositionZ; set { _selectedMeshPositionZ = value; OnPropertyChanged(nameof(SelectedMeshPositionZ)); } }
        public double SelectedMeshScaleX { get => _selectedMeshScaleX; set { _selectedMeshScaleX = value; OnPropertyChanged(nameof(SelectedMeshScaleX)); } }
        public double SelectedMeshScaleY { get => _selectedMeshScaleY; set { _selectedMeshScaleY = value; OnPropertyChanged(nameof(SelectedMeshScaleY)); } }
        public double SelectedMeshScaleZ { get => _selectedMeshScaleZ; set { _selectedMeshScaleZ = value; OnPropertyChanged(nameof(SelectedMeshScaleZ)); } }
        public double SelectedMeshRotationX { get => _selectedMeshRotationX; set { _selectedMeshRotationX = value; OnPropertyChanged(nameof(SelectedMeshRotationX)); } }
        public double SelectedMeshRotationY { get => _selectedMeshRotationY; set { _selectedMeshRotationY = value; OnPropertyChanged(nameof(SelectedMeshRotationY)); } }
        public double SelectedMeshRotationZ { get => _selectedMeshRotationZ; set { _selectedMeshRotationZ = value; OnPropertyChanged(nameof(SelectedMeshRotationZ)); } }
        public bool IsTransformationPanelVisible { get => _isTransformationPanelVisible; set { _isTransformationPanelVisible = value; OnPropertyChanged(nameof(IsTransformationPanelVisible)); } }

        private double _addMeshPopupHorizontalOffset;
        private double _addMeshPopupVerticalOffset;
        public double AddMeshPopupHorizontalOffset
        {
            get => _addMeshPopupHorizontalOffset;
            set
            {
                _addMeshPopupHorizontalOffset = value;
                OnPropertyChanged(nameof(AddMeshPopupHorizontalOffset));
            }
        }
        public double AddMeshPopupVerticalOffset
        {
            get => _addMeshPopupVerticalOffset;
            set
            {
                _addMeshPopupVerticalOffset = value;
                OnPropertyChanged(nameof(AddMeshPopupVerticalOffset));
            }
        }
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
        public string NewMeshName 
        { 
            get => _newMeshName; 
            set 
            {
                if (_newMeshName != value) 
                {
                    _newMeshName = value;
                    OnPropertyChanged();
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

        private bool _isAddMeshPopupOpen = false;
        public bool IsAddMeshPopupOpen
        {
            get => _isAddMeshPopupOpen;
            set
            {
                _isAddMeshPopupOpen = value;
                OnPropertyChanged(nameof(IsAddMeshPopupOpen));
            }
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
            this.AddBoxCommand = new Command(AddBox);
            this.AddSphereCommand = new Command(AddSphere);
            this.AddCylinderCommand = new Command(AddCylinder);
            this.ShowAddMeshPopupCommand = new RelayCommand(_ => ShowAddMeshPopup());
            this.EnableTranslateCommand = new RelayCommand(_ => EnableTranslate());
            this.EnableScaleCommand = new RelayCommand(_ => EnableScale());
            this.EnableRotateCommand = new RelayCommand(_ => EnableRotate());
            this.DeleteMeshCommand = new RelayCommand(_ => DeleteSelectedMesh(), _ => SelectedPart != null);
            this.CopyCommand = new RelayCommand(_ => CopySelectedMesh());
            this.PasteCommand = new RelayCommand(_ => PasteCopiedMeshes());
            this.DuplicateCommand = new RelayCommand(_ => DuplicateSelectedMesh());
            this.ApplyTransformationsCommand = new RelayCommand(_ => ApplyTransformations());
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
            AddMeshToCollection(meshBuilder.ToMeshGeometry3D(), PhongMaterials.Chrome, "Cube");
        }
        private void AddSphere()
        {
            var meshBuilder = new MeshBuilder();
            meshBuilder.AddSphere(new Vector3(0, 0, 0), 1, 40, 40);
            AddMeshToCollection(meshBuilder.ToMeshGeometry3D(), PhongMaterials.Chrome, "Sphere");
        }
       
        private void AddCylinder()
        {
            var meshBuilder = new MeshBuilder();
            meshBuilder.AddCylinder(new Vector3(0, 0, 0), new Vector3(0, 6, 0), 1, 20);
            AddMeshToCollection(meshBuilder.ToMeshGeometry3D(), PhongMaterials.Chrome, "Cylinder");
        }

        private string MeshNaming(string name = null)
        {
            string result = null;
            string pattern = @"(\D+)\d*$";

            if (name != null)
            {
                result = Regex.Replace(name, pattern, "$1");
            }
            else
            {
                foreach (Element3D mesh in PartsCollection)
                {
                    if (Regex.IsMatch(mesh.Name, pattern))
                    {
                        result = Regex.Replace(mesh.Name, pattern, "$1");
                    }
                }
            }

            int count = 0;
            string baseName = result;
            while (PartsCollection.Any(m => m.Name.Equals(result)))
            {
                result = baseName + count;
                count++;
            }

            return result;
        }

        private void AddMeshToCollection(MeshGeometry3D mesh, PhongMaterial material, String name)
        {
            var newPart = new MeshGeometryModel3D
            {
                Geometry = mesh,
                Material = material,
                IsHitTestVisible = true,
                Name = name
            };
            newPart.PostEffects = "Wireframe";
            PartsCollection.Add(newPart);
            var newPartNumber = 
            newPart.Name = MeshNaming();
            OnPropertyChanged(nameof(PartsCollection));
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.AddMeshPopup.IsOpen = false;
        }

        private void PerformCGALOperation()
        {
            // Assuming you have a selected mesh from PartsCollection
            var selectedMesh = PartsCollection.FirstOrDefault() as MeshGeometryModel3D;

            if (selectedMesh != null && selectedMesh.Geometry is MeshGeometry3D meshGeometry)
            {
                var cgalMesh = CGALWrapper.ConvertToCGALMesh(meshGeometry);

                // Perform some CGAL operations here
                cgalMesh.Subdivide(2);
                cgalMesh.Simplify(0.5);

                // Convert back to HelixToolkit mesh
                var modifiedMesh = CGALWrapper.ConvertToManagedMesh(cgalMesh);
                selectedMesh.Geometry = modifiedMesh;
            }
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

        public void ShowAddMeshPopup()
        {
        var mainWindow = (MainWindow)Application.Current.MainWindow;
        mainWindow.AddMeshPopup.IsOpen = true;
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
            var mainWindow = (MainWindow)Application.Current.MainWindow;
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
                string name = null;
                foreach (var part in PartsCollection)
                {
                    if (part is MeshGeometryModel3D meshPart && meshPart.Geometry.Equals(mesh.Geometry) &&
                        meshPart.Material.Equals(mesh.Material))
                    {
                        name = part.Name;
                    }
                }
                var clonedMesh = new MeshGeometryModel3D
                {
                    Geometry = mesh.Geometry,
                    Material = mesh.Material,
                    Transform = mesh.Transform.Clone(),
                    Name = MeshNaming(name),
                    IsHitTestVisible = true
                };
                PartsCollection.Add(clonedMesh);
            }
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

        private void UpdateTransformationPanel()
        {
            if (SelectedPart != null)
            {
                // Assuming the mesh's transform is a MatrixTransform3D
                if (SelectedPart.Transform is MatrixTransform3D transform)
                {
                    var matrix = ConvertToSharpDXMatrix(transform.Matrix);

                    SelectedMeshPositionX = matrix.M41;
                    SelectedMeshPositionY = matrix.M42;
                    SelectedMeshPositionZ = matrix.M43;

                    // Decompose the matrix to get scale and rotation
                    var scale = new Vector3();
                    var rotation = new Quaternion();
                    var translation = new Vector3(matrix.M41, matrix.M42, matrix.M43);
                    matrix.Decompose(out scale, out rotation, out translation);

                    SelectedMeshScaleX = scale.X;
                    SelectedMeshScaleY = scale.Y;
                    SelectedMeshScaleZ = scale.Z;

                    var eulerAngles = ToEulerAngles(rotation);
                    SelectedMeshRotationX = eulerAngles.X * (180 / Math.PI);
                    SelectedMeshRotationY = eulerAngles.Y * (180 / Math.PI);
                    SelectedMeshRotationZ = eulerAngles.Z * (180 / Math.PI);

                    IsTransformationPanelVisible = true;
                }
            }
            else
            {
                IsTransformationPanelVisible = false;
            }
        }


        private void ApplyTransformations()
        {
            if (SelectedPart != null)
            {
                var translationMatrix = Matrix.Translation((float)SelectedMeshPositionX, (float)SelectedMeshPositionY, (float)SelectedMeshPositionZ);
                var scaleMatrix = Matrix.Scaling((float)SelectedMeshScaleX, (float)SelectedMeshScaleY, (float)SelectedMeshScaleZ);
                var rotationMatrix = Matrix.RotationYawPitchRoll(
                    (float)(SelectedMeshRotationY * Math.PI / 180),
                    (float)(SelectedMeshRotationX * Math.PI / 180),
                    (float)(SelectedMeshRotationZ * Math.PI / 180)
                );

                var transformMatrix = scaleMatrix * rotationMatrix * translationMatrix;
                var media3DMatrix = new Matrix3D(
                    transformMatrix.M11, transformMatrix.M12, transformMatrix.M13, transformMatrix.M14,
                    transformMatrix.M21, transformMatrix.M22, transformMatrix.M23, transformMatrix.M24,
                    transformMatrix.M31, transformMatrix.M32, transformMatrix.M33, transformMatrix.M34,
                    transformMatrix.M41, transformMatrix.M42, transformMatrix.M43, transformMatrix.M44
                );

                SelectedPart.Transform = new MatrixTransform3D(media3DMatrix);
            }
        }


        private Matrix ConvertToSharpDXMatrix(Matrix3D matrix3D)
        {
            return new Matrix(
                (float)matrix3D.M11, (float)matrix3D.M12, (float)matrix3D.M13, (float)matrix3D.M14,
                (float)matrix3D.M21, (float)matrix3D.M22, (float)matrix3D.M23, (float)matrix3D.M24,
                (float)matrix3D.M31, (float)matrix3D.M32, (float)matrix3D.M33, (float)matrix3D.M34,
                (float)matrix3D.OffsetX, (float)matrix3D.OffsetY, (float)matrix3D.OffsetZ, (float)matrix3D.M44
            );
        }

        private Vector3 ToEulerAngles(Quaternion q)
        {
            // Convert Quaternion to Euler angles
            float x = (float)Math.Atan2(2.0 * (q.W * q.X + q.Y * q.Z), 1.0 - 2.0 * (q.X * q.X + q.Y * q.Y));
            float y = (float)Math.Asin(2.0 * (q.W * q.Y - q.Z * q.X));
            float z = (float)Math.Atan2(2.0 * (q.W * q.Z + q.X * q.Y), 1.0 - 2.0 * (q.Y * q.Y + q.Z * q.Z));
            return new Vector3(x, y, z);
        }
        private void OnSelectedPartChanged()
        {
            // Clear the highlight effect from the previously selected part
            foreach (var part in PartsCollection)
            {
                // Clear any previous highlight effect (e.g., reset the material)
            }

            // Handle the logic for when a part is selected from the explorer
            if (_selectedPart != null)
            {
                // Apply highlight effect to the selected part
                _selectedPart.Material = PhongMaterials.Red; // Use a different material to highlight
            }
        }

        private void RenameMesh(object parameter)
        {
            if (SelectedPart != null)
            {
                SelectedPart.Name = NewMeshName; 
                OnPropertyChanged(nameof(PartsCollection)); 
                // Refresh the list
                }
        } private void SelectMesh(object parameter) 
        { 
            if (parameter is MeshGeometryModel3D mesh) 
            {
                SelectedPart = mesh; 
            } 
        }
        public event PropertyChangedEventHandler PropertyChanged; 
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) 
        { 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
        }


    }
}


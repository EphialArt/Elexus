   M S B A M L   `   `   `  ����A  =Elexus, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ElexusElexus  5   	       Elexus.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   ` \HelixToolkit.Wpf.SharpDX, Version=2.25.0.0, Culture=neutral, PublicKeyToken=52aa3500039caf0d-hx$http://helix-toolkit.org/wpf/SharpDX  5   	   Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5   	    localclr-namespace:Elexus  5   	   -   5          � Title$  Elexus��5   	   $
��1200��5	   	   $	��800��5
   	    � DataContext 5         Elexus.MainViewModel  5   
    � 	Resources 5          BooleanToVisibilityConverter��%   &	        �� 5   
   6'   5      .��5      ]� .+�5   
   �� $��Right=�6   $	��300��6,   ���	#FF1E1B20.+�5      �� $	��157��6   .+�5      �� $��Mesh Explorer��6   $
��Bold"�61   
������� J� RenderTransformOrigin$ 0.5,0.5&�6V   $��Center=�6v   $��Center=�6�   z�5      V� 5      .~�5      �� �� 5       �� AngleX$ -2.126Q�6,   �� 5      T� 5        T� X$ -0.296Q�61   5!      5#      �� 5$      -   5         ��Name$ 
objectview��5$      ���	#FF1E1B20��6/   �� *PartsCollection+ �� SelectedItem 6W   �� *SelectedPart+ �� Mode$ TwoWay=���6�   Z� ��5%      �� 5&      .R�5'      �� 	 �� Orientation$	 
Horizontal=�6)   .+�5(   "   �� $
��Auto��6r   $��0,0,10,0q�6   $��Stretch=�6�   
����������	#FF1E1B20���	#FF1E1B20��6*   �� *Name+$ TwoWay=�
 �� UpdateSourceTrigger$
 PropertyChanged=� �� SelectionTextBrush 6�   Z� 6�   5)      5*      5.      50      �� 52      $��	10,0,0,10q�6   .+�53      �� $��Transformations��6   $
��Bold"�63   $��0,0,0,10q�6E   �� 55      $��Position (meters)��6   $��0,0,0,5q�65   
�������6F   �� 56      $	 
Horizontal=�6   .+�57      �� $��X:��6    $��Center=�6*   $��0,0,5,0q�6E   
�������6V   �� 58      $��60��6   $��0,0,10,0q�6�   ���	#FF888C98��6)   �� *SelectedMeshPositionX+{�6P   �� *IsTransformationPanelVisible+6�   �� 59      $��Y:��6    $��Center=�6*   $��0,0,5,0q�6E   
�������6V   �� 5:      $��60��6   $��0,0,10,0q�6�   ���	#FF888C98��6)   �� *SelectedMeshPositionY+{�6P   �� *IsTransformationPanelVisible+6�   �� 5;      $��Z:��6    $��Center=�6*   $��0,0,5,0q�6E   
�������6V   �� 5<      $��60��6   ���	#FF888C98��6)   �� *SelectedMeshPositionZ+{�6P   �� *IsTransformationPanelVisible+6�   5=      �� 5?      $��Scale (meters)��6   $��10,0,0,5q�62   
�������6D   �� 5@      $	 
Horizontal=�6   .+�5A      �� $��X:��6    $��Center=�6*   $��0,0,5,0q�6E   
�������6V   �� 5B      $��60��6   $��0,0,10,0q�6�   ���	#FF888C98��6)   �� *SelectedMeshScaleX+{�6M   �� *IsTransformationPanelVisible+6�   �� 5C      $��Y:��6    $��Center=�6*   $��0,0,5,0q�6E   
�������6V   �� 5D      $��60��6   $��0,0,10,0q�6�   ���	#FF888C98��6)   �� *SelectedMeshScaleY+{�6M   �� *IsTransformationPanelVisible+6�   �� 5E      $��Z:��6    $��Center=�6*   $��0,0,5,0q�6E   
�������6V   �� 5F      $��60��6   ���	#FF888C98��6)   �� *SelectedMeshScaleZ+{�6M   �� *IsTransformationPanelVisible+6�   5G      �� 5I      $��Rotation (degrees)��6   $��10,0,0,5q�66   
�������6H   �� 5J      $	 
Horizontal=�6   .+�5K      �� $��X:��6    $��Center=�6*   $��0,0,5,0q�6E   
�������6V   �� 5L      $��60��6   $��0,0,10,0q�6�   ���	#FF888C98��6)   �� *SelectedMeshRotationX+{�6P   �� *IsTransformationPanelVisible+6�   �� 5M      $��Y:��6    $��Center=�6*   $��0,0,5,0q�6E   
�������6V   �� 5N      $��60��6   $��0,0,10,0q�6�   ���	#FF888C98��6)   �� *SelectedMeshRotationY+{�6P   �� *IsTransformationPanelVisible+6�   �� 5O      $��Z:��6    $��Center=�6*   $��0,0,5,0q�6E   
�������6V   �� 5P      $��60��6   ���	#FF888C98��6)   �� *SelectedMeshRotationZ+{�6P   �� *IsTransformationPanelVisible+6�   5Q      �� 5S      $��Sphere Radius (meters)��6   $��10,0,0,5q�6:   
�������6L   �� 5T      $	 
Horizontal=�6   .+�5U      �� $��R:��6    $��Center=�6*   $��0,0,5,0q�6E   
�������6V   �� 5V      $��60��6   $��0,0,10,0q�6t   ���	#FF888C98��6)   �� *SelectedMeshRadius+{�6M   �� *IsSphereSelected+6�   5W      �� 5X      $��Apply Transformations��6   $��	10,0,0,10q�6i   ���	#FF888C98��69   �� *ApplyTransformationsCommand+{�6|   �� *IsTransformationPanelVisible+6�   5Y      5Z      *  $HelixToolkit.Wpf.SharpDX.Viewport3DX  5]   
   -   5          Name$ view1��5]         ShowCoordinateSystem$
 True��5a      ���#1E1E1E   BackgroundColor$ #1E1E1E��5d         Title 5^      �� *Title+   Camera 5_      �� *Camera+   EffectsManager 5`      �� *EffectsManager+   SubTitle 5b      �� *
SubTitle+ J� InputBindings 5e      �� 5f      
 �� Key$ C��6=    �� 	Modifiers$ Controlf�6E    �� Command 6   �� *CopyCommand+6E   �� 5g      $ V��6>   $ Controlf�6F    6   �� *PasteCommand+6F   �� 5h      $ D��6B   $ Controlf�6J    6   �� *DuplicateCommand+6J   �� 5i      $ N��6I   $ Shiftf�6Q    6   �� *ShowAddMeshPopupCommand+6Q   �� 5j      $ G��6H    6   �� *EnableTranslateCommand+6H   �� 5k      $ S��6D    6   �� *EnableScaleCommand+6D   �� 5l      $ R��6E    6   �� *EnableRotateCommand+6E   �� 5m      $ Delete��6C    6   �� *DeleteMeshCommand+6C   d� 5n      $  hx:ViewportCommands.Rotate��6    d� Gesture$ 
RightClicka�6D   d� 5o      $ hx:ViewportCommands.Zoom��6   $ MiddleClicka�6B   d� 5p      $ hx:ViewportCommands.Pan��6   $ Shift+MiddleClicka�6A      Items. 5s      -  'HelixToolkit.Wpf.SharpDX.AmbientLight3D  &   HelixToolkit.Wpf.SharpDX.Light3D   Color 6    �� *AmbientLightColor+1  +HelixToolkit.Wpf.SharpDX.DirectionalLight3D  5t         	Direction 6$   �� *Camera.LookDirection+ 6O   �� *DirectionalLightColor+2  ,HelixToolkit.Wpf.SharpDX.LineGeometryModel3D  5v      -   5          Name$
 grid��5v   %      Color$ 	#FF646464��5y   %      	Thickness$	 0.3Q�5z   %   .  (HelixToolkit.Wpf.SharpDX.GeometryModel3D   Geometry 5w   %   �� *Grid+(  "HelixToolkit.Wpf.SharpDX.Element3D   	Transform 5x   %   �� *GridTransform+5z   %     5{      -   5        $ axisX��5{   %   $	 Red��6O   $	 0.5Q�6[    64   �� *AxisX+6[     5|      -   5        $ axisY��5|   %   $ Green��6O   $	 0.5Q�6]    64   �� *AxisY+6]   +	  %HelixToolkit.Wpf.SharpDX.GroupModel3D	  5~      -
  'HelixToolkit.Wpf.SharpDX.GroupElement3D  
  Children  5      +  %HelixToolkit.Wpf.SharpDX.ItemsModel3D  5�      !   ItemsSource! 6&   �� *PartsCollection+"   ItemTemplate" 5�      �� 5�      .R�5�   "   2  ,HelixToolkit.Wpf.SharpDX.MeshGeometryModel3D  #  Name$
# Mesh��69   -   5        $   IsHitTestVisible$
$ True��5�   9    5�   G   �� *
Geometry+6  0HelixToolkit.Wpf.SharpDX.MaterialGeometryModel3D%   Material% 5�   9   �� *
Material+& � DataContext& 5�   ^   �� $ OneWay=�5�      5�      5�      5  /HelixToolkit.Wpf.SharpDX.TransformManipulator3D  5�      -   5        '  Name$' manipulator��5�      (   	SizeScale$( 1Q�5�      )   EnableRotation$) False��5�   +   *   EnableScaling$* False��6B   +   IsRendering$+ False��6X   ,   CenterOffset, 5�      �� *CenterOffset+-   Target- 5�      �� *Target+6X   <  6HelixToolkit.Wpf.SharpDX.PostEffectMeshBorderHighlight  5�      -	   5        .  Name$. borderEffect��5�      8  2HelixToolkit.Wpf.SharpDX.PostEffectMeshOutlineBlur/   
EffectName$/ 	highlight��5�      0   Color$0 	#FFA75DC9��5�      1   NumberOfBlurPass$1 2��5�      2   DrawMode2 5�      �� *
DrawMode+5�      � 5�   
   -
   5        3 �Name$3 AddMeshPopup��5�      $��False��6&   4  .System.Windows.Automation.AutomationProperties4   IsOffscreenBehavior$4 Onscreen=�65   $��Mouse=�6i   .��5�      �� 
�������
���   �$��1q�6<   5 �� Padding$5 10q�6P   ._�5�      �� .+�5�      �� $��Add Box��6?   $��5q�6Q   ��6   �� *AddBoxCommand+6Q   �� 5�      $��
Add Sphere��6B   $��5q�6W   ��6   �� *AddSphereCommand+6W   �� 5�      $��
Add Sphere��6D   $��5q�6Y   ��6   �� *AddCylinderCommand+6Y   5�      5�      5�      5�      5�      
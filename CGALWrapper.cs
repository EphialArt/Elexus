using System.Linq;
using CGALDotNet;
using CGALDotNet.Geometry;
using CGALDotNet.Polyhedra;
using CGALDotNetGeometry.Numerics;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;

public static class CGALWrapper
{
    public static Polyhedron3<EIK> ConvertToCGALMesh(MeshGeometry3D helixMesh)
    {
        var positions = helixMesh.Positions.Select(p => new Point3d(p.X, p.Y, p.Z)).ToArray();
        var indices = helixMesh.TriangleIndices.ToArray();
        return new Polyhedron3<EIK>(positions, indices);
    }

    public static MeshGeometry3D ConvertToManagedMesh(Polyhedron3<EIK> shape)
    {
        var meshBuilder = new MeshBuilder();

        // Get vertex count and allocate array
        int vertexCount = shape.VertexCount;
        var points = new Point3d[vertexCount];
        shape.GetPoints(points, vertexCount);

        // Retrieve polygonal indices
        var indices = shape.GetPolygonalIndices();

        // Add points to meshBuilder
        foreach (var point in points)
        {
            meshBuilder.Positions.Add(new Vector3((float)point.x, (float)point.y, (float)point.z));
        }

        // Add faces to meshBuilder and calculate normals
        var normals = new Vector3[points.Length];
        for (int i = 0; i < indices.quads.Length; i += 4)
        {
            var p0 = meshBuilder.Positions[indices.quads[i]];
            var p1 = meshBuilder.Positions[indices.quads[i + 1]];
            var p2 = meshBuilder.Positions[indices.quads[i + 2]];
            var p3 = meshBuilder.Positions[indices.quads[i + 3]];

            // Add two triangles for the quad
            meshBuilder.TriangleIndices.Add(indices.quads[i]);
            meshBuilder.TriangleIndices.Add(indices.quads[i + 1]);
            meshBuilder.TriangleIndices.Add(indices.quads[i + 2]);

            meshBuilder.TriangleIndices.Add(indices.quads[i + 2]);
            meshBuilder.TriangleIndices.Add(indices.quads[i + 3]);
            meshBuilder.TriangleIndices.Add(indices.quads[i]);

            // Calculate normal for the quad
            var normal = Vector3.Cross(p1 - p0, p2 - p0);
            normal.Normalize();

            // Distribute the normal to the vertices of the quad
            for (int j = 0; j < 4; j++)
            {
                normals[indices.quads[i + j]] += normal;
            }
        }

        // Normalize normals and add to meshBuilder
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i].Normalize();
            meshBuilder.Normals.Add(normals[i]);
        }

        return meshBuilder.ToMeshGeometry3D();
    }
}

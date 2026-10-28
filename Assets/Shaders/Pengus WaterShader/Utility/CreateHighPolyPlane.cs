using UnityEngine;
using UnityEditor;



public class CreateHighPolyPlane
{
    [MenuItem("GameObject/3D Object/Water Plane (Subdivided)", false, 0)]
    private static void CreateSubdividedPlane()
    {
        int xSeg = 100;
        int zSeg = 100;
        float width = 10f;
        float length = 10f;

        Mesh mesh = new Mesh();
        Vector3[] verts = new Vector3[(xSeg + 1) * (zSeg + 1)];
        Vector2[] uvs = new Vector2[verts.Length];
        int[] tris = new int[xSeg * zSeg * 6];

        for (int i = 0, z = 0; z <= zSeg; z++)
        {
            for (int x = 0; x <= xSeg; x++, i++)
            {
                verts[i] = new Vector3((float)x / xSeg * width - width / 2f, 0, (float)z / zSeg * length - length / 2f);
                uvs[i] = new Vector2((float)x / xSeg, (float)z / zSeg);
            }
        }

        int v = 0, t = 0;
        for (int z = 0; z < zSeg; z++)
        {
            for (int x = 0; x < xSeg; x++)
            {
                tris[t + 0] = v;
                tris[t + 1] = v + xSeg + 1;
                tris[t + 2] = v + 1;
                tris[t + 3] = v + 1;
                tris[t + 4] = v + xSeg + 1;
                tris[t + 5] = v + xSeg + 2;
                v++;
                t += 6;
            }
            v++;
        }

        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        GameObject go = new GameObject("WaterPlane_HighPoly", typeof(MeshFilter), typeof(MeshRenderer));
        go.GetComponent<MeshFilter>().mesh = mesh;
        Selection.activeGameObject = go;
    }
}

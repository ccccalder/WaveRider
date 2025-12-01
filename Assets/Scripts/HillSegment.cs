using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HillSegment : MonoBehaviour
{
    public int pointCount = 20;
    public float amplitude = 5f;
    public float scale = 0.15f;
    public float lifeTime = 13;

    private EdgeCollider2D edge;
    private MeshFilter meshFilter;

    public float meshBottomY = -5f; // How far below the hills the mesh should go

    void Awake()
    {
        edge = GetComponent<EdgeCollider2D>();
        meshFilter = GetComponent<MeshFilter>();

        Destroy(gameObject, lifeTime);
    }

    public void Generate(float offset)
    {
        Vector2[] pts = new Vector2[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            float worldX = offset + i * 1f;
            float y = Mathf.PerlinNoise(worldX * scale, 0f) * amplitude;
            pts[i] = new Vector2(worldX - offset, y); // local X
        }

        edge.points = pts;

        // Build mesh
        BuildMesh(pts);
    }

    private void BuildMesh(Vector2[] pts)
    {
        int n = pts.Length;
        Vector3[] vertices = new Vector3[n * 2];
        int[] triangles = new int[(n - 1) * 6];
        Vector2[] uvs = new Vector2[n * 2];

        // Top vertices (hill outline)
        for (int i = 0; i < n; i++)
        {
            vertices[i] = pts[i];
            uvs[i] = new Vector2((float)i / (n - 1), 1);
        }
        // Bottom vertices (flat at y = 0)
        for (int i = 0; i < n; i++)
        {
            vertices[n + i] = new Vector3(pts[i].x, meshBottomY, 0);
            uvs[n + i] = new Vector2((float)i / (n - 1), 0);
        }

        // Triangles
        for (int i = 0; i < n - 1; i++)
        {
            int topLeft = i;
            int topRight = i + 1;
            int bottomLeft = n + i;
            int bottomRight = n + i + 1;

            // First triangle
            triangles[i * 6 + 0] = topLeft;
            triangles[i * 6 + 1] = topRight;
            triangles[i * 6 + 2] = bottomLeft;

            // Second triangle
            triangles[i * 6 + 3] = topRight;
            triangles[i * 6 + 4] = bottomRight;
            triangles[i * 6 + 5] = bottomLeft;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }


}

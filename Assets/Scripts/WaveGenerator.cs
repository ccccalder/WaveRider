using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WaveGenerator : MonoBehaviour
{
    [Header("Wave Settings")]
    public int points = 100;
    public float width = 20f;
    public float amplitude = 1f;
    public float frequency = 1f;
    public float scrollSpeed = 1f;
    public float baseHeight = 0f;
    public float depth = -10f;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private float phaseOffset = 0f;

    private EdgeCollider2D edgeCollider;
    private Vector2[] colliderPoints;

    void Awake()
    {
        mesh = new Mesh();
        mesh.name = "Wave Mesh";
        GetComponent<MeshFilter>().mesh = mesh;

        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    void Start()
    {
        GenerateWaveMesh();
        colliderPoints = new Vector2[points + 1];
    }

    void Update()
    {
        AnimateWave();
        UpdateCollider();
    }

    void GenerateWaveMesh()
    {
        vertices = new Vector3[(points + 1) * 2];
        triangles = new int[points * 6];

        float step = width / points;

        for (int i = 0; i <= points; i++)
        {
            float x = i * step;
            float y = GetWaveHeight(x, 0f);
            vertices[i] = new Vector3(x, y, 0f);
            vertices[i + points + 1] = new Vector3(x, depth, 0f);
        }

        int vert = 0;
        int tris = 0;
        for (int i = 0; i < points; i++)
        {
            triangles[tris + 0] = vert;
            triangles[tris + 1] = vert + points + 1;
            triangles[tris + 2] = vert + 1;
            triangles[tris + 3] = vert + 1;
            triangles[tris + 4] = vert + points + 1;
            triangles[tris + 5] = vert + points + 2;
            vert++;
            tris += 6;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    void AnimateWave()
    {
        if (mesh == null || vertices == null)
            return;

        phaseOffset += Time.deltaTime * scrollSpeed;

        Vector3[] updatedVerts = mesh.vertices;
        float step = width / points;

        for (int i = 0; i <= points; i++)
        {
            float x = i * step;
            float y = GetWaveHeight(x, phaseOffset);
            updatedVerts[i].y = y;
        }

        mesh.vertices = updatedVerts;
        mesh.RecalculateBounds();
    }

    
    void UpdateCollider()
    {
        float step = width / points;
        for (int i = 0; i <= points; i++)
        {
            float x = i * step;
            float y = GetWaveHeight(x, phaseOffset);
            colliderPoints[i] = new Vector2(x, y);
        }
        edgeCollider.points = colliderPoints;
    }
    

    public float GetWaveHeight(float x, float phase)
    {
        return (Mathf.PerlinNoise((x * 0.2f) + phase, 0f) - 0.5f) * amplitude * 2f + baseHeight;
    }
}
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public Transform player;
    public GameObject hillPrefab;

    public float segmentWidth = 30f;
    [SerializeField]private float nextX = 0f;

    [Header("Amplitude Settings")]
    public float amplitude = 5f;
    public float amplitudeGrowthRate = 0.1f; // units per second
    public float amplitudeCap = 11f;
    public float minAmplitudeCap = 8f;
    public float maxAmplitudeCap = 18f;

    private void Start()
    {
        amplitudeCap = Random.Range(minAmplitudeCap, maxAmplitudeCap);
    }
    void Update()
    {
        if (Mathf.RoundToInt(amplitude) < Mathf.RoundToInt(amplitudeCap))
        {
            amplitude += amplitudeGrowthRate * Time.deltaTime;
        }
        else if (Mathf.RoundToInt(amplitude) > Mathf.RoundToInt(amplitudeCap))
        {
            amplitude -= amplitudeGrowthRate * Time.deltaTime;
        }
        else
        {
            amplitudeCap = Random.Range(minAmplitudeCap, maxAmplitudeCap);
        }

        // Spawn new hill when player gets close to the end
        if (player.position.x + 60f > nextX)
        {
            SpawnSegment();
        }
    }

    void SpawnSegment()
    {
        GameObject segment = Instantiate(hillPrefab);
        segment.transform.position = new Vector3(nextX, 0, 0);

        HillSegment hills = segment.GetComponent<HillSegment>();
        hills.amplitude = amplitude; // Pass the current amplitude
        hills.Generate(nextX);

        nextX += segmentWidth;
    }
}

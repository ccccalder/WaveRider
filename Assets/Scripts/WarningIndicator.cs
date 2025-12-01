using UnityEngine;

public class WarningIndicator : MonoBehaviour
{
    public float warningDuration = 1.5f;

    private Camera mainCamera;

    public float edgeMarginX = 0.05f;

    // The world position where the seagull will spawn
    private Vector3 seagullSpawnPoint;
    private float timer;
    private float screenRightEdgeWorldX;
    private SpriteRenderer spriteRenderer;

    public void SetupWarning(Vector3 spawnPoint)
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (mainCamera == null || spriteRenderer == null)
        {
            Debug.LogError("Main Camera or Sprite Renderer missing!");
            return;
        }

        seagullSpawnPoint = spawnPoint;
        timer = warningDuration;


        Vector3 rightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1f - edgeMarginX, 0.5f, mainCamera.nearClipPlane));
        screenRightEdgeWorldX = rightEdge.x;


        transform.position = new Vector3(screenRightEdgeWorldX, seagullSpawnPoint.y, 0f);

        spriteRenderer.enabled = true;
    }

    void Update()
    {
        if (mainCamera == null || spriteRenderer == null) return;

        Vector3 screenPoint = mainCamera.WorldToViewportPoint(new Vector3(0, seagullSpawnPoint.y, 0));

        Vector3 newWorldPosition = mainCamera.ViewportToWorldPoint(new Vector3(1f - edgeMarginX, screenPoint.y, mainCamera.nearClipPlane));

        transform.position = new Vector3(screenRightEdgeWorldX, newWorldPosition.y, 0f);

        //blinking effect
        timer -= Time.deltaTime;

        float flashRate = (timer < 0.5f) ? 0.1f : 0.25f;
        spriteRenderer.enabled = Mathf.Round(Time.time / flashRate) % 2 == 0;

        if (seagullSpawnPoint.x < screenRightEdgeWorldX)
        {
            spriteRenderer.enabled = false;
        }

        if (timer <= 0f)
        {
            // The indicator's duration is over, it will be destroyed by the spawner.
        }
    }
}

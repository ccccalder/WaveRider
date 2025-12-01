using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    private float length;
    private float startPosX;
    private float startPosY;
    public Transform cam;

    public float parallaxEffectX = 0.5f;
    public float parallaxEffectY = 0.5f;

    private void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        Debug.Log(length);
    }

    private void Update()
    {
        float tempX = (cam.position.x * (1 - parallaxEffectX));
        float distX = (cam.position.x * parallaxEffectX);

        float tempY = (cam.position.y * (1 - parallaxEffectY));
        float distY = (cam.position.y * parallaxEffectY);

        transform.position = new Vector3(startPosX + distX, startPosY + distY, transform.position.z);

        if (tempX > startPosX + length) startPosX += length;
        else if (tempX < startPosX - length) startPosX -= length;
    }
}

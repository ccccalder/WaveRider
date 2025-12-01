using UnityEngine;

public class BGMusicScript : MonoBehaviour
{
    public static BGMusicScript instance;
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

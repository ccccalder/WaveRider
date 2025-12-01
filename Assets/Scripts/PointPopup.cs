using TMPro;
using UnityEngine;

public class PointPopup : MonoBehaviour
{
    private TextMeshProUGUI textComponent;
    
    // Call this immediately after instantiating the prefab
    public void Setup(int points)
    {
        // Get the TextMeshPro component
        textComponent = gameObject.GetComponent<TextMeshProUGUI>();

        // Set the points and color
        textComponent.text = "+" + points.ToString();

        Destroy(gameObject, 3f); // Destroy after 3 seconds
    }
}

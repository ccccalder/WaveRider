using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public GameObject timerSliderGameObject;
    private Slider timerSlider;
    private bool countingDown = false;

    private void Start()
    {
        timerSliderGameObject.SetActive(false);
        timerSlider = timerSliderGameObject.GetComponent<Slider>();
    }
    public void StartCountdown(float countdownTime)
    {
        //setup

        timerSliderGameObject.SetActive(true);
        countingDown = true;
        timerSlider.maxValue = countdownTime;
        timerSlider.value = countdownTime;


    }
    private void Update()
    {
        if (!countingDown) return;
        if (timerSlider.value > 0)
        {
            timerSlider.value -= Time.deltaTime;
        }
        else
        {
            timerSlider.value = 0;
            countingDown = false;
            timerSliderGameObject.SetActive(false);
        }
    }
}

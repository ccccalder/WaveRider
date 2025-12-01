using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1.4f;

    
    public void LoadEndResults(float timeBeforeAnim)
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1, timeBeforeAnim));
    }
    private IEnumerator LoadLevel(int levelIndex, float timeBeforeAnim)
    {
        yield return new WaitForSeconds(timeBeforeAnim);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
    
    
    public void RestartLevel(float transitionTimeForDeath, float timeBeforeAnim)
    {
        StartCoroutine(RestartLevel(SceneManager.GetActiveScene().buildIndex, transitionTimeForDeath, timeBeforeAnim));
    }
    private IEnumerator RestartLevel(int levelIndex, float transitionTimeForDeath, float timeBeforeAnim)
    {
        yield return new WaitForSeconds(timeBeforeAnim);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTimeForDeath);
        SceneManager.LoadScene(levelIndex);
    }
    public void ReplayGame()
    {
        StartCoroutine(ReplayGame(SceneManager.GetActiveScene().buildIndex - 1));
    }
    private IEnumerator ReplayGame(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }


}
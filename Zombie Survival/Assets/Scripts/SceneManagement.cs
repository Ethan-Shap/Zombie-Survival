using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{

    /// <summary>
    /// Loads scene from build index
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void SwitchScenes(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    /// <summary>
    /// Loads scene from name
    /// </summary>
    /// <param name="sceneName"></param>
    public void SwitchScenes(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Loads next scene in build order
    /// </summary>
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Quits game
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    /// <summary>
    /// Quits application after seconds
    /// </summary>
    /// <param name="seconds"></param>
    public void QuitGameAfterSeconds(int seconds)
    {
       StartCoroutine(QuitAfterSeconds(seconds));
    }

    private IEnumerator QuitAfterSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Application.Quit();
    }

}

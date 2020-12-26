using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScript : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }
}

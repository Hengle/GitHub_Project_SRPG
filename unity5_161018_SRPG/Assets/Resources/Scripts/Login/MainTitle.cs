using UnityEngine;
using UnityEngine.SceneManagement;

public class MainTitle : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene(2);
    }
}
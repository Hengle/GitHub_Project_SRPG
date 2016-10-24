using UnityEngine;
using UnityEngine.SceneManagement;

public class MainTitle : MonoBehaviour
{
    void OnGUI()
    {
        float btnW = 200f;
        float btnH = 50f;
        Rect rect = new Rect(Screen.width / 2f + 100f, Screen.height / 2f - 150f, btnW, btnH);
        if (GUI.Button(rect, "<size=20><color=#4000ff>" + "Game Start" + "</color></size>"))
        {
            SceneManager.LoadScene(2);
        }
    }
}
using UnityEngine;
using System.Collections;

public class MainTitle : MonoBehaviour
{
    void OnGUI()
    {
        float btnW = 200f;
        float btnH = 50f;
        Rect rect = new Rect(Screen.width / 2f - 100f, Screen.height / 2f + 180f, btnW, btnH);
        if(GUI.Button(rect, "<color=#4000ff>" + "Game Start" + "</color>"))
        {
            Application.LoadLevel(2);
        }
    }
}
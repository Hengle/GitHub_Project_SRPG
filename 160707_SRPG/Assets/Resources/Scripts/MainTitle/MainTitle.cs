using UnityEngine;
using System.Collections;

public class MainTitle : MonoBehaviour
{
    void OnGUI()
    {
        float btnW = 200f;
        float btnH = 50f;
        Rect rect = new Rect(Screen.width / 2 - btnW / 2, Screen.height / 2 + 200, btnW, btnH);
        if(GUI.Button(rect, "Game Start"))
        {
            Application.LoadLevel(1);
        }
    }
}
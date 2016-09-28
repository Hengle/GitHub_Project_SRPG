using UnityEngine;
using System.Collections;

public class GUIMgr
{
    private string curSet = "Passable";

    // 싱글톤
    private static GUIMgr inst = null;
    public static GUIMgr GetInst()
    {
        if(inst == null)
        {
            inst = new GUIMgr();
        }
        return inst;
    }
    
    public void DrawLeftLayout()
    {
        // // 3-03:9분 MapInfo가 들어갈 Layout그리기. Begin과 End는 시작과 끝,한쌍임.
        GUILayout.BeginArea(new Rect(0, 0, 200f, Screen.height), "MapInfo", GUI.skin.window);
        // 3-03:15분
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Size X: ");
        GUILayout.TextField("0");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Size Y: ");
        GUILayout.TextField("0");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Size Z: ");
        GUILayout.TextField("0");
        GUILayout.EndHorizontal();

        GUILayout.Button("Create");
        GUILayout.Button("Reset");
        GUILayout.Button("Save");

        GUILayout.Label("Current Selected");
        GUILayout.Label("Passable");
        GUILayout.Button("Passable");
        GUILayout.Button("Not Passable");

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}

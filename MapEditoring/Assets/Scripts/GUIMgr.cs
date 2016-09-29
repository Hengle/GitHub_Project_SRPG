using UnityEngine;
using System.Collections;

public class GUIMgr
{
    private string curSet = "Passable";
    private bool passable = true;

    private int sizeX = 0;
    private int sizeY = 0;
    private int sizeZ = 0;

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
        string sizeXstr = GUILayout.TextField("" + sizeX, 6, GUILayout.Width(50));
        sizeX = int.Parse(sizeXstr);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Size Y: ");
        string sizeYstr = GUILayout.TextField("" + sizeY, 6, GUILayout.Width(50));
        sizeY = int.Parse(sizeYstr);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Size Z: ");
        string sizeZstr = GUILayout.TextField("" + sizeZ, 6, GUILayout.Width(50));
        sizeZ = int.Parse(sizeZstr);
        GUILayout.EndHorizontal();

        if(GUILayout.Button("Create"))
        {
            MapMgr.GetInst().CreateMap(sizeX, sizeY, sizeZ);
        }
        GUILayout.Button("Reset");
        GUILayout.Button("Save");

        GUILayout.Label("Current Selected");
        // 3-03:20분
        GUILayout.Label(curSet);
        if(GUILayout.Button("Passable"))
        {
            curSet = "Passable";
            passable = true;
        }
        if(GUILayout.Button("Not Passable"))
        {
            curSet = "Not Passable";
            passable = false;
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}

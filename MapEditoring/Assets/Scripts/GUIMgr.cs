using UnityEngine;
using System.Collections;

public class GUIMgr
{
    public bool Passable = true;
    private string curSet = "Passable";
    
    public int SizeX = 5;
    public int SizeY = 5;
    public int SizeZ = 5;

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
        string sizeXstr = GUILayout.TextField("" + SizeX, 6, GUILayout.Width(50));
        SizeX = int.Parse(sizeXstr);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Size Y: ");
        string sizeYstr = GUILayout.TextField("" + SizeY, 6, GUILayout.Width(50));
        SizeY = int.Parse(sizeYstr);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Size Z: ");
        string sizeZstr = GUILayout.TextField("" + SizeZ, 6, GUILayout.Width(50));
        SizeZ = int.Parse(sizeZstr);
        GUILayout.EndHorizontal();
        
        if(GUILayout.Button("Create"))
        {
            MapMgr.GetInst().CreateMap(SizeX, SizeY, SizeZ);
        }
        // 3-05:9분
        if (GUILayout.Button("Reset"))
        {
            MapMgr.GetInst().RemoveMap();
        }
        if(GUILayout.Button("Save"))
        {
            FileMgr.GetInst().SaveData();
        }

        GUILayout.Label("Current Selected");
        // 3-03:20분
        GUILayout.Label(curSet);
        if(GUILayout.Button("Passable"))
        {
            curSet = "Passable";
            Passable = true;
        }
        if(GUILayout.Button("Not Passable"))
        {
            curSet = "Not Passable";
            Passable = false;
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}

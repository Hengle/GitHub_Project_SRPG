using UnityEngine;
using System.Collections.Generic;

public class GUIManager
{
    private PlayerManager pm = null;
    // 싱글턴
    private static GUIManager inst = null;
    public static GUIManager GetInst()
    {
        if(inst == null)
        {
            inst = new GUIManager();
            inst.pm = PlayerManager.GetInst();
        }
        return inst;
    }
    
    // 2-07:18분33초 => 32분40초(뭐계속바뀜..)
    public void DrawGUI() // TODO : 이부분을 호출하는 부분 필요함(2-08:18분10초)
    {
        if (pm.Players.Count > 0)
        {
            PlayerBase pb = pm.Players[pm.CurTurnIdx];
            if (pb is UserPlayer)
            {
                // 2-08:35분 현재 상태가 idle일때만 GUI활성화(행동중일때 다른거못하게할려고)
                if(pb.act == ACT.IDLE)
                {
                    DrawStatus(pm.Players[pm.CurTurnIdx]);
                    DrawCommand(pm.Players[pm.CurTurnIdx]);
                }
            }
        }
        DrawTurnInfo();
    }

    // 2-07:15분 UserPlayer에서 복사해옴 / 43분10초부터 GUILayout적용
    public void DrawStatus(PlayerBase pb) // TODO : 이부분을 호출하는 부분 필요함(2-08:18분10초)
    {
        GUILayout.BeginArea(new Rect(0, Screen.height / 2 + 127f, 150f, Screen.height / 4), "Player Info", GUI.skin.window);
        GUILayout.Label("Name: " + pb.status.Name);
        GUILayout.Label("HP: " + pb.status.CurHP);
        GUILayout.Label("MoveRange: " + pb.status.MoveRange);
        GUILayout.Label("AtkRange: " + pb.status.AtkRange);
        GUILayout.EndArea();
    }

    public void DrawCommand(PlayerBase pb)
    {
        GUILayout.BeginArea(new Rect(Screen.width - 150f, Screen.height / 2 + 127f, 150f, Screen.height / 4), "Command", GUI.skin.window);

        if(GUILayout.Button("Move"))
        {
            if(MapManager.GetInst().HighlightMoveRange(pb.CurHex, pb.status.MoveRange) == true)
            {
                pb.act = ACT.MOVEHIGHLIGHT;
            }
        }
        if(GUILayout.Button("Attack"))
        {
            if (MapManager.GetInst().HighlightAtkRange(pb.CurHex, pb.status.AtkRange) == true)
            {
                // HIGHLIGHT로 상태 전환(Move버튼을 눌렀을때만 이동하게 하려고)
                pb.act = ACT.ATTACKHIGHLIGHT;
            }
        }
        if (GUILayout.Button("Next Turn"))
        {
            PlayerManager.GetInst().NextTurn();
        }

        GUILayout.EndArea();
    }

    // 2-09:25분50초 Turn순서 캐릭터로 보여주기
    List<GameObject> players = new List<GameObject>();

    //public void InitTurnInfoMgr()
    //{
    //    players = new List<GameObject>();
    //}

    public void AddTurnPlayer(PlayerBase pb)
    {
        GameObject userPlayer = pm.GO_userPlayer;
        GameObject aiMonster = pm.GO_monster;

        if (pb is UserPlayer)
        {
            players.Add((GameObject)GameObject.Instantiate(userPlayer, new Vector3(0f, 0f, 0f), new Quaternion(0f, 0f, 0f, 0f)));
        }
        else if (pb is Monster)
        {
            players.Add((GameObject)GameObject.Instantiate(aiMonster, new Vector3(0f, 0f, 0f), new Quaternion(0f, 0f, 0f, 0f)));
        }
    }

    public void RemoveTurnPlayer(int turnIdx)
    {
        // 게임화면상의 turn정보용 캐릭터 오브젝트
        GameObject obj = players[turnIdx];
        // players리스트에서 삭제
        players.RemoveAt(turnIdx);
        // turn정보용 오브젝트 삭제
        GameObject.Destroy(obj);
    }

    float camX;
    float camY;
    float camZ;
    Quaternion turnRot;

    // turn정보 표시
    public void DrawTurnInfo()
    {
        GUILayout.BeginArea(new Rect(0, 0, 370f, 140f), "Turn Info", GUI.skin.window);
        GUILayout.EndArea();

        int curDraw = pm.CurTurnIdx;
        // 최대 표시 개수 지정
        int maxDraw = 5;
        if(maxDraw > pm.Players.Count)
        {
            maxDraw = pm.Players.Count;
        }

        for(int i = 0; i < maxDraw; i++)
        {
            players[curDraw].transform.position = new Vector3(camX + -4.2f + i * 0.8f, camY - 2.7f, camZ + 4.26f);
            players[i].transform.rotation = turnRot;
            curDraw++;
            if(curDraw == pm.Players.Count)
            {
                curDraw = 0;
            }
        }
    }

    // 2-10:48분에 수정함
    public void UpdateTurnInfoPos(Vector3 pos, Quaternion rot)
    {
        camX = pos.x;
        camY = pos.y;
        camZ = pos.z;
        turnRot = rot;
    }
}
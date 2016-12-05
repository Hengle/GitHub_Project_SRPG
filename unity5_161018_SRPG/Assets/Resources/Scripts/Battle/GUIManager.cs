using UnityEngine;
using System.Collections.Generic;

public enum SKILLWINDOW
{
    OFF,
    ON
}

public class GUIManager
{
    private PlayerManager pm = null;
    // 스킬 커맨드창 on/off
    public SKILLWINDOW skCommand = SKILLWINDOW.OFF;

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
                // 2-08:35분 현재 상태가 idle이고 skCommand가 off일때만 GUI활성화
                if(pb.act == ACT.IDLE && skCommand == SKILLWINDOW.OFF)
                {
                    DrawStatus(pm.Players[pm.CurTurnIdx]);
                    DrawCommand(pm.Players[pm.CurTurnIdx]);
                }
                if(skCommand == SKILLWINDOW.ON)
                {
                    DrawSkillTree(pm.Players[pm.CurTurnIdx]);
                }
            }
        }
    }

    // 2-07:15분 UserPlayer에서 복사해옴 / 43분10초부터 GUILayout적용
    public void DrawStatus(PlayerBase pb) // TODO : 이부분을 호출하는 부분 필요함(2-08:18분10초)
    {
        // Screen.width(넓이) : 741, Screen.height(높이) : 463
        GUILayout.BeginArea(new Rect(0, Screen.height - 130f, 150f, 130f), "Player Info", GUI.skin.window);
        GUILayout.Label("Name: Samurai");
        GUILayout.Label("HP: " + pb.status.CurHP);
        GUILayout.Label("MoveRange: " + pb.status.MoveRange);
        GUILayout.Label("AtkRange: " + pb.status.AtkRange);
        GUILayout.EndArea();
    }

    // 기본 동작 커맨드창
    public void DrawCommand(PlayerBase pb)
    {
        // Rect(화면 넓이 - 커맨드 가로폭 = x위치, 화면 높이 - 커맨드 세로폭 = y위치, 커맨드 가로폭 설정, 커맨드 세로폭 설정)
        GUILayout.BeginArea(new Rect(Screen.width - 150f, Screen.height - 130f, 150f, 130f), "Command", GUI.skin.window);
        if (GUILayout.Button("Move"))
        {
            if (MapManager.GetInst().HighlightMoveRange(pb.CurHex, pb.status.MoveRange) == true)
            {
                pb.act = ACT.MOVEHIGHLIGHT;
            }
        }
        if (GUILayout.Button("Attack"))
        {
            if (MapManager.GetInst().HighlightAtkRange(pb.CurHex, pb.status.AtkRange) == true)
            {
                // HIGHLIGHT로 상태 전환(Move버튼을 눌렀을때만 이동하게 하려고)
                pb.act = ACT.ATTACKHIGHLIGHT;
                pb.skillSet = SKILL.NONE;
            }
        }
        if (GUILayout.Button("Skill Tree"))
        {
            skCommand = SKILLWINDOW.ON;
        }
        if (GUILayout.Button("Next Turn"))
        {
            PlayerManager.GetInst().NextTurn();
        }
        GUILayout.EndArea();
    }

    // 스킬 동작 커맨드창
    void DrawSkillTree(PlayerBase pb)
    {
        GUILayout.BeginArea(new Rect(Screen.width - 150f, Screen.height / 2 + 127f, 150f, Screen.height / 4), "Skill Tree", GUI.skin.window);

        if (GUILayout.Button("Ice Age"))
        {
            if (MapManager.GetInst().HighlightAtkRange(pb.CurHex, pb.status.AtkRange) == true)
            {
                skCommand = SKILLWINDOW.OFF;
                // HIGHLIGHT로 상태 전환
                pb.act = ACT.ATTACKHIGHLIGHT;
                pb.skillSet = SKILL.SKILL1;
            }
        }
        if (GUILayout.Button("Tornado Blade"))
        {
            skCommand = SKILLWINDOW.OFF;
            if (MapManager.GetInst().HighlightAtkRange(pb.CurHex, pb.status.AtkRange) == true)
            {
                skCommand = SKILLWINDOW.OFF;
                // HIGHLIGHT로 상태 전환
                pb.act = ACT.ATTACKHIGHLIGHT;
                pb.skillSet = SKILL.SKILL2;
            }
        }
        if (GUILayout.Button("Skill 3"))
        {
            
        }
        if (GUILayout.Button("Skill 4"))
        {
            
        }

        GUILayout.EndArea();
    }
}
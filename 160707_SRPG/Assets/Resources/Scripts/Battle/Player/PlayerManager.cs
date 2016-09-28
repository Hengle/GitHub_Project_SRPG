using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class PlayerManager
{
    // 드래그&드롭으로 Player프리팹 적용 => 2-08오면서 코드로 수정(14분)
    public GameObject GO_userPlayer;
    public GameObject GO_monster;
    // Player관리 List
    public List<PlayerBase> Players = new List<PlayerBase>();
    // Player Turn Index
    public int CurTurnIdx = 0;

    private float turnOverTime = 2000f;
    private float curTurnOverTime = 0f;
    
    // 싱글턴
    private static PlayerManager inst = null;
    public static PlayerManager GetInst()
    {
        if(inst == null)
        {
            inst = new PlayerManager();
            inst.Init();
        }
        return inst;
    }

    public void Init()
    {
        turnOverTime = 0f;
        curTurnOverTime = 0f;
        GO_monster = (GameObject)Resources.Load("Prefabs/AIPlayer/cyclops");
        GO_userPlayer = (GameObject)Resources.Load("Prefabs/UserPlayer/samuzai");
    }

    /* TODO : 이부분을 호출하는 부분이 필요함(2-08:14분30초) => 29분:Update()삭제. 
              CheckTurnOver()는 public으로 바꾸고 GameManager의 Update()에서 호출하는걸로~ */
    //void Update()
    //{
    //    CheckTurnOver();
    //}

    public void CheckTurnOver()
    {
        if(curTurnOverTime != 0f)
        {
            curTurnOverTime += Time.deltaTime;
            if(curTurnOverTime >= turnOverTime)
            {
                curTurnOverTime = 0f;
                NextTurn();
            }
        }
    }

    public void GenPlayerTest()
    {
        // Player 오브젝트 가져옴
        UserPlayer userPlayer = ((GameObject)GameObject.Instantiate(GO_userPlayer)).GetComponent<UserPlayer>();
        // Hex정보 초기화(Player위치지정 0,0,0)
        HexGrid hex = MapManager.GetInst().GetPlayerHex(2, -3, 1);
        // Player위치 적용
        userPlayer.CurHex = hex;
        // Player를 적용한 위치에 생성
        userPlayer.transform.position = userPlayer.CurHex.transform.position;
        // Players[] List에 Player 저장
        Players.Add(userPlayer);
        userPlayer.name = "userPlayer1";
        GUIManager.GetInst().AddTurnPlayer(userPlayer);
        
        Monster monster = ((GameObject)GameObject.Instantiate(GO_monster)).GetComponent<Monster>();
        hex = MapManager.GetInst().GetPlayerHex(2, -2, 0);
        monster.CurHex = hex;
        monster.transform.position = monster.CurHex.transform.position;
        Players.Add(monster);
        monster.name = "aiMonster";
        GUIManager.GetInst().AddTurnPlayer(monster);
        
        //userPlayer = ((GameObject)GameObject.Instantiate(GO_userPlayer)).GetComponent<UserPlayer>();
        //hex = MapManager.GetInst().GetPlayerHex(3, -3, 0);
        //userPlayer.CurHex = hex;
        //userPlayer.transform.position = userPlayer.CurHex.transform.position;
        //Players.Add(userPlayer);
        //userPlayer.name = "userPlayer2";
        //GUIManager.GetInst().AddTurnPlayer(userPlayer);
        
        //userPlayer = ((GameObject)GameObject.Instantiate(GO_userPlayer)).GetComponent<UserPlayer>();
        //hex = MapManager.GetInst().GetPlayerHex(5, -3, -2);
        //userPlayer.CurHex = hex;
        //userPlayer.transform.position = userPlayer.CurHex.transform.position;
        //Players.Add(userPlayer);
        //userPlayer.name = "userPlayer3";
        //GUIManager.GetInst().AddTurnPlayer(userPlayer);
        
        //userPlayer = ((GameObject)GameObject.Instantiate(GO_userPlayer)).GetComponent<UserPlayer>();
        //hex = MapManager.GetInst().GetPlayerHex(4, -2, -2);
        //userPlayer.CurHex = hex;
        //userPlayer.transform.position = userPlayer.CurHex.transform.position;
        //Players.Add(userPlayer);
        //userPlayer.name = "userPlayer4";
        //GUIManager.GetInst().AddTurnPlayer(userPlayer);
    }

    // 넘겨받은 좌표로 Player의 위치를 이동시킴
    public void MovePlayer(HexGrid start, HexGrid dest)
    {
        // PlayerBase클래스로 pb선언, Players저장
        PlayerBase pb = Players[CurTurnIdx];
        // Hex간 거리 구함.
        int distance = MapManager.GetInst().GetDistance(start, dest);
        // 현재 상태가 HIGHLIGHT이면서,
        // 이동범위보다 distance(이동하고자하는 위치)가 작거나 같고,
        // 제자리(현재위치)가 아닐때
        if(pb.act == ACT.MOVEHIGHLIGHT && distance <= pb.status.MoveRange && distance != 0 && dest.Passable == true)
        {
            pb.MoveHexes = MapManager.GetInst().GetPath(start, dest);
            // 벽으로 모든길이 막히면 못움직임
            if(pb.MoveHexes.Count == 0)
            {
                return;
            }
            // 목적지 Hex위치를 마우스클릭한 좌표로 저장(1-08에서부터 위 코드로 변경)
            //pb.dest = dest;
            // 현재 상태를 이동상태로 전환(smooth하게 이동)
            pb.act = ACT.MOVING;
            // Hex위치를 Player가 이동한곳으로 reset.(1-08:45분에서부터 주석처리)
            //pb.CurHex = dest;
            // Player 순간이동
            //pb.transform.position = dest.transform.position;            
            // 다음 Player로 턴 넘김.
            //NextTurn();
            // 이동후 HexColor를 원래색으로 reset.
            MapManager.GetInst().ResetMapColor();
        }
    }

    public void SetTurnOverTime(float time)
    {
        turnOverTime = time;
        curTurnOverTime = Time.smoothDeltaTime;
    }

    // 다음 Player로 턴 넘기기
    public void NextTurn()
    {
        MapManager.GetInst().ResetMapColor();
        // PlayerBase클래스로 pb선언, Players저장
        PlayerBase pb = Players[CurTurnIdx];
        // 턴 넘기고 대기 상태로 전환
        pb.act = ACT.IDLE;
        // Turn Index 증가
        CurTurnIdx++;
        // Index가 List크기보다 크거나같아지면,
        if(CurTurnIdx >= Players.Count)
        {
            // 다시 처음 Player로 돌아옴
            CurTurnIdx = 0;
        }
        GameManager.GetInst().MoveCamPosToHex(Players[CurTurnIdx].CurHex);
    }

    public void RemovePlayer(PlayerBase pb)
    {
        // 2-09:53분
        int pos = Players.IndexOf(pb);
        // Turn표시 캐릭터 제거
        GUIManager.GetInst().RemoveTurnPlayer(pos);
        // userPlayer 제거(이건 list에서 삭제)
        Players.Remove(pb);
        // monster 제거(이건 게임화면의 오브젝트 삭제)
        GameObject.Destroy(pb.gameObject);

        int enemyCnt = 0;
        int userCnt = 0;
        foreach (PlayerBase pb2 in Players)
        {
            if(pb2 is Monster)
            {
                enemyCnt++;
            }
            else if(pb2 is UserPlayer)
            {
                userCnt++;
            }
        }

        if(enemyCnt == 0)
        {
            EventManager.GetInst().GameEnd = true;
            GameManager.GetInst().ShowStageClear();
        }
        else if(userCnt == 0)
        {
            EventManager.GetInst().GameEnd = true;
            GameManager.GetInst().ShowGameOver();
        }
    }
    
    public void MouseInputProc(int btn)
    {
        // btn = 1 : 마우스 우클릭
        if(btn == 1)
        {
            // step 0. monster턴일때는 넘어감
            PlayerBase pb = Players[CurTurnIdx];
            if(pb is Monster)
            {
                return;
            }

            // step 1. idle일때는 할일이 없음
            ACT act = Players[CurTurnIdx].act;
            if(act == ACT.IDLE)
            {
                return;
            }

            // step 2. attack이나 move일때는 하이라이트를 초기화하고 idle로 돌림
            if(act == ACT.MOVEHIGHLIGHT || act == ACT.ATTACKHIGHLIGHT)
            {
                Players[CurTurnIdx].act = ACT.IDLE;
                MapManager.GetInst().ResetMapColor();
            }
        }
    }
}
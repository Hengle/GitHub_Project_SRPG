using UnityEngine;

// 1-10
public class UserPlayer : PlayerBase
{

    void Awake()
    {
        // 현재 상태를 대기로 초기화
        act = ACT.IDLE;
        // Player의 status정보 가져옴
        status = new PlayerStatus();
        // 2-04: Animator component받음
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        PlayerManager pm = PlayerManager.GetInst();

        // 2-06:41분18초
        // 죽으면(removeTime = 애니메이션 끝나는 2초가 되면) 삭제
        if (removeTime != 0f)
        {
            removeTime += Time.deltaTime;
            if (removeTime >= 2f)
            {
                pm.RemovePlayer(this);
                // turn을 넘기지 않거나, RemovePlayer()전에 넘기면 에러뜸
                pm.NextTurn();
                return;
            }
        }

        if (act == ACT.IDLE)
        {
            if (pm.Players[pm.CurTurnIdx] == this)
            {
                MapManager.GetInst().SetHexColor(CurHex, Color.gray);
            }
        }
        else if (act == ACT.MOVING)
        {
            ani.SetBool("user_walking", true);

            HexGrid nextHex = MoveHexes[0];
            float distance = Vector3.Distance(transform.position, nextHex.transform.position);

            if (distance > 0.1f) // 이동중.
            {
                transform.position += (nextHex.transform.position - transform.position).normalized * status.MoveSpeed * Time.smoothDeltaTime;
                // 2-03:31분, 움직이는 방향으로 Player rotation 엄청헤매기때문에 여기부터보면됨
                transform.rotation = Quaternion.LookRotation((nextHex.transform.position - transform.position).normalized);
            }
            else // 목표 Hex에 도착함
            {
                transform.position = nextHex.transform.position;
                MoveHexes.RemoveAt(0);
                // 모든 Hex가 지워졌다면 = 최종 목적지에 도착
                if (MoveHexes.Count == 0)
                {
                    act = ACT.IDLE;
                    CurHex = nextHex;
                    ani.SetBool("user_walking", false);
                    PlayerManager.GetInst().NextTurn();
                }
            }
        }
    }

    // 2-05 => 2-07로 넘어가면서 GUI스크립트 따로 분출(그러면서 PlayerBase의 virtual & PlayerManager의 OnGUI도 같이 주석처리)
    //public override void DrawStatus()
    //{
    //    float btnW = 100f;
    //    float btnH = 50f;

    //    // Move버튼 그리기
    //    Rect rect = new Rect(0, (Screen.height / 2) - btnH * 4, btnW, btnH);
    //    GUI.Label(rect, "Name: " + status.Name);

    //    rect = new Rect(0, (Screen.height / 2) - btnH * 3, btnW, btnH);
    //    GUI.Label(rect, "HP: " + status.CurHP);

    //    rect = new Rect(0, (Screen.height / 2) - btnH * 2, btnW, btnH);
    //    GUI.Label(rect, "MoveRange: " + status.MoveRange);

    //    rect = new Rect(0, (Screen.height / 2) - btnH, btnW, btnH);
    //    GUI.Label(rect, "AttackRange: " + status.AtkRange);

    //    base.DrawStatus();
    //}

    //// OnGUI로 하게 되면 Player수만큼 한번에, button이 겹쳐 생성되므로
    //// DrawCommand로 그려주고 PlayerManager에서 호출해서 사용.
    //// 원래 PlayerBase에 있던걸 UserPlayer로 오버라이드사용(2-01:19분)
    //public override void DrawCommand()
    //{
    //    float btnW = 100f;
    //    float btnH = 50f;

    //    // Move버튼 그리기
    //    Rect rect = new Rect(0, Screen.height / 2, btnW, btnH);
    //    // 버튼 생성
    //    if (GUI.Button(rect, "Move"))
    //    {
    //        //Debug.Log("move button click.");
    //        // Player위치와 범위를 HighlightMoveRange로 넘김.
    //        if (MapManager.GetInst().HighlightMoveRange(CurHex, status.MoveRange) == true)
    //        {
    //            // HIGHLIGHT로 상태 전환(Move버튼을 눌렀을때만 이동하게 하려고)
    //            act = ACT.MOVEHIGHLIGHT;
    //        }
    //    }

    //    // Attack버튼 그리기
    //    rect = new Rect(0, Screen.height / 2 + btnH, btnW, btnH);
    //    if (GUI.Button(rect, "Attack"))
    //    {
    //        //Debug.Log("Attack button click.");
    //        // Player위치와 범위를 HighlightMoveRangeHighlightMoveRange로 넘김.
    //        if (MapManager.GetInst().HighlightAtkRange(CurHex, status.AtkRange) == true)
    //        {
    //            // HIGHLIGHT로 상태 전환(Move버튼을 눌렀을때만 이동하게 하려고)
    //            act = ACT.ATTACKHIGHLIGHT;
    //        }
    //    }

    //    // Turn Over버튼 그리기
    //    rect = new Rect(0, Screen.height / 2 + (btnH * 2), btnW, btnH);
    //    if (GUI.Button(rect, "Turn Over"))
    //    {
    //        //Debug.Log("Turn Over button click.");
    //        PlayerManager.GetInst().NextTurn();
    //    }

    //    base.DrawCommand();
    //}
}
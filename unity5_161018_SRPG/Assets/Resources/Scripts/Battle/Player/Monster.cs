using UnityEngine;

// 1-10
public class Monster : PlayerBase
{

    void Awake()
    {
        // 현재 상태를 대기로 초기화
        act = ACT.IDLE;
        // Player의 status정보 가져옴
        status = new PlayerStatus();
        // 애니메이터 컴포넌트
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
                // 2-10:55분
                pm.NextTurn();
                return;
            }
        }

        if (act == ACT.IDLE)
        {
            
            if (pm.Players[pm.CurTurnIdx] == this)
            {
                MapManager.GetInst().SetHexColor(CurHex, Color.gray);
                AIProc();
            }
        }
        else if (act == ACT.MOVING) // smooth이동 구현
        {
            ani.SetBool("mob_walking", true);

            if(MoveHexes.Count == 0) // 이미 목적지(player옆)에 도착한 상태
            {
                act = ACT.IDLE;
                PlayerManager.GetInst().NextTurn();
                return;
            }

            HexGrid nextHex = MoveHexes[0];
            float distance = Vector3.Distance(transform.position, nextHex.transform.position);

            if (distance > 0.1f) // 이동중.
            {
                transform.position += (nextHex.transform.position - transform.position).normalized * status.MoveSpeed * Time.smoothDeltaTime;
                // 이동방향으로 바라보기
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
                    ani.SetBool("mob_walking", false);
                    PlayerManager.GetInst().NextTurn();
                }
            }
        }
    }

    public void AIProc()
    {
        AI ai = AI.GetInst();
        // step 1. 근접한 플레이어를 찾고 바로 옆에까지 이동시도
        // 만약 이미 근접한 상태이면 act는 IDLE을 유지, 이동이 필요하면 MOVING으로 바뀜.
        ai.MoveAIToNearUserPlayer(this);

        // step 2. 유저플레이어와 근접한 상태일때는 공격을 시도한다
        if (act == ACT.IDLE)
        {
            ai.AtkMobToUser(this);
        }
    }

    void OnMouseDown()
    {
        PlayerManager pm = PlayerManager.GetInst();
        PlayerBase pb = pm.Players[pm.CurTurnIdx];
        if (pb.act == ACT.ATTACKHIGHLIGHT)
        {
            BattleManager.GetInst().AttackAtoB(pb, this, pb.skillSet);
        }
    }
}
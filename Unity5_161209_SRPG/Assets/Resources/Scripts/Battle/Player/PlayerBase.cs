using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum ACT
{   
    IDLE,
    MOVEHIGHLIGHT,
    MOVING,
    // 2-01:공격하기
    ATTACKHIGHLIGHT,
    ATTACKING,
    DEATH
}

public enum SKILL
{
    NONE,
    SKILL1,
    SKILL2,
    SKILL3,
    SKILL4
}

public class PlayerBase : MonoBehaviour
{
    // 2-04: Animator ani변수선언(Awake지움)
    public Animator ani;
    // Player의 status정보 가져옴
    public PlayerStatus status;
    // 현재 Player가 위치한 Hex(바닥에 깔린거)의 위치
    public HexGrid CurHex;
    // 목적지 Hex의 위치
    public List<HexGrid> MoveHexes;
    // 상태 선언
    public ACT act;
    // 사용할 스킬
    public SKILL skillSet = SKILL.NONE;
    // 삭제되는 시간
    public float removeTime = 0f;

    public GameObject tempCanvas;
    public Slider healthBarSlider;

    void Start()
    {
        tempCanvas = GameObject.Find("Canvas").transform.FindChild("Slider_Health").gameObject;
        healthBarSlider = tempCanvas.GetComponent<Slider>();
        healthBarSlider.maxValue = status.CurHP;
    }

    //void Update ()
    //{
    // 1-07:27분
    //if (act == ACT.MOVING)
    //{
    //    HexGrid nextHex = MoveHexes[0];

    //    float distance = Vector3.Distance(transform.position, nextHex.transform.position);
    //    // 1-08:46분부터 길찾기 알고리즘 추가되면서 수정함
    //    // 그전까지는 일직선으로 이동했는데 알고리즘이 적용되면 셀(Hex)단위로 움직임
    //    if (distance > 0.1f) // 이동중.
    //    {
    //        transform.position += (nextHex.transform.position - transform.position).normalized * status.MoveSpeed * Time.smoothDeltaTime;
    //    }
    //    else // 목표 Hex에 도착함
    //    {
    //        transform.position = nextHex.transform.position;
    //        MoveHexes.RemoveAt(0);
    //        // 모든 Hex가 지워졌다면 = 최종 목적지에 도착
    //        if(MoveHexes.Count == 0)
    //        {
    //            act = ACT.IDLE;
    //            CurHex = nextHex;
    //            PlayerManager.GetInst().NextTurn();
    //        }
    //    }
    //}
    //}

    //public virtual void DrawStatus()
    //{

    //}

    //public virtual void DrawCommand()
    //{

    //}

    // 2-01:29분48초
    public void GetDamage(int damage)
    {
        status.CurHP -= damage;
        
        if (status.CurHP <= 0)
        {
            Debug.Log(this.tag + " Die..ㅠㅠ");
            if (this.tag == "Player")
            {
                healthBarSlider.value -= damage;
                ani.SetTrigger("user_die");
            }
            else
            {
                ani.SetTrigger("mob_death");
            }
            act = ACT.DEATH;
            // 캐릭터 삭제 시간
            removeTime += Time.deltaTime;
        }
        else
        {
            Debug.Log(this.tag + " Hit !");
            if(this.tag == "Player")
            {
                healthBarSlider.value -= damage;
                ani.SetTrigger("user_hited");
            }
            else
            {
                ani.SetTrigger("mob_hit");
            }
        }
    }
}
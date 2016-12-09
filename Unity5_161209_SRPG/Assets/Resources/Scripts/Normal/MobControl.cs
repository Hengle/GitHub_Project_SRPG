using UnityEngine;
using System.Collections; // IEnumerator 사용(코루틴)
using UnityEngine.SceneManagement; // SceneManager 사용(화면전환)

public class MobControl : MonoBehaviour
{
    // 몬스터의 상태
    public enum MOBSTATE
    {
        MOVE = -1,  // 대기
        TRACE = 0,  // 추적
        ATTACK,     // 공격
        DIE         // 주금
    };
    // 몬스터 상태 초기화
    public MOBSTATE mobState = MOBSTATE.MOVE;

    // 몬스터 애니메이터
    private Animation animation;
    // 각종 컴포넌트 미리 저장
    private Transform monsterTrans;
    private Transform playerTrans;
    
    static public Vector3 offset = Vector3.zero;  // 생성 위치
    static public Vector3 nextPos = Vector3.zero; // 다음 이동 위치
    private float moveTimer = 0f;           // 이동 방향을 변경시킬 타이머
    private bool gizmos = false;

    // Use this for initialization
    void Start()
    {
        // 애니메이터 컴포넌트 저장
        animation = transform.GetComponentInChildren<Animation>();
        animation.CrossFade("walk", 0.3f);
        animation["walk"].wrapMode = WrapMode.Loop;

        // 몬스터 위치정보
        monsterTrans = GetComponent<Transform>();
        // 플레이어 위치정보
        playerTrans = GameObject.FindWithTag("Player").GetComponent<Transform>();
        // 생성 위치를 저장
        offset = transform.position;
        offset.y = 0f;
        // 씬전환시 오브젝트가 삭제되지 않게 함
        //DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(UserControl.mobFlag == true)
        {
            switch (mobState)
            {
                case MOBSTATE.MOVE:
                    MobStateIdle();
                    break;
                case MOBSTATE.TRACE:
                    MobStateTrace();
                    break;
                case MOBSTATE.ATTACK:
                    MobStateAttack();
                    break;
                case MOBSTATE.DIE:
                    MobStateDie();
                    break;
            }
        }
    }

    public void resetPos(Transform pos)
    {

    }
    
    public void MobStateIdle()
    {
        moveTimer -= Time.deltaTime;
        if (moveTimer < 0f)
        {
            moveTimer = Random.Range(2f, 4f);
            // 1. 방향을 정한다.
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            // 2. 다음 이동 위치 정한다.(임시)
            //   nextPos는 회전 할 때 다음 위치를 알아야 하기 때문에
            nextPos = transform.position + (direction * 10f);
        }

        float speed = 0.5f;
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        // 3. 다음 위치 - 현재 위치 => 방향 & 크기
        //    다음 위치와의 회전 정보를 얻어온다.
        Vector3 v = nextPos - transform.position;
        v.y = 0f;
        Quaternion q = Quaternion.LookRotation(v);
        // 4. 구형 보간 처리 : 
        // Slerp(현재 회전 정보, 최종 회전 정보, 회전 비율)
        // 회전 비율은 0에 가까울 수록 회전 반경이 커지고.
        //            1에 가까울 수록 회전 반경은 작아진다.
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 2f);
        float length = Vector3.Distance(offset, transform.position);
        // 행동 반경을 벋어났으므로 반경 안으로 위치를 보정한다.
        if (length > 5f)
        {
            moveTimer = Random.Range(2f, 4f);
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            direction.Normalize();  // 정규화(크기를 1로 만든다)
            nextPos = offset + direction;
        }

        if (IsAvalidChase() == true)
        {
            gizmos = true;
            mobState = MOBSTATE.TRACE;

            animation["walk"].speed = 1f;
        }
        else
        {
            gizmos = false;
        }
    }

    public void MobStateTrace()
    {
        float speed = 2f;
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

        Vector3 v = playerTrans.position - transform.position;
        v.y = 0f;
        Quaternion q = Quaternion.LookRotation(v);
        // 4. 구형 보간 처리 : 
        // Slerp(현재 회전 정보, 최종 회전 정보, 회전 비율)
        // 회전 비율은 0에 가까울 수록 회전 반경이 커지고.
        //            1에 가까울 수록 회전 반경은 작아진다.
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 2f);

        float length = Vector3.Distance(playerTrans.position, transform.position);
        if (length < 1f)
        {
            gizmos = false;
            mobState = MOBSTATE.ATTACK;
            animation.CrossFade("MobAttack", 0.3f);
        }
    }

    public void MobStateAttack()
    {
        // 애니메이션이 진행 중이니?
        if (animation.IsPlaying("MobAttack") == false)
        {
            gizmos = true;
            mobState = MOBSTATE.TRACE;
            animation.CrossFade("walk", 0.3f);
        }
    }

    public void MobStateDie()
    {

    }

    // 추적 여부 검사
    bool IsAvalidChase()
    {
        // 자신이 현재 향한 방향을 보관.
        Vector3 heading = transform.TransformDirection(Vector3.forward);
        // 플레이어의 방향을 보관.
        Vector3 other = transform.position - playerTrans.position;

        heading.y = 0.0f;
        other.y = 0.0f;
        heading.Normalize(); // 길이를 1로 하고 방향만 있는 벡터로.
        other.Normalize(); // 길이를 1로 하고 방향만 있는 벡터로.

        float dp = Vector3.Dot(heading, other); // 두 벡터의 내적을 구한다.
                                                // 내적이 60도인 코사인값보다 크면 시야에 있는 것이다.

        if (dp > Mathf.Cos(60f * Mathf.Deg2Rad))
        {
            float length = Vector3.Distance(transform.position, playerTrans.position);

            return (length <= 3f) ? true : false;
        }

        return false;
    }

    void OnDrawGizmos()
    {
        if (this == null || playerTrans == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(offset, 5f);

        Gizmos.color = Color.red;
        Vector3 heading = transform.position + transform.TransformDirection(Vector3.forward);
        Gizmos.DrawLine(transform.position, heading);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, playerTrans.position);

        if (gizmos == true)
        {
            Gizmos.color = Color.red;
            Vector3 pos = playerTrans.position + new Vector3(0f, 1.2f, 0f);
            Gizmos.DrawSphere(pos, 0.15f);
        }
    }
}
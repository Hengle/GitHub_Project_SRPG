using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserControl : MonoBehaviour
{
    // 페이드 이미지(판넬)
    public Image fade;
    // 투명도 조절
    private float fades = 0.0f;
    // 페이드 시간
    private float time = 0f;
    // 페이드 on/off
    private bool fadeSet = true;

    // move ver.1
    // 애니메이터 변수 선언
    private Animator animator;
    // 플레이어 바라보는 방향
    private Vector3 lookDirection;
    // 플레이어 회전 속도
    private float turnSpeed = 3f;

    // move ver.2(default)
    // 캐릭터 회전 속도
    public float rotateSpeed;
    // 캐릭터 이동 속도
    public float forwardSpeed;
    // CharacterController 변수 선언
    private CharacterController playerController;

    // Use this for initialization
    void Start()
    {
        // 플레이어의 애니메이터 컴포넌트 저장
        animator = GameObject.Find("UserPlayer").GetComponentInChildren<Animator>();

        // 플레이어의 캐릭터 컨트롤러 컴포넌트 저장
        playerController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //// move ver.1
        //float x = Input.GetAxisRaw("Vertical");     // 전,후
        //float z = Input.GetAxisRaw("Horizontal");   // 좌,우
        //// if문으로 방향이 0일때는 회전하지 않게함.
        //if (!(x == 0 && z == 0))
        //{
        //    // 바라보는 방향
        //    lookDirection = x * Vector3.forward + z * Vector3.right;
        //    // 회전
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDirection),
        //        Time.fixedDeltaTime * turnSpeed);
        //    // 이동
        //    transform.Translate(Vector3.forward * Time.fixedDeltaTime);
        //}

        // move ver.2(default)
        transform.Rotate(0f, Input.GetAxis("Horizontal") * rotateSpeed, 0f);
        Vector3 forward = transform.TransformDirection(Vector3.back);

        float speed = forwardSpeed * Input.GetAxis("Vertical");
        playerController.SimpleMove(speed * forward);

        // 점프
        if (Input.GetKeyDown("space") && playerController.isGrounded)
        {
            playerController.Move(Vector3.up);
        }

        // 걷는 애니메이션
        if (speed != 0)
        {
            animator.SetBool("user_walk2", true);
        }
        else
        {
            animator.SetBool("user_walk2", false);
        }

        // 테스트 스킬
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("user_skill");
        }

        // 씬 페이드 아웃
        if (fadeSet == false)
        {
            time += Time.smoothDeltaTime;
            if (time >= 0.1f)
            {
                fades += 0.01f;
                fade.color = new Color(1f, 1f, 1f, fades);
            }
            if (fades >= 1.0f)
            {
                fadeSet = true;
                time = 0f;
                SceneManager.LoadScene(1);
            }
        }
    }

    // 적과 부딪치면 페이드아웃 켜지면서 전투씬으로 화면전환
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Monster")
        {
            Debug.Log("적 조우 !!!");
            animator.SetTrigger("user_attack");
            fadeSet = false;
            SoundManager.GetInst().PlayBattleBuzzer(this.transform.position);
        }
    }
}
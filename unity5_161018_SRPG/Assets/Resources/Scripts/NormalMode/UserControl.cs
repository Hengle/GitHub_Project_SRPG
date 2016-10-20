using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserControl : MonoBehaviour
{
    Animator animator;
    Vector3 lookDirection;
    float turnSpeed = 3f;

    public Image fade;
    float fades = 0.0f;
    float time = 0f;
    bool fadeSet = true;

    // Use this for initialization
    void Start()
    {
        animator = GameObject.Find("UserPlayer").GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxisRaw("Vertical");     // 전,후
        float z = Input.GetAxisRaw("Horizontal");   // 좌,우
        
        if (x != 0 || z != 0)
        {
            animator.SetBool("user_walk2", true);
        }
        else
        {
            animator.SetBool("user_walk2", false);
        }

        // 기술
        if (Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("user_skill");
        }

        // if문으로 방향이 0일때는 회전하지 않게함.
        if (!(x == 0 && z == 0))
        {
            // 바라보는 방향
            lookDirection = x * Vector3.forward + z * Vector3.right;
            // 회전
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDirection),
                Time.fixedDeltaTime * turnSpeed);
            // 이동
            transform.Translate(Vector3.forward * Time.fixedDeltaTime);
        }

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
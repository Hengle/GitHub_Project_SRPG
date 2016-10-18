using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UserControl : MonoBehaviour
{
    Animator animator;
    Vector3 lookDirection;
    float turnSpeed = 3f;
    float x;
    float z;

    // Use this for initialization
    void Start()
    {
        animator = GameObject.Find("UserPlayer").GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Vertical");     // 전,후
        z = Input.GetAxisRaw("Horizontal");   // 좌,우
        
        if (x != 0 || z != 0)
        {
            animator.SetBool("user_walk2", true);
        }
        else
        {
            animator.SetBool("user_walk2", false);
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
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Monster")
        {
            Debug.Log("부딪혔다~~");
            animator.SetTrigger("user_attack");
            StartCoroutine(SceneChanges());
        }
    }

    IEnumerator SceneChanges()
    {
        // 3초 대기 후 아랫단 실행
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }
}
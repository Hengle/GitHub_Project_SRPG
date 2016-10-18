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
        time += Time.deltaTime;
        if (time >= 0.1f)
        {
            fades += 0.1f;
            fade.color = new Color(0f, 0f, 0f, fades);
        }
        else if (fades >= 1.0f)
        {
            // 3초 대기 후 아랫단 실행
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(1);
            time = 0f;
        }
    }
}
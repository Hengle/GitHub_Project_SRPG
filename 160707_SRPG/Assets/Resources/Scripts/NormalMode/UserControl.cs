using UnityEngine;
using System.Collections;

public class UserControl : MonoBehaviour
{
    Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Debug.Log("V : " + v);
        Debug.Log("H : " + h);
        animator.SetFloat("user_walk_float", v);
        animator.SetFloat("user_walk_float", h);

        transform.Translate(Vector3.forward * v * Time.deltaTime);  // 전/후
        transform.Translate(Vector3.right * h * Time.deltaTime);    // 좌/우
    }
}

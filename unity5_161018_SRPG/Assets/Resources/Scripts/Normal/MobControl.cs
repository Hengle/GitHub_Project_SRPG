using UnityEngine;

public class MobControl : MonoBehaviour
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

    }

    //void OnTriggerEnter(Collider col)
    //{
    //    if (col.tag == "Player")
    //    {
    //        Debug.Log("Player랑 부딪힘");
    //        SceneManager.LoadScene(1);
    //    }
    //    else
    //    {
    //        Debug.Log("??????????????");
    //    }
    //}
}
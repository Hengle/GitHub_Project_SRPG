using UnityEngine;
using System.Collections;

public class MobAttack : MonoBehaviour
{

    public void OnAttack()
    {
        GetComponent<CapsuleCollider>().enabled = true;
    }

    public void OffAttack()
    {
        GetComponent<CapsuleCollider>().enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            //GameObject.Find("GameRoot").GetComponent<GameStatus>().addSatiety(-0.1f);

            Debug.Log("맞았다 !!!!!");
        }
    }
}

using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour
{
    public GameObject A;  //A라는 GameObject변수 선언
    Transform AT;

	// Use this for initialization
	void Start ()
    {
        AT = A.transform;
    }
	
	// Update is called once per frame
	void LateUpdate ()
    {
        transform.position = new Vector3(AT.position.x, AT.position.y + 5f, transform.position.z);
	}
}

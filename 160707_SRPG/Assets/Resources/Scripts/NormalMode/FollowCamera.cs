using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    Vector3 offset;

    // Use this for initialization
    void Start ()
    {
        offset = transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = target.position;   // 바로 어께 위에서 따라 감 
        transform.position = target.position + offset;
    }
}

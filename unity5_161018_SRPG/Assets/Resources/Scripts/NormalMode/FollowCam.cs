using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour
{
    // 카메라가 쫒을 타겟
    public Transform cameraTarget;
    private float x = 0.0f;
    private float y = 0.0f;

    private int mouseXSpeedmod = 5;
    private int mouseYSpeedmod = 3;
    // 최대 줌아웃 크기
    public float maxViewDistance = 6;
    // 최소 줌인 크기
    public float minViewDistance = 1;
    // 줌 속도
    public int zoomRate = 30;
    // 플레이어와의 거리
    private float distance = 15.0f;
    // 플레이어와의 거리 계산용
    private float desiredDistance;
    private float correctedDistance;

    // Use this for initialization
    void Start ()
    {
        Vector3 angles = transform.eulerAngles;
        
        y = angles.y + 60f;
    }
	
	void LateUpdate ()
    {
        // 마우스 왼쪽(0)버튼 누르고 드래그하면 카메라 회전
        if(Input.GetMouseButton(0))
        {
            Debug.Log("Mouse X: " + Input.GetAxis("Mouse X"));
            Debug.Log("Mouse Y: " + Input.GetAxis("Mouse Y"));
            x += Input.GetAxis("Mouse X") * mouseXSpeedmod;
            y -= Input.GetAxis("Mouse Y") * mouseYSpeedmod;
        }
        // 위,아래 방향 최대/최소 회전각
        y = ClampAngle(y, -50, 80);
        // 회전 값 저장
        Quaternion rotations = Quaternion.Euler(y, x, 0);

        // 마우스 휠:카메라 줌인/아웃 거리 계산
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
        desiredDistance = Mathf.Clamp(desiredDistance, minViewDistance, maxViewDistance);
        correctedDistance = desiredDistance;
        // 거리(위치) 값 저장
        Vector3 positions = cameraTarget.position - (rotations * Vector3.forward * desiredDistance);

        // main camera의 rotations 적용
        transform.rotation = rotations;
        // main camera의 potitions 적용
        transform.position = positions;
    }

    // 위,아래 방향 최대/최소 회전각에 도달했을때
    private static float ClampAngle(float angle, float min, float max)
    {
        if(angle < -360)
        {
            angle += 360;
        }
        if(angle > 360)
        {
            angle -= 360;
        }

        return Mathf.Clamp(angle, min, max);
    }
}

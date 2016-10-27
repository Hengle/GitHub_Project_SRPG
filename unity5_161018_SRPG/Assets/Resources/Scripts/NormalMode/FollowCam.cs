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
    // 카메라 줌 속도
    public int zoomRate = 30;
    // 카메라 회전 속도
    private int lerpRate = 5;
    // 플레이어와의 거리(카메라 현재위치=5만큼 떨어지게 설정)
    private float desiredDistance = 5f;
    // 수정?된 거리..
    private float correctedDistance;
    // 
    public float cameraTargetHeight = 1.0f;

    // Use this for initialization
    void Start ()
    {
        // 카메라 각도 초기화
        Vector3 angles = transform.eulerAngles;
        y = angles.y + 60f;
    }
	
	void LateUpdate ()
    {
        // 마우스 왼쪽(0)버튼 누르고 드래그하면 카메라 회전
        if(Input.GetMouseButton(0))
        {
            //Debug.Log("Mouse X: " + Input.GetAxis("Mouse X"));
            //Debug.Log("Mouse Y: " + Input.GetAxis("Mouse Y"));
            x += Input.GetAxis("Mouse X") * mouseXSpeedmod;
            y -= Input.GetAxis("Mouse Y") * mouseYSpeedmod;
        }
        else if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            float targetRotationAngle = cameraTarget.eulerAngles.y;
            float cameraRotationAngle = transform.eulerAngles.y;
            x = Mathf.LerpAngle(cameraRotationAngle, targetRotationAngle, lerpRate * Time.deltaTime);
        }

        // 위,아래 방향 회전 최대/최소각
        y = ClampAngle(y, -50, 80);
        // 회전 값 저장
        Quaternion rotations = Quaternion.Euler(y, x, 0);

        // 마우스 휠:카메라 줌인/아웃 거리 구함
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
        desiredDistance = Mathf.Clamp(desiredDistance, minViewDistance, maxViewDistance);
        correctedDistance = desiredDistance;
        // 거리(위치) 값 저장
        Vector3 positions = cameraTarget.position - (rotations * Vector3.forward * desiredDistance);

        //RaycastHit collisionHit;
        //Vector3 cameraTargetPosition = new Vector3(cameraTarget.position.x, cameraTarget.position.y + cameraTargetHeight, cameraTarget.position.z);

        //bool isCorrected = false;
        //if(Physics.Linecast(cameraTargetPosition, positions, out collisionHit))
        //{
        //    positions = collisionHit.point;
        //    correctedDistance = Vector3.Distance(cameraTargetPosition, positions);
        //}

        // main camera의 rotations 적용
        transform.rotation = rotations;
        // main camera의 potitions 적용
        transform.position = positions;
    }

    // 위,아래 방향 회전 최대/최소값에 도달했을때
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

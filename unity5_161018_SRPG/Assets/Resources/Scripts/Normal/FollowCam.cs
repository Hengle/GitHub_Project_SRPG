using UnityEngine;
using UnityEngine.EventSystems;     // PointerEventData와 RaycastResult 사용
using UnityEngine.UI;               // GraphicRaycaster 사용
using System.Collections.Generic;   // List 사용

public class FollowCam : MonoBehaviour
{
    // 카메라가 쫒을 타겟
    public Transform cameraTarget;
    private float x = 0.0f;
    private float y = 0.0f;

    // 화면 회전 속도(마우스 기준)
    private int mouseXSpeedmod = 5;
    private int mouseYSpeedmod = 3;
    // 최대 줌아웃 크기
    public float maxViewDistance = 6;
    // 최소 줌인 크기
    public float minViewDistance = 1;
    // 카메라 줌 속도
    public int zoomRate = 30;
    // 카메라 회전 속도(캐릭터 기준)
    private int lerpRate = 5;
    // 거리 초기값(카메라 현재위치=플레이어와 5만큼 떨어지게 설정)
    private float distance = 5f;
    // 플레이어와의 거리 계산용
    private float desiredDistance;
    // 수정?된 거리
    private float correctedDistance;
    private float currentDistance;
    // 플레이어의 
    public float cameraTargetHeight = 1.0f;

    // Raycast가 될 캔버스 
    public Canvas canvas; 
    private GraphicRaycaster gr;
    private PointerEventData ped;


    // Use this for initialization
    void Start ()
    {
        // 카메라 각도 초기화
        Vector3 angles = transform.eulerAngles;
        y = angles.y + 60f;
        // 카메라와 플레이어간의 거리 초기화
        currentDistance = distance;
        desiredDistance = distance;
        correctedDistance = distance;

        // 캔버스에 대한 Raycast를 사용하기위한 컴포넌트 저장
        gr = canvas.GetComponent<GraphicRaycaster>();
        // 마우스 포인터 이벤트
        ped = new PointerEventData(null);
    }
	
	void LateUpdate ()
    {
        // 현재 포인터의 위치는 마우스 클릭 포지션임
        ped.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>(); // 여기에 히트 된 개체 저장 
        gr.Raycast(ped, results);

        // results에 개체가 들어오면
        if(results.Count != 0)
        {
            GameObject obj = results[0].gameObject;
            if(obj.CompareTag("Window") || obj.CompareTag("Inventory")) // 히트 된 오브젝트의 태그와 맞으면 실행 
            {
                //Debug.Log("Inventory Window hit !");
                // 선택한 창만 움직이고, 하위 코드는 실행하지 않는다(return)
                return;
            }
        }

        // 마우스 왼쪽(0)버튼 누르고 드래그하면 카메라 회전
        if(Input.GetMouseButton(0))
        {
            //Debug.Log("Mouse X: " + Input.GetAxis("Mouse X"));
            //Debug.Log("Mouse Y: " + Input.GetAxis("Mouse Y"));
            x += Input.GetAxis("Mouse X") * mouseXSpeedmod;
            y -= Input.GetAxis("Mouse Y") * mouseYSpeedmod;
        }
        else if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            // 카메라 = 캐릭터가 바라보는 방향으로 따라다님
            float targetRotationAngle = cameraTarget.eulerAngles.y;
            float cameraRotationAngle = transform.eulerAngles.y;
            x = Mathf.LerpAngle(cameraRotationAngle, targetRotationAngle, lerpRate * Time.deltaTime);
        }

        // 위,아래 방향 회전 최대/최소각
        y = ClampAngle(y, -50, 80);
        // 카메라 (상하좌우) 회전 값 저장
        Quaternion rotations = Quaternion.Euler(y, x, 0);

        // 카메라 줌 인/아웃 거리 구함(마우스 휠)
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
        desiredDistance = Mathf.Clamp(desiredDistance, minViewDistance, maxViewDistance);
        correctedDistance = desiredDistance;
        // 거리(위치) 값 저장
        Vector3 positions = cameraTarget.position - (rotations * Vector3.forward * desiredDistance);


        // =========================코드해석해야됨============================
        RaycastHit collisionHit;
        Vector3 cameraTargetPosition = new Vector3(cameraTarget.position.x, cameraTarget.position.y + cameraTargetHeight, cameraTarget.position.z);

        bool isCorrected = false;
        if(Physics.Linecast(cameraTargetPosition, positions, out collisionHit))
        {
            positions = collisionHit.point;
            correctedDistance = Vector3.Distance(cameraTargetPosition, positions);
            isCorrected = true;
        }

        // condition ? first_expression : second_expression
        currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * zoomRate) : correctedDistance;

        positions = cameraTarget.position - (rotations * Vector3.forward * currentDistance + new Vector3());
        // ===================================================================

        positions.y += 1f; // 카메라가 너무 바닥에 붙어서 살짝 올림.

        // Main Camera의 rotations 적용
        transform.rotation = rotations;
        // Main Camera의 potitions 적용
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

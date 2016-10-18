using UnityEngine;
using System.Collections;

public class ShakeCam : MonoBehaviour
{
    public enum STATE
    {
        NORMAL,     // 고정 카메라
        SHAKE,      // 흔들리는 연출 카메라
    }

    public STATE state = STATE.NORMAL;
    public float randShake = 0f;
    private float timer = 0f;
    private Vector3 oriPos = Vector3.zero; // 초기 카메라 위치 정보
    
    void Awake()
    {
        oriPos = this.transform.position;
        SetShake();
    }

    public void SetShake()
    {
        timer = 0f;
        state = STATE.SHAKE;
    }

    void Update()
    {
        if (state == STATE.SHAKE)
        {
            float x = Random.Range(-randShake, randShake);     // 0f <= value <= 1f 
            float y = Random.Range(-randShake, randShake);
            float z = Random.Range(-randShake, randShake);
            this.transform.position = oriPos + new Vector3(x, y, z);

            timer += Time.deltaTime;
            if (timer > 2f)
            {
                state = STATE.NORMAL;
                this.transform.position = oriPos;
            }
        }
    }
}
using UnityEngine;
using System.Collections;

public class ToggleLogin : MonoBehaviour
{
    // 로그인 오브젝트 저장
    public GameObject login;

    public void ToggleLoginWindow()
    {
        Debug.Log("Toggle Login Window !");
        if (login == null)
        {
            return;
        }
        // 현재 로그인창 활성화 상태 정보를 얻어 온다.
        bool flag = login.activeSelf;
        // 활성화 상태값의 반대 값으로 설정한다.
        login.SetActive(!flag);
    }
}

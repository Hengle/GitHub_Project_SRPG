using UnityEngine;

public class ToggleLogin : MonoBehaviour
{
    // 로그인 오브젝트
    public GameObject login;
    // 메시지박스 오브젝트
    public GameObject messageBox;

    // 로그인 창 토글
    public void LoginWindow()
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

    // 메시지 박스 토글
    public void MessageBox()
    {
        Debug.Log("Toggle MessageBox !");
        if (messageBox == null)
        {
            return;
        }
        bool flag = messageBox.activeSelf;
        messageBox.SetActive(!flag);
    }
}

using UnityEngine;
using System; // DateTime
using System.Collections.Generic; // Dictionary
using JsonFx.Json; // JsonReader
using UnityEngine.SceneManagement; // SceneManager
using UnityEngine.UI; // InputField -> NGUI 안써보려고..

public class LoginManager : MonoBehaviour
{
    // NGUI
    public UIInput id;
    public UIInput pw;
    // uGUI
    public InputField uid;
    public InputField upw;

    public void RequestLogin()
    {
        if (uid.text.Length < 4 || upw.text.Length < 4)
        {
            Debug.Log("계정과 암호는 4글자 이상으로 만들어야 합니다. 확인하고 재시도 바랍니다.");
            return;
        }

        Dictionary<string, object> sendData = new Dictionary<string, object>();
        // "폴더명", "login".php
        sendData.Add("contents", "login");
        // "query_value", inputfield.text
        sendData.Add("id", uid.text);
        sendData.Add("pw", upw.text);
        //sendData.Add("id", id.value);
        //sendData.Add("pw", pw.value);

        StartCoroutine(NetworkManagerEX.Instance.ProcessNetwork(sendData, ReplyLogin));
    }

    private class RecvLoginData
    {
        public int account;
        public string acc_name;
        public int timestamp;
        public string message;
    }
    
    public void ReplyLogin(string json)
    {
        RecvLoginData data = JsonReader.Deserialize<RecvLoginData>(json);

        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(data.timestamp);

        Debug.Log(data.message);
        Debug.Log((origin.ToLocalTime()).ToString("yyyy년 MM월 dd일의 tt HH시 mm분 s초에 로그인 했습니다."));

        SceneManager.LoadScene(0);
    }
}
﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;       // List, Dictionary  사용을 위해 추가해야 한다.
using JsonFx.Json;                      // Assets/Plugins/JsonFx.Json.dll을 사용하기 위해 추가 한다.

public class NetworkManager : MonoBehaviour
{
    // 델리게이트 함수 : 같은 구조를 가진 다른 이름의 함수를 대신 호출해 주는 함수
    // 델리케이트 함수 타입을 선언 : void(리턴값이 없고) ReplyData(string 문자열 인자를 가진 ) 델리게이트 함수
    // delegate는 실행 흐름(구조)이 같지만 전달되는(또는 전달 받는) 데이타 구조가 다를 때 사용 한다.
    public delegate void ReplyData(string json);

    public MessageBox messagebox;

    //====================== Sinlgeton Code ========================
    public static NetworkManager _instance = null;
    public static NetworkManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // MonoBehaviour에서는 new를 만들 수 없다.
                Debug.LogError("Singleton is Null");
            }
            return _instance;
        }
    }
    // Awake에서 자신을 인스턴스로 등록 한다.
    void Awake()
    {
        _instance = this;
        // 다른 씬으로 넘어가더라도 메모리에서 삭제하지 않는다.
        DontDestroyOnLoad(this);
    }
    //====================== =============== ========================

    // sendData : 전송할 데이타 ( 전송될 데이타 구조는 다를 수 있다. >>> Dictionary 사용 )
    // ReplyData  : 수신된 데이타 ( 수신된 데이타의 구조는 다를 수 있다. >>> delegate )
    // ReplyData는 delegate로 선언이 되어 있다.
    public IEnumerator ProcessNetwork(Dictionary<string, object> sendData, ReplyData _reply)
    {
        // 2. JSON 형식으로 변환
        string json = JsonWriter.Serialize(sendData);

        // 3. POST 방식의 데이타로 만든다.
        WWWForm form = new WWWForm();
        form.AddField("json", json);

        string URL = "http://52.78.153.240/161024_Project_SRPG/RequestClient.php";
        // 4. HTTP 프로토콜을 이용하여 데이타를 전송한다. ( Request : Send data)
        WWW www = new WWW(URL, form);
        // 송신이 완료될 때까지 대기한다.
        yield return www;
        // 여기서 부터는 송신이 완료된 시점.

        // 5. 회신이 올 때 까지 기다리면서 오류 유무를 체크 한다.
        while (!www.isDone && string.IsNullOrEmpty(www.error))
        {
            yield return null;
        }
        // 6. 수신이 완료된 상태 : www.isDone

        // 7. 수신된 데이타에 에러가 있으면 에러를 출력한다. 
        // 프로토콜 단에서 에러 발생 : URL이 잘못됐을 때나, 네트워크가 끊겼을 때
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
        }
        else     // 정상 수신 되었다.
        {
            if (www.text[0] == '{') // JSON 데이타 인가? 
            {
                // JSON 데이타를 Dictionary로 변환
                Dictionary<string, object> receivePacket =
                    (Dictionary<string, object>)JsonReader.Deserialize(www.text, typeof(Dictionary<string, object>));
                // 처리 결과를 출력
                if (receivePacket.ContainsKey("result"))
                {
                    Debug.Log("Receive Result code : " + receivePacket["result"]);
                    int errorcode = (int)receivePacket["result"];
                    switch (errorcode)
                    {
                        case 1000:
                            Debug.Log("패스워드가 올바르지 않습니다. \n확인하고 다시 시도하시기 바랍니다.");
                            messagebox.MessageBoxOpen("Failed Login", "비밀번호가 올바르지 않습니다. 확인하고 재시도 바랍니다.");
                            break;
                        case 1001:
                            Debug.Log("등록된 계정이 아닙니다. \n확인하고 다시 시도하시기 바랍니다.");
                            messagebox.MessageBoxOpen("Failed Login", "없는 계정입니다. 확인하고 재시도 바랍니다.");
                            break;
                        case 1002:
                            Debug.Log("캐릭터 생성에 실패했습니다.");
                            messagebox.MessageBoxOpen("Failed Create Character", "캐릭터 생성에 실패했습니다.");
                            break;
                        case 1003:
                            Debug.Log("존재하지 않는 캐릭터 입니다.");
                            messagebox.MessageBoxOpen("Not Found Character", "캐릭터를 찾지 못했습니다.");
                            break;
                        case 1004:
                            Debug.Log("인벤토리 정보를 찾을 수 없습니다.");
                            messagebox.MessageBoxOpen("Not Found Inventory", "인벤토리 정보를 찾을 수 없습니다.");
                            break;
                        case 1005:
                            Debug.Log("아이템 정보를 찾을 수 없습니다. \n확인하고 다시 시도하시기 바랍니다.");
                            messagebox.MessageBoxOpen("Not Found Item Info", "아이템 정보를 찾을 수 없습니다.");
                            break;
                        case 1006:
                            Debug.Log("골드가 부족합니다. \n확인하고 다시 시도하시기 바랍니다.");
                            messagebox.MessageBoxOpen("Gold is not enough !", "보유 골드가 충분치 않습니다.");
                            break;
                        case 1007:
                            Debug.Log("인벤토리에 빈 슬롯이 없습니다. \n확인하고 다시 시도하시기 바랍니다.");
                            messagebox.MessageBoxOpen("All Slots Full", "인벤토리가 가득 찼습니다.");
                            break;
                        case 1008:
                            Debug.Log("아이디 생성에 실패했습니다. \n확인하고 다시 시도하시기 바랍니다.");
                            messagebox.MessageBoxOpen("Failed New Join !", "아이디가 중복됩니다. 확인하고 다시 시도하시기 바랍니다.");
                            break;
                    }
                }
                // 실제 처리 결과물
                if (receivePacket.ContainsKey("data"))
                {
                    // delegate 함수에게 결과물을 돌려 준다.(결과물만 JSON을 변환해서 전달)
                    _reply(JsonWriter.Serialize(receivePacket["data"]));
                }
                Debug.Log("Receive JSON : " + www.text);
            }// end if()
            else
            {
                Debug.Log("Receive Debugging : " + www.text);
            }
        }// end if()
    }

    // DisplayDialog 사용해보자
    //public void DialogOpen(string message)
    //{
    //    if (UnityEditor.EditorUtility.DisplayDialog(" Message Box", message, "OK", "Cancel"))
    //    {
    //        Debug.Log("ok");
    //    }
    //    else
    //    {
    //        Debug.Log("cancel");
    //    }
    //}
}
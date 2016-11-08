using UnityEngine;
using System; // DateTime
using System.Collections.Generic; // Dictionary
using JsonFx.Json; // JsonReader
using UnityEngine.SceneManagement; // SceneManager
using UnityEngine.UI; // InputField -> NGUI 안써보려고..

public class LoginManager : MonoBehaviour
{
    // uGUI
    public InputField uid;
    public InputField upw;

    void Start()
    {
        ItemData.Instance.LoadTable();
    }

    // UI 구성 전, Test용 Update()
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameData.Instance.charInfo.char_id = 4;
            RequestGetCharacterInfo();
            RequestGetInventoryInfo();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Skill Data Count: " + GameData.Instance.charInfo.skill.Count);
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            ITEM item = ItemData.Instance.GetInfo(11311001);
            if (item != null)
            {
                Debug.Log("ItemData로 확인한 아이템 정보: " + item.name);
                Debug.Log(item.description);
            }
        }
        if(Input.GetKeyDown(KeyCode.V))
        {
            RequestGetItemInfo(11211001);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            RequestSetBuyItem(13211001);
        }
    }

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

        SceneManager.LoadScene(1);
    }

    public void RequestGetCharacterInfo()
    {
        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "get_character_info"); // contents : 디렉토리명, get_character_info : php 파일명
        sendData.Add("char_id", GameData.Instance.charInfo.char_id);

        StartCoroutine(NetworkManagerEX.Instance.ProcessNetwork(sendData, ReplyGetCharacterInfo));
    }

    private class RecvGetCharInfoData
    {
        public string message;
        public int timestamp;
        public int char_id;
        public string nickname;
        public int clas;
        public int level;
        public int exp;
        public int stat_hp;
        public int stat_at;
        public int stat_df;
        public List<int> skill = new List<int>();
        public List<int> equip_item = new List<int>();
        public int gold;
    }

    public void ReplyGetCharacterInfo(string json)
    {
        RecvGetCharInfoData data = JsonReader.Deserialize<RecvGetCharInfoData>(json);

        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(data.timestamp);
        Debug.Log(data.message + (origin.ToLocalTime()).ToString(" 응답시간 yyyy-MM-dd-tt HH:mm:s"));

        GameData.Instance.charInfo.nickname = data.nickname;
        GameData.Instance.charInfo.clas = data.clas;
        GameData.Instance.charInfo.level = data.level;
        GameData.Instance.charInfo.exp = data.exp;
        GameData.Instance.charInfo.stat_hp = data.stat_hp;
        GameData.Instance.charInfo.stat_at = data.stat_at;
        GameData.Instance.charInfo.stat_df = data.stat_df;
        GameData.Instance.charInfo.skill = data.skill;
        GameData.Instance.charInfo.equip_item = data.equip_item;
        GameData.Instance.charInfo.gold = data.gold;

        Debug.Log("Completed to save game info !! " + data.gold + " Gold");
    }

    public void RequestGetInventoryInfo()
    {
        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "get_inventory_info");
        sendData.Add("char_id", GameData.Instance.charInfo.char_id);

        StartCoroutine(NetworkManagerEX.Instance.ProcessNetwork(sendData, ReplyGetInventoryInfo));
    }

    private class RecvGetInvenInfoData
    {
        public string message;
        public int timestamp;
        public List<InvenSlotInfo> inventory = new List<InvenSlotInfo>();
    }

    public void ReplyGetInventoryInfo(string json)
    {
        RecvGetInvenInfoData data = JsonReader.Deserialize<RecvGetInvenInfoData>(json);

        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(data.timestamp);
        Debug.Log(data.message + (origin.ToLocalTime()).ToString(" 응답시간 yyyy-MM-dd-tt HH:mm:s"));

        GameData.Instance.invenInfo = data.inventory;

        Debug.Log("Completed to save inventory info !!");
    }

    public void RequestGetItemInfo(int item_id)
    {
        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "get_item_info");
        sendData.Add("item_id", item_id);

        StartCoroutine(NetworkManagerEX.Instance.ProcessNetwork(sendData, ReplyGetItemInfo));
    }

    private class RecvGetItemInfoData
    {
        public string message;
        public int timestamp;
        public ITEM item;
    }

    public void ReplyGetItemInfo(string json)
    {
        RecvGetItemInfoData data = JsonReader.Deserialize<RecvGetItemInfoData>(json);

        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(data.timestamp);
        Debug.Log(data.message + (origin.ToLocalTime()).ToString(" 응답시간 yyyy-MM-dd-tt HH:mm:s"));

        Debug.Log("Item ID: " + data.item.id);
        Debug.Log("Item Attack: " + data.item.attack);
        Debug.Log("Item Defence: " + data.item.defence);
        Debug.Log("Item Name: " + data.item.name);
        Debug.Log("Item Description: " + data.item.description);
        Debug.Log("Completed to save item info !!");
    }

    public void RequestSetBuyItem(int item_id)
    {
        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "set_buy_item");
        sendData.Add("char_id", GameData.Instance.charInfo.char_id);
        sendData.Add("item_id", item_id);

        StartCoroutine(NetworkManagerEX.Instance.ProcessNetwork(sendData, ReplySetBuyItem));
    }

    private class RecvSetBuyItemData
    {
        public string message;
        public int timestamp;
        public int item_id;
        public int slot;
        public int gold;
    }

    public void ReplySetBuyItem(string json)
    {
        RecvSetBuyItemData data = JsonReader.Deserialize<RecvSetBuyItemData>(json);

        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(data.timestamp);
        Debug.Log(data.message + (origin.ToLocalTime()).ToString(" 응답시간 yyyy-MM-dd-tt HH:mm:s"));

        GameData.Instance.charInfo.gold = data.gold;
        GameData.Instance.invenInfo[data.slot].item_id = data.item_id;
        GameData.Instance.invenInfo[data.slot].item_count = 1;

        ITEM item = ItemData.Instance.GetInfo(data.item_id);
        if(item != null)
        {
            Debug.Log(item.name + "가(이) " + item.price + "가격에 구매 됐습니다.");
            Debug.Log("아이템 설명: " + item.description);
        }
    }
}
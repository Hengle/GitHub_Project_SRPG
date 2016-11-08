using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public enum ITEM_GENDER
{
    COMMON = 1,
    MALE = 2,
    FEMALE = 3
}

public enum ITEM_TYPE
{
    EQUIP = 1,
    UNIVERSAL = 2,
    CONSUME = 3,
    COOPON = 4
}

public enum ITEM_TYPE_EQUIP
{
    WEAPON = 1,
    HELMET = 2,
    SHIELD = 3,
    ARMOR = 4,
    SHOES = 5,
    RING = 6,
    NECKLACE = 7
}

public enum ITEM_TYPE_UNIVERSAL
{
    MATERIAL = 1,
    ENCHANT = 2,
    QUEST = 3,
    GOLD = 4
}

public enum ITEM_TYPE_CONSUME
{
    SKILL = 1,
    HEALING = 2,
    MANA = 3,
    DEBUFF = 4,
    BUFF = 5
}

public enum ITEM_TYPE_COOPON
{
    REUSE = 1,
    ONETIME = 2
}

public enum ITEM_TYPE_CLASS
{
    COMMON = 1,
    WARRIOR = 2,
    ARCHER = 3,
    MAGICIAN = 4
}

public enum ITEM_TYPE_GRADE
{
    BEGINNER = 1,
    JUNIOR = 2,
    MAJOR = 3,
    RARE = 4,
    LEGEND =  5
}

public class ItemData
{
    // ===================== 싱글톤 인스턴스 저장 =====================
    // volatile 동시에 실행되는 여러 쓰레드에 의해 필드가 수정을 될수 있음을 나타낸다
    private static volatile ItemData uniqueInstance;
    private static object _lock = new System.Object();
    // 생성자
    private ItemData() { }
    // static : 전역으로 존재(외부에서 접근할 수 있도록 함)
    public static ItemData Instance
    {
        get
        {
            if (uniqueInstance == null)
            {
                // _lock으로 지정된 블록안의 코드를 하나의 쓰레드만 접근하도록 한다
                lock (_lock)
                {
                    uniqueInstance = new ItemData();
                }
            }
            return uniqueInstance;
        }
    }
    // ===================== ===================== =====================

    // 아이템 아이디를 Key로 사용
    private Dictionary<int, ITEM> table = new Dictionary<int, ITEM>();
    
    // 파일경로 알아오기: 실행 파일이 실행되는 위치의 경로를 얻어온다(Get Project Root Path)
    public string pathForDocumentsFile(string fileName)
    {
        // Application.dataPath: 실행파일이 실행된 위치경로
        if(Application.platform == RuntimePlatform.IPhonePlayer) // iOS에서 경로를 얻어온다
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            // 왜 내꺼만 System.IO에 접근이 안되는가.. 일단 수동(강제?)적으로 Path앞에 붙여서 해결
            return System.IO.Path.Combine(System.IO.Path.Combine(path, "Documents"), fileName);
        }
        else if(Application.platform == RuntimePlatform.Android) // Android에서 경로를 얻어온다
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return System.IO.Path.Combine(path, fileName);
        }
        else // PC 또는 Web의 경로를 얻어온다
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return System.IO.Path.Combine(path, fileName);
        }
    }

    // 실행파일의 Root 경로에 파일을 읽어오기
    public void LoadTable()
    {
        string path = pathForDocumentsFile("Tables/item_table.csv");
        if(File.Exists(path) == false)
        {
            Debug.Log("파일이 존재하지 않습니다 !" + path);
            return;
        }

        string str;
        table.Clear();

        FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader st = new StreamReader(file);

        while((str = st.ReadLine()) != null)
        {
            string[] datas = Regex.Split(str, "\r\n");
            // 유저 정보를 보관해둔다
            foreach(string data in datas)
            {
                // 데이터가 존재하지 않으면 foreach()문을 빠져나감
                if(data == "" || data.Length == 0)
                {
                    break;
                }
                // '#'문자로 시작하는 데이터는 무시
                if(data[0] == '#')
                {
                    continue;
                }
                string[] temp = data.Split(',');    // 콤마()
                int key = int.Parse(temp[0]);
                table.Add(key, new ITEM
                {
                id = int.Parse(temp[0]),
                attack = int.Parse(temp[6]),
                defence = int.Parse(temp[7]),
                //durability = int.Parse(temp[8]),
                fire_regist = int.Parse(temp[9]),
                ice_regist = int.Parse(temp[10]),
                regeneration = int.Parse(temp[11]),
                cooltime = float.Parse(temp[12]),
                weight = int.Parse(temp[13]),
                level = int.Parse(temp[14]),
                price = int.Parse(temp[15]),
                skill_01 = int.Parse(temp[16]),
                skill_02 = int.Parse(temp[17]),
                overlap = int.Parse(temp[18]),
                name = temp[19],
                description = temp[20],
                icon = temp[21],
                sound = temp[22],
                effect = temp[23]
                });
                Debug.Log("Dictionary table에 item_table.csv 데이터 등록(temp[0]): " + temp[0]);
            } // end foreach()
        } // end while()
        st.Close();
        file.Close();

        Debug.Log("파일 읽기 완료 [" + path + "]");
    }

    // key는 곧 아이템 아이디
    public ITEM GetInfo(int key)
    {
        // 해당하는 키의 아이템이 존재하면 아이템 정보를 리턴한다
        if (table.ContainsKey(key) == true)
        {
            return table[key];
        }
        // 해당 키의 아이템이 없으면 null 리턴
        return null;
    }
}

public class ITEM
{
    public int id;         // 성별, 타입, 직업, 등급, 인덱스

    public int attack;          // 공격력
    public int defence;         // 방어력
    //public int durability;      // 내구력
    public int fire_regist;     // 화염 저항
    public int ice_regist;      // 냉기 저항
    public int regeneration;    // 회복력
    public float cooltime;      // 쿨타임
    public int weight;          // 무게
    public int level;           // 허용 레벨
    public int price;           // 가격
    public int skill_01;        // 스킬 번호
    public int skill_02;        // "
    public int overlap;         // 중첩 가능 여부
    public int count;           // 아이템 개수 
    public string name;         // 아이템 이름
    public string description;  // 아이템 설명
    public string icon;         // 아이콘 이름
    public string sound;        // 효과음 이름
    public string effect;       // 이펙트 이름
}
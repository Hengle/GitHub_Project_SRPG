using System.Collections.Generic; // List<>사용

// 게임이 돌아가는 내내 들고 있어야할 정보라 싱글톤으로 만듬
public class GameData
{
    // ===================== 싱글톤 인스턴스 저장 =====================
    // volatile 동시에 실행되는 여러 쓰레드에 의해 필드가 수정을 될수 있음을 나타낸다
    private static volatile GameData uniqueInstance;
    private static object _lock = new System.Object();
    // 생성자(static:전역으로존재)
    public static GameData Instance
    {
        get
        {
            if(uniqueInstance == null)
            {
                // _lock으로 지정된 블록안의 코드를 하나의 쓰레드만 접근하도록 한다
                lock(_lock)
                {
                    uniqueInstance = new GameData();
                }
            }
            return uniqueInstance;
        }
	}
    // ===================== ===================== =====================

    public HeroInfo charInfo = new HeroInfo();
    public List<InvenSlotInfo> invenInfo = new List<InvenSlotInfo>();
}

public class HeroInfo
{
    public int char_id;
    public string nickname;
    public int clas;
    public int level;
    public int exp;
    public int stat_hp;
    public int stat_at;
    public int stat_df;
    public List<int> skill;// = new List<int>();
    public List<int> equip_item;// = new List<int>();
    public int gold;
}

public class InvenSlotInfo // 인벤토리 슬롯 정보 정의
{
    public int item_id;
    public int item_count;
}
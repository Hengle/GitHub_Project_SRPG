using UnityEngine;
using System.Collections;
// TODO : 유니티 UI타입을 쓰려면 선언해야함(DamageText 표시해줄라고)
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    MapManager mm;
    PlayerManager pm;
    // 2-07:18분 GUI관리
    GUIManager gm;
    // 2-08:25분30초 코드수정후 어택땅이 안돼서 추가됨
    BattleManager bm;
    // 2-10:24분 배경음악
    SoundManager sm;
    // 2-10:59분 이벤트
    EventManager em;
    // Canvas넣을 GameObject
    GameObject canvas;
    // 2-10:1시간14분 스테이지 텍스트
    GameObject BattleStartString;
    GameObject PlayerWinString;
    GameObject GameOverString;
    // 씬 분기
    int nextTurnIdx = 0;

    private static GameManager inst = null;
    public static GameManager GetInst()
    {
        return inst;
    }
    void Awake()
    {
        inst = this;
        mm = MapManager.GetInst();
        pm = PlayerManager.GetInst();
        gm = GUIManager.GetInst();
        bm = BattleManager.GetInst();
        sm = SoundManager.GetInst();
        em = EventManager.GetInst();

        BattleStartString = GameObject.FindGameObjectWithTag("BattleStart");
        PlayerWinString = GameObject.FindGameObjectWithTag("PlayerWin");
        GameOverString = GameObject.FindGameObjectWithTag("GameOver");

        PlayerWinString.SetActive(false);
        GameOverString.SetActive(false);
    }

	void Start ()
    {
        // 맵 생성 -> 3-05:19분 맵정보 xml file을 불러와서 적용할꺼라 주석처리
        //mm.CreateMap();
        mm.CreateTestMap();
        // 플레이어 생성
        pm.GenPlayerTest();
        // 배경음악 시작
        sm.PlayMusic(transform.position);
        // DamageText를 그려낼 Canvas찾음
        canvas = GameObject.Find("Canvas");
    }
	
	void Update ()
    {
        if(em.GameEnd == true)
        {   
            return;
        }

        // 2 - 10:59분30초부터 event추가하면서 if문으로 묶음
        if (em.StageStarted == true)
        {
            // 2-10 쿼터뷰로 바뀌면서 주석처리
            //CheckMouseZoom();
            CheckMouseButtonDown();
            // 2-08 26분
            bm.CheckBattle();
            // 2-08 29분15초
            pm.CheckTurnOver();
        }
        else if(em.StageStartEvent == false)
        {   
            em.ShowStartEvent();
        }
    }

    // GUI관련된건 OnGUI에서.
    void OnGUI()
    {
        // 2-10:1시간17분20초 스테이지가 시작되면 OnGUI그림
        if(em.StageStarted == true && em.GameEnd == false)
        {
            // 2-07:18분55초 => 32분40초
            gm.DrawGUI();
            // 2-09:42분 => 2-10:48분 수정
            gm.UpdateTurnInfoPos(transform.position, transform.rotation);
        }
    }

    // 2-02 => 2-10 쿼터뷰로 바뀌면서 주석처리
    //void CheckMouseZoom()
    //{
    //    // 마우스의 최저는 7, 최대는 12
    //    float mouse = Input.GetAxis("Mouse ScrollWheel");
    //    float mouseY = camera.transform.position.y + mouse + 5f;
    //    if (mouseY < 7)
    //    {
    //        mouseY = 7;
    //    }
    //    else if (mouseY > 12)
    //    {
    //        mouseY = 12;
    //    }
    //    Vector3 newPos = new Vector3(camera.transform.position.x, mouseY, camera.transform.position.z);
    //    camera.transform.position = newPos;
    //}

    void CheckMouseButtonDown()
    {
        // TODO : 마우스 우클릭하면 하이라이트 상태 취소(IDLE 상태로 복귀)
        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log("mouse right button down");
            Debug.Log(GUIManager.GetInst().skCommand);
            pm.MouseInputProc(1);
        }
    }

    public void MoveCamPosToHex(HexGrid hex)
    {
        float destX = hex.transform.position.x;
        float destZ = hex.transform.position.z;
        // 2-10 쿼터뷰로 바뀌면서 z값에 -3.5f 더함
        GetComponent<Camera>().transform.position = new Vector3(destX, GetComponent<Camera>().transform.position.y, destZ -3.5f);
    }

    // TODO : 데미지 텍스트 표시
    public HexGrid damagedHex;
    public int damage;
    IEnumerator ShowDamage()
    {
        Vector3 objPos = new Vector3(damagedHex.transform.position.x - 0.05f, damagedHex.transform.position.y + 1.5f, damagedHex.transform.position.z - 0.5f);
        GameObject GO_DamageText = (GameObject)Resources.Load("Prefabs/Effect/Damage_Text");
        GameObject obj = (GameObject)GameObject.Instantiate(GO_DamageText, objPos, GameManager.GetInst().gameObject.transform.rotation);
        obj.transform.SetParent(canvas.transform);
        // GetComponet의 <Text>를 사용할라믄 위에 using추가해줘야됨.
        // 유니티 4.x버전부터 바뀐듯. 3.x버전에선 <TextMesh>로 접근함
        obj.GetComponent<Text>().text = "" + damage;

        // 0.5f초 대기
        yield return new WaitForSeconds(0.5f);
        // text 서서히 사라짐
        for (float i = 1; i >= 0; i -= 0.05f)
        {
            obj.GetComponent<Text>().color = new Vector4(255, 0, 0, i);
            // 0.5f초 동안 표시
            yield return new WaitForFixedUpdate();
        }

        Destroy(obj);
    }
    
    IEnumerator ShowStageStart()
    {
        // 3초 대기 후 아랫단 실행
        yield return new WaitForSeconds(3f);
        Destroy(BattleStartString);
        em.StageStarted = true;
    }

    public void ShowStageClear()
    {
        PlayerWinString.SetActive(true);
        // 씬 분기
        nextTurnIdx = 1;
        StartCoroutine(NextScene());
    }

    public void ShowGameOver()
    {
        GameOverString.SetActive(true);
        // 씬 분기
        nextTurnIdx = 2;
        StartCoroutine(NextScene());
    }

    IEnumerator NextScene()
    {
        if(nextTurnIdx == 1)
        {
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(2);
        }
        else if(nextTurnIdx == 2)
        {
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(0);
        }
    }
}
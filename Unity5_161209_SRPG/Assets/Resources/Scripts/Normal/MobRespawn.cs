using UnityEngine;
using System.Collections; // IEnumerator 사용(코루틴)

public class MobRespawn : MonoBehaviour
{
    // 몹 프리팹
    private GameObject mobPrefab = null;
    // 몹 생성 타이머
    private float createTimer = 0f;
    // 현재 몹 수
    private int mobCount = 1;
    // 남은 몹 수
    static public int mobRegen = 5;

    // Use this for initialization
    void Start ()
    {
        // 프리팹 불러옴
        if(mobPrefab == null)
        {
            mobPrefab = (GameObject)Resources.Load("Prefabs/AIPlayer/Cyclop/Cyclops(Monster)");
            Debug.Log("몹 생성: " + mobPrefab.name);
        }
        else
        {
            Debug.Log("몹 생성 error: " + mobPrefab.name);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (mobCount <= mobRegen)
        {
            createTimer -= Time.deltaTime;
            if (createTimer <= 0f)
            {
                // 생성 타이머 초기화(10초마다 하나씩 생성)
                createTimer = 10f;
                // 몬스터를 생성하고, 
                GameObject mob = Instantiate(mobPrefab);
                // 몬스터 수 1증가
                Debug.Log("현재 몹 수: " + mobCount + ",\nregen될 총 몹수: " + mobRegen);
                mobCount++;
                if (mob != null)
                {
                    // 생성 위치 지정
                    float x = Random.Range(5f, 23f);
                    float z = Random.Range(5f, 23f);
                    mob.transform.position = new Vector3(x, 1f, z);
                    Debug.Log("x,z = " + x + "," + z);
                }
                else
                {
                    Debug.Log("mob 생성 실패");
                }
            }
        }
        else if(mobRegen == 0)// mobCount가 mobRegen보다 작으면, 모든 몹을 잡았다는 것임으로
        {
            Debug.Log("현재 몹 수: " + mobRegen);
            Debug.Log("Game Clear !!");
            Time.timeScale = 0;
            return;
        }
        else
        { 
            return;
        }
    }
}
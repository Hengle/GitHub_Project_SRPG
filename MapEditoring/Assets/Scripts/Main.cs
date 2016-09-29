using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{
    // 3-01 XML 저장 및 로드 test
    FileMgr test;
    GUIMgr gm;
    MapMgr mm;

	// Use this for initialization
	void Start ()
    {
        // test로 저장 & 불러오기, 3-03하면서 주석처리
        //test = FileMgr.GetInst();
        //test.SaveData();
        //test.LoadData();

        gm = GUIMgr.GetInst();
	}
	
	// Update is called once per frame
	void Update ()
    {
        CheckMouseWheel();
        CheckArrow();
	}

    // GUI는 OnGUI에서.
    void OnGUI()
    {
        gm.DrawLeftLayout();
    }

    // 3-03 줌 인/아웃
    void CheckMouseWheel()
    {
        float wheel = Input.GetAxis("Mouse ScrollWheel");

        if(wheel != 0)
        {
            //Debug.Log("Wheel : " + wheel);
            camera.orthographicSize += wheel * 5f;
        }
    }

    // 3-03:4분30초 화면이동
    void CheckArrow()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if(vertical == 0 && horizontal == 0)
        {
            return;
        }

        transform.position = new Vector3(transform.position.x + horizontal, transform.position.y, transform.position.z + vertical);
    }
}
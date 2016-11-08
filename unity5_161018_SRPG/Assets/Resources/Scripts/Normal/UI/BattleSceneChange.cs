using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleSceneChange : MonoBehaviour
{
    // 페이드 이미지(판넬)
    public Image fade;
    // 투명도 조절
    private float fades = 0.0f;
    // 페이드 시간
    private float time = 0f;
    // 페이드 On/Off
    private bool fadeSet = true;
    
	// Update is called once per frame
	void Update ()
    {
        // 씬 페이드 아웃
        if (fadeSet == false)
        {
            time += Time.smoothDeltaTime;
            if (time >= 0.1f)
            {
                fades += 0.01f;
                fade.color = new Color(1f, 1f, 1f, fades);
            }
            if (fades >= 1.0f)
            {
                fadeSet = true;
                time = 0f;
                SceneManager.LoadScene(2);
            }
        }
    }

    // 적과 부딪치면 페이드아웃 켜지면서 전투씬으로 화면전환
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Monster")
        {
            Debug.Log("적 조우 !!!");
            fadeSet = false;
            SoundManager.GetInst().PlayBattleBuzzer(this.transform.position);
        }
    }
}

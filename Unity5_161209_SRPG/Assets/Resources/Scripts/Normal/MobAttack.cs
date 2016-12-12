using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MobAttack : MonoBehaviour
{
    // 페이드 이미지(판넬)
    public Image fade;
    // 투명도 조절
    private float fades = 0.0f;
    // 페이드 시간
    private float time = 0f;
    // 페이드 on/off
    private bool fadeSet = true;

    void Start()
    {
        fade = GameObject.Find("Canvas/Panel_Fade").GetComponent<Image>();
    }

    void Update()
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

    public void OnAttack()
    {
        GetComponent<CapsuleCollider>().enabled = true;
    }

    public void OffAttack()
    {
        GetComponent<CapsuleCollider>().enabled = false;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("맞았다 !!!!!");

            // 씬 페이드 아웃 flag
            fadeSet = false;
            SoundManager.GetInst().PlayBattleBuzzer(this.transform.position);
        }
    }
}

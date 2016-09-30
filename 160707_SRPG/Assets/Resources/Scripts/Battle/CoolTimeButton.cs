using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoolTimeButton : MonoBehaviour
{
    public Image img;
    public Button btn;
    public float cooltime = 10.0f;
    public bool disableOnStart = true;
    float leftTime = 10.0f;

	// Use this for initialization
	void Start ()
    {
	    if(img == null)
        {
            img = gameObject.GetComponent<Image>();
        }
        if(btn == null)
        {
            btn = gameObject.GetComponent<Button>();
        }
        if(disableOnStart)
        {
            ResetCooltime();
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(leftTime > 0f)
        {
            leftTime -= Time.deltaTime;
            if(leftTime == 0f)
            {
                leftTime = 0f;
                if(btn)
                {
                    btn.enabled = true;
                }
            }
            float ratio = 1.0f - (leftTime / cooltime);
            if(img)
            {
                img.fillAmount = ratio;
            }
        }
	}

    public void ResetCooltime()
    {
        leftTime = cooltime;
        if(btn)
        {
            btn.enabled = false;
        }
    }
}

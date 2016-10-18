using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    private float fadeSpeed = 0.5f;
    private int drawDepth = -1000;
    private float fadeDir = -1.0f;
    public float time;
    private float alpha = 1.0f;
    //씬이 넘어갈때 사용될 텍스쳐 그림
    public Texture2D FadeTexture;
    //로딩화면을 사용할때 사용할 텍스쳐
    //public Texture2D LodingTexture;

    void Start()
    {
        alpha = 0;
        fadeOut();
    }

    void OnGUI()
    {
        //타임저장
        time += Time.deltaTime;
        //색이 바뀌는 부분
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        //알파값 연산해서 변환
        alpha = Mathf.Clamp01(alpha);                           
        GUI.skin = null;
        Color tmpcolor = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.color = tmpcolor;
        GUI.depth = drawDepth;
        //텍스쳐를 받아 그리는 부분
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), FadeTexture);
        //페이드 아웃
        if (alpha == 1)
        {
            //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), LodingTexture);
            if (time > 10)
            {
                fadeIn();
            }
        }
        //페이드 인
        if (alpha == 0) 
        {
            fadeDir = 1;
            time = 0;
            //씬이 넘어가는거
            SceneManager.LoadScene(1);
        }

    }

    //----------------------- 
    // 사용자 정의 함수 영역 
    //-----------------------

    void fadeIn()
    {
        fadeDir = -1;
    }
    void fadeOut()//원래 대로 돌아가는 부분 
    {
        fadeDir = 1;
    }
}

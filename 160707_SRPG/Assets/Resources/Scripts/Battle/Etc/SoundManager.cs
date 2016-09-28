using UnityEngine;
using System.Collections;

public class SoundManager
{
    public AudioClip AC_Attack;
    public AudioClip AC_Music;

    // 싱글턴
    private static SoundManager inst = null;
    public static SoundManager GetInst()
    {
        if(inst == null)
        {
            inst = new SoundManager();
            inst.Init();
        }
        return inst;
    }

    // TODO : 리소스를 불러와야 뭘하든 하니깐 싱글턴에서 호출하는거 잊지말도록.
    public void Init() 
    {
        // (GameObject)가 아니라 (AudioClip)임 헷갈리지마라
        AC_Attack = (AudioClip)Resources.Load("Sound/Effect/Crash");
        AC_Music = (AudioClip)Resources.Load("Sound/battle_bgm");
    }

    // 2-08:20분30초 battle manager에서 position 넘겨받는걸로 수정됨
    public void PlayAttackSound(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(AC_Attack, pos);
    }

    // 2-10:24분 배경음악
    public void PlayMusic(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(AC_Music, pos);
    }
}
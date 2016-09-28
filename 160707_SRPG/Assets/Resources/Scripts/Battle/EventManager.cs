using UnityEngine;
using System.Collections;

public class EventManager
{
    public bool StageStartEvent = false;
    public bool StageStarted = false;
    public bool GameEnd = false;

    private static EventManager inst = null;
    public static EventManager GetInst()
    {
        if(inst == null)
        {
            inst = new EventManager();
        }
        return inst;
    }

    public void ShowStartEvent()
    {
        GameManager.GetInst().StartCoroutine("ShowStageStart");
        StageStartEvent = true;
    }
}

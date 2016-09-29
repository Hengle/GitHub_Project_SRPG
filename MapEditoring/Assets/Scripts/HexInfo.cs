using UnityEngine;
using System.Collections;

public class HexInfo : MonoBehaviour
{
    public int X;
    public int Y;
    public int Z;

    public bool Passable = true;

    void Awake ()
    {   
        transform.renderer.material.color = Color.green;
    }
	
	public void SetMapPos(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public void SetPassable(bool passable)
    {
        Passable = passable;
    }

    // 3-05:5분 Passable 켜고 끄기
    void OnMouseDown()
    {   
        //Debug.Log("mouse down");
        if(GUIMgr.GetInst().Passable)
        {
            // Hex프리팹에 material shader설정(diffuse) & mesh collider추가해이지 색바뀜
            Passable = true;
            transform.renderer.material.color = Color.green;
        }
        else
        {
            Passable = false;
            transform.renderer.material.color = Color.red;
        }
    }
}

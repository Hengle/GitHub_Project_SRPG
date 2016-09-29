using UnityEngine;
using System.Collections;

public class HexInfo : MonoBehaviour
{
    public int X;
    public int Y;
    public int Z;

    public bool Passable;

    void Awake ()
    {
        Passable = true;
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
}

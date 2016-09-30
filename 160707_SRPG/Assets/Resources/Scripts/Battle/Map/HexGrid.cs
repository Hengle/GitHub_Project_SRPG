using UnityEngine;
using System.Collections;

public class Point
{
    public int X;
    public int Y;
    public int Z;

    public Point(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    // 들어온 데이터(x,y,z)를 아래 형식으로 반환
    public override string ToString()
    {
        return "[" + X + " " + Y + " " + Z + " " + "]";
    }

    // Point +연산 오버라이드
    public static Point operator +(Point p1, Point p2)
    {
        return new Point(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
    }

    // bool 연산 오버라이드
    public static bool operator ==(Point p1, Point p2)
    {
        return (p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z);
    }

    // 위에 operator ==과 짝을 이룰 !=가 있어야해서 어쩔수없이 만듬
    public static bool operator !=(Point p1, Point p2)
    {
        return (p1.X != p2.X || p1.Y != p2.Y || p1.Z != p2.Z);
    }
}

public class HexGrid : MonoBehaviour
{
    // Map Position (x,y,z)
    public Point MapPos;
    // Hex가 지나갈수있는지 확인
    public bool Passable = true;

    public Color OriColor = Color.white;

    public void SetMapPos(Point pos)
    {
        MapPos = pos;
    }

    public void SetMapPos(int x, int y, int z)
    {
        MapPos = new Point(x, y, z);
    }

    // 마우스로 클릭한 곳의 좌표를 MovePlayer()로 넘김
    void OnMouseDown()
    {
        // 1-06:9분
        Debug.Log(MapPos + "OnMouseDown");

        PlayerManager pm = PlayerManager.GetInst();
        PlayerBase pb = pm.Players[pm.CurTurnIdx];
        
        if(pb.act == ACT.IDLE)
        {
            // 벽 토글 기능
            if (Passable == true) // 벽 On
            {
                transform.renderer.material.color = Color.yellow;
                OriColor = Color.yellow;
            }
            else // 벽 Off
            {
                transform.renderer.material.color = Color.black;
                OriColor = Color.black;
            }
            Passable = !Passable;
        }
        else if(pb.act == ACT.MOVEHIGHLIGHT) // 이동 기능
        {
            pm.MovePlayer(pm.Players[pm.CurTurnIdx].CurHex, this);
        }
    }
}

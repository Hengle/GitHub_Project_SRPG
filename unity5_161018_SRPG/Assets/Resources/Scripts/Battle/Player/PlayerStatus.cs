using UnityEngine;
using System.Collections;

public class PlayerStatus
{
    public string Name = "test";
    // userPlayer HP
    public int CurHP = 30;
    // Player 이동범위 한정
    public int MoveRange = 3;
    // Player 공격범위 한정
    public int AtkRange = 1;
    // Player 이동속도
    public float MoveSpeed = 5f;
    
    public PlayerStatus()
    {
        Name = "test";
        CurHP = 30;
        MoveRange = 3;
        AtkRange = 1;
        MoveSpeed = 5f;
    }
}
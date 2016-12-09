using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 1-10
public class AI
{
    private static AI inst = null;
    public static AI GetInst()
    {
        if (inst == null)
        {
            inst = new AI();
        }
        return inst;
    }

    public void MoveAIToNearUserPlayer(PlayerBase aiMonster)
    {
        PlayerManager pm = PlayerManager.GetInst();
        MapManager mm = MapManager.GetInst();

        // step 1. 근접한 플레이어를 찾는다
        PlayerBase nearUserPlayer = null;
        int nearDistance = 1000;
        foreach(PlayerBase up in pm.Players)
        {
            if(up is UserPlayer)
            {
                int distance = mm.GetDistance(up.CurHex, aiMonster.CurHex);
                if (nearDistance > distance)
                {
                    nearUserPlayer = up;
                    nearDistance = distance;
                }
            }
        }

        if (nearUserPlayer != null)
        {
            // step 2. 근접한 플레이어로 이동한다
            List<HexGrid> path = mm.GetPath(aiMonster.CurHex, nearUserPlayer.CurHex);
            //Debug.Log("pathCount: " + path.Count);

            // 이동범위를 넘는지 체크
            if (path.Count > aiMonster.status.MoveRange)
            {
                // ex 길이 = 10, 이동범위 = 3
                path.RemoveRange(aiMonster.status.MoveRange, path.Count - aiMonster.status.MoveRange);
                //Debug.Log("path After Remove Count: " + path.Count);
            }

            aiMonster.MoveHexes = path;
            //Debug.Log("aiMonster.MoveHexes.Count: " + aiMonster.MoveHexes.Count);

            // 근접 플레이어와 겹치는지 체크
            if (nearUserPlayer.CurHex.MapPos == aiMonster.MoveHexes[aiMonster.MoveHexes.Count -1].MapPos)
            {
                // userPlayer와 겹치지 않게함.
                aiMonster.MoveHexes.RemoveAt(aiMonster.MoveHexes.Count - 1);
            }
            
            // userPlayer가 바로 옆에 있는 경우
            if(aiMonster.MoveHexes.Count == 0)
            {
                // 종료(ACT.MOVING되면 안되니깐)
                return;
            }
            aiMonster.act = ACT.MOVING;
            MapManager.GetInst().ResetMapColor(aiMonster.CurHex.MapPos);
        }

        // step 3. 만약 근접후 공격이 가능하면 공격한다
    }

    public void AtkMobToUser(PlayerBase aiMonster)
    {
        PlayerManager pm = PlayerManager.GetInst();
        MapManager mm = MapManager.GetInst();

        // step 1. 근접한 유저플레이어를 찾는다
        PlayerBase nearUserPlayer = null;
        int nearDistance = 1000;
        foreach (PlayerBase up in pm.Players)
        {
            if (up is UserPlayer)
            {
                int distance = mm.GetDistance(up.CurHex, aiMonster.CurHex);
                if (nearDistance > distance)
                {
                    nearUserPlayer = up;
                    nearDistance = distance;
                }
            }
        }

        // step 2. 찾았다면 공격을 한다
        if(nearUserPlayer != null)
        {
            BattleManager.GetInst().AttackAtoB(aiMonster, nearUserPlayer, aiMonster.skillSet);
            return;

            // 2-06 32분30초 주석처리
            //nearUserPlayer.GetDamage(10);
            ////Debug.Log("Monster Attack!! -> Player");
            //// Player방향으로 바라본다(회전)
            //aiMonster.transform.rotation = Quaternion.LookRotation((nearUserPlayer.CurHex.transform.position - aiMonster.transform.position).normalized);
            //aiMonster.ani.SetTrigger("mob_attack");
        }

        // step 2-1. 못찾았다면 턴을 넘긴다(찾았던 못찾았던 턴은 넘긴다)
        pm.NextTurn();
    }
}
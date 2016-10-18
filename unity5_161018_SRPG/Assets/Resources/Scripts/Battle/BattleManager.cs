using UnityEngine;
using System.Collections;

public class BattleManager
{
    private float normalAttackTime = 0f;
    private PlayerBase attacker = null;
    private PlayerBase defender = null;
    bool skillSet;
    // ======================================
    // TODO :   싱글턴 만드는 거 외우기
    private static BattleManager inst = null;
    public static BattleManager GetInst()
    {
        if(inst == null)
        {
            inst = new BattleManager();
        }
        return inst;
    }
    // ======================================
        
    public void CheckBattle()/* TODO : 이부분을 호출하는 부분 필요함(노멀매니저화되면서 Hierarchy오브젝트를 없앴기때문에)
                              당연히 나중에 호출할때 이름도 바꿔서 Update()말고 다른걸로 변경 */
    {
        if (normalAttackTime != 0)
        {
            normalAttackTime += Time.smoothDeltaTime;
            if(normalAttackTime >= 0.5f)
            {
                normalAttackTime = 0f;
                // 데미지 받는 부분 처리
                //Debug.Log("Attack!! " + attacker.status.Name + " to " + defender.status.Name);
                Debug.Log("Attack!! " + attacker.tag + " to " + defender.tag);
                if(skillSet == false)
                {
                    // TODO : 데미지를 준다(only one)
                    defender.GetDamage(10);
                    // 데미지 화면 표시
                    EffectManager.GetInst().ShowDamage(defender.CurHex, 10);
                }
                else
                {
                    defender.GetDamage(20);
                    EffectManager.GetInst().Skill1(defender.transform);
                    EffectManager.GetInst().ShowDamage(defender.CurHex, 20);
                }
                EffectManager.GetInst().ShowEffect(defender.transform);
                SoundManager.GetInst().PlayAttackSound(attacker.transform.position);
                // turn 넘기는 시간
                PlayerManager.GetInst().SetTurnOverTime(2.5f);
            }
        }
	}

    public void AttackAtoB(PlayerBase a, PlayerBase b, bool skill)
    {
        // attack 애니메이션이 때리는 시점은 0.18초
        // Monster(or Player)방향으로 바라본다(회전)
        a.transform.rotation = Quaternion.LookRotation((b.CurHex.transform.position - a.transform.position).normalized);
        if(skill == false)
        {
            if (a.tag == "Player")
            {
                a.ani.SetTrigger("user_attack");
            }
            else
            {
                a.ani.SetTrigger("mob_attack");
            }
        }
        else // 나중에 일반공격이랑 구분되게 애니메이션 바꿔줘야함
        {
            if (a.tag == "Player")
            {
                a.ani.SetTrigger("user_attack");
            }
        }
        a.act = ACT.ATTACKING;
        normalAttackTime = Time.smoothDeltaTime;
        attacker = a;
        defender = b;
        skillSet = skill;
    }
}
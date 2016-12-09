using UnityEngine;
using System.Collections;

public class BattleManager
{
    private float normalAttackTime = 0f;
    private PlayerBase attacker = null;
    private PlayerBase defender = null;
    private SKILL skillState;

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
        // 공격 개시
        if (normalAttackTime != 0)
        {
            normalAttackTime += Time.smoothDeltaTime;
            if(normalAttackTime >= 0.5f)
            {
                normalAttackTime = 0f;
                //Debug.Log("Attack!! " + attacker.status.Name + " to " + defender.status.Name);
                Debug.Log("Attack!! " + attacker.tag + " to " + defender.tag);
                // attacker를 확인해서 skill 분기
                if(attacker.tag == "Player")
                {
                    UserSkillEffect();
                }
                else
                {
                    MobSkillEffect();
                }
            }
        }
	}

    public void AttackAtoB(PlayerBase a, PlayerBase b, SKILL skill)
    {
        // attack 애니메이션이 때리는 시점은 0.18초
        // Monster(or Player)방향으로 바라본다(회전)
        a.transform.rotation = Quaternion.LookRotation((b.CurHex.transform.position - a.transform.position).normalized);
        
        a.act = ACT.ATTACKING;
        normalAttackTime = Time.smoothDeltaTime;
        attacker = a;
        defender = b;
        skillState = skill;

        SkillAnimation();
    }
    
    // TODO : 스킬 애니메이션 분기
    public void SkillAnimation()
    {
        // Monster skillState 랜덤 변경(skillSet을 하나만 쓰기때문에 먼저 셋팅하는거임)
        int a = Random.Range(0, 2);
        if(attacker.tag == "Monster")
        {
            Debug.Log("mob random skill : " + a);
            if (a <= 0)
            {
                skillState = SKILL.NONE;
            }
            else if(a <= 1)
            {
                skillState = SKILL.SKILL1;
            }
            //else if (a <= 2)
            //{
            //    skillState = SKILL.SKILL2;
            //}
            //else if (a <= 3)
            //{
            //    skillState = SKILL.SKILL3;
            //}
            //else if (a <= 4)
            //{
            //    skillState = SKILL.SKILL4;
            //}
            else
            {
                skillState = SKILL.NONE;
            }
        }

        if (skillState == SKILL.NONE)// 그냥 공격 '애니메이션'
        {
            if (attacker.tag == "Player")
            {
                attacker.ani.SetTrigger("user_attack");
            }
            else
            {
                attacker.ani.SetTrigger("mob_attack");
            }
        }
        else if (skillState == SKILL.SKILL1)// 스킬 애니메이션
        {
            if (attacker.tag == "Player")
            {
                attacker.ani.SetTrigger("user_attack");
            }
            else
            {
                attacker.ani.SetTrigger("mob_attack");
            }
        }
        else if (skillState == SKILL.SKILL2)// 스킬 애니메이션
        {
            if (attacker.tag == "Player")
            {
                attacker.ani.SetTrigger("user_skill");
            }
            else
            {
                attacker.ani.SetTrigger("mob_attack");
            }
        }
    }

    // [ UserPlayer ] 의 일반공격 & 스킬 '이펙트' 분기
    public void UserSkillEffect()
    {
        switch (skillState)
        {
            case SKILL.NONE:
                // TODO : 데미지 값을 넘겨준다
                defender.GetDamage(10);
                // 데미지 화면 표시
                EffectManager.GetInst().ShowDamageText(defender.CurHex, 10);
                // 기본 데미지 이펙트
                EffectManager.GetInst().ShowDamageEffect(defender.transform);
                // 데미지 입는 소리
                SoundManager.GetInst().PlayAttackSound(attacker.transform.position);
                // turn 넘기는 시간
                PlayerManager.GetInst().SetTurnOverTime(2.5f);
                break;
            case SKILL.SKILL1:
                defender.GetDamage(20);
                EffectManager.GetInst().IceAge(defender.transform);
                EffectManager.GetInst().ShowDamageEffect(defender.transform);
                EffectManager.GetInst().ShowDamageText(defender.CurHex, 20);
                SoundManager.GetInst().PlayAttackSound(attacker.transform.position);
                PlayerManager.GetInst().SetTurnOverTime(2.5f);
                break;
            case SKILL.SKILL2:
                defender.GetDamage(30);
                EffectManager.GetInst().TornadoBlade(defender.transform);
                EffectManager.GetInst().ShowDamageEffect(defender.transform);
                EffectManager.GetInst().ShowDamageText(defender.CurHex, 30);
                SoundManager.GetInst().PlayAttackSound(attacker.transform.position);
                PlayerManager.GetInst().SetTurnOverTime(4f);
                break;
            case SKILL.SKILL3:
                
                break;
            case SKILL.SKILL4:

                break;
        }
    }

    // [ Monster ] 의 일반공격 & 스킬 '이펙트' 분기
    public void MobSkillEffect()
    {
        switch (skillState)
        {
            case SKILL.NONE:
                defender.GetDamage(10);
                EffectManager.GetInst().ShowDamageEffect(defender.transform);
                EffectManager.GetInst().ShowDamageText(defender.CurHex, 10);
                SoundManager.GetInst().PlayAttackSound(attacker.transform.position);
                PlayerManager.GetInst().SetTurnOverTime(2.5f);
                break;
            case SKILL.SKILL1:
                defender.GetDamage(5);
                EffectManager.GetInst().BeastBite(defender.transform);
                EffectManager.GetInst().ShowDamageText(defender.CurHex, 5);
                SoundManager.GetInst().PlayAttackSound(attacker.transform.position);
                PlayerManager.GetInst().SetTurnOverTime(2.5f);
                break;
            case SKILL.SKILL2:

                break;
            case SKILL.SKILL3:

                break;
            case SKILL.SKILL4:

                break;
        }
    }
}
﻿using UnityEngine;

// TODO : 2-08(여기부터 시작) 스크립트 & 코드 정리 작업하면서 코드가 조금씩달라짐
// 필요치않게 MonoBehaviour쓰인 부분, 주로 Manager들을 NormalClass로 수정(아닌것도 있음!)
// 싱글턴 수정, 드래그&드롭을 코드로 대체 등등
// 노멀매니저화하고 드래그&드롭을 코드로 대체 된 스크립트는 Hierarchy 오브젝트 삭제..해도 되는가봄
public class EffectManager
{   
    public GameObject GO_AttackEffect;
    public GameObject GO_pSkill1;
    public GameObject GO_pSkill2;
    public GameObject GO_mSkill1;

    // 싱글턴
    private static EffectManager inst = null;
    public static EffectManager GetInst()
    {
        // 2-08
        if(inst == null) // TODO : Awake()의 inst = this를 대체함
        {
            inst = new EffectManager();
            // 호출해줘야해!!!꼭!!
            inst.Init();
        }
        return inst;
    }

    public void Init()
    {
        // GO_AttackEffect에 damage이펙트를 드래그&드롭한것과 같음(이건 코드화한거임)
        GO_AttackEffect = (GameObject)Resources.Load("Prefabs/Effect/Attack");
        GO_pSkill1 = (GameObject)Resources.Load("Prefabs/Effect/skill1");
        GO_pSkill2 = (GameObject)Resources.Load("Prefabs/Effect/Hit_blood");
        GO_mSkill1 = (GameObject)Resources.Load("Prefabs/Effect/Claw_Hit");
    }

    // 피 터지는 효과
    public void ShowDamageEffect(Transform other)
    {
        GameObject go = (GameObject)GameObject.Instantiate(GO_AttackEffect, other.transform.position, other.transform.rotation);
        //Debug.Log("defender name : " + other.name);

        Vector3 effectPos = go.transform.position;
        effectPos.y = effectPos.y + 0.7f;
        go.transform.position = effectPos;
    }

    // 2-10:8분 데미지 텍스트 표시
    public void ShowDamageText(HexGrid hex, int damage)
    {
        GameManager.GetInst().damagedHex = hex;
        GameManager.GetInst().damage = damage;
        GameManager.GetInst().StartCoroutine("ShowDamage");
    }

    // Player skill
    public void IceAge(Transform target)
    {
        GameObject skill = (GameObject)GameObject.Instantiate(GO_pSkill1, target.transform.position, Quaternion.Euler(0f, 0f, 0f));
    }

    // Player skill
    public void TornadoBlade(Transform target)
    {
        GameObject go = (GameObject)GameObject.Instantiate(GO_pSkill2, target.transform.position, Quaternion.Euler(0f, 0f, 0f));
        Vector3 effectPos = go.transform.position;
        effectPos.y = effectPos.y + 0.7f;
        go.transform.position = effectPos;
    }

    // Monster skill
    public void BeastBite(Transform target)
    {
        GameObject go = (GameObject)GameObject.Instantiate(GO_mSkill1, target.transform.position, Quaternion.Euler(0f, 0f, 0f));
        Vector3 effectPos = go.transform.position;
        effectPos.y = effectPos.y + 0.7f;
        go.transform.position = effectPos;
    }
}
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] public int m_partsCountMax = 1;
    [SerializeField] public List<PartsBody> m_partsBodyList = new List<PartsBody>();
    [SerializeField] public float m_health;
    [SerializeField] public SpaceShip m_target;

    virtual protected void Start()
    {
        
    }

    public void Initialize(GameObject partsBodyPrefab, GameObject partsWeaponPrefab)
    {
        m_health = 100.0f;

        GameObject body = Instantiate(partsBodyPrefab, transform.position, transform.rotation);
        body.transform.SetParent(transform);
        PartsBody partsBody = body.AddComponent<PartsBody>();
        partsBody.Initialize(partsWeaponPrefab);
        m_partsBodyList.Add(partsBody);


        //GameObject weapon = Instantiate(partsWeaponPrefab, ship.transform.position, ship.transform.rotation);





        StartCoroutine(AutoCombat());
    }

    private IEnumerator AutoCombat()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f); // 1초마다 공격
            Attack();
        }

    }


    public void Attack()
    {
        m_target = ObjectManager.Instance.GetEnemy();
        if(m_target == null) return;

        for(int i=0; i< m_partsBodyList.Count; i++)
        {
            if (m_partsBodyList[i].m_health <= 0) continue;
            m_partsBodyList[i].Attack(m_target);
        }
    }

    public void UpgradeParts()
    {
        // 임시로 0번 인덱스만 업그레이드
        m_partsBodyList[0].UpgradePart();
    }

    virtual public void TakeDamage(float attackPower)
    {
        m_health -= attackPower;
        if (m_health <= 0.0f)
            Debug.Log("SpaceShip Down");
    }


}
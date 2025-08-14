//------------------------------------------------------------------------------
using NUnit.Framework;
using System;
using UnityEngine;
using System.Collections.Generic;

public class PartsBase : MonoBehaviour
{
    [SerializeField] public int m_classId;
    [SerializeField] public int m_positionIndex;

    [SerializeField] public int m_level = 1;
    [SerializeField] public float m_health;
    [SerializeField] public float m_attackPower;

    [SerializeField] public int m_upgradeMoneyCost = 10;
    [SerializeField] public int m_upgradeMaterialCost = 5;

    virtual public void Start()
    {
        
    }

    virtual public void UpgradePart()
    {
        if (ObjectManager.Instance.m_myMoney < m_upgradeMoneyCost
            || ObjectManager.Instance.m_myMaterial < m_upgradeMaterialCost)
            return;

        ObjectManager.Instance.m_myMoney -= m_upgradeMoneyCost;
        ObjectManager.Instance.m_myMaterial -= m_upgradeMaterialCost;

        m_level += 1;
        // 상세한 능력치 조절이 필요함, 레벨별 종류별 능력치 정보 table 만들 예정


    }

    virtual public void TakeDamage(float damage)
    {
        m_health -= damage;
        if (m_health < 0.0f) m_health = 0.0f;
        Debug.Log($"PartsBase took {damage} damage, remaining health: {m_health}");
        
    }

    virtual public void Attack(SpaceShip target)
    {
        target.TakeDamage(m_attackPower);
    }

}

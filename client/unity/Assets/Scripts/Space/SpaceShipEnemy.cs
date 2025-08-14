//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Runtime.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class SpaceShipEnemy : SpaceShip
{
    // enemy�� ���� �ӽ÷� spaceShip �� ���ݷ��� ����
    [SerializeField] public float attackPower;

    override protected void Start()
    {
        //base.Start();
        
        //Initialize();
    }

    public void Initialize(SpaceShip target)
    {
        m_target = target;
        m_health = 50.0f;

        attackPower = 10.0f;

        StartCoroutine(AutoCombat());
    }

    private IEnumerator AutoCombat()
    {
        while (m_health > 0 && m_target.m_health > 0)
        {
            yield return new WaitForSeconds(1.0f); // 1�ʸ��� ����
            //m_target.TakeDamage(attackPower);
        }
    }

    override public void TakeDamage(float damage)
    {
        m_health -= damage;
        if (m_health <= 0)
        {
            Int64 moneyAmount = UnityEngine.Random.Range(10, 50);
            ObjectManager.Instance.m_myMoney += moneyAmount;
            ObjectManager.Instance.m_enemyList.Remove(this);
            Destroy(gameObject);
        }
    }
}

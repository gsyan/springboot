//------------------------------------------------------------------------------
using UnityEngine;

public class PartsWeapon : PartsBase
{
    [HideInInspector] public Transform m_originTransform;

    override public void Start()
    {
        m_classId = 1;
        m_positionIndex = 1;
        m_level = 1;
        m_health = 50.0f;
        m_attackPower = 10.0f;

        m_upgradeMoneyCost = 10;
        m_upgradeMaterialCost = 5;

        
    }

    //override public void Attack(SpaceShip target)
    //{
    //    base.Attack(target);
    //}
}

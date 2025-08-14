//------------------------------------------------------------------------------
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PartsBody : PartsBase
{
    List<PartsWeapon> m_weapons = new List<PartsWeapon>();

    override public void Start()
    {
        m_classId = 1;
        m_positionIndex = 1;
        m_level = 1;
        m_health = 100.0f;
        m_attackPower = 0.0f;

        m_upgradeMoneyCost = 10;
        m_upgradeMaterialCost = 5;

    }

    public void Initialize(GameObject partsWeaponPrefab)
    {
        Transform WeaponPositions = transform.Find("WeaponPositions");
        foreach(Transform child in WeaponPositions)
        {
            GameObject tempObj = Instantiate(partsWeaponPrefab, child.position, child.rotation);
            PartsWeapon tempWeapon = tempObj.AddComponent<PartsWeapon>();
            tempWeapon.transform.SetParent(transform);
            tempWeapon.m_originTransform = child;
            m_weapons.Add(tempWeapon);
        }
    }


    override public void Attack(SpaceShip target)
    {
        for (int i = 0; i < m_weapons.Count; i++)
        {
            m_weapons[i].Attack(target);
        }
    }

}

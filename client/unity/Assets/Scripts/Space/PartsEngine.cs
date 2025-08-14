//------------------------------------------------------------------------------
using UnityEngine;

public class PartsEngine : PartsBase
{
    override public void Start()
    {
        m_classId = 1;
        m_positionIndex = 1;
        m_level = 1;
        m_health = 50.0f;
        m_attackPower = 0.0f;

        m_upgradeMoneyCost = 10;
        m_upgradeMaterialCost = 5;
    }
}

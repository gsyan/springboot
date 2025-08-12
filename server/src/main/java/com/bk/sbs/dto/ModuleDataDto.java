package com.bk.sbs.dto;

import lombok.Data;

@Data
public class ModuleDataDto {
    private int m_type; // EModuleType 값 (0: Body, 1: Weapon, 2: Engine)
    private int m_level;
    public float m_health;

    public int m_attackFireCount;
    public float m_attackPower;
    public float m_attackCoolTime;

    public float m_movementSpeed;
    public float m_rotationSpeed;
    public float m_cargoCapacity;

    public int m_upgradeMoneyCost;
    public int m_upgradeMaterialCost;
}
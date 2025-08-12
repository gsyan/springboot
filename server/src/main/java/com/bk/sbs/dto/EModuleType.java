package com.bk.sbs.dto;

public enum EModuleType {
    Body(0),
    Weapon(1),
    Engine(2);

    private final int value;

    EModuleType(int value) {
        this.value = value;
    }

    public int getValue() {
        return value;
    }
}

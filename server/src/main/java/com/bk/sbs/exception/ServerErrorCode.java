
package com.bk.sbs.exception;

public enum ServerErrorCode {
    SUCCESS(0, "Success"),
    ACCOUNT_REGISTER_FAIL_REASON1(1001, "Account registration failed due to duplicate email"),
    LOGIN_FAIL_REASON1(2001, "Invalid email or password"),
    CHARACTER_CREATE_FAIL_REASON1(3001, "Character creation failed"),
    UNKNOWN_ERROR(Integer.MAX_VALUE, "Unknown error");

    private final int code;
    private final String message;

    ServerErrorCode(int code, String message) {
        this.code = code;
        this.message = message;
    }

    public int getCode() {
        return code;
    }

    public String getMessage() {
        return message;
    }
}
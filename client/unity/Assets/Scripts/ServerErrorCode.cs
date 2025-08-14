//---------------------------------------------------------------------------------
using System.Collections.Generic;

 public enum ServerErrorCode
{
    SUCCESS = 0,
    ACCOUNT_REGISTER_FAIL_REASON1 = 1001,
    LOGIN_FAIL_REASON1 = 2001,
    CHARACTER_CREATE_FAIL_REASON1 = 3001,
    UNKNOWN_ERROR = int.MaxValue,
}

public static class ErrorCodeMapping
{
    public static readonly Dictionary<ServerErrorCode, string> Messages = new Dictionary<ServerErrorCode, string>
    {
        { ServerErrorCode.SUCCESS, "Success" },
        { ServerErrorCode.ACCOUNT_REGISTER_FAIL_REASON1, "Account registration failed due to duplicate email" },
        { ServerErrorCode.LOGIN_FAIL_REASON1, "Invalid email or password" },
        { ServerErrorCode.CHARACTER_CREATE_FAIL_REASON1, "Character creation failed" },
        { ServerErrorCode.UNKNOWN_ERROR, "Unknown error" },
    };
}
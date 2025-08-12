//--------------------------------------------------------------------------------------------------
package com.bk.sbs.dto;

import com.bk.sbs.exception.ServerErrorCode;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class ApiResponse<T> {
    private int errorCode;
    private String errorMessage;
    private T data;

    public ApiResponse(ServerErrorCode errorCode, T data) {
        this.errorCode = errorCode.getCode();
        this.errorMessage = errorCode.getMessage();
        this.data = data;
    }

    public static <T> ApiResponse<T> success(T data) {
        return new ApiResponse<>(ServerErrorCode.SUCCESS, data);
    }

    public static <T> ApiResponse<T> error(ServerErrorCode errorCode) {
        return new ApiResponse<>(errorCode, null);
    }
}
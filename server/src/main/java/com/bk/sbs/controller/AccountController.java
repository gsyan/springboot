//--------------------------------------------------------------------------------------------------
package com.bk.sbs.controller;

import com.bk.sbs.dto.*;
import com.bk.sbs.exception.ServerErrorCode;
import com.bk.sbs.service.AccountService;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/account")
public class AccountController {

    private final AccountService accountService;

    public AccountController(AccountService accountService) {
        this.accountService = accountService;
    }

    @PostMapping("/signup")
    public ApiResponse<String> signUp(@RequestBody SignUpRequest request) {
        try {
            String message = accountService.signUp(request);
            return ApiResponse.success(message);
        } catch (IllegalArgumentException e) {
            return ApiResponse.error(ServerErrorCode.ACCOUNT_REGISTER_FAIL_REASON1);
        } catch (Exception e) {
            return ApiResponse.error(ServerErrorCode.SUCCESS); // 기본 에러 처리
        }

    }

    @PostMapping("/login")
    public ApiResponse<AuthResponse> login(@RequestBody LoginRequest request)
    {
        try {
            AuthResponse response = accountService.login(request);
            return ApiResponse.success(response);
        } catch (IllegalArgumentException e) {
            return ApiResponse.error(ServerErrorCode.LOGIN_FAIL_REASON1);
        } catch (Exception e) {
            return ApiResponse.error(ServerErrorCode.SUCCESS); // 기본 에러 처리
        }
    }

    @PostMapping("/refresh")
    public ApiResponse<AuthResponse> refresh(@RequestBody RefreshTokenRequest request) {
        try {
            AuthResponse response = accountService.refreshToken(request);
            return ApiResponse.success(response);
        } catch (IllegalArgumentException e) {
            return ApiResponse.error(ServerErrorCode.LOGIN_FAIL_REASON1); // 리프레시 실패도 LOGIN_FAIL로 간주
        } catch (Exception e) {
            return ApiResponse.error(ServerErrorCode.SUCCESS); // 기본 에러 처리
        }
    }

    @GetMapping("/characters")
    public ApiResponse<List<CharacterResponse>> getAllCharacters() {
        return accountService.getAllCharacters();
    }

}
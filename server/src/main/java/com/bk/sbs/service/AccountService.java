//--------------------------------------------------------------------------------------------------
package com.bk.sbs.service;

import com.bk.sbs.dto.*;
import com.bk.sbs.entity.Account;
import com.bk.sbs.entity.Character;
import com.bk.sbs.exception.ServerErrorCode;
import com.bk.sbs.repository.AccountRepository;
import com.bk.sbs.repository.CharacterRepository;
import com.bk.sbs.security.JwtUtil;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.stereotype.Service;

import java.time.format.DateTimeFormatter;
import java.util.List;
import java.util.stream.Collectors;

@Service
public class AccountService {
    private final PasswordEncoder passwordEncoder;
    private final JwtUtil jwtUtil;
    private final AccountRepository accountRepository;
    private final CharacterRepository characterRepository;


    public AccountService(PasswordEncoder passwordEncoder, JwtUtil jwtUtil,AccountRepository accountRepository, CharacterRepository characterRepository) {
        this.passwordEncoder = passwordEncoder;
        this.jwtUtil = jwtUtil;
        this.accountRepository = accountRepository;
        this.characterRepository = characterRepository;
    }

    public String signUp(SignUpRequest request) {
        if (accountRepository.existsByEmail(request.getEmail())) {
            throw new IllegalArgumentException("Email already exists");
        }
        Account account = new Account();
        account.setEmail(request.getEmail());
        account.setPassword(passwordEncoder.encode(request.getPassword()));
        Account savedAccount = accountRepository.save(account);
        return "Account created successfully";
    }

    public AuthResponse login(LoginRequest request) {
        Account account = accountRepository.findByEmail(request.getEmail())
                .orElseThrow(() -> new IllegalArgumentException("Invalid email or password"));

        if (!passwordEncoder.matches(request.getPassword(), account.getPassword())) {
            throw new IllegalArgumentException("Invalid email or password");
        }

        AuthResponse response = new AuthResponse();
        response.setAccessToken(jwtUtil.createAccessToken(account.getEmail()));
        response.setRefreshToken(jwtUtil.createRefreshToken(account.getEmail()));
        return response;
    }

    public AuthResponse refreshToken(RefreshTokenRequest request) {
        String refreshToken = request.getRefreshToken();
        if (!jwtUtil.validateToken(refreshToken)) {
            throw new IllegalArgumentException("Invalid refresh token");
        }

        String email = jwtUtil.getEmailFromToken(refreshToken);
        Account account = accountRepository.findByEmail(email)
                .orElseThrow(() -> new IllegalArgumentException("Account not found"));

        AuthResponse response = new AuthResponse();
        response.setAccessToken(jwtUtil.createAccessToken(email));
        response.setRefreshToken(jwtUtil.createRefreshToken(email));
        return response;
    }

    public ApiResponse<List<CharacterResponse>> getAllCharacters() {
        try {
            String email = SecurityContextHolder.getContext().getAuthentication().getName();
            Account account = accountRepository.findByEmail(email)
                    .orElseThrow(() -> new IllegalArgumentException("Account not found"));

            List<Character> characters = characterRepository.findByAccountId(account.getId());
            List<CharacterResponse> characterResponses = characters.stream()
                    .map(character -> new CharacterResponse(
                            character.getId(),
                            character.getCharacterName(),
                            character.getDateTime().format(DateTimeFormatter.ISO_LOCAL_DATE_TIME),
                            1 // worldId는 설정 파일에서 가져오거나 기본값 사용
                    ))
                    .collect(Collectors.toList());

            return ApiResponse.success(characterResponses);
        } catch (IllegalArgumentException e) {
            return ApiResponse.error(determineErrorCode(e.getMessage()));
        } catch (Exception e) {
            return ApiResponse.error(ServerErrorCode.SUCCESS); // 기본 에러 처리
        }
    }

    private ServerErrorCode determineErrorCode(String message) {
        if (message.contains("Account not found")) {
            return ServerErrorCode.LOGIN_FAIL_REASON1;
        }
        return ServerErrorCode.SUCCESS; // 기본값
    }

}
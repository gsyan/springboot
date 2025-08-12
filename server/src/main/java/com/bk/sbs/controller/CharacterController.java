//--------------------------------------------------------------------------------------------------
package com.bk.sbs.controller;

import com.bk.sbs.dto.ApiResponse;
import com.bk.sbs.dto.CharacterCreateRequest;
import com.bk.sbs.dto.CharacterResponse;
import com.bk.sbs.exception.ServerErrorCode;
import com.bk.sbs.service.CharacterService;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/character")
public class CharacterController {

    private final CharacterService characterService;

    public CharacterController(CharacterService characterService) {
        this.characterService = characterService;
    }

    @PostMapping("/create")
    public ApiResponse<CharacterResponse> createCharacter(@RequestBody CharacterCreateRequest request) {
        try {
            CharacterResponse response = characterService.createCharacter(request);
            return ApiResponse.success(response);
        } catch (IllegalArgumentException e) {
            return ApiResponse.error(ServerErrorCode.CHARACTER_CREATE_FAIL_REASON1);
        } catch (Exception e) {
            return ApiResponse.error(ServerErrorCode.SUCCESS); // 기본 에러 처리
        }
    }
}
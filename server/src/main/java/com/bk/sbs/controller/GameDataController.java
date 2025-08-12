package com.bk.sbs.controller;

import com.bk.sbs.dto.ApiResponse;
import com.bk.sbs.dto.DataTableDto;
import com.bk.sbs.exception.ServerErrorCode;
import com.bk.sbs.service.GameDataService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api")
public class GameDataController {

    @Autowired
    private GameDataService gameDataService;

    @PostMapping("/upload-data")
    public ApiResponse<String> uploadData(@RequestBody DataTableDto dataTable) {
        try {
            gameDataService.loadGameData(dataTable);
            return ApiResponse.success("Game data loaded successfully");
        } catch (Exception e) {
            return ApiResponse.error(ServerErrorCode.UNKNOWN_ERROR);
        }
    }
}
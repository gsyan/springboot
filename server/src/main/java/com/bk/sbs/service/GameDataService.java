package com.bk.sbs.service;

import com.bk.sbs.dto.DataTableDto;
import com.bk.sbs.dto.EModuleType;
import com.bk.sbs.dto.ModuleDataDto;
import org.springframework.stereotype.Service;

import java.util.*;
import java.util.stream.Collectors;

@Service
public class GameDataService {
    private Map<EModuleType, List<ModuleDataDto>> gameData = Collections.synchronizedMap(new HashMap<>());

    public void loadGameData(DataTableDto dataTable) {
        if (dataTable == null || dataTable.getModules() == null) {
            throw new IllegalArgumentException("Invalid DataTableDto provided");
        }

        gameData = dataTable.getModules().entrySet().stream()
                .collect(Collectors.toMap(
                        entry -> EModuleType.values()[entry.getKey()], // 인덱스를 EModuleType으로 변환
                        Map.Entry::getValue
                ));
    }

    public List<ModuleDataDto> getModulesByType(EModuleType type) {
        return gameData.getOrDefault(type, new ArrayList<>());
    }

    public ModuleDataDto getFirstModuleByType(EModuleType type) {
        List<ModuleDataDto> modules = getModulesByType(type);
        return modules.isEmpty() ? new ModuleDataDto() : modules.get(0);
    }
}
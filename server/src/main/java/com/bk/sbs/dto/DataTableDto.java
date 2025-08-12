package com.bk.sbs.dto;

import lombok.Data;

import java.util.List;
import java.util.Map;

@Data
public class DataTableDto {
    private Map<Integer, List<ModuleDataDto>> modules; // 키는 EModuleType 인덱스 (0,1,2)
}





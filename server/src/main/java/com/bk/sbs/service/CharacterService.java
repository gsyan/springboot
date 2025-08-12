//--------------------------------------------------------------------------------------------------
package com.bk.sbs.service;

import com.bk.sbs.dto.CharacterCreateRequest;
import com.bk.sbs.dto.CharacterResponse;
import com.bk.sbs.entity.Account;
import com.bk.sbs.entity.Character;
import com.bk.sbs.repository.AccountRepository;
import com.bk.sbs.repository.CharacterRepository;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Service;

import java.time.format.DateTimeFormatter;

@Service
public class CharacterService {

    private final CharacterRepository characterRepository;
    private final AccountRepository accountRepository;

    @Value("${worldid}")
    private int worldId;

    public CharacterService(CharacterRepository characterRepository, AccountRepository accountRepository) {
        this.characterRepository = characterRepository;
        this.accountRepository = accountRepository;
    }

    public CharacterResponse createCharacter(CharacterCreateRequest request) {
        String email = SecurityContextHolder.getContext().getAuthentication().getName();
        Account account = accountRepository.findByEmail(email)
                .orElseThrow(() -> new IllegalArgumentException("Account not found"));

        if (characterRepository.existsByCharacterName(request.getCharacterName())) {
            throw new IllegalArgumentException("Character name already exists");
        }

        Character character = new Character();
        character.setAccountId(account.getId());
        character.setCharacterName(request.getCharacterName());

        Character savedCharacter = characterRepository.save(character);

        return new CharacterResponse(
                savedCharacter.getId(),
                savedCharacter.getCharacterName(),
                savedCharacter.getDateTime().format(DateTimeFormatter.ISO_LOCAL_DATE_TIME),
                worldId
        );
    }
}
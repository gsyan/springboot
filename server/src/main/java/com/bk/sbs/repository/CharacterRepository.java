//--------------------------------------------------------------------------------------------------
package com.bk.sbs.repository;

import com.bk.sbs.entity.Character;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;
import java.util.Optional;

public interface CharacterRepository extends JpaRepository<Character, Long> {
    boolean existsByCharacterName(String characterName);
    Optional<Character> findById(Long id);
    List<Character> findByAccountId(Long accountId);
}

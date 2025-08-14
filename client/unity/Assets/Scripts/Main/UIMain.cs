//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static ApiClient;

public class UIMain : UIManager
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField characterNameInput;
    public Button registerButton;
    public Button loginButton;
    public Button createCharacterButton;
    public Button getCharactersButton;
    
    void Start()
    {
        registerButton.onClick.AddListener(() => Register());
        loginButton.onClick.AddListener(() => Login());
        createCharacterButton.onClick.AddListener(() => CreateCharacter());
        getCharactersButton.onClick.AddListener(() => GetCharacters());

    }

    private void Register()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        NetworkManager.Instance.Register(email, password);
    }

    private void Login()
    {
        string email = emailInput.text;
        string password = passwordInput.text;
        NetworkManager.Instance.Login(email, password);
    }

    
    private void CreateCharacter()
    {
        string name = characterNameInput.text;
        NetworkManager.Instance.CreateCharacter(name);
    }

    private void GetCharacters()
    {
        NetworkManager.Instance.GetCharacters();
    }

    
}
//------------------------------------------------------------------------------
using TMPro;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    #region MonoSingleton ---------------------------------------------------------------
    protected override bool ShouldDontDestroyOnLoad => false;   // ´Ù¸¥ ¾À ·Îµå½Ã ÆÄ±«µÊ

    protected override void OnInitialize()
    {

    }
    #endregion

    public TMP_Text m_resultText;

}

//------------------------------------------------------------------------------
using TMPro;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    #region MonoSingleton ---------------------------------------------------------------
    protected override bool ShouldDontDestroyOnLoad => false;   // �ٸ� �� �ε�� �ı���

    protected override void OnInitialize()
    {

    }
    #endregion

    public TMP_Text m_resultText;

}

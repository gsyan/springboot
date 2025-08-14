//------------------------------------------------------------------------------
using System.Data;
using System.Resources;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private DataTableManager m_dataTable;

    #region MonoSingleton ---------------------------------------------------------------
    protected override bool ShouldDontDestroyOnLoad => false;   // 다른 씬 로드시 파괴됨

    protected override void OnInitialize()
    {
//        m_dataTable = Resources.Load<DataTableManager>("DataTable/DataTable");
//        if (m_dataTable == null)
//        {
//            m_dataTable = ScriptableObject.CreateInstance<DataTableManager>();
//#if UNITY_EDITOR
//            // Assets에 저장
//            UnityEditor.AssetDatabase.CreateAsset(m_dataTable, "Assets/Resources/DataTable/DataTable.asset");
//            UnityEditor.AssetDatabase.SaveAssets();
//#endif
//            // 초기 데이터 설정
//            m_dataTable.AddModule(new ModuleData { m_type = EModuleType.Body, m_health = 100, m_cargoCapacity = 50 });
//            m_dataTable.AddModule(new ModuleData { m_type = EModuleType.Weapon, m_health = 50, m_attackPower = 20 });
//            m_dataTable.AddModule(new ModuleData { m_type = EModuleType.Engine, m_movementSpeed = 5.0f, m_rotationSpeed = 3.0f });

//            // JSON 내보내기
//            string json = m_dataTable.ExportToJson();
//            Debug.Log("Exported JSON: " + json);
//        }
    }
    #endregion

    public void ModifyData(EModuleType type/*, string subType*/, float health)
    {
        ModuleData module = m_dataTable.GetModule(type/*, subType*/);
        if (module != null)
        {
            module.m_health = health;
            Debug.Log($"{type} health updated to {health}");
            //Debug.Log($"{type} {subType} health updated to {health}");
        }
    }



    private void Start()
    {
        
    }

    public void UpgradeWeapon()
    {
        ObjectManager.Instance.m_spaceShip.UpgradeParts();
    }

    public void UpgradeDefense()
    {
        //spaceShip.UpgradePart("Defense");
    }

    public void UpgradeEngine()
    {
        //spaceShip.UpgradePart("Engine");
    }

    public void IncreaseShipSize()
    {
        //spaceShip.IncreaseSize();
    }

    public void SendExplorationCommand()
    {
        ObjectManager.Instance.SendExploration();
    }
}
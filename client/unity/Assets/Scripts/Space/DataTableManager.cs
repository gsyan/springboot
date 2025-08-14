// -------------------------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class ModuleData
{
    public EModuleType m_type; // "Body", "Weapon", "Engine"

    public int m_level = 1;
    public float m_health;
    
    public int m_attackFireCount;   // Weapon/Barrier�� ����, Fighter�� ���༱ �� * ���ݷ�
    public float m_attackPower = 1.0f;
    public float m_attackCoolTime = 1.0f;

    public float m_movementSpeed; // �̵���
    public float m_rotationSpeed; // ȸ����
    public float m_cargoCapacity; // ���� ���� �ɷ�
    
    public int m_upgradeMoneyCost = 10;
    public int m_upgradeMaterialCost = 5;
}

[CreateAssetMenu(fileName = "DataTable", menuName = "Managers/DataTable")]
public class DataTableManager : ScriptableObject
{
    [SerializeField] private List<ModuleData> bodyModules = new List<ModuleData>();
    [SerializeField] private List<ModuleData> weaponModules = new List<ModuleData>();
    [SerializeField] private List<ModuleData> engineModules = new List<ModuleData>();

    //[SerializeField] public Dictionary<EModuleType, List<ModuleData>> modules = new Dictionary<EModuleType, List<ModuleData>>();

    public void AddModule(ModuleData data)
    {
        //if (modules.ContainsKey(data.m_type))
        //    modules[data.m_type].Add(data);
        //else
        //    modules[data.m_type] = new List<ModuleData> { data };

        switch(data.m_type)
        {
            case EModuleType.Body:
                bodyModules.Add(data);
                break;
            case EModuleType.Weapon:
                weaponModules.Add(data);
                break;
            case EModuleType.Engine:
                engineModules.Add(data);
                break;
            default:
                Debug.LogWarning("Unknown module type: " + data.m_type);
                break;
        }

    }

    public void RemoveModule(ModuleData data)
    {
        //if (modules.ContainsKey(data.m_type))
        //    modules[data.m_type].Remove(data);

        switch (data.m_type)
        {
            case EModuleType.Body:
                bodyModules.Remove(data);
                break;
            case EModuleType.Weapon:
                weaponModules.Remove(data);
                break;
            case EModuleType.Engine:
                engineModules.Remove(data);
                break;
            default:
                Debug.LogWarning("Unknown module type: " + data.m_type);
                break;
        }
    }

    public ModuleData GetModule(EModuleType type/*, string subType*/)
    {
        //if (modules.ContainsKey(type) && modules[type].Count > 0)
        //    return modules[type][0]; // ù ��° �׸� ��ȯ (�߰� ���� �ʿ� �� ����)

        switch (type)
        {
            case EModuleType.Body:
                return bodyModules[0];
                break;
            case EModuleType.Weapon:
                return  weaponModules[0];
            case EModuleType.Engine:
                return engineModules[0];
            default:
                Debug.LogWarning("Unknown module type: " + type);
                return null;
        }



        return null;
    }

    public List<ModuleData> GetAllModules(EModuleType type)
    {
        //if (modules.ContainsKey(type))
        //    return modules[type];
        //return new List<ModuleData>();

        switch (type)
        {
            case EModuleType.Body:
                return bodyModules;
                break;
            case EModuleType.Weapon:
                return weaponModules;
            case EModuleType.Engine:
                return engineModules;
            default:
                return null;
        }
        return null;
    }

    public string ExportToJson()
    {
        //return JsonUtility.ToJson(this, true);
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    public void ImportFromJson(string json)
    {
        //JsonUtility.FromJsonOverwrite(json, this);
        JsonConvert.PopulateObject(json, this);
    }
}
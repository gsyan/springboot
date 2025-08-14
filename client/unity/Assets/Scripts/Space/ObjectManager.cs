//------------------------------------------------------------------------------
using Mono.Cecil;
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoSingleton<ObjectManager>
{
    #region MonoSingleton ---------------------------------------------------------------
    protected override bool ShouldDontDestroyOnLoad => false;   // �ٸ� �� �ε�� �ı���

    protected override void OnInitialize()
    {
        settings = Resources.Load<GameSettingsManager>("GameSettings/GameSettings");
        if (settings != null) settings.ApplySettings();
        else Debug.LogError("GameSettings.asset �� Resources ���� �ȿ� �־��ּ���.");
    }
    #endregion

    private GameSettingsManager settings;

    [HideInInspector] public SpaceShip m_spaceShip;
    
    [SerializeField] private GameObject m_partsBodyPrefab;
    [SerializeField] private GameObject m_partsWeaponPrefab;


    
    [SerializeField] private GameObject m_enemyPrefab;
    [HideInInspector] public List<SpaceShipEnemy> m_enemyList = new List<SpaceShipEnemy>();
    
    [SerializeField] private GameObject m_spaceMineralPrefab;
    [HideInInspector] public List<SpaceMineral> m_mineralList = new List<SpaceMineral>();


    [HideInInspector] public Int64 m_myMoney = 0; // ���� ��
    [HideInInspector] public Int64 m_myMaterial = 0; // ���� ����
    [HideInInspector] public int m_myTotalPeople = 10; // ���� �α�
    [HideInInspector] public int m_myWorkingPeople = 0; // �۾��� �α�
    [HideInInspector] public int m_myTechnologyLevel = 1; // ��� ����

    private void Start()
    {
        Vector3 position = Vector3.zero;
        Quaternion rotaion = Quaternion.identity;
        GameObject ship = new GameObject("MySpaceShip");
        ship.transform.position = position;
        ship.transform.rotation = rotaion;
        m_spaceShip = ship.AddComponent<SpaceShip>();
        m_spaceShip.Initialize(m_partsBodyPrefab, m_partsWeaponPrefab);

        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnResources());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(settings.spawnSpaceShipInterval);
            GameObject tempObject = Instantiate(m_enemyPrefab, RandomPosition(), Quaternion.identity);
            SpaceShipEnemy temp = tempObject.GetComponent<SpaceShipEnemy>();
            temp.Initialize(m_spaceShip);
            m_enemyList.Add(temp);
        }
    }

    private IEnumerator SpawnResources()
    {
        while (true)
        {
            yield return new WaitForSeconds(settings.explorationInterval);
            GameObject tempObject = Instantiate(m_spaceMineralPrefab, RandomPosition(), Quaternion.identity);
            SpaceMineral temp = tempObject.GetComponent<SpaceMineral>();
            //temp.Initialize(m_spaceShip);
            m_mineralList.Add(temp);
        }
    }

    public SpaceShipEnemy GetEnemy()
    {
        if (m_enemyList.Count > 0)
        {
            return m_enemyList[0];
        }
        return null;
    }

    public void SendExploration()
    {
        if (m_myTotalPeople - m_myWorkingPeople > 1) // Ž�� ���� �ο�
        {
            SpaceMineral mineral = GetAvailableMineral();
            if (mineral == null) return;

            m_myWorkingPeople += 1;
            StartCoroutine(ExploreMineral(mineral));
        }
        else
        {
            Debug.Log("Insufficient people for exploration");
        }
    }

    private IEnumerator ExploreMineral(SpaceMineral mineral)
    {
        yield return new WaitForSeconds(5.0f); // 5�� Ž��
        Int64 materialAmount = UnityEngine.Random.Range(10, 50);
        m_myMaterial += materialAmount;
        m_myWorkingPeople -= 1; // Ž�� �Ϸ� �� �α� ����
        m_mineralList.Remove(mineral);
        Destroy(mineral.gameObject);
        Debug.Log($"Exploration completed, gained {materialAmount} materials");
    }

    private SpaceMineral GetAvailableMineral()
    {
        foreach(var mineral in m_mineralList)
        {
            if (mineral.m_spaceMineralState != ESpaceMineralState.None) continue;
            mineral.m_spaceMineralState = ESpaceMineralState.Occupied;
            return mineral;
        }
        return null;
    }


    private Vector3 RandomPosition()
    {
        return new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), 0, UnityEngine.Random.Range(-10.0f, 10.0f));
    }

}


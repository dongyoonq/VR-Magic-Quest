using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private static PoolManager poolManager;
    private static ResourceManager resourceManager;
    private static SoundManager soundManager;
    private static QuestManager questManager;
    private static LoadManager loadManager;
    private static RecordManager recordManager;

    public static GameManager Instance { get { return instance; } }
    public static PoolManager Pool { get { return poolManager; } }
    public static ResourceManager Resource { get { return resourceManager; } }
    public static SoundManager Sound { get { return soundManager; } }
    public static LoadManager Load { get { return loadManager; } }
    public static RecordManager Record { get { return recordManager; } }

    public static QuestManager Quest { get { return questManager; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
        InitManagers();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void InitManagers()
    {
        GameObject resourceObj = new GameObject();
        resourceObj.name = "ResourceManager";
        resourceObj.transform.parent = transform;
        resourceManager = resourceObj.AddComponent<ResourceManager>();

        GameObject poolObj = new GameObject();
        poolObj.name = "PoolManager";
        poolObj.transform.parent = transform;
        poolManager = poolObj.AddComponent<PoolManager>();

        GameObject soundObj = new GameObject();
        soundObj.name = "SoundManager";
        soundObj.transform.parent = transform;
        soundManager = soundObj.AddComponent<SoundManager>();

        GameObject questObj = new GameObject();
        questObj.name = "QuestManager";
        questObj.transform.parent = transform;
        questManager = questObj.AddComponent<QuestManager>();

        GameObject loadObj = new GameObject();
        loadObj.name = "LoadManager";
        loadObj.transform.parent = transform;
        loadManager = loadObj.AddComponent<LoadManager>();

        GameObject recordObj = new GameObject();
        recordObj.name = "RecordManager";
        recordObj.transform.parent = transform;
        recordManager = recordObj.AddComponent<RecordManager>();
    }
}
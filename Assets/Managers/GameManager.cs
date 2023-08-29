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

    public static GameManager Instance { get { return instance; } }
    public static PoolManager Pool { get { return poolManager; } }
    public static ResourceManager Resource { get { return resourceManager; } }
    public static SoundManager Sound { get { return soundManager; } }

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
    }
}
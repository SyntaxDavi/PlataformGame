using UnityEngine;
using System.Collections.Generic;
public class PoolSpawner : MonoBehaviour
{
    public static PoolSpawner Instance;

    public GameObject PrefabShot;

    [SerializeField] private float PoolSize = 15f;

    private readonly List<GameObject> BulletPool = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InstanciarPrefab(BulletPool, PrefabShot);

    }
    private void InstanciarPrefab(List<GameObject> Pool, GameObject Prefab)
    {
        for (int i = 0; i < PoolSize; i++)
        {
            GameObject Obj = Instantiate(Prefab);
            Obj.SetActive(false);
            Pool.Add(Obj);
        }
    }

    public GameObject GetBullet(int Type)
    {
        List<GameObject> Pool;

        switch (Type)
        {
            case 1:
                Pool = BulletPool;
                break;
            default:
                Debug.LogWarning("Tipo de tiro inválido: " + Type);
                return null;
        }

        foreach (GameObject bullet in Pool)
        {
            if (!bullet.activeInHierarchy)
            {
                return bullet;
            }
        }
        return null;
    }
}

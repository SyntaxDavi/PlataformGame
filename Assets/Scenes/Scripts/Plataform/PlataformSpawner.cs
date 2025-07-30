    using UnityEngine;
    using System.Collections.Generic;

public class PlataformSpawner : MonoBehaviour
{
   /* public GameObject PlatPrefab1;
    public GameObject PlatPrefab2;
    public GameObject PlatPrefab3;

    private float SpawnMinX = -50f;
    private float SpawnMaxX = 32f;
    public float SpawnMaxY = 30f;
    private float SpawnMinY = 1f;

    public float VerticalStep = 5f;
    public float MinXGap = 2f;
    public float MaxXGap = 10f;

    public int PlatPerFloorMin = 2;
    public int PlatPerFloorMax = 4;

    private List<GameObject> PrefabPool = new List<GameObject>();
    private GameObject[] PrefabOptions;

    private void Start()
    {
        PrefabOptions = new GameObject[] { PlatPrefab1, PlatPrefab2, PlatPrefab3 };
        InstantiatePlataforms();
    }

    private void InstantiatePlataforms()
    {
        for (float y = SpawnMinY; y < SpawnMaxY; y += VerticalStep)
        {
            int PlatformsThisLevel = Random.Range(PlatPerFloorMin, PlatPerFloorMax + 1);
            List<float> UsedXPositions = new List<float>();

            for (int i = 0; i < PlatformsThisLevel; i++)
            {
                int MaxAttempts = 10;
                float XPosition = 0f;
                bool PositionValid = false;

                while (MaxAttempts-- > 0)
                {
                    XPosition = Random.Range(SpawnMinX, SpawnMaxX);
                    PositionValid = true;

                    foreach (float UsedX in UsedXPositions)
                    {
                        if (Mathf.Abs(UsedX - XPosition) < MinXGap)
                        {
                            PositionValid = false;
                            break;
                        }
                    }

                    if (PositionValid)
                        break;
                }

                if (!PositionValid)
                    continue;

                UsedXPositions.Add(XPosition);

                Vector3 SpawnPos = new Vector3(XPosition, y, 0);
                GameObject SelectedPrefab = PrefabOptions[Random.Range(0, PrefabOptions.Length)];
                GameObject Plataform = Instantiate(SelectedPrefab, SpawnPos, Quaternion.identity);
                PrefabPool.Add(Plataform);

            }
        }
    }

    public void ReorganizePlataforms()
    {

        int PoolIndex = 0;

        for (float y = SpawnMinY; y < SpawnMaxY; y += VerticalStep)
        {
            int PlatformsThisLevel = Random.Range(PlatPerFloorMin, PlatPerFloorMax + 1);
            List<float> UsedXPositions = new List<float>();

            for (int i = 0; i < PlatformsThisLevel; i++)
            {
                if(PoolIndex >= PrefabPool.Count)
                {
                    return;
                }

                int MaxAttempts = 10;
                float XPosition = 0f;
                bool PositionValid = false;

                while (MaxAttempts-- > 0)
                {
                    XPosition = Random.Range(SpawnMinX, SpawnMaxX);
                    PositionValid = true;

                    foreach (float UsedX in UsedXPositions)
                    {
                        if (Mathf.Abs(UsedX - XPosition) < MinXGap)
                        {
                            PositionValid = false;
                            break;
                        }
                    }

                    if (PositionValid)
                        break;
                }

                if (!PositionValid)
                    continue;

                UsedXPositions.Add(XPosition);

                Vector3 NewPos = new Vector3(XPosition, y, 0);
                PrefabPool[PoolIndex].transform.position = NewPos;
                PoolIndex++;

            }
        }

        for(int i = PoolIndex; i < PrefabPool.Count; i++)
        {
            PrefabPool[i].transform.position = new Vector3(9999, 9999, 0);
        }
    }
   */
}

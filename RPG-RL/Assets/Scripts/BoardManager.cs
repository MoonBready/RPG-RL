using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{

    [Serializable] public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);
    public Count potionCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floor;
    public GameObject[] rock;
    public GameObject[] potion;
    public GameObject[] enemy;
    public GameObject[] wall;

    private Transform boardHolder;

    private List<Vector3> gridPositions = new List<Vector3>();


    void InitialiseList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }


    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floor[Random.Range(0, floor.Length)];

                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = wall[Random.Range(0, wall.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }

        }
    }


    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);

        Vector3 randomPosition = gridPositions[randomIndex];

        gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for(int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();

            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];

            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }


    public void SetupScene(int level)
    {
        BoardSetup();

        InitialiseList();

        LayoutObjectAtRandom(wall, wallCount.minimum, wallCount.maximum);

        LayoutObjectAtRandom(potion, potionCount.minimum, potionCount.maximum);

        int enemyCount = (int)Mathf.Log(level, 2f);

        LayoutObjectAtRandom(enemy, enemyCount, enemyCount);

        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}

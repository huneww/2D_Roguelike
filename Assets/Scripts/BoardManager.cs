using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    [SerializeField]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    // ������ ���� ũ��
    public int columes = 8;
    public int rows = 8;
    // ���� �� ������ ���� �ּ�, �ִ�
    public Count wallCount = new Count(5, 9);
    // ���� �� ������ ������ �ּ�, �ִ�
    public Count foodCount = new Count(1, 5);
    // Ż�� ���� ������
    public GameObject exit;
    // �ٴ� Ÿ�ϸ� ������
    public GameObject[] floorTiles;
    // �� Ÿ�ϸ� ������
    public GameObject[] wallTiles;
    // ���� ������
    public GameObject[] foodTiles;
    // �� ������
    public GameObject[] enemyTiles;
    // �ܰ� �� ������
    public GameObject[] outerWallTiles;

    // ���� ���� ������ ���� transform ������Ʈ
    private Transform boardHolder;
    // ������Ʈ ���� ������ ��ġ ���� ����Ʈ
    private List<Vector3> gridPosition = new List<Vector3>();

    private void InitialiseList()
    {
        // ����Ʈ �ʱ�ȭ
        gridPosition.Clear();

        // ����Ʈ�� (1,1) ~ (columes-1,rows-1) ���� ����
        for (int x = 1; x < columes - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPosition.Add(new Vector3(x, y, 0f));
            }
        }
    }

    private void BoardSetUp()
    {
        // Board������Ʈ ���� �� transfor boardHolder�� ����
        boardHolder = new GameObject("Board").transform;

        // (-1,-1) ~ (columes, rows) ���� �ݺ�
        for (int x = -1; x < columes + 1; x++)
        {
            for (int y= - 1; y < rows + 1; y++)
            {
                // �ٴ� Ÿ�ϸ� �����鿡�� �ν��Ͻ��� ������Ʈ ���� ���� �� ����
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                // x,y�� ���� -1, columes, rows ��� outerWallTiles ������Ʈ�� ���� ���� �� ����
                if (x == -1 || x == columes || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                // ���õ� ������Ʈ ����, ��ġ�� (x, y, 0f)�� ����
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);

                // ������ ������Ʈ�� boardHolder�� �ڽ� ��ü�� ����
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    private Vector3 RandomPosition()
    {
        // ������Ʈ ���� ���� ��ġ���� ���� ��ȣ ȹ��
        int randomIndex = Random.Range(0, gridPosition.Count);
        // Vector3�� ����Ʈ�� �� ����
        Vector3 randomPosition = gridPosition[randomIndex];
        // ȹ���� ���� ����Ʈ���� ����
        gridPosition.RemoveAt(randomIndex);

        return randomPosition;
    }

    private void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        // ������ ������Ʈ�� ���� ����
        // Random.Range�� minmum ~ maxmum - 1 ������ ���� �����ϱ� ������ maxmum�� +1
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void  SetupScene(int level)
    {
        BoardSetUp();
        InitialiseList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columes - 1, rows - 1, 0f), Quaternion.identity);
    }

}

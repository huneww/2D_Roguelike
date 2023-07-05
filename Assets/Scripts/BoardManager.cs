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

    // 생성될 판의 크기
    public int columes = 8;
    public int rows = 8;
    // 레벨 당 생성될 벽의 최소, 최대
    public Count wallCount = new Count(5, 9);
    // 레벨 당 생성될 음식의 최소, 최대
    public Count foodCount = new Count(1, 5);
    // 탈출 지점 프리펩
    public GameObject exit;
    // 바닥 타일맵 프리펩
    public GameObject[] floorTiles;
    // 벽 타일맵 프리펩
    public GameObject[] wallTiles;
    // 음식 프리펩
    public GameObject[] foodTiles;
    // 적 프리펩
    public GameObject[] enemyTiles;
    // 외각 벽 프리펩
    public GameObject[] outerWallTiles;

    // 계층 구조 정리를 위한 transform 오브젝트
    private Transform boardHolder;
    // 오브젝트 생성 가능한 위치 저장 리스트
    private List<Vector3> gridPosition = new List<Vector3>();

    private void InitialiseList()
    {
        // 리스트 초기화
        gridPosition.Clear();

        // 리스트에 (1,1) ~ (columes-1,rows-1) 까지 저장
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
        // Board오브젝트 생성 및 transfor boardHolder에 저장
        boardHolder = new GameObject("Board").transform;

        // (-1,-1) ~ (columes, rows) 까지 반복
        for (int x = -1; x < columes + 1; x++)
        {
            for (int y= - 1; y < rows + 1; y++)
            {
                // 바닥 타일맵 프리펩에서 인스턴스할 오브젝트 랜덤 선택 및 저장
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                // x,y의 값이 -1, columes, rows 라면 outerWallTiles 오브젝트로 랜덤 선택 및 변경
                if (x == -1 || x == columes || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                // 선택된 오브젝트 생성, 위치를 (x, y, 0f)로 지정
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity);

                // 생성한 오브젝트는 boardHolder의 자식 객체로 지정
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    private Vector3 RandomPosition()
    {
        // 오브젝트 생성 가능 위치에서 랜덤 번호 획득
        int randomIndex = Random.Range(0, gridPosition.Count);
        // Vector3에 리스트의 값 저장
        Vector3 randomPosition = gridPosition[randomIndex];
        // 획득한 값은 리스트에서 제거
        gridPosition.RemoveAt(randomIndex);

        return randomPosition;
    }

    private void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        // 생성할 오브젝트의 갯수 지정
        // Random.Range는 minmum ~ maxmum - 1 사이의 값을 추출하기 때문에 maxmum에 +1
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

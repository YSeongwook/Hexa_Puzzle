using System.Collections;
using UnityEngine;

public class GemManager : MonoBehaviour
{
    public GameObject gemPrefab;
    public int maxRows = 6;
    public int maxCols = 7;
    private GameObject[,] grid;

    private void Start()
    {
        grid = new GameObject[maxRows, maxCols];
        StartCoroutine(SpawnGem());
    }

    private IEnumerator SpawnGem()
    {
        int centerCol = maxCols / 2;
        GameObject gemObject = Instantiate(gemPrefab, new Vector2(centerCol, maxRows + 1), Quaternion.identity);
        Gem gem = gemObject.GetComponent<Gem>();

        while (true)
        {
            int row = GetCurrentRow(gem.transform.position.y);
            int col = GetCurrentCol(gem.transform.position.x);

            if (IsEmpty(row + 1, col))
            {
                gem.SetTargetPosition(new Vector2(col, row + 1));
            }
            else if (IsEmpty(row + 1, col - 1) || IsEmpty(row + 1, col + 1))
            {
                int randomDir = Random.Range(0, 2);
                if (randomDir == 0 && IsEmpty(row + 1, col - 1))
                    gem.SetTargetPosition(new Vector2(col - 1, row + 1));
                else if (randomDir == 1 && IsEmpty(row + 1, col + 1))
                    gem.SetTargetPosition(new Vector2(col + 1, row + 1));
            }
            else
            {
                break;
            }

            yield return null;  // 한 프레임 대기
        }
    }

    private bool IsEmpty(int row, int col)
    {
        return row >= 0 && row < maxRows && col >= 0 && col < maxCols && grid[row, col] == null;
    }

    private int GetCurrentRow(float yPos)
    {
        return Mathf.FloorToInt(yPos);
    }

    private int GetCurrentCol(float xPos)
    {
        return Mathf.FloorToInt(xPos);
    }
}
using System.Linq;
using UnityEngine;

public class HexGridManager : MonoBehaviour
{
    public RectTransform hexGridLayout;  // 그리드 레이아웃 (Canvas 하위의 UI 요소)
    public GameObject hexBackgroundPrefab;  // 배경 타일 프리팹
    public float tileWidth = 90f;  // 타일의 가로 크기
    public float tileHeight = 78f;  // 타일의 세로 크기
    public float globalYOffset = 0f;  // 전체 타일의 yOffset
    public float sizeMultiplier = 1.5f;  // 간격을 벌리기 위한 배율 (타일 크기 비율과 동일)

    // 각 열에 들어가는 타일 수 정의 (왼쪽부터 오른쪽)
    private int[] _tilesInColumns = new int[] { 3, 4, 5, 6, 5, 4, 3 };

    private void Start()
    {
        CreateHexGrid();
    }

    // 헥사 그리드를 생성하는 메서드
    private void CreateHexGrid()
    {
        // 타일 크기 비율에 맞춰 간격 조정
        float adjustedTileWidth = tileWidth * sizeMultiplier;
        float adjustedTileHeight = tileHeight * sizeMultiplier;

        // xOffset을 계산하여 그리드의 중앙을 맞추기
        float xOffset = (_tilesInColumns.Length - 1) * adjustedTileWidth * 0.75f / 2;

        // 각 열에 대한 타일 배치
        for (int col = 0; col < _tilesInColumns.Length; col++)
        {
            // 열을 구분할 빈 오브젝트 생성 (RectTransform 사용)
            GameObject columnParent = new GameObject($"Column_{col}", typeof(RectTransform));
            RectTransform columnRectTransform = columnParent.GetComponent<RectTransform>();
            columnRectTransform.SetParent(hexGridLayout, false);  // 열을 그리드의 자식으로 설정

            int tilesInCurrentColumn = _tilesInColumns[col];  // 현재 열의 타일 개수
            float xPos = col * adjustedTileWidth * 0.75f - xOffset;  // 가로 위치 계산

            // 열 오브젝트의 y값 설정 (전체 yOffset 적용)
            columnRectTransform.anchoredPosition = new Vector2(xPos, globalYOffset);

            // yOffset을 계산하여 해당 열이 중앙에 맞춰지도록 조정
            float yOffset = ((_tilesInColumns.Max() - tilesInCurrentColumn) * adjustedTileHeight) / 2;

            // 타일을 해당 열에 배치
            for (int row = 0; row < tilesInCurrentColumn; row++)
            {
                // 세로 위치 계산
                float yPos = row * adjustedTileHeight + yOffset;

                Vector2 tilePosition = new Vector2(0, yPos);  // 각 열에 타일을 수직으로 배치

                // 배경 타일 배치
                GameObject hexBackground = Instantiate(hexBackgroundPrefab, columnRectTransform);
                RectTransform hexBackgroundRect = hexBackground.GetComponent<RectTransform>();
                hexBackgroundRect.anchoredPosition = new Vector2(tilePosition.x, tilePosition.y);

                // 타일 크기를 1.5배로 확장
                hexBackgroundRect.localScale = new Vector3(sizeMultiplier, sizeMultiplier, 1);
            }
        }
    }
}

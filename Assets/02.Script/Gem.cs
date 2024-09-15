using System.Collections;
using EnumTypes;
using EventLibrary;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public float dropSpeed = 5f;
    public GemType gemType; // 보석의 종류
    public Sprite[] gemSprites; // 보석 스프라이트 배열 (5개)

    private Vector2 _targetPosition;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(GemType type)
    {
        gemType = type;
        _spriteRenderer.sprite = gemSprites[(int)gemType]; // 보석 타입에 맞는 스프라이트 설정
    }
    
    private void OnEnable()
    {
        EventManager<GemEvents>.StartListening(GemEvents.MoveGem, OnMoveGem);
    }

    private void OnDisable()
    {
        EventManager<GemEvents>.StopListening(GemEvents.MoveGem, OnMoveGem);
    }

    // 보석 이동 시작
    public void SetTargetPosition(Vector2 targetPos)
    {
        _targetPosition = targetPos;
        EventManager<GemEvents>.TriggerEvent(GemEvents.MoveGem);
    }

    // 이동 이벤트 처리
    private void OnMoveGem()
    {
        StartCoroutine(MoveGem());
    }

    // 코루틴으로 이동
    private IEnumerator MoveGem()
    {
        while (Vector2.Distance(transform.position, _targetPosition) > 0.01f)
        {
            transform.position = Vector2.Lerp(transform.position, _targetPosition, dropSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = _targetPosition;
        EventManager<GemEvents>.TriggerEvent(GemEvents.GemLanded);
    }
}
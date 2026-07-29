using UnityEngine;

/// <summary>
/// 씬 뷰에 지정된 크기의 와이어 박스를 그려주는 디버깅용 스크립트
/// </summary>
public class TileMapGizmo : MonoBehaviour
{
    [Header("Gizmo Settings")]
    [SerializeField] private int width = 50;
    [SerializeField] private int height = 50;
    [SerializeField] private Color guideColor = Color.green;

    private void OnDrawGizmos()
    {
        Gizmos.color = guideColor;
        Vector3 center = transform.position;
        Vector3 size = new Vector3(width, height, 0f);

        // 오프셋 계산 버그 수정 후 연산
        Gizmos.DrawWireCube(center, size);
    }
}
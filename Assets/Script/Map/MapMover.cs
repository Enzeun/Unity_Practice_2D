using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapMover : MonoBehaviour
{

    [Header("Tilemap Settings")]
    [SerializeField, Required]
    private Tilemap currentTileMap;

    [BoxGroup("Config"), SerializeField, Required]
    private List<Tilemap> tileMaps;

    [BoxGroup("Config"), SerializeField]
    private int mapMovingBound = 10;

    [Header("Player Target")]
    [SerializeField, Required]
    private Transform playerTransform;

    [BoxGroup("currentTileMap 디버깅"), SerializeField, ReadOnly]
    private Vector3Int _localToCell;
    [BoxGroup("currentTileMap 디버깅"), SerializeField, ReadOnly]
    private Vector3Int _worldToCell;
    [BoxGroup("currentTileMap 디버깅"), SerializeField, ReadOnly]
    private Vector3Int _tileMapSize;
    [BoxGroup("currentTileMap 디버깅"), SerializeField, ReadOnly]
    private bool _hasTile;


    private void Awake()
    {
        if (currentTileMap != null)
        {
            _tileMapSize = currentTileMap.cellBounds.size;
        }
    }
    private void Update()
    {
        if (currentTileMap == null || playerTransform == null) return;

        _localToCell = currentTileMap.LocalToCell(playerTransform.position); // 월드 기준 플레이어의 위치
        _worldToCell = currentTileMap.WorldToCell(playerTransform.position); // 현재 타일과 플레이어간의 거리
        _hasTile = currentTileMap.HasTile(_worldToCell);

        CalculatePlayerCurrentPosition();
    }

    // 플레이어 현재 위치 계산
    private void CalculatePlayerCurrentPosition()
    {
        foreach (var tilemap in tileMaps)
        {
            // 현재 타일맵이면 스킵
            if (tilemap == currentTileMap)
            {
                continue;
            }
            // 현재 타일맵이 아닌 다른 타일맵들의 행동

            // 먼저 플레이어와의 거리 계산함
            Vector3Int playerCell = tilemap.WorldToCell(playerTransform.position);

            // 플레이어의 타일이 변경되어야 할 때.
            // 플레이어가 내 범위에 들어왔으면, 현재 타일로 변경하고 다음 타일로 스킵
            // 그게 아니면, 플레이어가 얼마나 떨어져있는지 확인. => 너무 멀리 떨어지면 타일 이동
            // 플레이어의 타일이 변경될 필요가 없을 때.
            // 플리에어가 얼마나 떨어진지 확인.
            if (!_hasTile)
            {

                if (tilemap.HasTile(playerCell))
                {
                    currentTileMap = tilemap;
                    continue;
                }
            }

            // 플레이어의 포지션 (현재 타일맵에서의 거리) 계산
            if (playerCell.x > _tileMapSize.x + mapMovingBound)
            {
                tilemap.transform.position += new Vector3(_tileMapSize.x * 2, 0, 0);
            }
            else if (playerCell.x < -(_tileMapSize.x + mapMovingBound))
            {
                tilemap.transform.position -= new Vector3(_tileMapSize.x * 2, 0, 0);
            }
            else if (playerCell.y > _tileMapSize.y + mapMovingBound)
            {
                tilemap.transform.position += new Vector3(0, _tileMapSize.y * 2, 0);
            }
            else if (playerCell.y < -(_tileMapSize.y + mapMovingBound))
            {
                tilemap.transform.position -= new Vector3(0, _tileMapSize.y * 2, 0);
            }

        }
    }
}
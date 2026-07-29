using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance;

    public enum MonsterType
    {
        Slime,
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        pools = new Dictionary<MonsterType, ObjectPool<EnemyBase>>();
    }

    //---------------------------------------------------------------------

    // 참조 필드
    [SerializeField, Required]
    private PlayerBase _player;
    [SerializeField, Required]
    private EnemyBase[] Prefabs;

    // 오브젝트 풀 (등급에 따른 풀)
    private Dictionary<MonsterType, ObjectPool<EnemyBase>> pools;


    void Start()
    {
        pools[MonsterType.Slime] = new ObjectPool<EnemyBase>(
            CreateEnemy,
            OnGetEnemy,
            OnReleaseEnemy
            );
    }


    // 몬스터 스폰을 위한 필드
    private float spawnTimer = 0f;
    private float spawnDelay = 1f;


    void Update()
    {
        RunSpawnTimer();
    }

    private EnemyBase CreateEnemy()
    {
        EnemyBase enemy = Instantiate(Prefabs[0]);

        //enemy.OnDie += OnEnemyDie;
        enemy.OnDie += (eee) => StartCoroutine(Release3Seconds(eee));

        return enemy;
    }
    private void OnGetEnemy(EnemyBase enemy)
    {
        enemy.Initialize(_player, pools[MonsterType.Slime]);

        Vector2 randomDir = Random.insideUnitCircle.normalized;

        enemy.transform.position = (Vector2)_player.transform.position + randomDir * 8;

        enemy.gameObject.SetActive(true);
    }
    private void OnReleaseEnemy(EnemyBase enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    private void OnEnemyDie(EnemyBase enemy)
    {
        StartCoroutine(Release3Seconds(enemy));
    }

    private IEnumerator Release3Seconds(EnemyBase enemy)
    {
        yield return new WaitForSeconds(3f);

        pools[MonsterType.Slime].Release(enemy);
    }

    // ------------------------------------------------------------------------

    private void RunSpawnTimer()
    {
        if (spawnTimer <= 0)
        {
            SpawnEnemy();
            spawnTimer = spawnDelay;
            return;
        }
        spawnTimer -= Time.deltaTime;
    }

    private void SpawnEnemy()
    {
        pools[MonsterType.Slime].Get();
    }

}

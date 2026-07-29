using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    //---------------------------------------------------------------------

    // 참조 필드
    [SerializeField, Required]
    private EnemyBase[] Prefabs;

    // 오브젝트 풀 (등급에 따른 풀)
    private ObjectPool<EnemyBase> pool1;
    //private ObjectPool<EnemyBase> pool2;
    //private ObjectPool<EnemyBase> pool3;
    //private ObjectPool<EnemyBase> pool4;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

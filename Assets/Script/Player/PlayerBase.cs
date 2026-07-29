using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    // 플레이어 스탯 필드
    [BoxGroup("Player Status"), ShowInInspector]
    public float maxHp { get; private set; } = 100f;
    [BoxGroup("Player Status"), ShowInInspector]
    public float hp { get; private set; } = 100f;
    [BoxGroup("Player Status"), ShowInInspector]
    public bool isDead { get; private set; } = false;
    [BoxGroup("Player Status"), ShowInInspector]
    public bool canMove {  get; private set; } = false;

}

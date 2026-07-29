using DG.Tweening;
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
    public bool canMove { get; private set; } = false;

    [BoxGroup("Require!!"), SerializeField, Required]
    private DOTweenAnimation myDOTween;



    // 데미지 입는 곳
    [ButtonGroup("메서드 디버깅")]
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        hp = Mathf.Clamp(hp - damage, 0, maxHp);

        PlayHurtAnimation();
    }

    [ButtonGroup("메서드 디버깅")]
    private void PlayHurtAnimation()
    {
        myDOTween.DORestartAllById("Hurt");
    }
}

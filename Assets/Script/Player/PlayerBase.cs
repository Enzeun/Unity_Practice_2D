using DG.Tweening;
using Sirenix.OdinInspector;
using System;
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
    public bool canMove { get; private set; } = true;

    [BoxGroup("Require!!"), SerializeField, Required]
    private DOTweenAnimation myDOTween;


    // 데미지 입는 곳
    [BoxGroup("메서드 디버깅"), Button]
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        float prevHp = hp;

        hp = Mathf.Clamp(hp - damage, 0, maxHp);

        OnDamaged?.Invoke(prevHp, hp);

        if (hp <= 0)
        {
            Die();
        }

        PlayHurtAnimation();
    }

    [BoxGroup("메서드 디버깅"), Button]
    private void PlayHurtAnimation()
    {
        myDOTween.DORestartAllById("Hurt");
    }

    [BoxGroup("메서드 디버깅"), Button]
    private void Die()
    {
        canMove = false;

        isDead = true;

        OnDie?.Invoke();
    }

    public Action OnDie;
    /// <summary>
    /// 반환 : 이전체력, 현재체력
    /// </summary>
    public Action<float, float> OnDamaged;
}

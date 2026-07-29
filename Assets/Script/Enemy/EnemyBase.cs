using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyBase : MonoBehaviour
{
    // 컴포넌트 참조
    [BoxGroup("Required Component"), SerializeField, Required]
    private DOTweenAnimation myAnim;
    [BoxGroup("Required Component"), SerializeField, Required]
    private Rigidbody2D myRigidbody;
    [BoxGroup("Required Component"), SerializeField, Required]
    private SpriteRenderer mySpriteRenderer;
    [BoxGroup("Required Component"), SerializeField, Required]
    private Animator myAnimator;

    // 애니메이션 해시
    private static readonly int MovingHash = Animator.StringToHash("IsMoving");
    private static readonly int DieHash = Animator.StringToHash("IsDead");

    // 몬스터 스탯 필드
    [BoxGroup("Enemy Status"), ShowInInspector]
    public float maxHp { get; private set; } = 100f;
    [BoxGroup("Enemy Status"), ShowInInspector]
    public float hp { get; private set; } = 100f;
    [BoxGroup("Enemy Status"), ShowInInspector]
    public float movementSpeed { get; private set; } = 1f;
    [BoxGroup("Enemy Status"), ShowInInspector]
    public bool isDead { get; private set; } = false;
    [BoxGroup("Enemy Status"), ShowInInspector]
    public bool canMove { get; private set; } = true;
    [BoxGroup("Enemy Status"), SerializeField]
    private PlayerBase _player;
    [BoxGroup("Enemy Status"), SerializeField]
    private float attackRange = 1f;
    [BoxGroup("Enemy Status"), SerializeField]
    private float attackPower = 10f;
    [BoxGroup("Enemy Status"), SerializeField]
    private float attackDelay = 1f;
    [BoxGroup("Enemy Status"), SerializeField]
    private float attackTimer = 0;


    // 현재 상태들
    [BoxGroup("Current Status"), ReadOnly, ShowInInspector]
    private Vector2 movingDirection;
    [BoxGroup("Current Status"), ReadOnly, ShowInInspector]
    private bool isLookRight = true;
    [BoxGroup("Current Status"), ReadOnly, ShowInInspector]
    private Vector2 distance;
    [BoxGroup("Current Status"), ReadOnly, ShowInInspector]
    private bool isInRange;
    [BoxGroup("Current Status"), ReadOnly, ShowInInspector]
    private bool isInAttackRange;
    [BoxGroup("Current Status"), ReadOnly, ShowInInspector]
    private bool isMoving;


    private ObjectPool<EnemyBase> myPool;

    

    public void Initialize(PlayerBase player, ObjectPool<EnemyBase> pool)
    {
        if (_player == null)
        {
            _player = player;
        }
        myPool = pool;
        canMove = true;
        isDead = false;
        isLookRight = true;
        isInRange = false;
        isInAttackRange = false;
        attackTimer = attackDelay;
        hp = maxHp;
        myAnimator.SetBool(DieHash, false);
    }

    private void Update()
    {
        CalculateDistance();
        CalculateMovingDirection();
        TryAttackPlayer();
    }
    private void FixedUpdate()
    {
        MoveToPlayer();
    }


    [BoxGroup("메서드 디버깅"), Button]
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        hp = Mathf.Clamp(hp - damage, 0, maxHp);

        if (hp <= 0)
        {
            Die();
        }

        PlayHurtAnim();
    }

    [BoxGroup("메서드 디버깅"), Button]
    public void PlayHurtAnim()
    {
        myAnim.DORestart();
    }

    [BoxGroup("메서드 디버깅"), Button]
    private void Die()
    {
        if (isDead) return;

        canMove = false;

        isDead = true;

        myRigidbody.linearVelocity = Vector2.zero;

        PlayDieAnimation();

        OnDie?.Invoke(this);

    }

    private void PlayDieAnimation()
    {
        myAnimator.SetBool(DieHash, isDead);
    }

    
    public Action<EnemyBase> OnDie;

    //-------------------------------------------------------------------------------------

    // 이동 처리. 몬스터의 이동은 단순하므로 클래스를 따로 나누지 않음

    private void CalculateDistance()
    {
        if (isDead) return;

        if (!canMove) return;

        distance = (_player.transform.position - gameObject.transform.position);

        isInAttackRange = (distance.magnitude <= attackRange);

        isInRange = (distance.magnitude <= (attackRange * 0.8f));
    }


    private void CalculateMovingDirection()
    {
        if (isDead) return;

        if (!canMove) return;

        movingDirection = distance.normalized;

        FlipEnemySprite();
    }

    private void FlipEnemySprite()
    {
        // 한 번 거르기 때문에 죽었는지 확인 X
        bool currentLookRight = movingDirection.x >= 0;

        if (isLookRight == currentLookRight) return;

        isLookRight = currentLookRight;

        mySpriteRenderer.flipX = !isLookRight;
    }

    private void MoveToPlayer()
    {
        if (isDead || !canMove || isInRange)
        {
            if (isMoving)
            {
                isMoving = false;
                SetMovingAnimation();
                myRigidbody.linearVelocity = Vector2.zero;
            }
            return;
        }

        myRigidbody.linearVelocity = movingDirection * movementSpeed;

        if (!isMoving)
        {
            isMoving = true;

            SetMovingAnimation();
        }
    }

    private void SetMovingAnimation()
    {
        myAnimator.SetBool(MovingHash, isMoving);
    }

    //--------------------------------------------------------------------------

    private void TryAttackPlayer()
    {
        if (isDead) return;

        if (isInAttackRange)
        {
            if (attackTimer <= 0)
            {
                AttackPlayer(attackPower);
                attackTimer = attackDelay;

                return;
            }
        }
        attackTimer -= Time.deltaTime;

    }

    [BoxGroup("메서드 디버깅"), Button]

    private void AttackPlayer(float damage)
    {
        _player.TakeDamage(damage);
    }



    //--------------------------------------------------------------------------

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, attackRange);
        Gizmos.color = Color.coral;
        Gizmos.DrawWireSphere(gameObject.transform.position, (attackRange * 0.8f));
    }


}

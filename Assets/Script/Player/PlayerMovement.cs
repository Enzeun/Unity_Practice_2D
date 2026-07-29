using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [BoxGroup("Components"), SerializeField, Required]
    private CinemachinePositionComposer camComposer;
    [BoxGroup("Components"), SerializeField, Required]
    private Rigidbody2D _rb;
    [BoxGroup("Components"), SerializeField, Required]
    private Animator _animator;
    [BoxGroup("Components"), SerializeField, Required]
    private SpriteRenderer _characterRenderer;

    private InputManager inputInstance;

    [BoxGroup("플레이어 상태")]
    public bool canMove = true;
    [BoxGroup("플레이어 상태"), SerializeField] 
    private float movementSpeed = 8f;
    [BoxGroup("플레이어 상태"), ShowInInspector, ReadOnly]
    private bool _lookRight = true;
    [BoxGroup("플레이어 상태"), ShowInInspector, ReadOnly]
    private bool _isMoving = false;

    private Vector2 _input;
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private float _camOffset;

    private void Awake()
    {
        inputInstance = InputManager.Instance;
    }

    void Update()
    {
        // 플레이어 이동 인풋 저장
        if (inputInstance != null)
        {
            _input = inputInstance.moveInput;
        }

        // 플레이어가 이동중인지 체크
        SetCharacterMovingAnimation();

        // 플레이어가 바라보는 방향 업데이트
        SetCharacterLookAt();

    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }


    private void MoveCharacter()
    {
        if (!canMove)
        {
            _rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = _input.normalized;

        _rb.linearVelocity = direction * movementSpeed;
    }

    /// <summary>
    /// 플레이어가 이동중인지 체크
    /// </summary>
    private void SetCharacterMovingAnimation()
    {
        bool currentMovingState = _input != Vector2.zero;
        if (_isMoving != currentMovingState)
        {
            _isMoving = currentMovingState;
            _animator.SetBool(IsMovingHash, _isMoving);
        }
    }

    /// <summary>
    /// 플레이어가 바라보는 방향 업데이트
    /// </summary>
    private void SetCharacterLookAt()
    {
        if (!canMove || _input.x == 0) return;

        bool shouldLookRight = _input.x > 0;
        if (_lookRight != shouldLookRight)
        {
            _lookRight = shouldLookRight;
            _characterRenderer.flipX = !_lookRight;

            if (camComposer != null)
            {
                camComposer.TargetOffset.x = _lookRight ? _camOffset : -_camOffset;
            }
        }
    }

}

using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 프로젝트 전역 입력 관리를 위한 Singleton 인풋 매니저
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    [Header("Input Values")]
    public Vector2 lookInput { get; private set; }
    public Vector2 moveInput { get; private set; }
    public float attackInputPressed { get; private set; }

    private InputAction _lookAction;
    private InputAction _moveAction;
    //private InputAction _jumpAction;
    private InputAction _attackAction;
    //private InputAction _interactAction;

    private void Awake()
    {
        // Singleton 패턴 적용
        if (Instance != null && Instance != gameObject)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Input Action 바인딩
        _lookAction = InputSystem.actions["Look"];
        _moveAction = InputSystem.actions["Move"];
        //_jumpAction = InputSystem.actions["Jump"];
        _attackAction = InputSystem.actions["Attack"];
        //_interactAction = InputSystem.actions["Interact"];
    }
    private void OnEnable()
    {

    }

    private void Update()
    {
        // 이동 및 시선 입력 처리
        lookInput = _lookAction != null ? _lookAction.ReadValue<Vector2>() : Vector2.zero;
        moveInput = _moveAction != null ? _moveAction.ReadValue<Vector2>() : Vector2.zero;
        attackInputPressed = _attackAction != null ? _attackAction.ReadValue<float>() : 0;
    }
}

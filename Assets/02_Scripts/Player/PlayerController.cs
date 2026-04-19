using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// JoystickController에서 전달받은 입력으로 이동,회전
/// BlenTree 파라미터 갱신
/// </summary>
public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private CharacterController cc;

    private Vector3 moveInput;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float gravity = -20f;

    [Header("Mining Action")]
    [SerializeField] private GameObject weaponObject;

    private float verticalVelocity;

    private readonly int MoveHash = Animator.StringToHash("Move");
    private readonly int DiggingHash = Animator.StringToHash("Digging");


    void Awake()
    {
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        weaponObject.SetActive(false);
    }

    private void Update()
    {
        HandleGravity();
        Move();
        Turn();
        UpdateAnimation();
    }

    private void HandleGravity()
    {
        if (cc.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;
    }

    private void Move()
    {
        Vector3 move = moveInput * moveSpeed;
        move.y = verticalVelocity;

        cc.Move(move * Time.deltaTime);
    }

    private void Turn()
    {
        Vector3 lookDir = new Vector3(moveInput.x, 0f, moveInput.z);

        if (lookDir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
        }
    }

    private void UpdateAnimation()
    {
        float targetMove = Mathf.Clamp01(moveInput.magnitude);
        float currentMove = anim.GetFloat(MoveHash);
        float smoothMove = Mathf.Lerp(currentMove, targetMove, 10f * Time.deltaTime);

        anim.SetFloat(MoveHash, smoothMove);
    }

    public void InputJoystick(Vector2 input)
    {
        moveInput = new Vector3(input.x, 0f, input.y);
    }

    public void PlayMineAction()
    {
        anim.SetTrigger(DiggingHash);
    }

    // Animation Event에서 호출
    public void ShowWeapon()
    {
        if (weaponObject != null)
            weaponObject.SetActive(true);
    }

    // Animation Event에서 호출
    public void HideWeapon()
    {
        if (weaponObject != null)
            weaponObject.SetActive(false);
    }
}


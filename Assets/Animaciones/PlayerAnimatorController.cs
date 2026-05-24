using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorController : MonoBehaviour
{
    public Animator animator;

    private static readonly int IsJumpingHash = Animator.StringToHash("IsJumping");
    private static readonly int IsGroundedHash = Animator.StringToHash("IsGrounded");
    private static readonly int IsChangingLeftHash = Animator.StringToHash("IsChangingLeft");
    private static readonly int IsChangingRightHash = Animator.StringToHash("IsChangingRight");
    private static readonly int IsLosingHash = Animator.StringToHash("IsLosing");

    void Awake() => animator = GetComponent<Animator>();

    public void SetJumping(bool value) => animator.SetBool(IsJumpingHash, value);
    public void SetGrounded(bool value) => animator.SetBool(IsGroundedHash, value);
    public void SetChangingLeft(bool value) => animator.SetBool(IsChangingLeftHash, value);
    public void SetChangingRight(bool value) => animator.SetBool(IsChangingRightHash, value);
    public void SetLosing(bool value) => animator.SetBool(IsLosingHash, value);
}
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    const string IS_WALKING = "IsWalking";
    const string IS_ATTACKING = "IsAttacking";
    const string IS_SPECIAL = "IsSpecial";
    const string IS_DEAD = "IsDead";

    [SerializeField] Player player;
    [SerializeField] GameInput gameInput;
    [SerializeField] ParticleSystem particleAttack;
    [SerializeField] ParticleSystem particleSpecial;

    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
        animator.SetBool(IS_ATTACKING, gameInput.IsAttacking());
        animator.SetBool(IS_SPECIAL, gameInput.IsSpecial());
    }

    public void OnAttackStart()
    {
        player.IsAttacking(true);

    }

    public void OnAttackEnd()
    {
        player.IsAttacking(false);
        particleAttack.gameObject.SetActive(false);
    }

    public void ParticleActivate()
    {
        particleAttack.gameObject.SetActive(true);
        particleAttack.Play();
    }

    public void OnSpecialStart()
    {
        player.IsSpecialAttack(true);
    }

    public void OnSpecialEnd()
    {
        player.IsSpecialAttack(false);
        particleSpecial.gameObject.SetActive(false);
    }

    public void ParticleSpecialActivate()
    {
        particleSpecial.gameObject.SetActive(true);
        particleSpecial.Play();
    }
}

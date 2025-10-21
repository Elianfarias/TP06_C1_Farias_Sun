using System;
using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private static readonly int State = Animator.StringToHash("State");
    public event Action onMove;
    public event Action onAttack;
    [Header("Enemy Settings")]
    [SerializeField] EnemySettingsSO data;
    [Header("Patrol Positions")]
    [SerializeField] private GameObject patrolA;
    [SerializeField] private GameObject patrolB;

    private bool toPatrolA = true;
    private bool stopMovement = false;
    private bool isDie = false;
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (toPatrolA)
            MoveToPatrol(patrolA);
        else
            MoveToPatrol(patrolB);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out HealthSystem healthSystem)
            && collision.gameObject.layer == LayerMask.NameToLayer("Player")
            && !isDie)
            StartCoroutine(Attack(healthSystem));
    }

    public void Die()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        isDie = true;
        StopMovement();
    }

    private IEnumerator Attack(HealthSystem healthSystem)
    {
        StopMovement();
        onAttack.Invoke();
        healthSystem.DoDamage(data.Damage);
        animator.SetInteger(State, (int)PlayerAnimatorEnum.Attack);

        yield return new WaitForSeconds(data.TimeStun);

        ResumeMovement();
        animator.SetInteger(State, (int)PlayerAnimatorEnum.Idle);
    }

    private void MoveToPatrol(GameObject patrol)
    {
        if (stopMovement)
            return;

        onMove.Invoke();

        Vector2 direction = (patrol.transform.position - transform.position).normalized;
        rb.velocity = (data.SpeedMovement) * Time.fixedDeltaTime * direction;
        float distance = Vector2.Distance(transform.position, patrol.transform.position);

        //Rotation
        float sign = patrol.transform.position.x > transform.position.x ? 0 : 180;
        transform.rotation = new Quaternion(0, sign, 0, 0);

        if (distance < 0.1f)
        {
            rb.velocity = Vector2.zero;
            toPatrolA = !toPatrolA;
        }
    }

    public void StopMovement()
    {
        rb.velocity = Vector2.zero;
        stopMovement = true;
    }

    public void ResumeMovement()
    {
        stopMovement = false;
    }
}

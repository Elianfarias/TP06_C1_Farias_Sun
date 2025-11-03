using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private static readonly int State = Animator.StringToHash("State");

        public event Action<float> onDashCD;
        public event Action<float> onDash;
        public event Action<float, float> onChargerJump;
        public event Action<bool> onJump;

        [Header("Player Settings")]
        [SerializeField] private PlayerDataSO data;
        [SerializeField] private ParticleSystem waterSplash;
        [Header("Dash Settings")]
        [SerializeField] private float dashCooldown = 1f;
        [Header("Sound clips")]
        [SerializeField] private AudioClip clipJump;
        [SerializeField] private AudioClip clipWalk;

        private HealthSystem healthSystem;
        private Animator animator;
        private Rigidbody2D rb;
        private bool _isDashing = false;
        private float _lastDashTime = -Mathf.Infinity;
        private bool isJumping = false;
        private bool isCharging = false;
        private float chargeStartTime = 0f;
        private float currentCharge = 0f;

        private void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (!isJumping)
                HandleJumpAndChargeInput();

            // IsOnGround();
        }

        private void FixedUpdate()
        {
            RotateTowardsMouseScreen();

            if (!Input.GetKey(data.keyCodeLeft) && !Input.GetKey(data.keyCodeRight) && !isJumping && !isCharging)
                StopMovement();

            if (Input.GetKey(data.keyCodeLeft))
                MoveX(new Vector2(-1, rb.velocityY));

            if (Input.GetKey(data.keyCodeRight))
                MoveX(new Vector2(1, rb.velocityY));

            if (Input.GetKey(data.keyCodeDown))
                MoveY(new Vector2(rb.velocityX, -1));

            if (Input.GetKey(data.keyCodeDash))
                TryDash();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                onJump?.Invoke(false);
                isJumping = false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Vector3 center = transform.position + (-transform.up * data.distanceOffset);
            Gizmos.DrawWireSphere(center, data.radiusCircleRaycast);
        }

        private void IsOnGround()
        {
            Vector3 center = transform.position + (-transform.up * data.distanceOffset);

            RaycastHit2D[] hits = Physics2D.CircleCastAll(center, data.radiusCircleRaycast, Vector2.down, 0.5f);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    onJump?.Invoke(false);
                    isJumping = false;
                    return;
                }
            }

            onJump?.Invoke(true);
            isJumping = true;
        }

        private void HandleJumpAndChargeInput()
        {
            if (Input.GetKeyDown(data.keyCodeJump) || Input.GetKeyDown(KeyCode.Space))
            {
                chargeStartTime = Time.time;
                isCharging = false;
            }

            if (Input.GetKey(data.keyCodeJump) || Input.GetKey(KeyCode.Space))
            {
                if (!isCharging && Time.time >= chargeStartTime + data.tapThreshold)
                    StartCharging();

                if (isCharging)
                    ContinueCharging();
            }

            if (Input.GetKeyUp(data.keyCodeJump) || Input.GetKeyUp(KeyCode.Space))
            {
                float held = Time.time - chargeStartTime;
                if (!isCharging && held < data.tapThreshold)
                    Jump();
                else if (isCharging)
                    ReleaseCharge();

                isCharging = false;
                currentCharge = 0f;
            }
        }

        private void StartCharging()
        {
            isCharging = true;
            currentCharge = 0f;
            onChargerJump?.Invoke(currentCharge, data.timeToFullCharge);
        }

        private void ContinueCharging()
        {
            if (!isCharging) return;
            if (data.timeToFullCharge <= 0f) currentCharge = 1f;
            else currentCharge += (1f / data.timeToFullCharge) * Time.deltaTime;
            currentCharge = Mathf.Clamp01(currentCharge);
            onChargerJump?.Invoke(currentCharge, data.timeToFullCharge);
        }

        private void ReleaseCharge()
        {
            if (currentCharge < data.minChargeToRelease)
                return;

            float impulse = Mathf.Lerp(0f, data.maxImpulseForce, currentCharge);
            impulse = impulse < data.jumpForce ? data.jumpForce : impulse;

            Vector2 dir = GetChargeDirection();
            isJumping = true;
            rb.AddForce(dir * impulse, ForceMode2D.Impulse);
            onJump?.Invoke(true);
        }

        private Vector2 GetChargeDirection()
        {
            Camera cam = Camera.main;
            if (cam == null)
            {
                float sign = transform.localScale.x >= 0 ? 1f : -1f;
                return new Vector2(sign, 0f).normalized;
            }

            Vector3 mouseScreen = Input.mousePosition;
            Vector3 mouseWorld = cam.ScreenToWorldPoint(mouseScreen);
            mouseWorld.z = transform.position.z;

            Vector3 diff = mouseWorld - transform.position;
            if (diff.sqrMagnitude < 0.0001f)
            {
                float sign = transform.localScale.x >= 0 ? 1f : -1f;
                return new Vector2(sign, 0f).normalized;
            }

            return (Vector2)diff.normalized;
        }

        private void Jump()
        {
            if (isJumping)
                return;

            AudioController.Instance.PlaySoundEffect(clipJump, priority: 1);
            isJumping = true;
            animator.SetInteger(State, (int)PlayerAnimatorEnum.Jump);
            rb.AddForce(data.jumpForce * Vector2.up, ForceMode2D.Impulse);
            onJump?.Invoke(true);
        }

        private void StopMovement()
        {
            animator.SetInteger(State, (int)PlayerAnimatorEnum.Idle);
            rb.velocity = new Vector2(0, rb.velocityY);
        }

        private void MoveX(Vector2 axis)
        {
            if (!isJumping)
                animator.SetInteger(State, (int)PlayerAnimatorEnum.Run);

            AudioController.Instance.PlaySoundEffect(clipWalk);
            float newSpeed = data.speed > rb.velocityX ? data.speed : rb.velocityX;
            Vector2 movementSpeed = new(newSpeed * axis.x, rb.velocityY);

            rb.velocity = movementSpeed;
        }

        private void MoveY(Vector2 axis)
        {
            float newSpeed = data.speed > rb.velocityY ? data.speed : rb.velocityY;
            Vector2 movementSpeed = new(rb.velocityX, newSpeed * axis.y);

            rb.velocity = movementSpeed;
        }

        private void TryDash()
        {
            if (Time.time < _lastDashTime + dashCooldown)
                return;
            if (_isDashing)
                return;
            if (rb.velocity == Vector2.zero)
                return;

            StartCoroutine(DashRoutine(rb.velocity));
        }

        private IEnumerator DashRoutine(Vector2 velocity)
        {
            _isDashing = true;
            _lastDashTime = Time.time;

            onDashCD.Invoke(data.dashCD);
            onDash.Invoke(data.dashDuration);

            // Ignore enemies
            GameStateManager.Instance.inmortalMode = true;
            gameObject.layer = LayerMask.NameToLayer("PlayerDash");

            rb.AddForceX(velocity.normalized.x * data.dashSpeed, ForceMode2D.Impulse);

            yield return new WaitForSeconds(data.dashDuration);

            GameStateManager.Instance.inmortalMode = false;
            gameObject.layer = LayerMask.NameToLayer("Player");

            _isDashing = false;
        }

        private void RotateTowardsMouseScreen()
        {
            Vector3 mousePos = Input.mousePosition;
            float playerScreenX = Camera.main.WorldToScreenPoint(transform.position).x;
            float sign = mousePos.x > playerScreenX ? 1f : -1f;
            transform.localScale = new Vector3(sign, 1f, 1f);
        }
    }
}
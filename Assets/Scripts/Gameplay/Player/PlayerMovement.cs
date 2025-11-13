using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private static readonly int State = Animator.StringToHash("State");

        public event Action<float> OnDashCD;
        public event Action<float> OnDash;
        public event Action<float, float> OnChargerJump;
        public event Action<bool> OnJump;

        [Header("Player Settings")]
        public PlayerDataSO data;
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

        private void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            RotateTowardsMouseScreen();

            if (Input.GetKey(data.keyCodeDash))
                TryDash();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Vector3 center = transform.position + (-transform.up * data.distanceOffset);
            Gizmos.DrawWireSphere(center, data.radiusCircleRaycast);
        }

        public bool IsOnGround()
        {
            Vector3 center = transform.position + (-transform.up * data.distanceOffset);

            RaycastHit2D[] hits = Physics2D.CircleCastAll(center, data.radiusCircleRaycast, Vector2.down, 0.1f);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    return true;
                }
            }

            return false;
        }

        public void OnGroundInvoke()
        {
            OnJump?.Invoke(false);
        }


        public void Charging(ref float currentCharge)
        {
            if (data.timeToFullCharge <= 0f) currentCharge = 1f;
            else currentCharge += (1f / data.timeToFullCharge) * Time.deltaTime;
            currentCharge = Mathf.Clamp01(currentCharge);
            OnChargerJump?.Invoke(currentCharge, data.timeToFullCharge);
        }

        public void ReleaseCharge(ref float currentCharge)
        {
            if (currentCharge < data.minChargeToRelease)
                return;

            float impulse = Mathf.Lerp(0f, data.maxImpulseForce, currentCharge);
            impulse = impulse < data.jumpForce ? data.jumpForce : impulse;

            Vector2 dir = GetChargeDirection();
            rb.AddForce(dir * impulse, ForceMode2D.Impulse);
            OnJump?.Invoke(true);
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

        public void Jump()
        {
            AudioController.Instance.PlaySoundEffect(clipJump, priority: 1);
            animator.SetInteger(State, (int)PlayerAnimatorEnum.Jump);
            rb.AddForce(data.jumpForce * Vector2.up, ForceMode2D.Impulse);
            OnJump?.Invoke(true);
        }

        public void StopMovement()
        {
            animator.SetInteger(State, (int)PlayerAnimatorEnum.Idle);
            rb.velocity = new Vector2(0, rb.velocityY);
        }

        public void MoveX(float axisX)
        {
            float newSpeed = data.speed > rb.velocityX ? data.speed : rb.velocityX;
            Vector2 movementSpeed = new(newSpeed * axisX, rb.velocityY);

            rb.velocity = movementSpeed;
        }

        public void MoveY(int axisY)
        {
            float newSpeed = data.speed > rb.velocityY ? data.speed : rb.velocityY;
            Vector2 movementSpeed = new(rb.velocityX, newSpeed * axisY);

            rb.velocity = movementSpeed;
        }

        public void TryDash()
        {
            if (Time.time < _lastDashTime + dashCooldown)
                return;
            if (_isDashing)
                return;
            if (rb.velocity == Vector2.zero)
                return;

            StartCoroutine(DashRoutine(rb.velocity));
        }

        public float GetVelocityX()
        {
            return rb.velocity.x;
        }

        private IEnumerator DashRoutine(Vector2 velocity)
        {
            _isDashing = true;
            _lastDashTime = Time.time;
            int sign = velocity.x > 0 ? 1 : -1;

            OnDashCD.Invoke(data.dashCD);
            OnDash.Invoke(data.dashDuration);

            // Ignore enemies
            GameStateManager.Instance.inmortalMode = true;
            gameObject.layer = LayerMask.NameToLayer("PlayerDash");

            rb.velocity = new Vector2(sign * data.dashSpeed, velocity.y);

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
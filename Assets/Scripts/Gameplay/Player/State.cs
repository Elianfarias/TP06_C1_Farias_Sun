using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Player
{
    public abstract class State
    {
        public PlayerAnimatorEnum state { get; protected set; } = PlayerAnimatorEnum.Idle;
        protected PlayerController playerController;
        protected PlayerMovement playerMovement;
        protected PlayerAttack playerAttack;

        public virtual void OnEnter()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void OnExit()
        {
        }
    }
}
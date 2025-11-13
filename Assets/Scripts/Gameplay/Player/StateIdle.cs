using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Player
{
    public class StateIdle : State
    {
        public StateIdle(PlayerMovement playerMovement, PlayerController playerController)
        {
            this.playerMovement = playerMovement;
            this.playerController = playerController;
            state = PlayerAnimatorEnum.Idle;
        }

        public override void OnEnter()
        {
            playerController.ChangeAnimatorState((int)state);
        }

        public override void Update()
        {
            // Conexiones de Salida
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
                playerController.SwapStateTo(PlayerAnimatorEnum.Run);
            else if (Input.GetKeyDown(KeyCode.LeftControl))
                playerController.SwapStateTo(PlayerAnimatorEnum.Attack);
            else if (Input.GetKeyDown(KeyCode.Space))
                playerController.SwapStateTo(PlayerAnimatorEnum.Jump);

            // UPDATE
            playerMovement.StopMovement();
        }
    }
}
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Gameplay.Player
{
    public class StateWalk : State
    {
        public StateWalk(PlayerMovement playerMovement, PlayerController playerController)
        {
            this.playerMovement = playerMovement;
            this.playerController = playerController;
            state = PlayerAnimatorEnum.Run;
        }

        public override void OnEnter()
        {
            playerController.ChangeAnimatorState((int)state);
        }

        public override void Update()
        {
            // Conexiones de Salida
            if (Input.GetKeyDown(KeyCode.LeftControl))
                playerController.SwapStateTo(PlayerAnimatorEnum.Attack);
            else if (Input.GetKeyDown(KeyCode.Space))
                playerController.SwapStateTo(PlayerAnimatorEnum.Jump);
            else if (Input.GetKeyDown(KeyCode.LeftShift))
                playerController.SwapStateTo(PlayerAnimatorEnum.Dash);

            // UPDATE
            playerController.ChangeAnimatorState((int)state);
            if (Input.GetKey(playerMovement.data.keyCodeLeft))
                playerMovement.MoveX(-1);
            else if (Input.GetKey(playerMovement.data.keyCodeRight))
                playerMovement.MoveX(1);
            else if (Input.GetKey(playerMovement.data.keyCodeDown))
                playerMovement.MoveY(-1);
            else
                playerController.SwapStateTo(PlayerAnimatorEnum.Idle);
        }
    }
}
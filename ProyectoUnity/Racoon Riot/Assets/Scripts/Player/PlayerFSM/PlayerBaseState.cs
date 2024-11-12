
using UnityEngine;

namespace PLAYERAI.PlayerFSM
{
    public abstract class PlayerBaseState
    {
        public enum PLAYER_STATE
        {
            NORMAL, CAPTURED, RECOVERING
        }

        public enum EVENT
        {
            ENTER, UPDATE, EXIT
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}

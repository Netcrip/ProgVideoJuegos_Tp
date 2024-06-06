using UnityEngine;

public class PlayerState : MonoBehaviour
{
 public enum playerState
    {  
        Idle,
        Waiting,
        Walking,
        Running,
        Jumping,
        FullJumping,
        Attacking,
        Attacking2,
        Attacking3,
        SpinAttack,
        Dead,
        Victory,
        DefenceOn,
        DefenceOff,
        Sprint
    }
}

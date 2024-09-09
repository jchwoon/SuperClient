using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationData
{
    private string idleParameterName = "Idle";
    private string walkParameterName = "Walk";
    private string moveSpeedParameterName = "MoveSpeed";
    public int IdleHash { get; private set; }
    public int WalkHash { get; private set; }
    public int MoveSpeedHash { get; private set; }

    public AnimationData()
    {
        IdleHash = Animator.StringToHash(idleParameterName);
        WalkHash = Animator.StringToHash(walkParameterName);
        MoveSpeedHash = Animator.StringToHash(moveSpeedParameterName);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationData
{
    private string moveSpeedParameterName = "MoveSpeed";
    private string moveParameterName = "IsMove";
    private string idleParameterName = "IsIdle";
    private string dieParameterName = "IsDie";
    private string revivalParameterName = "Revival";

    private string hitStateName = "Base Layer.Hit";

    public int MoveSpeedHash { get; private set; }
    public int IdleHash { get; private set; }
    public int MoveHash { get; private set; }
    public int DieHash { get; private set; }
    public int RevivalHash { get; private set; }
    public int HitHash { get; private set; }

    public AnimationData()
    {
        MoveSpeedHash = Animator.StringToHash(moveSpeedParameterName);
        IdleHash = Animator.StringToHash(idleParameterName);
        MoveHash = Animator.StringToHash(moveParameterName);
        DieHash = Animator.StringToHash(dieParameterName);
        RevivalHash = Animator.StringToHash(revivalParameterName);
        HitHash = Animator.StringToHash(hitStateName);
    }
}

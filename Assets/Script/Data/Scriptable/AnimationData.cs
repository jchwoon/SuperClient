using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationData
{
    private string idleParmeterName = "Idle";
    private string walkParmeterName = "Walk";
    public int IdleHash { get; private set; }
    public int WalkHash { get; private set; }

    public AnimationData()
    {
        IdleHash = Animator.StringToHash(idleParmeterName);
        WalkHash = Animator.StringToHash(walkParmeterName);
    }

}

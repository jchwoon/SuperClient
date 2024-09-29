using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationData
{

    private string moveSpeedParameterName = "MoveSpeed";
    private string attackParameterName = "Attack";
    private string attackComboParameterName = "AttackCombo";
    public int MoveSpeedHash { get; private set; }
    public int AttackHash { get; private set; }
    public int AttackComboHash { get; private set; }

    public AnimationData()
    {
        MoveSpeedHash = Animator.StringToHash(moveSpeedParameterName);
        AttackHash = Animator.StringToHash(attackParameterName);
        AttackComboHash = Animator.StringToHash(attackComboParameterName);
    }

}

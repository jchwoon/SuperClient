using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationData
{

    private string moveSpeedParameterName = "MoveSpeed";
    private string attackComboParameterName = "AttackCombo";
    private string attackComboIdxParameterName = "ComboIdx";
    private string attackParameterName = "IsAttack";
    private string moveParameterName = "IsMove";
    private string idleParameterName = "IsIdle";
    private string hitParameterName = "IsHit";
    private string dieParameterName = "IsDie";

    public int MoveSpeedHash { get; private set; }
    public int AttackComboHash { get; private set; }
    public int AttackComboIdxHash { get; private set; }
    public int AttackHash { get; private set; }
    public int IdleHash { get; private set; }
    public int MoveHash { get; private set; }
    public int DieHash { get; private set; }
    public int HitHash { get; private set; }

    public AnimationData()
    {
        MoveSpeedHash = Animator.StringToHash(moveSpeedParameterName);
        AttackHash = Animator.StringToHash(attackParameterName);
        AttackComboHash = Animator.StringToHash(attackComboParameterName);
        AttackComboIdxHash = Animator.StringToHash(attackComboIdxParameterName);
        IdleHash = Animator.StringToHash(idleParameterName);
        MoveHash = Animator.StringToHash(moveParameterName);
        DieHash = Animator.StringToHash(dieParameterName);
        HitHash = Animator.StringToHash(hitParameterName);
    }

}

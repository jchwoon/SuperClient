using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationData
{

    private string moveSpeedParameterName = "MoveSpeed";
    private string skillParameterName = "IsSkill";
    private string moveParameterName = "IsMove";
    private string idleParameterName = "IsIdle";
    private string dieParameterName = "IsDie";
    private string attackSpeedParameterName = "AttackSpeed";

    public int MoveSpeedHash { get; private set; }
    public int AttackSpeedHash { get; private set; }
    public int SkillHash { get; private set; }
    public int IdleHash { get; private set; }
    public int MoveHash { get; private set; }
    public int DieHash { get; private set; }

    public AnimationData()
    {
        MoveSpeedHash = Animator.StringToHash(moveSpeedParameterName);
        SkillHash = Animator.StringToHash(skillParameterName);
        AttackSpeedHash = Animator.StringToHash(attackSpeedParameterName);
        IdleHash = Animator.StringToHash(idleParameterName);
        MoveHash = Animator.StringToHash(moveParameterName);
        DieHash = Animator.StringToHash(dieParameterName);
    }


}

using Data;
using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using MyHeroState;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class MyHeroStateMachine : StateMachine
{
    public IdleState IdleState { get; set; }
    public MoveState MoveState { get; set; }
    public SkillState SkillState { get; set; }
    public Vector2 MoveInput { get; set; } = Vector2.zero;
    public float MoveRatio { get; private set; } = 0.2f;
    public MoveToS MovePacket { get; set; }
    public Creature Target { get; set; }
    public MyHero Owner { get; set; }
    //어택모드가 활성화 되어있는지 아닌지
    public bool Attacking { get; set; } = false;
    //Target을 기준으로 움직일지 Input을 기준으로 움직일지
    public bool TargetMode { get; set; } = false;
    //스킬 요청에 대한 응답을 대기중인지
    public bool isWaitSkillRes { get; set; } = false;
    public int? CurrentActiveSkillHash { get; set; }
    public MyHeroStateMachine(MyHero myHero)
    {
        MovePacket = new MoveToS() { PosInfo = new PosInfo()};
        Owner = myHero;
        SetState();
        ChangeState(IdleState);
        Managers.GameManager.OnJoystickChanged += UpdateMoveInput;
    }
    public override void FindTargetAndAttack()
    {
        if (Attacking == false)
        {
            if (Target == null)
                Target = FindTarget();
            if (Target == null)
            {
                Managers.UIManager.ShowToasUI("주위에 지정할 타겟이 없습니다.");
                return;
            }
            TargetMode = true;
        }
        else
        {
            TargetMode = false;
        }

        ToggleAttacking();
    }

    public Creature FindTarget()
    {
        List<Creature> creatures = Managers.ObjectManager.GetAllCreatures();
        Creature target = null;
        float closestDist = float.MaxValue;
        foreach(Creature creature in creatures)
        {
            float dist = (creature.gameObject.transform.position - Owner.transform.position).sqrMagnitude;
            if (dist < closestDist)
            {
                closestDist = dist;
                target = creature.gameObject.GetComponent<Creature>();
            }
        }
        return target;
    }

    public override void ChangeState(IState changeState)
    {
        base.ChangeState(changeState);
    }

    public override void UseSkill(SkillData skillData, Creature target)
    {
        if (skillData == null || target == null)
            return;

        BaseSkill skill = Owner.SkillComponent.GetSkillById(skillData.SkillId);
        skill.UseSkill();
        SetAnimParameter(Owner, Owner.AnimData.SkillHash, true);
        ChangeState(SkillState);
        Owner.transform.LookAt(target.transform);

        //target에게 데미지 입히기
    }

    public float GetModifiedSpeed()
    {
        return 20 * MoveRatio;
    }

    private void SetState()
    {
        IdleState = new IdleState(this);
        MoveState = new MoveState(this);
        SkillState = new SkillState(this);
    }

    private void UpdateMoveInput(Vector2 moveInput)
    {
        MoveInput = moveInput;
        if (moveInput != Vector2.zero)
            TargetMode = false;
        else if (Attacking == true)
            TargetMode = true;
    }

    private void ToggleAttacking()
    {
        Attacking = Attacking == true ? false : true;
        JoySceneUI ui = Managers.UIManager.ShowSceneUI<JoySceneUI>();
        ui.ChangeAtkBtnActivation(Attacking);
    }
}

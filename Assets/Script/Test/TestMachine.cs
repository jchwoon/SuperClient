using Google.Protobuf.Enum;
using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using MyHeroState;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class TestMachine : StateMachine
{

    public Vector2 MoveInput { get; private set; } = Vector2.zero;
    public TestMyHe Owner { get; set; }
    //스킬 요청에 대한 응답을 대기중인지
    public bool isWaitSkillRes { get; set; } = false;
    public TestMachine(TestMyHe myHero)
    {
        Owner = myHero;
        Enter();
        Managers.GameManager.OnJoystickChanged += UpdateMoveInput;
    }
    public override void Update()
    {
        CheckAndMove();
    }

    private void StateUpdate()
    {
        if (CurrentState == null)
            return;

        if (CreatureState == ECreatureState.Die)
            return;

        CurrentState.Update();
    }
    private void UpdateMoveInput(Vector2 moveInput)
    {
        MoveInput = moveInput;
    }
    public void Enter()
    {
        SetAnimParameter(Owner, Owner.AnimData.MoveSpeedHash, 4);
    }


    private void CheckAndMove()
    {
        Vector2 inputDir = MoveInput.normalized;
        Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.y);

        Vector3 destPos = Owner.transform.position + moveDir;

        ToMove(destPos);
        RotateToMoveDir(destPos);
    }
    private void ToMove(Vector3 destPos)
    {
        Owner.transform.position = Vector3.MoveTowards(Owner.transform.position, destPos, 4 * Time.deltaTime);
    }
    private void RotateToMoveDir(Vector3 target)
    {
        Vector3 targetDir = (target - Owner.transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDir);
        Owner.transform.rotation = Quaternion.Slerp(Owner.transform.rotation, targetRotation, 10 * Time.deltaTime);
    }
}

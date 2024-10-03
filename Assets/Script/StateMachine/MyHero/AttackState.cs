using Data;
using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyHeroState {
    public class AttackState : BaseState
    {
        private float _comboExitTime;
        private Coroutine _coroutine;
        public AttackState(MyHeroStateMachine heroMachine) : base(heroMachine)
        {
            HeroData heroData;
            if (Managers.DataManager.HeroDict.TryGetValue(EHeroClassType.Warrior, out heroData)  == true)
            {
                _comboExitTime = heroData.ComboExitTime;
            }
        }

        public override void Exit()
        {
            base.Exit();
            _heroMachine.SetAnimParameter(_owner, _owner.AnimData.AttackHash, false);
        }
        public override void Enter()
        {
            base.Enter();
            _heroMachine.SetAnimParameter(_owner, _owner.AnimData.AttackHash, true);

            if (_heroMachine.Target != null)
            {
                float dist = (_heroMachine.Target.transform.position - _owner.transform.position).magnitude;
                if (dist > 1)
                {
                    _owner.Animator.SetLayerWeight((int)Enums.AnimLayer.LowerBody, 1);
                    CoroutineHelper.Instance.StartHelperCoroutine(Dash());
                }
            }
        }
        public override void Update()
        {
            base.Update();

        }

        public void StartComboExitRoutine()
        {
            StopComboExitRoutine();
            _coroutine = CoroutineHelper.Instance.StartHelperCoroutine(ComboExitRoutine());
        }
        public void StopComboExitRoutine()
        {
            if (_coroutine != null)
                CoroutineHelper.Instance.StopHelperCoroutine(_coroutine);
        }

        IEnumerator ComboExitRoutine()
        {
            yield return new WaitForSeconds(_comboExitTime);
            _heroMachine.ChangeState(_heroMachine.IdleState);
            _heroMachine.Attacking = false;
        }

        IEnumerator Dash()
        {
            while (Vector3.Distance(_heroMachine.Owner.transform.position, _heroMachine.Target.transform.position) > 1)
            {
                _owner.Agent.Move(_owner.transform.forward * 10 * Time.deltaTime);
                yield return null;
            }
            _owner.Animator.SetLayerWeight((int)Enums.AnimLayer.LowerBody, 0);
        }
    }
}

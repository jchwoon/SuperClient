using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartyTabUI : BaseUI
{
    enum Texts
    {
        DungeonInfoTxt
    }
    
    enum GameObjects
    {
        FirstMemberSlot,
        SecondMemberSlot,
        ThirdMemberSlot,
        FourthMemberSlot
    }

    List<GameObject> _memberSlots = new List<GameObject>(4);
    protected override void Awake()
    {
        base.Awake();

        Bind<TMP_Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        _memberSlots.Add(Get<GameObject>((int)GameObjects.FirstMemberSlot));
        _memberSlots.Add(Get<GameObject>((int)GameObjects.SecondMemberSlot));
        _memberSlots.Add(Get<GameObject>((int)GameObjects.ThirdMemberSlot));
        _memberSlots.Add(Get<GameObject>((int)GameObjects.FourthMemberSlot));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Managers.EventBus.AddEvent(Enums.EventType.UpdatePartyMembers, UpdateMembers);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Managers.EventBus.RemoveEvent(Enums.EventType.UpdatePartyMembers, UpdateMembers);
    }

    public void Refresh()
    {
        Party myParty = Managers.PartyManager.MyParty;
        if (myParty == null)
        {
            return;
        }

        List<MemberInfo> members = myParty.Members;

        for (int i = 0; i < _memberSlots.Count; i++)
        {
            if (i < members.Count)
            {
                Utils.GetOrAddComponent<MemberSlot>(_memberSlots[i]).SetInfo(members[i]);
            }
            else
            {
                Utils.GetOrAddComponent<MemberSlot>(_memberSlots[i]).SetInfo(null);
            }

        }
    }

    private void UpdateMembers()
    {

    }
}

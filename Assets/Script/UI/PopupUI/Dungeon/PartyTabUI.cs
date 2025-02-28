using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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
        FourthMemberSlot,
        InteractionColumn,
        ApplierContents,
        LeavePartyBtn,
        EnterDungeonBtn
    }

    List<GameObject> _memberSlots = new List<GameObject>(4);

    GameObject _interactionColumn;
    GameObject _applierContents;

    const int BASE_APPLIER_SLOT_COUNT = 10;

    protected override void Awake()
    {
        base.Awake();

        Bind<TMP_Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        _interactionColumn = Get<GameObject>((int)GameObjects.InteractionColumn);
        _applierContents = Get<GameObject>((int)GameObjects.ApplierContents);

        for (int i = 0; i < BASE_APPLIER_SLOT_COUNT; i++)
        {
            Managers.UIManager.GenerateSlot<ApplierSlot>(_applierContents.transform);
        }

        _memberSlots.Add(Get<GameObject>((int)GameObjects.FirstMemberSlot));
        _memberSlots.Add(Get<GameObject>((int)GameObjects.SecondMemberSlot));
        _memberSlots.Add(Get<GameObject>((int)GameObjects.ThirdMemberSlot));
        _memberSlots.Add(Get<GameObject>((int)GameObjects.FourthMemberSlot));

        BindEvent(Get<GameObject>((int)GameObjects.LeavePartyBtn), OnClickedLeavePartyBtn);
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

        RefreshPartyMemberSection(myParty.Members);
        RefreshApplySection();
    }

    private void RefreshPartyMemberSection(List<MemberInfo> members)
    {
        if (members == null)
            return;

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

    private void RefreshApplySection()
    {
        Party myParty = Managers.PartyManager.MyParty;

        int applierCount = myParty.Appliers.Count;
        int applierSlotCount = _applierContents.transform.childCount;

        for (int i = 0; i < applierCount; i++)
        {
            if (i < applierSlotCount)
            {
                GameObject go = _applierContents.transform.GetChild(i).gameObject;
                Utils.GetOrAddComponent<ApplierSlot>(go).SetInfo(myParty.Appliers[i]);
            }
            else
            {
                Managers.UIManager.GenerateSlot<ApplierSlot>(_applierContents.transform).SetInfo(myParty.Appliers[i]);
            }
        }
    }

    private void UpdateMembers()
    {

    }

    private void OnClickedLeavePartyBtn(PointerEventData eventData)
    {
        Managers.PartyManager.LeaveParty();
    }
}

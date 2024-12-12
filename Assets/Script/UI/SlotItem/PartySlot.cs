using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PartySlot : BaseUI
{
    protected enum Texts
    {
        Member1Name,
        Member2Name,
        Member3Name,
        Member4Name,
        Member1LV,
        Member2LV,
        Member3LV,
        Member4LV,
    }
    [HideInInspector] public int PartyId;
    private const int MaxPartyMembers = 4;
    
    private TMP_Text[] _memberNames = new TMP_Text[MaxPartyMembers];
    private TMP_Text[] _memberLevels = new TMP_Text[MaxPartyMembers];

    protected override void Awake()
    {
        base.Awake();

        Bind<TMP_Text>(typeof(Texts));

        for (int i = 0; i < MaxPartyMembers; i++)
        {
            _memberNames[i] = Get<TMP_Text>((int)Texts.Member1Name + i);
            _memberLevels[i] = Get<TMP_Text>((int)Texts.Member1LV + i);
        }

        ResetUI();

        Managers.PartyManager.OnPartyUpdated += UpdateUI;
    }

    private void ResetUI()
    {
        for (int i = 0; i < MaxPartyMembers; i++)
        {
            _memberNames[i].text = "";
            _memberLevels[i].text = "";
        }
    }    

    private void UpdateUI()
    {
        ResetUI();

        Party party = Managers.PartyManager.GetParty(PartyId);
        if (party == null) 
            return;

        for (int i = 0; i < party.PartyMembers.Count; i++)
        {
            UpdateMemberUI(i, party.PartyMembers[i]);
        }
    }

    private void UpdateMemberUI(int index, PartyMember member)
    {
        if (index < 0 || index >= MaxPartyMembers)
            return;

        _memberNames[index].text = member.Name;
        _memberLevels[index].text = $"LV. {member.Level}";
    }

    public void ClickPartyInfo()
    {
        Debug.Log(gameObject.name);
        Debug.Log(PartyId);
    }
}

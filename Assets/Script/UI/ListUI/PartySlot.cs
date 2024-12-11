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

    private struct PartyMember
    {
        public string Name;
        public int Level;

        public PartyMember(string name, int level)
        {
            Name = name;
            Level = level;
        }
    }

    private const int MaxPartyMembers = 4;
    private List<PartyMember> _partyMembers = new List<PartyMember>();

    // UI 요소
    private TMP_Text[] _memberNames = new TMP_Text[MaxPartyMembers];
    private TMP_Text[] _memberLevels = new TMP_Text[MaxPartyMembers];

    protected override void Awake()
    {
        base.Awake();

        Bind<TMP_Text>(typeof(Texts));

        _memberNames[0] = Get<TMP_Text>((int)Texts.Member1Name);
        _memberNames[1] = Get<TMP_Text>((int)Texts.Member2Name);
        _memberNames[2] = Get<TMP_Text>((int)Texts.Member3Name);
        _memberNames[3] = Get<TMP_Text>((int)Texts.Member4Name);

        _memberLevels[0] = Get<TMP_Text>((int)Texts.Member1LV);
        _memberLevels[1] = Get<TMP_Text>((int)Texts.Member2LV);
        _memberLevels[2] = Get<TMP_Text>((int)Texts.Member3LV);
        _memberLevels[3] = Get<TMP_Text>((int)Texts.Member4LV);

        ResetUI();
    }

    private void ResetUI()
    {
        for (int i = 0; i < MaxPartyMembers; i++)
        {
            _memberNames[i].text = "";
            _memberLevels[i].text = "";
        }
    }

    public void AddPartyMember(string userName, int userLv)
    {
        if (_partyMembers.Count >= MaxPartyMembers)
        {
            // 인원초과
            return;
        }

        if (_partyMembers.Exists(m => m.Name == userName))
        {
            // 이미 존재하는 멤버인지 확인
            return;
        }

        _partyMembers.Add(new PartyMember(userName, userLv));
        UpdateUI();
    }

    public void RemovePartyMember(string userName)
    {
        var member = _partyMembers.Find(m => m.Name == userName);
        if (member.Name != null)
        {
            _partyMembers.Remove(member);
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        ResetUI(); // 초기화 후 다시 세팅

        for (int i = 0; i < _partyMembers.Count; i++)
        {
            _memberNames[i].text = _partyMembers[i].Name;
            _memberLevels[i].text = $"LV. {_partyMembers[i].Level}";
        }
    }
}

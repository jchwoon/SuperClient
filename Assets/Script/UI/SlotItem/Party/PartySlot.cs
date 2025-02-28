using Google.Protobuf.Struct;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartySlot : BaseUI
{
    enum Transforms
    {
        FirstMemberInfo,
        SecondMemberInfo,
        ThirdMemberInfo,
        FourthMemberInfo
    }
    enum GameObjects
    {
        ApplyPartyBtn
    }

    public PartyInfo PartyInfo { get; private set; }

    List<Transform> _memberInfo = new List<Transform>(4);

    GameObject _applyBtn;
    readonly float applyCooltime = 10f;
    bool isApplyCool = false;
    float applyElapsedTime = 0f;

    protected override void Awake()
    {
        base.Awake();
        Bind<Transform>(typeof(Transforms));
        Bind<GameObject>(typeof(GameObjects));

        _memberInfo.Add(Get<Transform>((int)Transforms.FirstMemberInfo));
        _memberInfo.Add(Get<Transform>((int)Transforms.SecondMemberInfo));
        _memberInfo.Add(Get<Transform>((int)Transforms.ThirdMemberInfo));
        _memberInfo.Add(Get<Transform>((int)Transforms.FourthMemberInfo));

        _applyBtn = Get<GameObject>((int)GameObjects.ApplyPartyBtn);

        BindEvent(_applyBtn, OnApplyBtnClicked);
    }
    public void SetInfo(PartyInfo info)
    {
        PartyInfo = info;
        
        for (int i = 0; i < _memberInfo.Count; i++)
        {
            MemberInfoSlot memberSlot = Utils.GetOrAddComponent<MemberInfoSlot>(_memberInfo[i].gameObject);
            if (i < info.MemberInfos.Count)
            {
                memberSlot.SetInfo(info.MemberInfos[i]);
            }
            else
            {
                memberSlot.SetInfo(null);
            }
        }

        if (Managers.PartyManager.MyParty != null)
        {
            _applyBtn.SetActive(false);
        }
    }

    private void OnApplyBtnClicked(PointerEventData eventData)
    {
        if (isApplyCool)
        {
            Managers.UIManager.ShowToasUI($"해당 파티에 {Mathf.Round(applyCooltime - applyElapsedTime)}초 후에 지원할 수 있습니다.");
            return;
        }
        Managers.PartyManager.ReqApplyParty(PartyInfo.PartyId);
        StartCoroutine(CoCooltimeSendApply());
    }

    //지원 요청 쿨타임
    IEnumerator CoCooltimeSendApply()
    {
        isApplyCool = true;
        applyElapsedTime = 0f;

        while (applyElapsedTime < applyCooltime)
        {
            applyElapsedTime += Time.deltaTime;
            yield return null;
        }

        applyElapsedTime = 0f;
        isApplyCool = false;
    }
}

public class MemberInfoSlot : BaseUI
{
    enum Texts
    {
        LevelTxt,
        NicknameTxt
    }

    enum Images
    {
        ClassImage
    }

    protected override void Awake()
    {
        base.Awake();
        Bind<TMP_Text>(typeof(Texts));
        Bind<Image>(typeof(Images));


    }

    public void SetInfo(MemberInfo memberInfo)
    {
        TMP_Text levelText = Get<TMP_Text>((int)Texts.LevelTxt);
        TMP_Text nicknameText = Get<TMP_Text>((int)Texts.NicknameTxt);
        Image classImage = Get<Image>((int)Images.ClassImage);
        if (memberInfo == null)
        {
            levelText.text = string.Empty;
            nicknameText.text = string.Empty;
            classImage.sprite = null;
            classImage.color = Color.clear;
        }
        else
        {
            levelText.text = $"Lv. {memberInfo.Info.Level}";
            nicknameText.text = memberInfo.Info.Nickname;
            classImage.sprite = Managers.ResourceManager.GetResource<Sprite>($"{memberInfo.Info.ClassType}_sprite");
            classImage.color = Color.white;
        }
    }
}

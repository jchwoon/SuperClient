using Google.Protobuf.Struct;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    public PartyInfo PartyInfo { get; private set; }

    List<Transform> _memberInfo = new List<Transform>(4);

    protected override void Awake()
    {
        base.Awake();
        Bind<Transform>(typeof(Transforms));

        _memberInfo.Add(Get<Transform>((int)Transforms.FirstMemberInfo));
        _memberInfo.Add(Get<Transform>((int)Transforms.SecondMemberInfo));
        _memberInfo.Add(Get<Transform>((int)Transforms.ThirdMemberInfo));
        _memberInfo.Add(Get<Transform>((int)Transforms.FourthMemberInfo));
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
            levelText.text = $"Lv. {memberInfo.Level}";
            nicknameText.text = memberInfo.Name;
            classImage.sprite = Managers.ResourceManager.GetResource<Sprite>($"{memberInfo.Class}_sprite");
            classImage.color = Color.white;
        }
    }
}

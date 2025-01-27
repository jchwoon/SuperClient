using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MemberSlot : BaseUI
{
    enum Images
    {
        LeaderMarkImage,
        ClassImage
    }
    enum Texts
    {
        NicknameTxt,
        LevelAndCombatTxt
    }
    enum GameObjects
    {
        MemberInfo,
        MemberMenu,
        GiveLeaderBtn,
        KickBtn,
        CloseBtn
    }

    GameObject _memberInfoObj;
    GameObject _memberMenuObj;

    TMP_Text _nicknameTxt;
    TMP_Text _levelAndCombatTxt;

    Image _leaderMarkImage;
    Image _classMarkImage;

    public MemberInfo MemberInfo { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));
        Bind<TMP_Text>(typeof(Texts));

        _memberInfoObj = Get<GameObject>((int)GameObjects.MemberInfo);
        _memberInfoObj = Get<GameObject>((int)GameObjects.MemberMenu);

        _leaderMarkImage = Get<Image>((int)Images.LeaderMarkImage);
        _classMarkImage = Get<Image>((int)Images.ClassImage);

        _nicknameTxt = Get<TMP_Text>((int)Texts.NicknameTxt);
        _levelAndCombatTxt = Get<TMP_Text>((int)Texts.LevelAndCombatTxt);

        BindEvent(_memberInfoObj, ToggleSlot);
        BindEvent(Get<GameObject>((int)GameObjects.CloseBtn), ToggleSlot);

        //BindEvent(Get<GameObject>((int)GameObjects.GiveLeaderBtn), ToggleSlot);
        //BindEvent(Get<GameObject>((int)GameObjects.KickBtn), ToggleSlot);
    }

    public void SetInfo(MemberInfo info)
    {
        MemberInfo = info;
        if (info != null)
        {
            //Temp Combat
            _levelAndCombatTxt.text = $"{info.Level}\n 5657";
            _nicknameTxt.text = info.Name;
            _classMarkImage.sprite = Managers.ResourceManager.GetResource<Sprite>($"{info.Class}_sprite");
            _leaderMarkImage.gameObject.SetActive(info.IsLeader ? true : false);
        }
        else
        {
            _nicknameTxt.text = string.Empty;
            _levelAndCombatTxt.text = string.Empty;
            _classMarkImage.gameObject.SetActive(false);
            _leaderMarkImage.gameObject.SetActive(false);
        }
    }

    private void ToggleSlot(PointerEventData eventData)
    {
        if (CheckLeader() == false)
            return;

        if (_memberInfoObj.activeSelf)
        {
            _memberMenuObj.SetActive(true);
            _memberInfoObj.SetActive(false);
        }
        else
        {
            _memberMenuObj.SetActive(false);
            _memberInfoObj.SetActive(true);
        }
    }

    // 해당 유저의 Hero가 파티의 Leader가 맞는지 체크
    private bool CheckLeader()
    {
        Party myParty = Managers.PartyManager.MyParty;

        if (myParty == null)
            return false;

        return myParty.CheckLeader();
    }
}

using Google.Protobuf.Protocol;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbySceneUI : SceneUI
{
    enum GameObjects
    {
        CharacterSlotLayout
    }
    enum Buttons
    {
        BackBtn,
        CreateBtn,
        StartBtn
    }
    enum Texts
    {
        CurrentSlotNumTxt
    }

    static readonly int MAX_SLOT_COUNT = 5;
    List<LobbyHeroInfo> _heroInfos = new List<LobbyHeroInfo>(MAX_SLOT_COUNT);
    TMP_Text _currentCharacterSlotTxt;
    GameObject _characterSlotLayout;
    GameObject _selectedSlot;
    protected override void Awake()
    {
        base.Awake();

        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        _currentCharacterSlotTxt = Get<TMP_Text>((int)Texts.CurrentSlotNumTxt);
        _characterSlotLayout = Get<GameObject>((int)GameObjects.CharacterSlotLayout);

        BindEvent(Get<Button>((int)Buttons.BackBtn).gameObject, OnBackBtnClicked);
        BindEvent(Get<Button>((int)Buttons.CreateBtn).gameObject, OnCreateBtnClicked);
    }

    public void SetCharacterSlotInfo(List<LobbyHeroInfo> heroInfos)
    {
        _heroInfos = heroInfos;
        _currentCharacterSlotTxt.text = _heroInfos.Count.ToString();
        Refresh();
    }

    private void Refresh()
    {
        float slotSize = 0;
        for (int i = 0; i < _heroInfos.Count; i++)
        {
            //매번 Instantiate 수정
            GameObject go = Managers.ResourceManager.Instantiate("CharacterSlot", _characterSlotLayout.transform);
            go.name = $"Slot{i+1}";
            go.GetComponent<CharacterSlot>().SetSlotInfo(_heroInfos[i], ChangeSelectSlot);
            slotSize += go.GetComponent<RectTransform>().rect.width;
        }

        if (_heroInfos.Count == 1)
            slotSize -= _characterSlotLayout.GetComponent<HorizontalLayoutGroup>().spacing;
        RectTransform layoutRect = _characterSlotLayout.GetComponent<RectTransform>();
        layoutRect.localPosition = new Vector3(- (slotSize / 2), layoutRect.localPosition.y, layoutRect.localPosition.z);
    }
    private void ChangeSelectSlot(GameObject newSlot)
    {
        if (newSlot == _selectedSlot)
            return;

        _selectedSlot?.transform.Find("SelectEffect").gameObject.SetActive(false);
        newSlot?.transform.Find("SelectEffect").gameObject.SetActive(true);
        _selectedSlot = newSlot;
    }

    private void OnBackBtnClicked(PointerEventData eventData)
    {
        Managers.SceneManagerEx.ChangeScene(Enums.SceneType.Login);
    }

    private void OnCreateBtnClicked(PointerEventData eventData)
    {
        if (MAX_SLOT_COUNT <= _heroInfos.Count)
            Managers.UIManager.ShowAlertPopup("그만 생성해라");
        Managers.UIManager.ShowSceneUI<CreateHeroSceneUI>();
    }


}

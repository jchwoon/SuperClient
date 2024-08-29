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
        CharacterSlotLayout,
        FadeEffect
    }
    enum Buttons
    {
        BackBtn,
        CreateBtn,
        StartBtn
    }
    enum Texts
    {
        CurrentSlotNumTxt,
    }

    static readonly int MAX_SLOT_COUNT = 5;
    List<LobbyHeroInfo> _heroInfos = new List<LobbyHeroInfo>(MAX_SLOT_COUNT);
    List<GameObject> _slots = new List<GameObject>(MAX_SLOT_COUNT);
    TMP_Text _currentCharacterSlotTxt;
    GameObject _characterSlotLayout;
    GameObject _selectedSlot;
    GameObject _fadeEffect;

    float _slotSpacing;
    protected override void Awake()
    {
        base.Awake();

        Bind<Button>(typeof(Buttons));
        Bind<TMP_Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        _currentCharacterSlotTxt = Get<TMP_Text>((int)Texts.CurrentSlotNumTxt);
        _characterSlotLayout = Get<GameObject>((int)GameObjects.CharacterSlotLayout);
        _fadeEffect = Get<GameObject>((int)GameObjects.FadeEffect);
        _slotSpacing = _characterSlotLayout.GetComponent<HorizontalLayoutGroup>().spacing;

        BindEvent(Get<Button>((int)Buttons.BackBtn).gameObject, OnBackBtnClicked);
        BindEvent(Get<Button>((int)Buttons.CreateBtn).gameObject, OnCreateBtnClicked);

        PreCreateSlot();
    }

    public void PreCreateSlot()
    {
        for (int i = 0; i < MAX_SLOT_COUNT; i++)
        {
            GameObject go = Managers.ResourceManager.Instantiate("CharacterSlot", _characterSlotLayout.transform);
            go.name = $"Slot{i + 1}";
            go.SetActive(false);
            _slots.Add(go);
        }
    }

    public void SetCharacterSlotInfo(List<LobbyHeroInfo> heroInfos)
    {
        _fadeEffect.GetComponent<FadeEffect>().FadeInOut();
        _heroInfos = heroInfos;
        Refresh();
    }

    private void Refresh()
    {
        _currentCharacterSlotTxt.text = _heroInfos.Count.ToString();
        float slotSize = 0;
        for (int i = 0; i < MAX_SLOT_COUNT; i++)
        {
            if (i < _heroInfos.Count)
            {
                _slots[i].GetComponent<CharacterSlot>().SetSlotInfo(_heroInfos[i], ChangeSelectSlot);
                _slots[i].SetActive(true);
                slotSize += _slots[i].GetComponent<RectTransform>().rect.width;
            }
            else
            {
                _slots[i].SetActive(false);
            }
        }

        if (_heroInfos.Count == 1)
            slotSize -= _slotSpacing;
        else
            slotSize += (_slotSpacing * (_heroInfos.Count - 2));
        RectTransform layoutRect = _characterSlotLayout.GetComponent<RectTransform>();
        layoutRect.localPosition = new Vector3(-(slotSize / 2), layoutRect.localPosition.y, layoutRect.localPosition.z);
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
        Managers.NetworkManager.Disconnect();
    }

    private void OnCreateBtnClicked(PointerEventData eventData)
    {
        if (MAX_SLOT_COUNT <= _heroInfos.Count)
            Managers.UIManager.ShowAlertPopup("그만 생성해라");
        Managers.UIManager.ShowSceneUI<CreateHeroSceneUI>();
    }


}

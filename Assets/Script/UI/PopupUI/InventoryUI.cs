using Google.Protobuf.Enum;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public struct InvenInfo
{
    public List<Item> Items;
    public GameObject Content;
    public int SlotCount;
}

public class InventoryUI : PopupUI
{
    enum GameObjects
    {
        CloseBtn,
        EquipInvenBtn,
        ConsumeInvenBtn,
        EtcInvenBtn,
        EquipInven,
        ConsumeInven,
        EtcInven,
        EquipContent,
        ConsumeContent,
        EtcContent,
        EquippedContent
    }
    enum InvenType
    {
        Equip,
        Consume,
        Etc
    }

    //TabButton
    GameObject _equipInvenTab;
    GameObject _consumeInvenTab;
    GameObject _etcInvenTab;
    //Item들 부모
    GameObject _equipContent;
    GameObject _consumeContent;
    GameObject _etcContent;

    GameObject _equipInven;
    GameObject _consumeInven;
    GameObject _etcInven;

    GameObject _currentActiveInvenTab;
    GameObject _currentActiveInven;
    InvenType? _currentInvenType = null;

    ItemSlot _activeSlot;

    Color _activeColor = Color.yellow;
    Color _deActiveColor = Color.white;

    Dictionary<ESlotType, EquippedSlot> _equippedSlotDict = new Dictionary<ESlotType, EquippedSlot>();

    protected override void Awake()
    {
        base.Awake();

        Bind<GameObject>(typeof(GameObjects));

        //TabButton
        _equipInvenTab = Get<GameObject>((int)GameObjects.EquipInvenBtn);
        _consumeInvenTab = Get<GameObject>((int)GameObjects.ConsumeInvenBtn);
        _etcInvenTab = Get<GameObject>((int)GameObjects.EtcInvenBtn);

        //Item들 부모
        _equipContent = Get<GameObject>((int)GameObjects.EquipContent);
        _consumeContent = Get<GameObject>((int)GameObjects.ConsumeContent);
        _etcContent = Get<GameObject>((int)GameObjects.EtcContent);

        _equipInven = Get<GameObject>((int)GameObjects.EquipInven);
        _consumeInven = Get<GameObject>((int)GameObjects.ConsumeInven);
        _etcInven = Get<GameObject>((int)GameObjects.EtcInven);

        GameObject equippedContent = Get<GameObject>((int)GameObjects.EquippedContent);
        int equippedChildCount = equippedContent.transform.childCount;
        for (int i = 0; i < equippedChildCount; i++)
        {
            EquippedSlot slot = equippedContent.transform.GetChild(i).GetComponent<EquippedSlot>();
            RegisterEquippedSlot(slot.slotType, slot);
        }

        BindEvent(Get<GameObject>((int)GameObjects.CloseBtn), OnCloseBtnClicked);
        BindEvent(_equipInvenTab, (eventData) => { OnChangeTab(eventData, _equipInvenTab, _equipInven, InvenType.Equip); });
        BindEvent(_consumeInvenTab, (eventData) => { OnChangeTab(eventData, _consumeInvenTab, _consumeInven, InvenType.Consume); });
        BindEvent(_etcInvenTab, (eventData) => { OnChangeTab(eventData, _etcInvenTab, _etcInven, InvenType.Etc); });
    }

    protected override void Start()
    {
        base.Start();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        Managers.EventBus.AddEvent(Enums.EventType.UpdateInventory, Refresh);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Managers.EventBus.RemoveEvent(Enums.EventType.UpdateInventory, Refresh);
    }

    public void RegisterEquippedSlot(ESlotType slotType, EquippedSlot slot)
    {
        _equippedSlotDict.Add(slotType, slot);  
    }

    public void Refresh()
    {
        MyHero myHero = Managers.ObjectManager.MyHero;
        if (myHero == null)
            return;

        InventoryComponent inventory = myHero.Inventory;
        if (inventory == null)
            return;

        if (_currentInvenType == null)
            OnChangeTab(null, _equipInvenTab, _equipInven, InvenType.Equip);

        InvenInfo invenInfo;

        if (_currentInvenType == InvenType.Equip)
            invenInfo = GetCurrentActiveInvenInfo(inventory, myHero, InvenType.Equip);
        else if (_currentInvenType == InvenType.Consume)
            invenInfo = GetCurrentActiveInvenInfo(inventory, myHero, InvenType.Consume);
        else
            invenInfo = GetCurrentActiveInvenInfo(inventory, myHero, InvenType.Etc);

        RefreshSlot(invenInfo);
        RefreshItem(invenInfo);
        RefreshEquippedItem(inventory);
    }

    private void RefreshSlot(InvenInfo invenInfo)
    {
        for (int i = 0; i < invenInfo.SlotCount; i++)
        {
            invenInfo.Content.transform.GetChild(i).GetComponent<ItemSlot>().RefreshSlot();
        }
    }

    private void RefreshItem(InvenInfo invenInfo)
    {
        for (int i = 0; i < invenInfo.Items.Count; i++)
        {
            invenInfo.Content.transform.GetChild(i).GetComponent<ItemSlot>().RefreshItem(invenInfo.Items[i]);
        }
    }


    private void RefreshEquippedItem(InventoryComponent inventory)
    {
        foreach (EquippedSlot slot in _equippedSlotDict.Values)
            slot.Clear();

        List<Equipment> equipItems = inventory.GetAllEquippedItem();
        foreach (Equipment equip in equipItems)
        {
            _equippedSlotDict[equip.SlotType].SetInfo(equip);
        }
    }

    private InvenInfo GetCurrentActiveInvenInfo(InventoryComponent inventory, MyHero myHero, InvenType invenType)
    {
        List<Item> items;
        GameObject currentContent;
        int slotCount;

        if (invenType == InvenType.Equip)
        {
            items = inventory.GetAllItemsInType(EItemType.Equip);
            currentContent = _equipContent;
            slotCount = myHero.MyHeroInfo.EquipInvenSlotCount;
        }
        else if (invenType == InvenType.Consume)
        {
            items = inventory.GetAllItemsInType(EItemType.Consume);
            currentContent = _consumeContent;
            slotCount = myHero.MyHeroInfo.ConsumeInvenSlotCount;
        }
        else
        {
            items = inventory.GetAllItemsInType(EItemType.Etc);
            currentContent = _etcContent;
            slotCount = myHero.MyHeroInfo.EtcInvenSlotCount;
        }
        InvenInfo invenInfo = new InvenInfo()
        {
            Items = items,
            Content = currentContent,
            SlotCount = slotCount,
        };

        return invenInfo;
    }

    private void OnCloseBtnClicked(PointerEventData eventData)
    {
        ClosePopup<InventoryUI>();
        CloseItemTooltip();
    }

    public void OnSlotClicked(ItemSlot itemSlot)
    {
        if (_activeSlot != null)
        {
            _activeSlot.DeActiveInteractOverlay();
        }
        _activeSlot = itemSlot;
    }

    private void CloseItemTooltip()
    {
        Transform infoUI = Managers.UIManager.Parent.Find(typeof(ItemInfoTooltipUI).Name);
        if (infoUI != null && infoUI.gameObject.activeSelf)
        {
            infoUI.GetComponent<ItemInfoTooltipUI>().Close();
        }
    }

    private void OnChangeTab(PointerEventData eventData, GameObject changedTab, GameObject changeInven, InvenType invenType)
    {
        if (changedTab == _currentActiveInvenTab)
            return;

        CloseItemTooltip();
        ChangeTabColor(changedTab);
        ChangeInven(changeInven, invenType);
        Refresh();
    }

    private void ChangeInven(GameObject changeInven, InvenType invenType)
    {
        if (_currentActiveInven != null)
            _currentActiveInven.SetActive(false);
        changeInven.SetActive(true);
        _currentActiveInven = changeInven;
        _currentInvenType = invenType;
    }

    private void ChangeTabColor(GameObject changedTab)
    {
        if (_currentActiveInvenTab != null)
            _currentActiveInvenTab.GetComponent<Image>().color = _deActiveColor;
        changedTab.GetComponent<Image>().color = _activeColor;
        _currentActiveInvenTab = changedTab;
    }
}

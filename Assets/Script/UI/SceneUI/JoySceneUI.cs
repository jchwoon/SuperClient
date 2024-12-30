using Data;
using Google.Protobuf.Enum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoySceneUI : SceneUI
{
    enum GameObjects
    {
        Movestick,
        InteractBtn,
    }

    enum Images
    {
        InteractImg
    }

    JoyMoveController _joyMoveController;

    Image _interactImg;
    [SerializeField]
    Sprite AttackSprite;
    //Temp InteractType으로 교체
    [SerializeField]
    Sprite InteractSprite;

    public float layoutSkillDist = 200.0f;
    public float[] layoutSkillAngles = new float[4];
    public Transform[] layoutSkillTransform = new Transform[4];

    JoySkillSlot[] _joySkillSlots = new JoySkillSlot[4];
    protected override void Awake()
    {
        base.Awake();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        GameObject movestick = Get<GameObject>((int)GameObjects.Movestick);
        GameObject interactBtn = Get<GameObject>((int)GameObjects.InteractBtn);


        _interactImg = Get<Image>((int)Images.InteractImg);
        _joyMoveController = movestick.GetComponent<JoyMoveController>();

        BindEvent(interactBtn, OnInteractBtnClicked);
        BindEvent(movestick, OnMovestickPointerDown, Enums.TouchEvent.PointerDown);
        BindEvent(movestick, OnMovestickPointerUp, Enums.TouchEvent.PointerUp);
        BindEvent(movestick, OnMovestickDrag, Enums.TouchEvent.Drag);

        //skill joystick layout
        InitJoySkillLayout(interactBtn.transform.localPosition);
        //skill joystic event Binding
        BindingJoySkillSlot();
    }

    protected override void Start()
    {
        base.Start();
        LoadSavedSlot();
    }
    protected override void OnEnable()
    {
        base.OnEnable();

        Managers.GameManager.OnInteractableChanged += ChangeInteractBtn;
        Managers.EventBus.AddEvent<SkillRegisterSlot[]>(Enums.EventType.UpdateSkillSet, OnChangedSkillSet);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        Managers.GameManager.OnInteractableChanged -= ChangeInteractBtn;
        Managers.EventBus.RemoveEvent<SkillRegisterSlot[]>(Enums.EventType.UpdateSkillSet, OnChangedSkillSet);
    }

    public void ChangeInteractBtn(IInteractable interactable)
    {
        if (interactable == null)
            _interactImg.sprite = AttackSprite;
        else
        {
            _interactImg.sprite = InteractSprite;
        }
    }

    #region MoveJoystick
    private void OnMovestickPointerDown(PointerEventData eventData)
    {
        _joyMoveController.OnHandlePointerDown(eventData);
    }

    private void OnMovestickPointerUp(PointerEventData eventData)
    {
        _joyMoveController.OnHandlePointerUp(eventData);
    }

    private void OnMovestickDrag(PointerEventData eventData)
    {
        _joyMoveController.OnHandleDrag(eventData);
    }
    #endregion

    private void OnInteractBtnClicked(PointerEventData eventData)
    {
        if (Managers.GameManager.Interactable == null)
            Managers.EventBus.InvokeEvent(Enums.EventType.AtkBtnClick);
        else
            Managers.GameManager.Interactable.Interact();
    }

    #region SkillJoystick
    private void LayoutSkillUI(Vector2 pivot, float angle, Transform transform)
    {
        float radian = angle * Mathf.Deg2Rad;
        Vector2 offset = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)) * layoutSkillDist;

        transform.localPosition = pivot + offset;
    }

    private void OnChangedSkillSet(SkillRegisterSlot[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            _joySkillSlots[i+1].SetInfo(slots[i].SkillData);
        }
    }

    private void OnSkillBtnClicked(PointerEventData eventData, JoySkillSlot slot)
    {
        if (slot == null)
            return;

        slot.UseSkill();
    }

    private void BindingJoySkillSlot()
    {
        JoySkillSlot[] slots = transform.GetComponentsInChildren<JoySkillSlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            int index = i;
            _joySkillSlots[i] = slots[i];
            BindEvent(_joySkillSlots[i].gameObject, (ed) =>
            {
                OnSkillBtnClicked(ed, _joySkillSlots[index]);
            });
        }

    }

    private void InitJoySkillLayout(Vector2 pivot)
    {
        for (int i = 0; i < layoutSkillTransform.Length; i++)
        {
            LayoutSkillUI(pivot, layoutSkillAngles[i], layoutSkillTransform[i]);
        }
    }

    private void LoadSavedSlot()
    {
        SkillComponent skill = Utils.GetMySkillComponent();

        _joySkillSlots[0].SetInfo(skill.GetSkillDataById(skill.DashSkillId));

        for (int i = 1; i < _joySkillSlots.Length; i++)
        {
            int templateId = GameSettings.GetSkillSlotTemplateId(i-1);
            SkillData skillData = skill.GetSkillDataById(templateId);

            if (skillData == null) continue;

            _joySkillSlots[i].SetInfo(skillData);
        }
    }
    #endregion


}

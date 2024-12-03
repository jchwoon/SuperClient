using Data;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : BaseObject
{
    // TODO: NPC ��ȭ
    protected InteractUI _interUI;
    GameObject _slot;
    public NPCData NpcData { get; private set; }

    public void Init(ObjectInfo objectInfo)
    {
        if (!Managers.DataManager.NpcDict.TryGetValue(objectInfo.TemplateId, out NPCData npcData))
            return;
        NpcData = npcData;
        Name = npcData.Name;
        SetPos(gameObject, objectInfo.PosInfo);
        SetObjInfo(objectInfo);
    }

    private void OnTriggerEnter(Collider other)
    {
        //ToDo: �� ĳ���͸� ���� ����ǰ� �ϱ�
        //_interUI = Managers.UIManager.ShowPopup<InteractUI>();
        //_slot = _interUI.AddSlot($"��ȭ�ϱ�");
        //_slot.GetComponent<InteractSlot>().SlotBtnClicked = StartDialogue;
    }

    private void OnTriggerExit(Collider other)
    {
        if (_slot != null)
            _interUI.DeleteSlot(_slot);
    }

    //NPC���� �ٸ�
    //�ش� ���� ������Ʈ���� GetComponent�� ���ؼ� Dialogue��ũ��Ʈ�� 
    //���� �ɾ��� �� � �ൿ�� �Ұ��� 
    private void StartDialogue()
    {

        Camera.main.GetComponent<CameraController>().enabled = false;
    }
}

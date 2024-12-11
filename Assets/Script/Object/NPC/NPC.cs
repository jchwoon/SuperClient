using Data;
using Google.Protobuf.Struct;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact();
}

public abstract class NPC : BaseObject, IInteractable
{
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

    public abstract void Interact();

    public override void OnContactMyHero(MyHero hero)
    {
        Managers.GameManager.SetInteractable(this);
    }

    public override void OnDetactMyHero(MyHero hero)
    {
        Managers.GameManager.SetInteractable(null);
    }

    //NPC���� �ٸ�
    //�ش� ���� ������Ʈ���� GetComponent�� ���ؼ� Dialogue��ũ��Ʈ�� 
    //���� �ɾ��� �� � �ൿ�� �Ұ��� 
    private void StartDialogue()
    {

        Camera.main.GetComponent<CameraController>().enabled = false;
    }
}

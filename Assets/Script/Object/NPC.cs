using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : BaseObject
{
    protected InteractUI _interUI;
    GameObject _slot;
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

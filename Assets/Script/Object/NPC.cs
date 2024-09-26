using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : BaseObject
{
    protected InteractUI _interUI;
    GameObject _slot;
    private void OnTriggerEnter(Collider other)
    {
        //ToDo: 내 캐릭터만 로직 실행되게 하기
        //_interUI = Managers.UIManager.ShowPopup<InteractUI>();
        //_slot = _interUI.AddSlot($"대화하기");
        //_slot.GetComponent<InteractSlot>().SlotBtnClicked = StartDialogue;
    }

    private void OnTriggerExit(Collider other)
    {
        if (_slot != null)
            _interUI.DeleteSlot(_slot);
    }

    //NPC마다 다름
    //해당 게임 오브젝트에서 GetComponent를 통해서 Dialogue스크립트의 
    //말을 걸었을 때 어떤 행동을 할건지 
    private void StartDialogue()
    {
        
        Camera.main.GetComponent<CameraController>().enabled = false;
    }
}

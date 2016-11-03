using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    // 각종 Window들
    public GameObject inventory;
    public GameObject skilltree;
    public GameObject status;
    public GameObject setup;

    // 창 드래그 이동 좌표
    private float offsetX;
    private float offsetY;
    
    public void BeginDrag()
    {
        offsetX = transform.position.x - Input.mousePosition.x;
        offsetY = transform.position.y - Input.mousePosition.y;
    }

    public void OnDrag()
    {
        transform.position = new Vector3(offsetX + Input.mousePosition.x, offsetY + Input.mousePosition.y, 0f);
    }

    public void ToggleInventory()
    {
        if (inventory == null)
        {
            return;
        }

        // 현재 인벤토리창의 활성화 상태 정보를 얻어 온다.
        bool flag = inventory.activeSelf;
        // 활성화 상태값의 반대 값으로 설정한다.
        inventory.SetActive(!flag);
        // 
        if (!flag == true)
        {
            inventory.GetComponentInChildren<InventoryManager>().InitializeItemInfo();
        }
    }

    public void ToggleSkillTree()
    {
        Debug.Log("Toggle SkillTree Window !");
        if (skilltree == null)
        {
            return;
        }
        bool flag = skilltree.activeSelf;
        skilltree.SetActive(!flag);
    }
    
    public void ToggleStatus()
    {
        Debug.Log("Toggle Status Window !");
        if (status == null)
        {
            return;
        }
        bool flag = status.activeSelf;
        status.SetActive(!flag);
    }

    public void ToggleSetUp()
    {
        Debug.Log("Toggle Setup Window !");
        if (setup == null)
        {
            return;
        }
        bool flag = setup.activeSelf;
        setup.SetActive(!flag);
    }
}
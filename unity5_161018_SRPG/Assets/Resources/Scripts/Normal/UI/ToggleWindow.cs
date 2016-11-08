using UnityEngine;

public class ToggleWindow : MonoBehaviour
{
    // 각종 Window들
    public GameObject inventory;
    public GameObject skilltree;
    public GameObject status;
    public GameObject setup;
    
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
        // 인벤토리창이 열리면 InitializeItemInfo() 실행.
        //if (!flag == true)
        //{
        //    inventory.GetComponentInChildren<InventoryManager>().InitializeItemInfo();
        //}
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

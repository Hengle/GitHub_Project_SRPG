using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemManager : MonoBehaviour, IDropHandler
{
    public InventoryManager invenManager = null;
    
    public int index;               // 슬롯의 인덱스
    public int gold;                // 아이템의 가격
    public Text itemName;           // 아이템 이름
    public Image itemIcon;          // 아이템 아이콘 이미지

    public GameObject item
    {
        get
        {
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    // Use this for initialization
    void Start()
    {
        gold = 0;
        //itemName.GetComponent<Text>().text = "";
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(!item)
        {
            ItemHandler.itemBeingDragged.transform.SetParent(transform);
        }
    }
}

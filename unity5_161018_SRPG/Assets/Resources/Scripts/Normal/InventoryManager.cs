using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour, IDropHandler
{
    // 
    public List<GameObject> itemSlot;// = new List<GameObject>();
    // 바뀔 이미지
    public List<Sprite> itemImage;
    // 드래그 할 아이템
    //public GameObject dragItem;
    // -1은 선택된 슬롯이 없다.
    public int selectSlot = -1;

    public GameObject dragItem
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

    public void OnDrop(PointerEventData eventData)
    {
        if (!dragItem)
        {
            ItemHandler.itemBeingDragged.transform.SetParent(transform);
        }
    }

    // Use this for initialization
    void Start ()
    {
        InitializeItemInfo();
    }
	
	// Update is called once per frame
	//void Update ()
    //{
        // 마우스 좌표를 NGUI 상의 2D 좌표계로 변환
        //Vector3 mousePosition = UICamera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //dragItem.transform.position = mousePosition;
    //}

    public void InitializeItemInfo()
    {   
        // 아이템 슬롯 갯수 만큼 루프를 돈다.
        for (int i = 0; i < itemSlot.Count; i++)
        {
            int index = Random.Range(0, 24); // 0 ~ 23
            ItemManager slot = itemSlot[i].GetComponent<ItemManager>();
            if (slot != null)
            {
                slot.invenManager = this.GetComponent<InventoryManager>();
                slot.index = i;
                slot.gold = Random.Range(5, 20) * 100;
                slot.itemIcon.sprite = itemImage[index];
                slot.itemName.text = itemImage[index].name;
                //slot.itemCount.text = ((int)Random.Range(1, 100)).ToString();
            }
        }
    }
}
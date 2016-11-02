using UnityEngine;
using UnityEngine.UI; // Image 사용
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    // 드래그 할 아이템
    public GameObject dragItem;
    public List<GameObject> itemSlot;// = new List<GameObject>();
    public List<Sprite> itemImage;
    // -1은 선택된 슬롯이 없다.
    public int selectSlot = -1;

    // Use this for initialization
    void Start ()
    {
        InitializeItemInfo();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // 마우스 좌표를 NGUI 상의 2D 좌표계로 변환
        //Vector3 mousePosition = UICamera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //dragItem.transform.position = mousePosition;
    }

    public void InitializeItemInfo()
    {
        string[] name = { "apple", "armor", "axe", "bag", "belts", "book", "boots", "bracers", "cloaks", "coins", "gem", "gloves",
            "helmets", "hp_potion", "ingots", "Meat", "mp_potion", "necklace", "pants", "rings", "scroll", "shield", "short_bow",
            "sword" };

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
                //slot.itemName.text = name[index];
                //slot.itemCount.text = ((int)Random.Range(1, 100)).ToString();
            }
        }
    }

    public void ChangeSelect(int index)
    {
        // 기존에 선택된 슬롯이 있다면 선택 해제
        if (selectSlot > -1)
        {
            itemSlot[selectSlot].GetComponent<ItemManager>().SetSelect(false);
        }
        // 새로운 선택으로 지정
        selectSlot = index;
        itemSlot[selectSlot].GetComponent<ItemManager>().SetSelect(true);
    }

    public void SetDragItem(ItemManager item)
    {
        ItemManager slot = dragItem.GetComponent<ItemManager>();
        //string path = string.Format("Images/RPG_inventory_icons/{0}", item.itemName.text);
        //Sprite sourceImage = Resources.Load<Sprite>(path);
        //if (sourceImage != null)
        //{
            slot.itemIcon.sprite = item.itemIcon.sprite;
            //slot.itemName.text = item.itemName.text;
            //slot.itemCount.text = item.itemCount.text;
        //}
    }

    public void SetDropItem(ItemManager item)
    {
        // 드래그 중인 아이템 정보를 드롭할 슬롯에 설정
        ItemManager drag = dragItem.GetComponent<ItemManager>();
        
        item.itemIcon.sprite = item.itemIcon.sprite;
        //item.itemName.text = drag.itemName.text;
        //item.itemCount.text = drag.itemCount.text;

        // 드래그 아이템 정보는 삭제
        dragItem.GetComponent<ItemManager>().itemIcon.sprite = null;
        //dragItem.GetComponent<ItemManager>().itemName.text = "";
        //dragItem.GetComponent<ItemManager>().itemCount.text = "";
    }

    public void ReleaseDragIcon()
    {
        dragItem.GetComponent<ItemManager>().itemIcon.sprite = null;
    }
}
using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public GameObject dragItem;         // 드래그 할 아이템

    public List<GameObject> itemSlot; // = new List<GameObject>();
    public int selectSlot = -1;         // -1은 선택된 슬롯이 없다.

    // Use this for initialization
    void Start ()
    {
        InitializeItemInfo();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // 마우스 좌표를 NGUI 상의 2D 좌표계로 변환
        Vector3 mousePosition = UICamera.mainCamera.ScreenToWorldPoint(Input.mousePosition);
        dragItem.transform.position = mousePosition;
    }

    public void InitializeItemInfo()
    {
        string[] name = { "Berry_02", "Berry_03", "Book_00", "Book_03", "Cloak_02", "Cloak_03", "Coin_03", "Crystal Ball_02", "Crystal Ball_03", "Essence_03",
            "Flower Bunch_03", "Gems_01", "Gems_03", "Metal Case_00", "Metal Case_01", "metal chest plate", "metal sword", "Necklace_02", "Necklace_03", "Ornament_02",
        "Ornament_03", "Parchment_02", "Ruin Stone_01", "Ruin Stone_02", "Shirt_00", "Shirt_01", "Shirt_01", "Small Potion_00", "Small Potion_03", "Wood_00", "Wood_01"};

        // 아이템 슬롯 갯수 만큼 루프를 돈다.
        for (int i = 0; i < itemSlot.Count; i++)
        {
            int index = Random.Range(0, 30); // 0 ~ 29
            string path = string.Format("Icon/{0}", name[index]);
            // Resources.Load() : 이 함수는 반드시 Assets 폴더 자식으로 Resources  폴더를 만들어 써야 한다.
            Texture2D texture = (Texture2D)Resources.Load(path);
            if (texture != null)
            {
                ItemManager slot = itemSlot[i].GetComponent<ItemManager>();
                if (slot != null)
                {
                    slot.invenManager = this.GetComponent<InventoryManager>();
                    slot.index = i;
                    slot.gold = Random.Range(5, 20) * 100;
                    slot.itemIcon.mainTexture = texture;
                    slot.itemName.text = name[index];
                    slot.itemCount.text = ((int)Random.Range(1, 100)).ToString();
                }
            }
            else
            {
                Debug.Log(path);
            }
        } // end for()

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

        string path = string.Format("Icon/{0}", item.itemName.text);
        Texture2D texture = (Texture2D)Resources.Load(path);
        if (texture != null)
        {
            slot.itemIcon.mainTexture = texture;
            slot.itemCount.text = item.itemCount.text;
            slot.itemName.text = item.itemName.text;
        }
    }

    public void SetDropItem(ItemManager item)
    {
        // 드래그 중인 아이템 정보를 드롭할 슬롯에 설정
        ItemManager drag = dragItem.GetComponent<ItemManager>();
        string path = string.Format("Icon/{0}", drag.itemName.text);
        Texture2D texture = (Texture2D)Resources.Load(path);
        if (texture != null)
        {
            item.itemIcon.mainTexture = texture;
            item.itemCount.text = drag.itemCount.text;
            item.itemName.text = drag.itemName.text;
        }

        // 드래그 아이템 정보는 삭제
        dragItem.GetComponent<ItemManager>().itemIcon.mainTexture = null;
        dragItem.GetComponent<ItemManager>().itemCount.text = "";
        dragItem.GetComponent<ItemManager>().itemName.text = "";
    }

    public void ReleaseDragIcon()
    {
        dragItem.GetComponent<ItemManager>().itemIcon.mainTexture = null;
    }
}
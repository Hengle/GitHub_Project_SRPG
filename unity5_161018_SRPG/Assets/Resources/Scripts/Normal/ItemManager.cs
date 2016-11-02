using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public InventoryManager invenManager = null;

    public int index;               // 슬롯의 인덱스
    public int gold;                // 아이템의 가격
    //public Text itemName;           // 아이템 이름
    public Image itemIcon;          // 아이템 아이콘 이미지
    
    //public UILabel itemCount;       // 아이템의 갯수
    //public UISprite[] gradeStar;    // 아이템의 등급
    //public UISprite selectFrame;    // 선택되었음을 알려 주는 이미지

    // Use this for initialization
    void Start ()
    {
        gold = 0;
        //itemName.text = "";
        //itemCount.text = "";

        SetSelect(false);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void SetSelect(bool flag)
    {
        // 판매할 itemSlot 테두리 표시
        //selectFrame.gameObject.SetActive(flag);
    }

    // NGUI 지원 함수
    // 마우스에 의해 클릭되면 자동으로 호출 되는 함수
    void OnClick()
    {
        Debug.Log("OnClick !!!");

        if (invenManager == null)
        {
            return;
        }
        // 인벤토리 매니저에 클릭 이벤트에 대한 정보를 제공해 준다.
        invenManager.ChangeSelect(index);
    }

    // 마우스가 충돌 박스에 올려지거나 충돌 박스에서 벗어나면 호출되는 함수
    // isOver : 충돌 박스에 올려지면 true, 충돌 박스에서 벗어나면 false
    void OnHover(bool isOver)
    {
        //Debug.Log("OnHover !!!" + isOver + "    " + this.itemName.text);

        if (isOver == false)
        {
            invenManager.ReleaseDragIcon();
        }
    }

    // 마우스가 충돌박스 영역내에서 버튼이 눌려지거나 눌렸다 떼어질때 호출되는 함수
    // isPressed : 최초 눌려질때 true, 눌렸다 떼어질때 false
    void OnPress(bool isPressed)
    {
        Debug.Log("OnPress !!!" + isPressed);

        // 아이템 슬롯 안에서 마우스를 클릭할 때가 드래그의 시작 타이밍
        if (isPressed == true)
        {
            // 아이템 정보를 전달한다
            invenManager.SetDragItem(this);
        }
    }

    // 드래그 되는 동안에 호출되는 함수
    // delta : 드래그되어 이동되는 좌표값
    void OnDrag(Vector2 delta)
    {
        Debug.Log("OnDrag !!!");
    }

    // 드래그 중 버튼을 뗄 때 충돌 박스와 충돌이 되면 호출되는함수
    // 버튼을 뗄 때 충돌 박스와 충돌되지 않으면 호출되지 않음
    void OnDrop()
    {
        Debug.Log("OnDrop TheBeat !!");

        invenManager.SetDropItem(this);
    }
}

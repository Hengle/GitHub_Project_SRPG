using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject itemBeingDragged = null;
    Vector3 startPosition;
    Transform startParent;

    public GameObject mainCamere;
    public Text itemText;

    // Use this for initialization
    void Start()
    {
        itemText.text = gameObject.GetComponent<Image>().sprite.name;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        // 메인카메라에 붙은 FollowCam 컴포넌트 끔
        mainCamere.GetComponent<FollowCam>().enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        // 메인카메라에 붙은 FollowCam 컴포넌트 켬
        mainCamere.GetComponent<FollowCam>().enabled = true;
        if (transform.parent != startParent)
        {
            transform.position = startPosition;
        }
    }
}

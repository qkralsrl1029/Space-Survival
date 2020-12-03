using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class virtualStickController : MonoBehaviour,IDragHandler,IPointerDownHandler,IPointerUpHandler
{
    Image BG;
    Image Stick;
    Vector3 inputVector;

    public float HorizontalVal
    {
        get { return inputVector.x; }
    }

    public float VerticalVal
    {
        get { return inputVector.y; }
    }

    // Start is called before the first frame update
    void Start()
    {
        BG = GetComponent<Image>();
        Stick = transform.GetChild(0).GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //해당 게임오브젝트에서(스틱BG)
    //인터페이스 구현, 해당 이벤트 발생시 호출
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(BG.rectTransform,eventData.position,eventData.pressEventCamera,out pos))
        {
            pos.x = (pos.x / BG.rectTransform.sizeDelta.x);
            pos.x = (pos.y / BG.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x, pos.x, 0);
            inputVector = (inputVector.magnitude > 1) ? inputVector.normalized : inputVector;

            Stick.rectTransform.anchoredPosition = new Vector3(inputVector.x * (BG.rectTransform.sizeDelta.x / 3),
                                                               inputVector.y * (BG.rectTransform.sizeDelta.y / 3));
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector3.zero;
        Stick.rectTransform.anchoredPosition = Vector2.zero;
    }
}

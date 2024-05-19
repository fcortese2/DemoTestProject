using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FlippableCard : MonoBehaviour, IPointerClickHandler
{
    //public just for debug, this can probably just be made private or protected (if we inherit from this later on)
    [field: SerializeField] public string LinkedUid { get; private set; }

    public Image iconImage;
    public CanvasGroup imageCanvasGroup;
    
    public void SetLink(string uid)
    {
        LinkedUid = uid;

        ImageDataSet dataset = ImageData.Instance.GetFromUid(LinkedUid);
        iconImage.sprite = dataset.Graphics;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
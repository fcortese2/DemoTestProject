using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FlippableCard : MonoBehaviour, IPointerClickHandler
{
    //public just for debug, this can probably just be made private or protected (if we inherit from this later on)
    [field: SerializeField] public string LinkedUid { get; private set; }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
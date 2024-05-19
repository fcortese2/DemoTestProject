using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class FlippableCard : MonoBehaviour, IPointerClickHandler
{
    //public just for debug, this can probably just be made private or protected (if we inherit from this later on)
    [field: SerializeField] public string LinkedUid { get; private set; }

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    
    public Image iconImage;
    public CanvasGroup imageCanvasGroup;
    public RectTransform imageRectTransform;

    private bool interactable = false;

    private AudioSource audioSource;
    [Header("Audio")] 
    public AudioClip UnableToClickSfx;
    public AudioClip ClickSfx;

    private void OnValidate()
    {
        GetComponent<AudioSource>().spatialize = false;
        GetComponent<AudioSource>().spatialBlend = 0;
        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<AudioSource>().loop = false;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        
        audioSource = GetComponent<AudioSource>();
    }

    private void PlayAudioClip(AudioClip clip, float volume = 1f)
    {
        if (!clip)
        {
            ADebug.LogInvalidParam("Audio clip cannot be null");
            return;
        }
        
        //would be better to have this go through an audio sfx manager service but this is fine for now...
        audioSource.PlayOneShot(clip, volume);
    }

    public void SetLink(string uid)
    {
        LinkedUid = uid;

        ImageDataSet dataset = ImageData.Instance.GetFromUid(LinkedUid);
        iconImage.sprite = dataset.Graphics;
        imageCanvasGroup.alpha = 0;

        StartCoroutine(FadeInThenOut());
    }

    public IEnumerator FadeInThenOut(float tFade = .8f, float tHold = 1f, float tFadeOut = .8f)
    {
        //could have used a tween sequence for this but this works too...
        interactable = false;
        yield return imageCanvasGroup.DOFade(1f, tFade).WaitForCompletion();
        yield return new WaitForSeconds(tHold);
        yield return imageCanvasGroup.DOFade(0f, tFadeOut).WaitForCompletion();
        interactable = true;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (interactable)
        {
            PlayAudioClip(ClickSfx);
            PairManager.Instance.RegisterCardFlipped(this);
        }
        else
        {
            PlayAudioClip(UnableToClickSfx, .7f);
        }
    }

    public IEnumerator RemoveFromTable(float tFade)
    {
        interactable = false;

        imageCanvasGroup.DOFade(1f, tFade);
        rectTransform.DOScale(Vector3.one * 2.2f, tFade);
        yield return canvasGroup.DOFade(0f, tFade).WaitForCompletion();
        GetComponent<Image>().raycastTarget = false;

        rectTransform.localScale = Vector3.one;
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public IEnumerator FlipHide(float tFade)
    {
        rectTransform.DOScale(Vector3.one * 1f, tFade).SetEase(Ease.OutBounce);
        rectTransform.DOPunchRotation(new Vector3(0, 0, 10f), tFade);
        yield return imageCanvasGroup.DOFade(0f, tFade).WaitForCompletion();
        
        interactable = true;
    }
    
    public IEnumerator FlipShow(float tFade)
    {
        rectTransform.DOScale(Vector3.one * 1.2f, tFade).SetEase(Ease.OutElastic);
        rectTransform.DOPunchRotation(new Vector3(0, 0, 0f), tFade);
        yield return imageCanvasGroup.DOFade(1f, tFade).WaitForCompletion();
        
        interactable = false;
    }
}
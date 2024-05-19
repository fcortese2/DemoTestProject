using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PairManager : MonoBehaviour
{
    private static PairManager instance;

    public static PairManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<PairManager>();
            }

            return instance;
        }
    }

    [SerializeField] private AudioClip pairFoundSoundEffect;
    
    [SerializeField] private FlippedCardCouple flipCouple = new FlippedCardCouple();

    private AudioSource audioSource;

    [Header("Game End")]
    [SerializeField] private GameObject endScreen;
    [SerializeField] private AudioClip gameOverSfx;
    
    private void OnValidate()
    {
        GetComponent<AudioSource>().spatialize = false;
        GetComponent<AudioSource>().spatialBlend = 0;
        GetComponent<AudioSource>().playOnAwake = false;
        GetComponent<AudioSource>().loop = false;
    }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        endScreen.SetActive(false);
    }

    public void RegisterCardFlipped(FlippableCard card)
    {
        if (flipCouple.LastCoupleCompleted)
        {
            flipCouple.HideLastCouple();
            flipCouple = new FlippedCardCouple();
        }

        flipCouple.AppendFlip(card);
        
        if (flipCouple is { LastCoupleCompleted: true, SuccessfulPair: true })
        {
            audioSource.PlayOneShot(pairFoundSoundEffect);
            ColumnLimitedGridArea.MarkCardPairFound(flipCouple.CardOne.LinkedUid);
            StartCoroutine(flipCouple.CardOne.RemoveFromTable(.8f));
            StartCoroutine(flipCouple.CardTwo.RemoveFromTable(.8f));
        }
        else
        {
            if (flipCouple.CardOne && flipCouple.CardTwo)
            {
                GameData.Instance.MatchesStraight = 1;
            }
            StartCoroutine(card.FlipShow(.8f));
        }
        
    }

    public void EndGame()
    {
        //play sfx/vfx here...
        GameData.Instance.WonGame = true;
        audioSource.PlayOneShot(gameOverSfx);
        endScreen.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
        endScreen.SetActive(true);
    }
}

[Serializable]
public struct FlippedCardCouple
{
    public FlippableCard CardOne;
    public FlippableCard CardTwo;

    public bool LastCoupleCompleted => CardOne != default && CardTwo != default;
    public bool SuccessfulPair => CardOne && CardTwo && CardOne.LinkedUid == CardTwo.LinkedUid;

    public void AppendFlip(FlippableCard card)
    {
        if (LastCoupleCompleted)
        {
            ADebug.FatalLogicError("Cannot add card to couple check that is already full");
            return;
        }

        if (!CardOne)
        {
            CardOne = card;
        }
        else if (!CardTwo)
        {
            CardTwo = card;
        }
    }

    public void HideLastCouple()
    {
        PairManager.Instance.StartCoroutine(CardOne.FlipHide(0.8f));
        PairManager.Instance.StartCoroutine(CardTwo.FlipHide(0.8f));
    }
}

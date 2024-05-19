using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    
    [SerializeField] private FlippedCardCouple flipCouple = new FlippedCardCouple();

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
            ColumnLimitedGridArea.MarkCardPairFound(flipCouple.CardOne.LinkedUid);
            StartCoroutine(flipCouple.CardOne.RemoveFromTable(.8f));
            StartCoroutine(flipCouple.CardTwo.RemoveFromTable(.8f));
        }
        else
        {
            StartCoroutine(card.FlipShow(.8f));
        }
        
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

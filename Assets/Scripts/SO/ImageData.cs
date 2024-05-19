using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Singleton SO/Image Data", fileName = "ImageData")]
public class ImageData : SingletonScriptableObject<ImageData>
{
    public List<ImageDataSet> imageDataSets = new List<ImageDataSet>();

    public ImageDataSet GetFromUid(string uid)
    {
        return imageDataSets.FirstOrDefault(ids => ids.UID == uid);
    }

    public List<string> GetDataSetsForCount(int cardCount)
    {
        float errCheck = cardCount % 2;
        if (errCheck != 0)
        {
            ADebug.LogInvalidParam("cardCount is not a multiple of 2. Number of cards MUST be even.");
            return null;
        }

        int getCount = cardCount / 2;
        if (getCount > imageDataSets.Count)
        {
            ADebug.FatalLogicError("Trying to get more card data sets than available.");
            return null;
        }

        return imageDataSets.GetRange(0, cardCount / 2).Select(ids => ids.UID).ToList();
    }
    
    private void OnValidate()
    {
        for (int i = 0; i < imageDataSets.Count; i++)
        {
            imageDataSets[i].UID = string.IsNullOrEmpty(imageDataSets[i].UID)
                ? Guid.NewGuid().ToString()
                : imageDataSets[i].UID;

            while (imageDataSets.Count(set => set.UID == imageDataSets[i].UID) > 1)
            {
                imageDataSets[i].UID = Guid.NewGuid().ToString();
            }
        }
    }
}

[Serializable]
public class ImageDataSet
{
    public string UID;
    
    public Sprite Graphics;
    [Range(1, 5)] public float ScoreValue = 1;

    public ImageDataSet()
    {
        UID = Guid.NewGuid().ToString();
    }
}

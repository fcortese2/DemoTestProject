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
    
    private void OnValidate()
    {
        for (int i = 0; i < imageDataSets.Count; i++)
        {
            imageDataSets[i].UID = string.IsNullOrEmpty(imageDataSets[i].UID)
                ? Guid.NewGuid().ToString()
                : imageDataSets[i].UID;
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

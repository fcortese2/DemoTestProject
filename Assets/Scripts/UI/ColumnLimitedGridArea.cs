using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColumnLimitedGridArea : GridLayoutGroup
{
    [Header("Tweaking")]
    public Vector2 spacingMinMax = new Vector2(20, 300);
    public Vector2 cellSizeMinMax = new Vector2(100, 1000);
    [Range(0, 100)] public float areaWidthFillPercentage = 80;
    [Range(0, 100)] public float areaHeightFillPercentage = 65;
    
    protected override void OnValidate()
    {
        base.OnValidate();

        constraint = Constraint.FixedColumnCount;

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            rectTransform.parent.GetComponent<RectTransform>().sizeDelta.x * areaWidthFillPercentage / 100f);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            rectTransform.parent.GetComponent<RectTransform>().sizeDelta.y * areaHeightFillPercentage / 100f);
    }

    protected override void Awake()
    {
        base.Awake();
        
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            rectTransform.parent.GetComponent<RectTransform>().sizeDelta.x * areaWidthFillPercentage / 100f);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            rectTransform.parent.GetComponent<RectTransform>().sizeDelta.y * areaHeightFillPercentage / 100f);
    }

    protected override void Start()
    {
        base.Start();
        
        SetMaxColumnCount(4);
    }

    public void SetMaxColumnCount(int columnCount)
    {
        if (columnCount <= 0)
        {
            ADebug.LogInvalidParam("columnCount cannot be smaller or equal to 0");
            return;
        }

        constraintCount = columnCount;
        
        CalculateRectBestFit();
    }

    public void CalculateRectBestFit()
    {

        var outCalc = CalculateCellSizeAndSpacing(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y,
            spacingMinMax.x, spacingMinMax.y, cellSizeMinMax.x, cellSizeMinMax.y, constraintCount, 7);
        cellSize = Vector2.one * outCalc.cellSize;
        spacing = Vector2.one * outCalc.spacing;
        
        Debug.Log($"cSize = {outCalc.cellSize}");
        Debug.Log($"cSpacing = {outCalc.spacing}");
    }

    public static (float cellSize, float spacing) CalculateCellSizeAndSpacing(float width, float height,
        float minSpacing, float maxSpacing, float minCellSize, float maxCellSize, int itemCountX, int itemCountY)
    {
        var horizontalResult = FindBestFitLinear(width, minSpacing, maxSpacing, minCellSize, maxCellSize, itemCountX);
        var verticalResult = FindBestFitLinear(height, minSpacing, maxSpacing, minCellSize, maxCellSize, itemCountY);

        return (Mathf.Min(horizontalResult.cellSize, verticalResult.cellSize), Mathf.Min(horizontalResult.spacing, verticalResult.spacing));
    }

    public static (float cellSize, float spacing) FindBestFitLinear(float dimension, float minSpacing, float maxSpacing, float minCellSize, float maxCellSize, int cellCount)
    {
        float bestCellSize = minCellSize;
        float bestSpacing = maxSpacing;

        for (float cellSize = maxCellSize; cellSize >= minCellSize; cellSize -= 0.01f)
        {
            float totalCellSize = cellSize * cellCount;
            float remainingSpace = dimension - totalCellSize;

            float spacing = remainingSpace / (cellCount - 1);

            if (spacing >= minSpacing && spacing <= maxSpacing)
            {
                bestCellSize = cellSize;
                bestSpacing = spacing;

                break;
            }
        }

        return (bestCellSize, bestSpacing);
    }
}

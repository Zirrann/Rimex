using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseItemData : MonoBehaviour
{
    public Image ItemImage;
    public TextMeshProUGUI ItemCount;

    private void Awake()
    {
        ItemImage.color = Color.clear;
        ItemCount.text = "";
    }
}

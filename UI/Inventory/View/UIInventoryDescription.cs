using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryDescription : MonoBehaviour
{
    [SerializeField] private Image selectedImage;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemDescription;
    [SerializeField] private TMP_Text itemProperty;
    [SerializeField] private TMP_Text itemQuantity;

    public void SetDescription(Sprite image, string itemName, string itemDescription, string itemProperty)
    {
        this.selectedImage.gameObject.SetActive(true);
        this.selectedImage.sprite = image;
        this.itemName.text = itemName;
        this.itemDescription.text = itemDescription;
        this.itemProperty.text = itemProperty;
    }

    public void InitDescription()
    {
        selectedImage.sprite = null;
        selectedImage.gameObject.SetActive(false);
        itemName.text = "";
        itemDescription.text = "어떤 아이템을\n조사해볼까...";
        itemProperty.text = "";
    }
}

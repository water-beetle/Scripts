using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image selectedImage;

    [SerializeField]
    public event Action<UIInventoryItem> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag,
        OnRightMouseBtnClick;

    public void SetData(Sprite sprite, int quantity)
    {

        itemImage.gameObject.SetActive(true);
        itemImage.sprite = sprite;
        quantityText.text = quantity + "";
    }

    public void ResetData()
    { 
        if (itemImage.gameObject.activeSelf)
            itemImage.gameObject.SetActive(false);
        quantityText.text = "";
    }

    public void SelectItem()
    {
        selectedImage.gameObject.SetActive(true);
    }

    public void DeSelectItem()
    {
        selectedImage.gameObject.SetActive(false);
    }




    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnPointerClick(PointerEventData pointerData)
    {
        if (pointerData.button == PointerEventData.InputButton.Left)
            OnItemClicked?.Invoke(this);
        else
            OnRightMouseBtnClick?.Invoke(this);
    }

}

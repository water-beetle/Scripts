using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryBackground : MonoBehaviour
{

    [SerializeField] private RectTransform bookDesk;
    [SerializeField] private RectTransform book;
    [SerializeField] private RectTransform bookContents;

    public bool isDoing = false;



    public void Open()
    {   if(!isDoing)
            StartCoroutine(OpenInventoryAnimation());
    }

    public void Close()
    {
        if(!isDoing)
            StartCoroutine(CloseInventoryAnimation());
    }

    private IEnumerator OpenInventoryAnimation()
    {
        isDoing = true;
        bookDesk.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        book.gameObject.SetActive(true);
        book.gameObject.GetComponent<Animator>().SetBool(Settings.AnimatorParams.isOpen, true);
        yield return new WaitForSeconds(0.3f);
        bookContents.gameObject.SetActive(true);
        isDoing = false;
    }

    private IEnumerator CloseInventoryAnimation()
    {
        isDoing = true;
        bookContents.gameObject.SetActive(false);
        book.gameObject.GetComponent<Animator>().SetBool(Settings.AnimatorParams.isOpen, false);
        yield return new WaitForSeconds(.8f);
        book.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        bookDesk.gameObject.SetActive(false);
        isDoing = false;
    }
}

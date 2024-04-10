using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSubMenu : MonoBehaviour
{
    public void Show()
    {
        this.gameObject.GetComponent<RectTransform>().DOAnchorPosY(0, 1).SetEase(Ease.OutExpo);
    }
}

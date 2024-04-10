using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOnBall : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private BallOperator _ballOperator;
    public LevelOperator LevelOperator;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _ballOperator.IsLastOfLink = false;

        LevelOperator.AddToLink(_ballOperator);
    }
}

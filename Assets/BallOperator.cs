using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static LevelOperator;

public class BallOperator : MonoBehaviour
{
    [SerializeField]
    private Image _ballImage;

    private BallType _ballType;

    public void Init(BallType type)
    {
        _ballType = type;
        _ballImage.sprite = _ballType.BallSprite;
    }


}

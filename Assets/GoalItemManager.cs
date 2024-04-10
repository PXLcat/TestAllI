using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LevelOperator;

public class GoalItemManager : MonoBehaviour
{
    [SerializeField]
    private Image _image;
    [SerializeField]
    private TextMeshProUGUI _txt;

    public int BallsNeeded;

    public void Init(BallType ballType, int count)
    {
        _image.sprite = ballType.BallSprite;
        BallsNeeded = count;
        _txt.text = count.ToString();
    }

    public void UpdateCount()
    {
        
    }
}

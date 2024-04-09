using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static LevelOperator;

public class BallOperator : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Image _ballImage;

    private LevelOperator _levelOperator;

    [SerializeField]
    private Button _button;

    private BallType _ballType;

    public bool IsLinked;
    public bool IsLastOfLink;
    public int IndexInLink;
    public int XCoord;
    public int YCoord;

    public LineRenderer _lineRenderer;
    public Vector2 _lineDestination;

    public void Init(BallType type, LevelOperator levelOperator)
    {
        _levelOperator = levelOperator;
        _ballType = type;
        _ballImage.sprite = _ballType.BallSprite;
    }

    public void LinkFromThisBall()
    {
        IsLinked = true;

        _lineRenderer.enabled = true;
        _lineRenderer.SetPosition(0, this.transform.position);

        IsLastOfLink = true;
        IndexInLink = 0;
    }

    private void Update()
    {
        if (IsLinked && IsLastOfLink) {
            _lineRenderer.SetPosition(IndexInLink+1, Input.mousePosition);
            Debug.Log($"toto");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _levelOperator.StartNewLink(this);
        Debug.Log($"OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log($"OnPointerUp");
    }
}

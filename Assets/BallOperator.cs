using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static LevelOperator;
using DG.Tweening;



public class BallOperator : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    [SerializeField]
    private Image _ballImage;

    private LevelOperator _levelOperator;

    [SerializeField]
    private HoverOnBall _hoverOnBall;

    public BallType BallType;

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
        BallType = type;
        _ballImage.sprite = BallType.BallSprite;

        _hoverOnBall.LevelOperator = levelOperator;
    }

    public void LinkFromThisBall()
    {
        IsLinked = true;

        _lineRenderer.enabled = true;
        _lineRenderer.startColor = BallType.LinkColor;
        _lineRenderer.endColor = BallType.LinkColor;
        _lineRenderer.SetPosition(0, this.transform.position);

        IsLastOfLink = true;
        IndexInLink = 0;
    }

    private void Update()
    {
        if (IsLinked && IsLastOfLink) {

            Vector2 distance = new Vector2(Input.mousePosition.x - this.transform.position.x,
                Input.mousePosition.y - this.transform.position.y);
                //Debug.Log($"distance = {distance}"); 
            Vector2 direction = distance.normalized;
            //Debug.Log($"x {direction.x} y {direction.y}");
            Vector2 linkDestination = ((Vector2)this.transform.position + direction 
                * new Vector2(System.Math.Min(distance.Abs().x ,_levelOperator.LinkSize), 
                System.Math.Min(distance.Abs().y, _levelOperator.LinkSize)));
            _lineRenderer.SetPosition(IndexInLink+1, linkDestination);

        }
        
    }

    public void Unlink()
    {
        IsLinked = false;
        IsLastOfLink = false;

        _lineRenderer.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _levelOperator.StartNewLink(this);
        Debug.Log($"OnPointerDown");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"OnPointerEnter");
    }

    internal void AdjustLine(Vector2 nextBallPosition)
    {
        _lineRenderer.SetPosition(1, nextBallPosition);
    }

    internal void Validate()
    {
        Vector2 initialPos = _ballImage.transform.position;
        _ballImage.transform.DOMoveY(initialPos.y - 10, 0.25f).OnComplete(() =>
        {
            _ballImage.DOFade(0,2).SetEase(Ease.OutCubic);
            _ballImage.transform.DOMoveY(initialPos.y + 200, 0.75f).SetEase(Ease.OutCubic);
        });
    }

    internal void LowerCells(float cells)
    {
        Vector2 initialPos = _ballImage.transform.position;
        _ballImage.color = Color.black;
        _ballImage.transform.DOMoveY(initialPos.y- 120*cells, 2).SetEase(Ease.InCubic);

    }
}

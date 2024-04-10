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

    public Vector2 InitialImagePosition;
    public bool Deleted;

    private bool HasAnActiveTween;
    public void Init(BallType type, LevelOperator levelOperator)
    {
        _levelOperator = levelOperator;
        BallType = type;
        _ballImage.sprite = BallType.BallSprite;

        _hoverOnBall.LevelOperator = levelOperator;
        InitialImagePosition = _ballImage.transform.position;
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

    internal void Validate(BallType newRandomColor)
    {
        Vector2 initialPos = _ballImage.transform.position;
        HasAnActiveTween = true;
        _ballImage.transform.DOMoveY(initialPos.y - 10, 0.25f).OnComplete(() =>
        {
            _ballImage.transform.DOMoveY(initialPos.y + 200, 0.75f).SetEase(Ease.OutCubic);
            _ballImage.DOFade(0,0.8f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                ResetColor(newRandomColor);
            });
        });
        Deleted= true;
    }

    internal void LowerCells(float cells)
    {
        //Debug.Log($"LowerCells aaa");
        //Vector2 initialPos = _ballImage.transform.position;
        //_ballImage.color = Color.black;
        //HasAnActiveTween = true;
        //_ballImage.transform.DOMoveY(initialPos.y - 120 * cells, 0.0f).SetEase(Ease.InCubic).OnComplete(() =>
        //{
        //    HasAnActiveTween = false;
        //});

    }

    internal void ResetColor(BallType ballType)
    {
        BallType = ballType;
        _ballImage.rectTransform.anchoredPosition = Vector2.zero;
        _ballImage.sprite = ballType.BallSprite;
        _ballImage.color = Color.white;
    }
    public IEnumerator ResetAfterAnim(BallType ballType)
    {
        Debug.Log($"ResetAfterAnim aaa");
        yield return StartCoroutine(WaitForTweens());
        //BallType = ballType;
        //_ballImage.rectTransform.position = Vector2.zero;
        //_ballImage.sprite = ballType.BallSprite;
        //_ballImage.color = Color.white;
        
    }
    IEnumerator WaitForTweens()
    {
        yield return new WaitUntil(() => !HasAnActiveTween);

    }
}

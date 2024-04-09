using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOperator : MonoBehaviour
{
    [SerializeField]
    private LevelSO _levelData;
    [SerializeField]
    private RectTransform _gridTransform;
    [SerializeField]
    private GameObject _ballPrefab;

    #region Ball Types
    [SerializeField]
    private BallType _redB;
    [SerializeField]
    private BallType _greenB;
    [SerializeField]
    private BallType _blueB;
    [SerializeField]
    private BallType _orangeB;
    [SerializeField]
    private BallType _purpleB;
    #endregion

    #region Ball link
    public List<BallOperator> LinkedBalls;
    #endregion

    private void Start()
    {
        Init();
    }

    [ContextMenu("Init")]
    public void Init()
    {
        BallType[] ballTypes = new BallType[5] { _redB , _greenB, _blueB, _orangeB, _purpleB };

        _gridTransform.sizeDelta = new Vector2(_levelData.Size * 120, _levelData.Size * 120);
        for (int i = 0; i < _levelData.Size* _levelData.Size; i++)
        {
            BallOperator newBall = GameObject.Instantiate(_ballPrefab, _gridTransform.transform).GetComponent<BallOperator>();

            int rdmColor = UnityEngine.Random.Range(0, 4);
            newBall.Init(ballTypes[rdmColor], this);

        }

    }

    public void StartNewLink(BallOperator firstBall)
    {
        LinkedBalls = new List<BallOperator>();
        LinkedBalls.Add(firstBall);

        firstBall.LinkFromThisBall();
    }

    [Serializable]
    public struct BallType
    {
        public ColorType ColorType;
        public Color32 LinkColor;
        public Sprite BallSprite;
    }

    public enum ColorType
    {
        RED,
        GREEN,
        BLUE,
        ORANGE,
        PURPLE
    }
}

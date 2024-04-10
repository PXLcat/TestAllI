using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

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
    public float LinkSize = 10f;
    #endregion

    #region Screen size adjustment
    public RenderTexture rt;
    public Camera lineRenderCamera;
    public RawImage ri;
    public CanvasScaler cs;
    #endregion

    private List<BallOperator> _allBalls;

    private void Awake()
    {
        rt.Release();
        rt.width = Screen.width;
        rt.height = Screen.height;
        rt.Create();
        Debug.Log(lineRenderCamera.targetTexture);
        cs.referenceResolution = new Vector2(Screen.width, Screen.height);
        lineRenderCamera.orthographicSize = Screen.height/2;
        ri.SetNativeSize();
         
    }

    private void Start()
    {
        Init();
    }

    [ContextMenu("Init")]
    public void Init()
    {
        _allBalls = new List<BallOperator>();
        BallType[] ballTypes = new BallType[5] { _redB , _greenB, _blueB, _orangeB, _purpleB };

        _gridTransform.sizeDelta = new Vector2(_levelData.Size * 120, _levelData.Size * 120);
        for (int i = 0; i < _levelData.Size; i++)
        {
            for (int j = 0; j < _levelData.Size; j++)
            {
                BallOperator newBall = GameObject.Instantiate(_ballPrefab, _gridTransform.transform).GetComponent<BallOperator>();

                int rdmColor = UnityEngine.Random.Range(0, 4);
                newBall.Init(ballTypes[rdmColor], this);

                newBall.XCoord = j;
                newBall.YCoord = _levelData.Size - i - 1;

                _allBalls.Add(newBall);
            }


        }

    }

    public void StartNewLink(BallOperator firstBall)
    {
        LinkedBalls = new List<BallOperator>();
        LinkedBalls.Add(firstBall);

        firstBall.LinkFromThisBall();
    }

    public void AddToLink(BallOperator newBall)
    {
        if (LinkedBalls == null || LinkedBalls.Count == 0)
        {
            return;
        }

        if (((Math.Abs(LinkedBalls[LinkedBalls.Count - 1].XCoord - newBall.XCoord) > 1))
            || (Math.Abs(LinkedBalls[LinkedBalls.Count - 1].YCoord - newBall.YCoord) > 1))
        {
            return;
        }


        if (!LinkedBalls.Contains(newBall) 
            && (LinkedBalls[0].BallType.ColorType == newBall.BallType.ColorType))
        {
            newBall.LinkFromThisBall();
            LinkedBalls[LinkedBalls.Count - 1].IsLastOfLink = false;
            LinkedBalls[LinkedBalls.Count - 1].AdjustLine(newBall.transform.position);
            LinkedBalls.Add(newBall);
        }
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (LinkedBalls.Count>2)
            {
                foreach (var ballToDelete in LinkedBalls)
                {
                    ballToDelete.Validate();
                    if (ballToDelete.YCoord != _levelData.Size-1)
                    {
                        //descendre les balles au dessus
                        foreach (var item2 in _allBalls.Where(b => ((b.XCoord == ballToDelete.XCoord) && (b.YCoord > ballToDelete.YCoord))))
                        {
                            Debug.Log($"forea"); 
                            int count = LinkedBalls.Count((b2 => (b2.XCoord == ballToDelete.XCoord) && (b2.YCoord < item2.YCoord)));
                            item2.LowerCells(count/*, ballToDelete*/);
                            _allBalls.First(b => ((b.XCoord == ballToDelete.XCoord) && (b.YCoord == ballToDelete.YCoord)));
                        }
                        
                        
                    }
                }
            }

            foreach (var item in LinkedBalls)
            {
                Debug.Log($"GetMouseButtonUp");
                item.Unlink();

            }
            LinkedBalls = new List<BallOperator>();
        }
        
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

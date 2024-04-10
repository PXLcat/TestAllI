using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class LevelOperator : MonoBehaviour
{
    [SerializeField]
    private DataManagerSO _dataManager;
    private LevelSO _levelData;
    [SerializeField]
    private RectTransform _gridTransform;
    [SerializeField]
    private GameObject _ballPrefab;

    #region Ball Types
    BallType[] _ballTypes;

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

    #region UI
    [SerializeField]
    private Transform _goalParent;
    [SerializeField]
    private TextMeshProUGUI _movesText;
    [SerializeField]
    private GameObject _goalItemPrefab;

    [SerializeField]
    private GraphicRaycaster _gameGraphicRaycaster;

    [SerializeField]
    private GameSubMenu _winMenu;

    [SerializeField]
    private GameSubMenu _looseMenu;
    #endregion

    private List<BallOperator> _allBalls;
    private List<GoalItemManager> _allGoalItemsManager;

    #region Goals
    GoalItemManager redG;
    GoalItemManager blueG;
    GoalItemManager greenG;
    GoalItemManager purpleG;
    GoalItemManager orangeG;
    #endregion

    private void Awake()
    {
        _levelData = _dataManager.CurrentLevel;
        #region UI setup
        _allGoalItemsManager = new List<GoalItemManager>();
        //crade mais pas ltime
        if (_levelData.RedBallsRequired>0)
        {
            redG = GameObject.Instantiate(_goalItemPrefab, _goalParent).GetComponent<GoalItemManager>();
            redG.Init(_redB, _levelData.RedBallsRequired);
            _allGoalItemsManager.Add(redG);
        }
        if (_levelData.GreenBallsRequired > 0)
        {
            greenG = GameObject.Instantiate(_goalItemPrefab, _goalParent).GetComponent<GoalItemManager>();
            greenG.Init(_greenB, _levelData.GreenBallsRequired);
            _allGoalItemsManager.Add(greenG);
        }
        if (_levelData.BlueBallsRequired > 0)
        {
            blueG = GameObject.Instantiate(_goalItemPrefab, _goalParent).GetComponent<GoalItemManager>();
            blueG.Init(_blueB, _levelData.BlueBallsRequired);
            _allGoalItemsManager.Add(blueG);
        }
        if (_levelData.PurpleBallsRequired > 0)
        {
            purpleG = GameObject.Instantiate(_goalItemPrefab, _goalParent).GetComponent<GoalItemManager>();
            purpleG.Init(_purpleB, _levelData.PurpleBallsRequired);
            _allGoalItemsManager.Add(purpleG);
        }
        if (_levelData.OrangeBallsRequired > 0)
        {
            orangeG = GameObject.Instantiate(_goalItemPrefab, _goalParent).GetComponent<GoalItemManager>();
            orangeG.Init(_orangeB, _levelData.OrangeBallsRequired);
            _allGoalItemsManager.Add(orangeG);
        }

        _movesText.text = _levelData.MaxMoves.ToString();
        #endregion

        #region Screen size adaptation
        rt.Release();
        rt.width = Screen.width;
        rt.height = Screen.height;
        rt.Create();
        Debug.Log(lineRenderCamera.targetTexture);
        cs.referenceResolution = new Vector2(Screen.width, Screen.height);
        lineRenderCamera.orthographicSize = Screen.height / 2;
        ri.SetNativeSize();
        #endregion         
    }

    private void Start()
    {
        Init();
    }

    [ContextMenu("Init")]
    public void Init()
    {
        _allBalls = new List<BallOperator>();
        _ballTypes = new BallType[5] { _redB , _greenB, _blueB, _orangeB, _purpleB };

        _gridTransform.sizeDelta = new Vector2(_levelData.Size * 120, _levelData.Size * 120);
        for (int i = 0; i < _levelData.Size; i++)
        {
            for (int j = 0; j < _levelData.Size; j++)
            {
                BallOperator newBall = GameObject.Instantiate(_ballPrefab, _gridTransform.transform).GetComponent<BallOperator>();

                int rdmColor = UnityEngine.Random.Range(0, 4);
                newBall.Init(_ballTypes[rdmColor], this);

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
                List<int> columnsImpacted = new List<int>();
                foreach (var ballToDelete in LinkedBalls)
                {
                    ballToDelete.Validate(_ballTypes[UnityEngine.Random.Range(0, 4)]);
                    if (!columnsImpacted.Contains(ballToDelete.YCoord))
                    {
                        columnsImpacted.Add(ballToDelete.YCoord);
                    }
                    if (ballToDelete.YCoord != _levelData.Size-1)
                    {
                        foreach (var item2 in _allBalls.Where(b => ((b.XCoord == ballToDelete.XCoord) && (b.YCoord > ballToDelete.YCoord))))
                        {
                            Debug.Log($"forea"); 
                            int count = LinkedBalls.Count((b2 => (b2.XCoord == ballToDelete.XCoord) && (b2.YCoord < item2.YCoord)));
                            BallOperator ballToReplace = 
                                _allBalls.First(b => ((b.XCoord == ballToDelete.XCoord) && (b.YCoord == ballToDelete.YCoord)));
                            item2.LowerCells(count);
                            
                        }                 
                    }
                }
                //descendre les balles au dessus
                //List<BallOperator> ballsFromColumnsImpacted = _allBalls.Where(b => columnsImpacted.Contains(b.YCoord)).ToList();
                //foreach (var ballToFall in ballsFromColumnsImpacted)
                //{
                //    int floors = ballsFromColumnsImpacted.Count(b => (b.Deleted && (b.YCoord < ballToFall.YCoord)));
                //    if (floors > 0)
                //    {
                //        ballsFromColumnsImpacted.First(b3 => b3.YCoord == ballToFall.YCoord).ResetColor(ballToFall.BallType);
                //    }
                //    //w84tween
                //}

                //décompte
                switch (LinkedBalls[0].BallType.ColorType)
                {
                    case ColorType.RED:
                        if (redG != null)
                        {
                            redG.BallsNeeded -= LinkedBalls.Count();
                            if (redG.BallsNeeded<0)
                                redG.BallsNeeded = 0;
                            redG.UpdateCount();
                        }
                        break;
                    case ColorType.GREEN:
                        if (greenG != null)
                        {
                            greenG.BallsNeeded -= LinkedBalls.Count();
                            if (greenG.BallsNeeded < 0)
                                greenG.BallsNeeded = 0;
                            greenG.UpdateCount();
                        }
                        break;
                    case ColorType.BLUE:
                        if (blueG != null)
                        {
                            blueG.BallsNeeded -= LinkedBalls.Count();
                            if (blueG.BallsNeeded < 0)
                                blueG.BallsNeeded = 0;
                            blueG.UpdateCount();
                        }
                        break;
                    case ColorType.ORANGE:
                        if (orangeG != null)
                        {
                            orangeG.BallsNeeded -= LinkedBalls.Count();
                            if (orangeG.BallsNeeded < 0)
                                orangeG.BallsNeeded = 0;
                            orangeG.UpdateCount();
                        }
                        break;
                    case ColorType.PURPLE:
                        if (purpleG != null)
                        {
                            purpleG.BallsNeeded -= LinkedBalls.Count();
                            if (purpleG.BallsNeeded < 0)
                                purpleG.BallsNeeded = 0;
                            purpleG.UpdateCount();
                        }
                        break;
                    default:
                        break;
                }
                if (_allGoalItemsManager.All(g => g.BallsNeeded == 0))
                {
                    _gameGraphicRaycaster.enabled = false;
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

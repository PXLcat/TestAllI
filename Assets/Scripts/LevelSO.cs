using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level")]
public class LevelSO : ScriptableObject
{
    public int Index;
    public int Size;
    public int MaxMoves;

    public int RedBallsRequired;
    public int GreenBallsRequired;
    public int BlueBallsRequired;
    public int PurpleBallsRequired;
    public int OrangeBallsRequired;

}

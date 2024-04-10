using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationButtonBehaviour : MonoBehaviour
{
    [SerializeField]
    private DataManagerSO _dataManagerSO;
    [SerializeField]
    private LevelSO _levelSO;

    public void GoToScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void GoToGameplayScene()
    {
        _dataManagerSO.CurrentLevel = _levelSO;
        SceneManager.LoadScene(2);
    }
}

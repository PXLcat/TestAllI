using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static LevelOperator;
using Coffee.UIExtensions;

public class GoalItemManager : MonoBehaviour
{
    [SerializeField]
    private Image _image;
    [SerializeField]
    private TextMeshProUGUI _txt;
    [SerializeField]
    private Coffee.UIExtensions.UIParticle _UIParticle;

    public int BallsNeeded;

    private void Awake()
    {
        _UIParticle.Stop();
    }

    public void Init(BallType ballType, int count)
    {
        _image.sprite = ballType.BallSprite;
        BallsNeeded = count;
        _txt.text = count.ToString();
    }

    public void UpdateCount()
    {
        StartCoroutine(PlayParticles());
        _txt.text = BallsNeeded.ToString();
    }
    IEnumerator PlayParticles()
    {
        _UIParticle.Play();
        yield return new WaitForSecondsRealtime(0.2f);
        _UIParticle.StopEmission();
    }
}

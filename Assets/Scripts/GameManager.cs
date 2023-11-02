using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;


public class GameManager : MonoBehaviour
{
    //Singletonパターン
    public static GameManager instance;

    public UnityEvent onReset;

    //ラウンドの待機中・終了時に表示
    public GameObject readyPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI messageText;

    //ラウンド進行中の判断
    public bool isRoundActive = false;

    private int score = 0;

    //ready中のShooterRotatorを非活性化
    public ShooterRotator shooterRotator;
    //追跡対象設定
    public CamFollow cam;

    private void Awake()
    {
        instance = this;
        UpdateUI();
    }

    private void Start()
    {
        StartCoroutine("RoundRoutine");
    }

    /*現在score更新*/
    public void AddScore(int newScore)
    {
        score += newScore;
        UpdateBestScore();
        UpdateUI();
    }

    /*BestScore更新(PlayerPrefs使用)*/
    void UpdateBestScore()
    {
        if(GetBestScore() < score)
            PlayerPrefs.SetInt("BestScore", score);
    }

    /*BestScore読み込み(PlayerPrfs使用)*/
    int GetBestScore()
    {
        return PlayerPrefs.GetInt("BestScore");
    }

    /*UI更新*/
    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        bestScoreText.text = "Best Score: " + GetBestScore();
    }

    /*Ballが爆発する時,Ballで実行
     UI更新、isRoundActive更新*/
    public void OnBallDestory()
    {
        UpdateUI();
        isRoundActive = false;
    }

    public void Reset()
    {
        score = 0;
        UpdateUI();

        //ラウンドを最初から
        StartCoroutine("RoundRoutine");
    }

    IEnumerator RoundRoutine()
    {
        //Ready
        onReset.Invoke();

        readyPanel.SetActive(true);
        cam.SetTarget(shooterRotator.transform, CamFollow.State.Idle);
        shooterRotator.enabled = false;

        isRoundActive = false;

        messageText.text = "Ready...";

        yield return new WaitForSeconds(3f);

        //Play
        isRoundActive = true;
        readyPanel.SetActive(false);
        shooterRotator.enabled = true;

        cam.SetTarget(shooterRotator.transform, CamFollow.State.Ready);
        while(isRoundActive)
        {
            yield return null;
        }

        //End
        readyPanel.SetActive(true);
        shooterRotator.enabled = false;

        messageText.text = "Wait For Next Round...";

        yield return new WaitForSeconds(3f);
        Reset();
    }

}

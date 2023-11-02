using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour
{
    //cameraが追跡するtaragetの設定用
    public CamFollow cam;
    //発射するballの生成用
    public Rigidbody ball;
    //発射する位置情報
    public Transform firePos;
    //発射ゲージ用UI
    public Slider powerSlider;

    //発射用AudioSource
    public AudioSource shootingAudio;
    public AudioClip fireClip;
    public AudioClip chargingClip;

    //発射ゲージmin・max値
    public float miniForce = 15f;
    public float maxForce = 30f;
    //チャージング時間計算用
    public float charngingTime = 0.75f;

    private float currentForce;
    private float chargeSpeed;
    private bool fired;


    /*BallShooterスクリプトが活性化される度の初期化*/
    private void OnEnable()
    {
        currentForce = miniForce;
        powerSlider.value = miniForce;
        fired = false;

    }

    private void Start()
    {
        //速度 = 移動距離 / 掛かった時間
        chargeSpeed = (maxForce - miniForce) / charngingTime;
    }

    private void Update()
    {
        //既に発射されたら、終了させる
        if(fired == true)
        {
            return;
        }
        powerSlider.value = miniForce;
        //currentForceがmax値の以上になった時、強制発射
        if (currentForce >= maxForce && !fired)
        {
            currentForce = maxForce;
            Fire();
        }
        //Fire1ボタンを一回押した時、currentForceをminiForceに、clipをChargingClipに設定
        else if (Input.GetButtonDown("Fire1"))
        {
            fired = false; 
            currentForce = miniForce;

            shootingAudio.clip = chargingClip;
            shootingAudio.Play();
        }
        //Fire1ボタンを押している間、CurrentForceの更新、sliderの更新
        else if (Input.GetButton("Fire1") && !fired)
        {
            currentForce += chargeSpeed * Time.deltaTime;
            powerSlider.value = currentForce;
        }
        //Fire1ボタンを離した時、発射
        else if(Input.GetButtonUp("Fire1") && !fired)
        {
            Fire();
        }
    }

    /*Ballの発射*/
    private void Fire()
    {
        fired = true;

        Rigidbody ballInstance =  Instantiate(ball, firePos.position, firePos.rotation);

        //生成したballの速度を、fireposの前方向にチャージしたcurrentForceを掛けた値にする
        ballInstance.velocity = currentForce * firePos.forward;

        //発射用Audioを再生
        shootingAudio.clip = fireClip;
        shootingAudio.Play();

        //発射後、チャージした値を初期化する
        currentForce = miniForce;

        //cameraが追跡する対象をballにする
        cam.SetTarget(ballInstance.transform, CamFollow.State.Tracking);
    }
}

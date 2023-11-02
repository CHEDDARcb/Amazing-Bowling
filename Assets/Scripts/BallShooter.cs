using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour
{
    public CamFollow cam;
    public Rigidbody ball;
    public Transform firePos;
    public Slider powerSlider;
    public AudioSource shootingAudio;
    public AudioClip fireClip;
    public AudioClip chargingClip;
    public float miniForce = 15f;
    public float maxForce = 30f;
    public float charngingTime = 0.75f;

    private float currentForce;
    private float chargeSpeed;
    private bool fired;

    private void OnEnable()
    {
        currentForce = miniForce;
        powerSlider.value = miniForce;
        fired = false;

    }

    private void Start()
    {
        chargeSpeed = (maxForce - miniForce) / charngingTime;
    }

    private void Update()
    {
        if(fired == true)
        {
            return;
        }
        powerSlider.value = miniForce;
        if(currentForce >= maxForce && !fired)
        {
            //currentForceがmax値になった時
            currentForce = maxForce;
            Fire();
        }
        else if(Input.GetButtonDown("Fire1"))
        {
            fired = false; 
            //Fire1ボタンを一回押した時、currentForceをminiForceに、clipをChargingClipに設定
            currentForce = miniForce;

            shootingAudio.clip = chargingClip;
            shootingAudio.Play();
        }
        else if(Input.GetButton("Fire1") && !fired)
        {
            //Fire1ボタンを押している間、CurrentForceの更新、sliderの更新
            currentForce += chargeSpeed * Time.deltaTime;
            powerSlider.value = currentForce;
        }
        else if(Input.GetButtonUp("Fire1") && !fired)
        {
            Fire();
        }
    }

    private void Fire()
    {
        fired = true;

        Rigidbody ballInstance =  Instantiate(ball, firePos.position, firePos.rotation);

        ballInstance.velocity = currentForce * firePos.forward;

        shootingAudio.clip = fireClip;
        shootingAudio.Play();

        currentForce = miniForce;

        cam.SetTarget(ballInstance.transform, CamFollow.State.Tracking);
    }
}

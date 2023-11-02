using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterRotator : MonoBehaviour
{
    //Shooterの状態
    private enum RotateState
    {
        Idle, Vertical, Horizontal, Ready
    }

    private RotateState state;
    public float verticalRotateSpeed;
    public float horizontallRotateSpeed;
    public BallShooter ballShooter;

    public AudioSource rotationAudio;
    public AudioClip rotationClip;

    private void Start()
    {
        state = RotateState.Idle;
        verticalRotateSpeed = 360f;
        horizontallRotateSpeed = 360f;
    }

    private void Update()
    {
        switch(state)
        {
            //回転の準備
            case RotateState.Idle:
                if (Input.GetButtonDown("Fire1"))
                {
                    state = RotateState.Horizontal;
                    rotationAudio.clip = rotationClip;
                }
            break;
            //y軸回転
            case RotateState.Horizontal:
                if (Input.GetButton("Fire1"))
                {
                    transform.Rotate(new Vector3(0, horizontallRotateSpeed * Time.deltaTime, 0));
                    rotationAudio.Play();
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    state = RotateState.Vertical;
                    rotationAudio.Stop();
                }
            break;
            //x軸回転
            case RotateState.Vertical:
                if (Input.GetButton("Fire1"))
                {
                    transform.Rotate(new Vector3(-verticalRotateSpeed * Time.deltaTime, 0, 0));
                    rotationAudio.Play();
                }
                else if (Input.GetButtonUp("Fire1"))
                {
                    rotationAudio.Stop();
                    state = RotateState.Ready;
                    ballShooter.enabled = true;
                }
            break;
            //発射準備完了
            case RotateState.Ready:
                break;
        }
    }

    private void OnEnable()
    {
        transform.rotation = Quaternion.identity;
        state = RotateState.Idle;
        ballShooter.enabled = false;
    }
}

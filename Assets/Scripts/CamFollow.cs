using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    //cameraの状態
    public enum State
    {
        Idle, Ready, Tracking
    }
    private State state
    {
        set
        {
            switch(value)
            {
                case State.Idle:
                    targetZoomSize = roundReadyZoomSize;
                    break;
                case State.Ready:
                    targetZoomSize = readyShotZoomSize;
                    break;
                case State.Tracking:
                    targetZoomSize = trackingZoomSize;
                    break;
            }
        }
    }

    //追跡する対象
    private Transform target;

    //camera動き遅延時間
    public float smoothTime = 0.2f;

    //SmoothDamp用
    private Vector3 lastMovingVelocity;
    private Vector3 targetPosition;

    private Camera cam;
    private float targetZoomSize = 5f;

    //ラウンド待機中
    private const float roundReadyZoomSize = 14.5f;
    //ball発射準備する時
    private const float readyShotZoomSize = 5f;
    //ballを追いかける時
    private const float trackingZoomSize = 10f;

    //SmoothDamp用
    private float lastZoomSpeed;


    private void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        state = State.Idle;
    }

    /*cameraのpositionの移動をSmoothDampを利用して遅延させる*/
    private void Move()
    {
        targetPosition = target.transform.position;
        Vector3 smoothPostion = Vector3.SmoothDamp(transform.position, targetPosition, ref lastMovingVelocity, smoothTime);
        transform.position = smoothPostion;
    }

    /*cameraのsize(zoom)の変更をSmoothDampを利用して遅延させる*/
    private void Zoom()
    {
        float smoothZoomSize = Mathf.SmoothDamp(cam.orthographicSize, targetZoomSize, ref lastZoomSpeed, smoothTime);
        cam.orthographicSize = smoothZoomSize;
    }

    private void FixedUpdate()
    {
        if(target != null)
        {
            Move();
            Zoom();
        }
    }

    /*毎ラウンド初期化*/
    private void Reset()
    {
        state = State.Idle;
    }

    /*GameMangerで使う
     targetとstateを変更*/
    public void SetState(Transform newTarget, State newState)
    {
        target = newTarget;
        state = newState;
    }
}

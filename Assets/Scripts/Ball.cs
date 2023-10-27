using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public LayerMask whatIsProp;
    public ParticleSystem explosionParticle;
    public AudioSource explosionAudio;
    public float maxDamage = 100;
    public float explosionForce = 1000;
    public float lifeTime = 10.0f;
    public float explosionRadius = 20f;

    private void Start()
    {
        Destroy(gameObject, lifeTime); //부모가 파괴되면 자식(이펙트)도 같이파괴ㅅ
    }

    private void OnTriggerEnter(Collider other)
    {
        //爆発半径ないのPropを持ってくる
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, whatIsProp);
        for(int i = 0; i < colliders.Length; i++)
        {
            //持ってきたpropにAddExplosionForceで吹き飛ばせる
            Rigidbody targetRigidBody = colliders[i].GetComponent<Rigidbody>();
            targetRigidBody.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            //持ってきたpropにダメージを与える
            Prop targetProp = colliders[i].GetComponent<Prop>();
            targetProp.TakeDamage(CalculateDamage(colliders[i].transform.position));
        }
        explosionParticle.transform.parent = null;
        explosionParticle.Play();
        explosionAudio.Play();

        Destroy(explosionParticle.gameObject, explosionParticle.duration);
        Destroy(gameObject);
    }

    private float CalculateDamage(Vector3 targetPosition)
    {
        //Propと着弾地点の距離の計算
        Vector3 explosionToTarget = targetPosition - transform.position;
        float distance = explosionToTarget.magnitude;

        //爆発半径から近いほどダメージpercentageが上がる
        float edgeToCenterDistance = explosionRadius - distance;
        float percentage = edgeToCenterDistance / explosionRadius;

        return Mathf.Max(0, percentage * maxDamage) ;
    }
}

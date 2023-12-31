using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public int score = 5;
    public ParticleSystem explostionParticle;
    public float hp = 10f;

    public void TakeDamage(float damage)
    {
        hp -= damage;

        if(hp <= 0)
        {
            ParticleSystem instance = Instantiate(explostionParticle, transform.position, transform.rotation);
            AudioSource explosionAudio = instance.GetComponent<AudioSource>();
            explosionAudio.Play();
            instance.Play();

            GameManager.instance.AddScore(score);

            Destroy(instance.gameObject, instance.duration);
            gameObject.SetActive(false);
        }
    }
}

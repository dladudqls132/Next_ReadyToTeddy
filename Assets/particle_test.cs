using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle_test : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool isEatted;
    [SerializeField] private float eattedTime;
    private float eattedSpeed;

    public void SetTarget(Transform target) { this.target = target; }


	// Update is called once per frame
	void Update()
    {
        ParticleSystem.Particle[] p = new ParticleSystem.Particle[this.GetComponent<ParticleSystem>().particleCount + 1];
        int l = this.GetComponent<ParticleSystem>().GetParticles(p);

        if (isEatted)
        {
            eattedSpeed += Time.deltaTime / eattedTime;
            for(int i = 0; i < this.GetComponent<ParticleSystem>().particleCount; i++)
            {
                p[i].position = Vector3.Lerp(p[i].position, target.position, Time.deltaTime * eattedSpeed);
                p[i].velocity = Vector3.zero;

                this.GetComponent<ParticleSystem>().SetParticles(p, l);
            }
        }
        else
        {
            for (int i = 0; i < this.GetComponent<ParticleSystem>().particleCount; i++)
            {
                p[i].velocity = Vector3.Lerp(p[i].velocity, Vector3.zero, Time.deltaTime * 5);
           
                if (p[0].velocity.magnitude <= 0.3f)
                    isEatted = true;
                this.GetComponent<ParticleSystem>().SetParticles(p, l);
            }
        }
    }
}

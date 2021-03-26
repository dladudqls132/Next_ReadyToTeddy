using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle_test : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool isEatted;
    [SerializeField] private float eattedTime;
    [SerializeField] private Vector3[] particlePosition;

    private ParticleSystem particleSystem;
    private float eattedSpeed;

    public void SetTarget(Transform target) { this.target = target; }

    private void Start()
    {
        particleSystem = this.GetComponent<ParticleSystem>();
        particlePosition = new Vector3[this.GetComponent<ParticleSystem>().emission.GetBurst(0).maxCount];
    }

    // Update is called once per frame
    void Update()
    {
        ParticleSystem.Particle[] p = new ParticleSystem.Particle[particleSystem.particleCount + 1];
        int l = particleSystem.GetParticles(p);

        if (isEatted)
        {
                bool destroy= true;
            eattedSpeed += Time.deltaTime / eattedTime;
            for (int i = 0; i < particleSystem.particleCount; i++)
            {
                p[i].position = Vector3.Lerp(p[i].position, target.position, Time.deltaTime * eattedSpeed);
                p[i].velocity = Vector3.zero;
                particlePosition[i] = p[i].position;

                if (target.GetComponent<Collider>().bounds.Contains(p[i].position))
                {
                    if (target.CompareTag("Player"))
                    {
                        target.GetComponent<PlayerController>().IncreaseCombo(1);

                    }
                    p[i].remainingLifetime = 0;
                }

                if (p[i].remainingLifetime != 0)
                    destroy = false;

                

                particleSystem.SetParticles(p, l);
            }

            if (destroy)
                Destroy(this.gameObject);
        }
        else
        {
            for (int i = 0; i < particleSystem.particleCount; i++)
            {
                p[i].velocity = Vector3.Lerp(p[i].velocity, Vector3.zero, Time.deltaTime * 5);

                if (p[0].velocity.magnitude <= 0.3f)
                    isEatted = true;
                particleSystem.SetParticles(p, l);
            }
        }
    }
}

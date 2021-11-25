using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code adapted from Graphics and Interaction (COMP30019) 2021 Semester 2 Week 9 Lab DestroyWhenParticlesFinished.cs
public class DestroyParticleSystem : MonoBehaviour
{
    public ParticleSystem targetParticleSystem;

    void Update()
    {
        if (!this.targetParticleSystem.IsAlive())
        {
            Destroy(targetParticleSystem.gameObject);
        }
    }
}

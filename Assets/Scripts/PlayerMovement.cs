﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // PARAMETERS - for tuning, typically set in the editor
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip mainEngineOff;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftBoosterParticles;
    [SerializeField] ParticleSystem rightBoosterParticles;

    // CACHE - e.g. references for readability or speed
    Rigidbody rb;
    AudioSource audioSource;

    // STATE - private instance (member) variables
    bool engineOn = false;

        
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }

    }

    void ProcessRotation()
    {
        if(Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if(Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }

    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (engineOn == false)
        {
            engineOn = true;
            audioSource.PlayOneShot(mainEngine);
            mainEngineParticles.Play();
        }
    }

    private void StopThrusting()
    {
        if (engineOn == true)
        {
            engineOn = false;
            audioSource.Stop();
            audioSource.PlayOneShot(mainEngineOff);
            mainEngineParticles.Stop();
        }
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationThrust);
        leftBoosterParticles.Stop();
        if (!rightBoosterParticles.isPlaying)
        {
            rightBoosterParticles.Play();
        }
    }

    private void RotateRight()
    {
        ApplyRotation(-rotationThrust);
        rightBoosterParticles.Stop();
        if (!leftBoosterParticles.isPlaying)
        {
            leftBoosterParticles.Play();
        }
    }

    private void StopRotating()
    {
        leftBoosterParticles.Stop();
        rightBoosterParticles.Stop();
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; // unfreezing roation so physics system can take over
    }
}

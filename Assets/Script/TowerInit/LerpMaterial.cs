using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpMaterial : MonoBehaviour
{
    public Material startMaterial;
    public Material endMaterial;
    public float transitionDuration = 2.0f;

    private Material currentMaterial;
    private float transitionTimer = 0.0f;
    private bool isTransitioning = false;

    private void Start()
    {
        currentMaterial = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isTransitioning = true;
        }
    }

    private void FixedUpdate()
    {
        if (isTransitioning)
        {
            if (transitionTimer < transitionDuration)
            {
                transitionTimer += Time.deltaTime;
                float t = Mathf.Clamp01(transitionTimer / transitionDuration);
                GetComponent<Renderer>().material.Lerp(currentMaterial, endMaterial, t);
            }
        }
    }
}

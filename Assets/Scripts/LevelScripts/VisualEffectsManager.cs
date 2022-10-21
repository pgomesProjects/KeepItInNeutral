using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Cinemachine;

public enum CAMEFFECT { NONE, DISTORT, TUNNEL, MIRROR }

public class VisualEffectsManager : MonoBehaviour
{
    private Volume cameraVolume;
    [SerializeField] private VolumeProfile[] volumeProfiles;

    private IEnumerator currentEffectCoroutine;

    private void Start()
    {
        cameraVolume = FindObjectOfType<Volume>();
    }

    public void ShowEffect(CAMEFFECT camEffect)
    {
        //Stop any other effects active
        if(currentEffectCoroutine != null)
        {
            StopCoroutine(currentEffectCoroutine);
            InstantRemoveEffects();
        }

        //Decide which function to use based on the effect the script is calling for
        switch (camEffect)
        {
            case CAMEFFECT.DISTORT:
                DistortLens();
                break;
            case CAMEFFECT.TUNNEL:
                TunnelVision();
                break;
            case CAMEFFECT.MIRROR:
                FlipCamera();
                break;
        }
    }

    public void DistortLens()
    {
        //Set the post processing profile on the camera volume and gradually show the effects
        cameraVolume.profile = volumeProfiles[0];
        currentEffectCoroutine = ShowEffect(5);
        StartCoroutine(currentEffectCoroutine);
    }

    public void TunnelVision()
    {
        //Play sound effect
        FindObjectOfType<AudioManager>().PlayOneShot("OilSplat", PlayerPrefs.GetFloat("SFXVolume", 0.5f));

        //Set the post processing profile on the camera volume and gradually show the effects
        cameraVolume.profile = volumeProfiles[1];
        currentEffectCoroutine = ShowEffect(5);
        StartCoroutine(currentEffectCoroutine);
    }

    public void FlipCamera()
    {
        //Play sound effect
        FindObjectOfType<AudioManager>().PlayOneShot("MagicSFX", PlayerPrefs.GetFloat("SFXVolume", 0.5f));

        //Smoothly flip the camera
        currentEffectCoroutine = CameraFlipAnimation(180, 2);
        StartCoroutine(currentEffectCoroutine);
    }

    IEnumerator CameraFlipAnimation(float zValue, float seconds)
    {
        float timeElapsed = 0;
        Quaternion startRotation = FindObjectOfType<CinemachineVirtualCamera>().transform.rotation;
        Quaternion endRotation = Quaternion.Euler(new Vector3(10, 90, zValue));

        while (timeElapsed < seconds)
        {
            //Smooth lerp duration algorithm
            float t = timeElapsed / seconds;
            t = t * t * (3f - 2f * t);

            foreach(var i in FindObjectsOfType<CinemachineVirtualCamera>())
            {
                i.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            }

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        foreach (var i in FindObjectsOfType<CinemachineVirtualCamera>())
        {
            i.transform.rotation = endRotation;
        }

        //Wait for a few seconds before beginning to get rid of the current effect
        yield return new WaitForSeconds(5);

        StartCoroutine(ResetCamera(seconds));
    }

    IEnumerator ResetCamera(float seconds)
    {
        float timeElapsed = 0;
        Quaternion startRotation = FindObjectOfType<CinemachineVirtualCamera>().transform.rotation;
        Quaternion endRotation = Quaternion.Euler(new Vector3(10, 90, 0));

        while (timeElapsed < seconds)
        {
            //Smooth lerp duration algorithm
            float t = timeElapsed / seconds;
            t = t * t * (3f - 2f * t);

            foreach (var i in FindObjectsOfType<CinemachineVirtualCamera>())
            {
                i.transform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
            }

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        foreach (var i in FindObjectsOfType<CinemachineVirtualCamera>())
        {
            i.transform.rotation = endRotation;
        }
    }

    IEnumerator ShowEffect(float waitSeconds)
    {
        float timeElapsed = 0;
        float seconds = 1;
        float startWeight = 0;
        float endWeight = 1;    

        while (timeElapsed < seconds)
        {
            //Smooth lerp duration algorithm
            float t = timeElapsed / seconds;
            t = t * t * (3f - 2f * t);

            cameraVolume.weight = Mathf.Lerp(startWeight, endWeight, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        cameraVolume.weight = endWeight;


        //Wait for a few seconds before beginning to get rid of the current effect
        yield return new WaitForSeconds(waitSeconds);

        StartCoroutine(RemoveEffect());
    }

    IEnumerator RemoveEffect()
    {
        float timeElapsed = 0;
        float seconds = 0.25f;
        float startWeight = 1;
        float endWeight = 0;

        while (timeElapsed < seconds)
        {
            //Smooth lerp duration algorithm
            float t = timeElapsed / seconds;
            t = t * t * (3f - 2f * t);

            cameraVolume.weight = Mathf.Lerp(startWeight, endWeight, t);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        cameraVolume.weight = endWeight;

        cameraVolume.profile = null;
    }

    public void InstantRemoveEffects()
    {
        //Get rid of the camera's post processing volume
        cameraVolume.weight = 0;
        cameraVolume.profile = null;

        //Reset the camera rotations
        foreach (var i in FindObjectsOfType<CinemachineVirtualCamera>())
        {
            i.transform.rotation = Quaternion.Euler(new Vector3(10, 90, 0));
        }
    }
}

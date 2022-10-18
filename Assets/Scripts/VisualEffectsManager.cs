using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.Universal;

public enum CAMEFFECT { NONE, DISTORT, TUNNEL }

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
            StopCoroutine(currentEffectCoroutine);

        switch (camEffect)
        {
            case CAMEFFECT.DISTORT:
                DistortLens();
                break;
            case CAMEFFECT.TUNNEL:
                TunnelVision();
                break;
        }
    }

    public void DistortLens()
    {
        cameraVolume.profile = volumeProfiles[0];
        currentEffectCoroutine = ShowEffect(5);
        StartCoroutine(currentEffectCoroutine);
    }

    public void TunnelVision()
    {
        cameraVolume.profile = volumeProfiles[1];
        currentEffectCoroutine = ShowEffect(5);
        StartCoroutine(currentEffectCoroutine);
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
}

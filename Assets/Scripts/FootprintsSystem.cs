using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class FootprintsSystem : MonoBehaviour
{
    public GameObject CurrentFootprint;
    public GameObject NextFootprint;

    public Vector3 Offset;

    public bool Enabled;

    public float TimeBetweenFootprints;

    private IEnumerator FootprintsCoroutine;

    public void SetFootprintsEnabled(bool enabled)
    {
        if (Enabled == enabled)
        {
            return;
        }

        // If enabled, 
        if (enabled)
        {
            FootprintsCoroutine = SpawnFootprints();
            StartCoroutine(FootprintsCoroutine);
        }
        else
        {
            StopCoroutine(FootprintsCoroutine);
        }

        Enabled = enabled;
    }

    public IEnumerator SpawnFootprints()
    {
        while (true)
        {
            SpawnFootprint();
            yield return new WaitForSeconds(TimeBetweenFootprints);
        }
    }

    public void SpawnFootprint()
    {
        Instantiate(CurrentFootprint, transform.position + Offset, transform.rotation * CurrentFootprint.transform.rotation);
        (CurrentFootprint, NextFootprint) = (NextFootprint, CurrentFootprint);
        Offset = -Offset;
    }
}

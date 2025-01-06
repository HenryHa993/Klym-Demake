using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Subtitles : MonoBehaviour
{
    public GameObject SubtitlesGO;
    public TextMeshProUGUI Text;

    public float DialogueDuration = 3.0f;

    public string[] StartingDialogue;

    private void Start()
    {
        StartCoroutine(PlayDialogue(StartingDialogue));
    }

    public IEnumerator PlayDialogue(string[] dialogue)
    {
        SubtitlesGO.SetActive(true);
        
        foreach(string line in dialogue)
        {
            Text.text = line;
            for (float timer = DialogueDuration; timer > 0f; timer -= Time.deltaTime)
            {
                yield return null;
            }
        }
        
        SubtitlesGO.SetActive(false);
    }
}

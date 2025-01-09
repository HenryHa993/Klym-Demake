using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Subtitles : MonoBehaviour
{
    public GameObject SubtitlesGO;
    public TextMeshProUGUI Text;

    public float DialogueDuration = 3.0f;

    public string[] StartingDialogue;

    private PlayerInput _playerInput;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        
        StartCoroutine(PlayDialogue(StartingDialogue));
    }

    public void StartDialogue(string[] dialogue)
    {
        StartCoroutine(PlayDialogue(dialogue));
    }

    public IEnumerator PlayDialogue(string[] dialogue)
    {
        SubtitlesGO.SetActive(true);
        InputSetActive(false);
        
        foreach(string line in dialogue)
        {
            Text.text = line;

            yield return new WaitForSeconds(DialogueDuration);
        }
        
        SubtitlesGO.SetActive(false);
        InputSetActive(true);
    }

    private void InputSetActive(bool enabled)
    {
        if (enabled)
        {
            _playerInput.actions.Enable();
            _playerInput.actions.FindActionMap("Climbing").Disable();
        }
        else
        {
            _playerInput.actions.Disable();
        }
    }
}

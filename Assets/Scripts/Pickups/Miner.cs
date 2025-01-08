using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Miner : Pickup
{
    public string[] NakedDialogue;
    public string[] ReturnedBirdDialogue;
    public string[] NoBirdDialogue;
    public string[] CompleteDialogue;

    public AudioClip _gameEndMusic;
    
    private int _numBirds;
    
    public override IEnumerator Interaction()
    {
        yield return base.Interaction();

        BirdManager birdManager = _player.GetComponent<BirdManager>();
        Clothing _clothing = _player.GetComponent<Clothing>();
        Subtitles _subtitleComponent = _player.GetComponent<Subtitles>();

        if (!_clothing.IsClothed)
        {
            _subtitleComponent.StartDialogue(NakedDialogue);
            yield break;
        }

        // If player does not have bird, play dialogue
        if (birdManager.GiveBird())
        {
            _numBirds++;

            if (_numBirds == 2)
            {
                AudioSource _audioSource = _player.GetComponent<AudioSource>();
                _audioSource.clip = _gameEndMusic;
                _audioSource.Play();

                _subtitleComponent.DialogueDuration = 3.0f;
                yield return _subtitleComponent.PlayDialogue(CompleteDialogue);
                _subtitleComponent.SubtitlesGO.SetActive(true);

                while (_audioSource.isPlaying)
                {
                    yield return null;
                }
                
                // Restart level after
                SceneManager.LoadScene("Game");
                yield break;
            }
            
            _subtitleComponent.StartDialogue(ReturnedBirdDialogue);
            yield break;
        }
        
        // Otherwise, the player has no birds.
        _subtitleComponent.StartDialogue(NoBirdDialogue);
    }
}

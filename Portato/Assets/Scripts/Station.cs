using System.Collections;
using UnityEngine;

public class Station : MonoBehaviour {
    [SerializeField] float interactionTime = 2f;
    [SerializeField] Sprite _activatedSprite;
    
    private SpriteRenderer _stationSprite;
    private bool _activated = false;

    public void Init(SpriteRenderer sr) {
        _stationSprite = sr; // recebe o SR do PAI desta instância
    }

    public void StartInteraction(PlayerController player) {
        if (!_activated)
            StartCoroutine(Interact(player));
    }

    IEnumerator Interact(PlayerController player) {
        yield return new WaitForSeconds(interactionTime);
        _activated = true;
        _stationSprite.sprite = _activatedSprite;
        player.Resume();
    }
}
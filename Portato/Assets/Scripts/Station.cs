using System.Collections;
using UnityEngine;

public class Station : MonoBehaviour {
    [SerializeField] float interactionTime = 2f;
    [SerializeField] SpriteRenderer _stationSprite;
    [SerializeField] Sprite _activatedSprite;
    public void StartInteraction(PlayerController player) {
        StartCoroutine(Interact(player));
    }
    private void Start()
    {
        this._stationSprite = GetComponentInParent<SpriteRenderer>();
    }
    IEnumerator Interact(PlayerController player) {
        // Durante interactionTime, o Interactable.cs trata da energia
        yield return new WaitForSeconds(interactionTime);
        this._stationSprite.sprite = _activatedSprite;
        player.Resume();
    }
}
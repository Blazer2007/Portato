using System.Collections;
using UnityEngine;

public class Station : MonoBehaviour {
    [SerializeField] float interactionTime = 2f;

    public void StartInteraction(PlayerController player) {
        StartCoroutine(Interact(player));
    }

    IEnumerator Interact(PlayerController player) {
        // Durante interactionTime, o Interactable.cs trata da energia
        yield return new WaitForSeconds(interactionTime);
        player.Resume();
    }
}
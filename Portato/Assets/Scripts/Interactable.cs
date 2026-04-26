using UnityEngine;
using System.Collections.Generic;

public enum InteractableType { WaterPot, Peeler, Knife, ClumpRemover, Device }

public class Interactable : MonoBehaviour
{
    public InteractableType type;
    [SerializeField] float value = 20f;        // drain, força, energia requerida
    [SerializeField] float rate = 15f;        // só para Peeler e WaterPot
    bool active = false;
    Rigidbody2D potatoRb;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        potatoRb = col.GetComponent<Rigidbody2D>();
        active = true;

        switch (type)
        {
            case InteractableType.Knife:
                GameEvents.DrainEnergy(value); break;

            case InteractableType.Device:
                // regista este dispositivo como carregado
                DeviceTracker.Charge(gameObject.GetInstanceID()); break;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player")) { active = false; potatoRb = null; }
    }

    void FixedUpdate()
    {
        if (!active || potatoRb == null) return;

        switch (type)
        {
            case InteractableType.WaterPot:
                GameEvents.RechargeEnergy(rate * Time.fixedDeltaTime); break;

            case InteractableType.Peeler:
                GameEvents.DrainEnergy(rate * Time.fixedDeltaTime); break;

            case InteractableType.ClumpRemover:
                var dir = ((Vector2)transform.position - potatoRb.position).normalized;
                potatoRb.AddForce(dir * value); break;
        }
    }
}

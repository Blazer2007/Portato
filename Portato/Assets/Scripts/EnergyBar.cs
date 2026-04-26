using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private EnergySystem _energySystem;

    private readonly Color _fullColor = new Color(0.9f, 0.75f, 0.1f);
    private readonly Color _emptyColor = new Color(0.35f, 0.25f, 0.05f);

    void Update()
    {
        _fillImage.fillAmount = _energySystem.EnergyPercent;
        _fillImage.color = Color.Lerp(_emptyColor, _fullColor, _energySystem.EnergyPercent);
    }
}
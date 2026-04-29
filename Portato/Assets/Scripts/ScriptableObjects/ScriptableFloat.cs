using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableFloat", menuName = "Scriptable Objects/ScriptableFloat")]
public class ScriptableFloat : ScriptableObject
{
    public float _value;
    public float _initialValue;
    public event System.Action<float> OnValueChanged;

    public float Value
    {
        get { return _value; }
        set { _value = value;
            OnValueChanged?.Invoke(_value);
         }
    }
    public void OnEnable()
    {

    }
    public void OnDisable()
    {
        _value = _initialValue;
    }
}

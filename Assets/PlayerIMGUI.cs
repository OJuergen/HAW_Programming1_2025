using GameProgramming1;
using UnityEngine;

public class PlayerIMGUI : MonoBehaviour
{
    [SerializeField, Range(0, 100)] private int _amount = 5;

    private IHealth _health;

    private void Awake()
    {
        _health = GetComponentInChildren<IHealth>();
    }

    private void OnGUI()
    {
        if (_health != null && GUILayout.Button($"Take {_amount} Damage"))
        {
            _health.TakeDamage(_amount);
        }

        if (_health != null && GUILayout.Button($"Heal {_amount}"))
        {
            _health.Heal(_amount);
        }
    }
}
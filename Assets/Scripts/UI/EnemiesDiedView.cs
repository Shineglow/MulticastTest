using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemiesDiedView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpText;

    public void SetDiedEnemiesCount(int count)
    {
        tmpText.text = $"Killed enemies count: {count}";
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TimerText;

    private void Update()
    {
        TimerText.text = "Time Left: " + GameManager.Instance.GetTimeLeftRounded();
    }
}

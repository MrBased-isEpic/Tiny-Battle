using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorTextUI : MonoBehaviour
{
    private List<TextMeshProUGUI> ErrorTexts = new List<TextMeshProUGUI>();
    private List<float> Timers = new List<float>();
    [SerializeField] private RectTransform ErrorText;

    public void DisplayError(string ErrorMessage)
    {
        TextMeshProUGUI errorText = Instantiate(ErrorText, this.transform).GetComponent<TextMeshProUGUI>();
        errorText.text = ErrorMessage;
        ErrorTexts.Add(errorText);
        float timer = 3;
        Timers.Add(timer);
    }

    private void Update()
    {
        for (int i = 0; i < Timers.Count; i++)
        {
            Timers[i] -= Time.deltaTime;
            if (Timers[i] < 0)
            {
                RectTransform errorText = ErrorTexts[i].rectTransform;
                ErrorTexts.Remove(ErrorTexts[i]);
                Destroy(errorText.gameObject);
                Timers.Remove(Timers[i]);
            }
        }
    }
}

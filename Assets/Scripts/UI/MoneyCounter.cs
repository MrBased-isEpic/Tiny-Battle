using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI MoneyText;

    public void ShowMoneyAmount(int amount)
    {
        MoneyText.text = "Money: " + amount;
    }
}

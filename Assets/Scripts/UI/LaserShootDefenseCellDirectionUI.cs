using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserShootDefenseCellDirectionUI : MonoBehaviour
{
    [SerializeField] private Button rightButton;
    [SerializeField] private Button upButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button downButton;

    private void Awake()
    {
        rightButton.onClick.AddListener(() =>
        {
            gameObject.GetComponentInParent<LaserShootDefenseCell>().SetDirection(GridManager.Direction.Right);
        });
        leftButton.onClick.AddListener(() =>
        {
            gameObject.GetComponentInParent<LaserShootDefenseCell>().SetDirection(GridManager.Direction.Left);
        });
        upButton.onClick.AddListener(() =>
        {
            gameObject.GetComponentInParent<LaserShootDefenseCell>().SetDirection(GridManager.Direction.Up);
        });
        downButton.onClick.AddListener(() =>
        {
            gameObject.GetComponentInParent<LaserShootDefenseCell>().SetDirection(GridManager.Direction.Down);
        });
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCell : MonoBehaviour
{
    [SerializeField] private Transform selectedVisual;

    protected void Start()
    {
        HideSelectedVisual();
    }
    public virtual void Interact()
    {
        Debug.Log("Interacted with BaseCell at" + transform.position);
    }

    // Visual Functions
    public void ShowSelectedVisual()
    {
        selectedVisual.gameObject.SetActive(true);
    }
    public void HideSelectedVisual()
    {
        selectedVisual.gameObject.SetActive(false);
    }
}

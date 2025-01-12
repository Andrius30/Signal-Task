using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static Action<bool> onVictory; // event trigered when win codition satisfied
    [SerializeField] GameObject pannel;

    void ShowVictoryPannel(bool enable) => pannel.SetActive(enable); // enables UI pannel

    private void OnEnable()
    {
        onVictory += ShowVictoryPannel;
    }
    private void OnDestroy()
    {
        onVictory -= ShowVictoryPannel;
    }
    private void OnDisable()
    {
        onVictory -= ShowVictoryPannel;
    }
}

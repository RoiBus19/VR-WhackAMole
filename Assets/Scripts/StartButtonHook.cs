using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class StartButtonHook : MonoBehaviour
{
    public GameManager gameManager;
    private XRBaseInteractable interactable;
    private void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
    }
    private void OnEnable()
    {
        if (interactable != null)
            interactable.selectEntered.AddListener(OnPressed);
    }
    private void OnDisable()
    {
        if (interactable != null)
            interactable.selectEntered.RemoveListener(OnPressed);
    }
    private void OnPressed(SelectEnterEventArgs args)
    {
        if (gameManager != null)
            gameManager.StartRound();
    }
}
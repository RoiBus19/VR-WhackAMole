using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class StartButtonHook : MonoBehaviour
{
    public GameManager gameManager;
    private XRBaseInteractable interactable;
    void Awake()
    {
        interactable = GetComponent<XRBaseInteractable>();
    }
    void OnEnable()
    {
        if (interactable != null)
            interactable.selectEntered.AddListener(OnPressed);
    }
    void OnDisable()
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
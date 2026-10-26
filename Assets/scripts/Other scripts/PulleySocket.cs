using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PulleySocket : MonoBehaviour
{
    public AssemblyManager manager;
    public string requiredTag;

    private XRSocketInteractor socket;

    void Start()
    {
        socket = GetComponent<XRSocketInteractor>();
        socket.selectEntered.AddListener(OnInserted);
    }

    void OnInserted(SelectEnterEventArgs args)
    {
        GameObject insertedObject = args.interactableObject.transform.gameObject;

        if (!insertedObject.CompareTag(requiredTag))
        {
            // Если шкив неправильный — выталкиваем его
            socket.interactionManager.SelectExit(socket, args.interactableObject);
            return;
        }

        manager.OnPulleyInstalled();
    }
}
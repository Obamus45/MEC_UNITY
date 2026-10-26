using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FanSocket : MonoBehaviour
{
    public AssemblyManager manager;

    void Start()
    {
        GetComponent<XRSocketInteractor>()
            .selectEntered.AddListener(OnInserted);
    }

    void OnInserted(SelectEnterEventArgs args)
    {
        manager.OnFanInstalled();
    }
}
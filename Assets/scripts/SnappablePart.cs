using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnappablePart : MonoBehaviour
{
    public string PartID;
    public XRSocketInteractor targetSocket;

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private bool isPlaced = false;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        // Когда взяли ремень - даём ему физику
        if (rb != null && !isPlaced)
        {
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        // Проверяем, что отпустили именно в сокет
        if (targetSocket != null && args.interactorObject == targetSocket)
        {
            SnapToSocket();
        }
    }

    void SnapToSocket()
    {
        if (isPlaced) return;

        isPlaced = true;

        // ⚠️ ОТКЛЮЧАЕМ ФИЗИКУ МГНОВЕННО
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // XR Toolkit сам разместит объект через Attach Transform
        // Нам остаётся только отключить возможность брать
        if (grabInteractable != null)
        {
            grabInteractable.enabled = false;
        }

        // Сообщаем менеджеру
        EngineAssemblyManager.Instance?.OnPartPlaced(PartID);
        Debug.Log($"✅ {PartID} установлен!");
    }

    public void ForceUnsnap()
    {
        isPlaced = false;

        if (rb != null)
        {
            rb.isKinematic = false;
        }

        if (grabInteractable != null)
        {
            grabInteractable.enabled = true;
        }

        EngineAssemblyManager.Instance?.OnPartRemoved(PartID);
    }

    void Update()
    {
        // Для отладки: R снимает деталь
        if (isPlaced && Input.GetKeyDown(KeyCode.R))
            ForceUnsnap();
    }
}
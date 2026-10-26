using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BeltSnap : MonoBehaviour
{
    [Header("Точка установки")]
    public Transform snapTarget; // Пустой объект с точной позицией/поворотом ремня

    [Header("Настройки")]
    public float snapDistance = 0.15f; // Дистанция срабатывания (в метрах)
    public bool checkPulleys = true;   // Требовать установленные шкивы

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private bool isLocked = false;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        // Подписываемся на событие отпускания детали
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        if (isLocked) return;

        // 1. Проверяем дистанцию до целевой точки
        float dist = Vector3.Distance(transform.position, snapTarget.position);

        // 2. Проверяем готовность шкивов (если включено)
        bool pulleysReady = !checkPulleys ||
                            (EngineAssemblyManager.Instance != null && EngineAssemblyManager.Instance.ArePulleysInstalled());

        // 3. Если всё ок → фиксируем
        if (dist < snapDistance && pulleysReady)
        {
            LockBelt();
        }
        else
        {
            if (!pulleysReady)
                Debug.Log("⚠️ Сначала установите все 3 шкива!");
        }
    }

    private void LockBelt()
    {
        isLocked = true;

        // Мгновенная телепортация в целевую точку
        transform.position = snapTarget.position;
        transform.rotation = snapTarget.rotation;

        // Отключаем физику и возможность брать
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        grabInteractable.enabled = false;

        // Сообщаем менеджеру
        EngineAssemblyManager.Instance?.OnPartPlaced("GRM");
        Debug.Log("✅ Ремень ГРМ успешно установлен!");
    }

    // Для отладки: кнопка R снимает ремень
    void Update()
    {
        if (isLocked && Input.GetKeyDown(KeyCode.R))
        {
            isLocked = false;
            rb.isKinematic = false;
            grabInteractable.enabled = true;
            EngineAssemblyManager.Instance?.OnPartRemoved("GRM");
            Debug.Log("🔄 Ремень снят");
        }
    }
}
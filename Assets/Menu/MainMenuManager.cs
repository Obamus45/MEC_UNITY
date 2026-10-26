using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Панели")]
    [SerializeField] private CanvasGroup mainMenuPanel;
    [SerializeField] private CanvasGroup selectionPanel;

    [Header("Сцены")]
    [SerializeField] private string assemblySceneName = "AssemblyScene"; // Имя сцены со сборкой

    [Header("Настройки перехода")]
    [SerializeField] private float transitionDuration = 0.3f;
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private bool isTransitioning = false;

    void Start()
    {
        // Гарантируем начальное состояние
        SetPanelState(mainMenuPanel, true);
        SetPanelState(selectionPanel, false);
    }

    // ========== КНОПКА "НАЧАТЬ СБОРКУ" ==========
    public void OnStartClicked()
    {
        if (isTransitioning) return;
        StartCoroutine(SwitchPanel(mainMenuPanel, selectionPanel));
    }

    // ========== КНОПКА "НАЗАД" ==========
    public void OnBackClicked()
    {
        if (isTransitioning) return;
        StartCoroutine(SwitchPanel(selectionPanel, mainMenuPanel));
    }

    // ========== КНОПКА "СБОРКА ГРМ" ==========
    public void OnSelectGRM()
    {
        if (isTransitioning) return;

        Debug.Log("[VR Trainer] Запуск сцены: " + assemblySceneName);

        // Плавное затемнение перед загрузкой (опционально)
        StartCoroutine(LoadSceneWithFade());
    }

    // ========== КНОПКА "ВЫХОД" ==========
    public void OnExitClicked()
    {
        Debug.Log("[VR Trainer] Выход из игры...");
        ExitGame();
    }

    // ========== ЗАГРУЗКА СЦЕНЫ ==========
    private IEnumerator LoadSceneWithFade()
    {
        isTransitioning = true;

        // Отключаем панели
        SetPanelState(mainMenuPanel, false);
        SetPanelState(selectionPanel, false);

        // Небольшая задержка для плавности
        yield return new WaitForSeconds(0.1f);

        // Загружаем сцену сборки
        SceneManager.LoadScene(assemblySceneName);
    }

    // ========== ВЫХОД ИЗ ИГРЫ ==========
    private void ExitGame()
    {
#if UNITY_EDITOR
        // В редакторе Unity
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // В билде
            Application.Quit();
#endif
    }

    // ========== ПЕРЕКЛЮЧЕНИЕ ПАНЕЛЕЙ ==========
    private IEnumerator SwitchPanel(CanvasGroup from, CanvasGroup to)
    {
        isTransitioning = true;

        from.interactable = false;
        from.blocksRaycasts = false;

        float elapsed = 0f;
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / transitionDuration);
            float curve = fadeCurve.Evaluate(t);

            from.alpha = 1f - curve;
            to.alpha = curve;

            yield return null;
        }

        from.alpha = 0f;
        to.alpha = 1f;
        SetPanelState(to, true);
        SetPanelState(from, false);

        isTransitioning = false;
    }

    private void SetPanelState(CanvasGroup panel, bool active)
    {
        panel.alpha = active ? 1f : 0f;
        panel.interactable = active;
        panel.blocksRaycasts = active;
    }
}
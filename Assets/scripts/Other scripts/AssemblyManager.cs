using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.SceneManagement;


public class AssemblyManager : MonoBehaviour
{
    public EngineRotate engineRotate;
    public GameObject documentationCanvas;
    public GameObject checklistCanvas;

    public GameObject completeText;
    public GameObject engineToRotate;

    public XRSocketInteractor beltSocket;
    public XRSocketInteractor fanSocket;

    public TextMeshProUGUI pulleyText;
    public TextMeshProUGUI beltText;
    public TextMeshProUGUI fanText;

    private int pulleyCount = 0;

    void Start()
    {
        beltSocket.enabled = false;
        fanSocket.enabled = false;

        UpdatePulleyText();
    }

    public void OnPulleyInstalled()
    {
        pulleyCount++;
        UpdatePulleyText();

        if (pulleyCount >= 3)
        {
            beltSocket.enabled = true;
            beltText.text = "[ ] Установить ремень ГРМ";
        }
    }

    public void OnBeltInstalled()
    {
        beltText.text = "[✓] Установить ремень ГРМ";
        fanSocket.enabled = true;
    }

    public void OnFanInstalled()
    {
        fanText.text = "[✓] Установить вентилятор";
        completeText.SetActive(true);

        engineRotate.StartRotation();
    }

    private void UpdatePulleyText()
    {
        if (pulleyCount >= 3)
            pulleyText.text = "[✓] Установить 3 шкива";
        else
            pulleyText.text = "[ ] Установить 3 шкива (" + pulleyCount + "/3)";
    }
    public void ReturnToDocumentation()
    {
        checklistCanvas.SetActive(false);
        documentationCanvas.SetActive(true);
    }
    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
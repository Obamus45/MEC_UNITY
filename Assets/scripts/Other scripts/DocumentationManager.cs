using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DocumentationManager : MonoBehaviour
{
    public TextMeshProUGUI slideText;

    public GameObject startButton;
    public GameObject checklistCanvas;
    public GameObject documentationCanvas;

    private int currentSlide = 0;

    private string[] slides =
    {
        "Слайд 1\n\nУзел ГРМ предназначен для синхронизации вращения коленчатого и распределительного валов.",

        "Слайд 2\n\nПеред сборкой необходимо установить три шкива в предусмотренные посадочные места.",

        "Слайд 3\n\nПосле установки шкивов производится монтаж ремня ГРМ.",

        "Слайд 4\n\nЗавершающим этапом является установка вентилятора на соответствующий шкив.",

        "Слайд 5\n\nПосле изучения инструкции можно приступить к сборке узла."
    };

    void Start()
    {
        checklistCanvas.SetActive(false);
        ShowSlide();
    }

    public void NextSlide()
    {
        if (currentSlide < slides.Length - 1)
        {
            currentSlide++;
            ShowSlide();
        }
    }

    public void PrevSlide()
    {
        if (currentSlide > 0)
        {
            currentSlide--;
            ShowSlide();
        }
    }

    void ShowSlide()
    {
        slideText.text = slides[currentSlide];

        if (currentSlide == slides.Length - 1)
            startButton.SetActive(true);
        else
            startButton.SetActive(false);
    }

    public void StartAssembly()
    {
        documentationCanvas.SetActive(false);
        checklistCanvas.SetActive(true);
    }
}
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Важно для доступа к сокетам
using System.Collections.Generic;

public class EngineAssemblyManager : MonoBehaviour
{
    public static EngineAssemblyManager Instance { get; private set; }

    [Header("Сокеты (заполнить в инспекторе!)")]
    public XRSocketInteractor SocketVRM; // Сокет для ремня
    public XRSocketInteractor SocketFan; // Сокет для вентилятора

    [Header("Состояние шкивов")]
    private bool shkiv1Installed = false;
    private bool shkiv2Installed = false;
    private bool shkiv3Installed = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // В начале сборки сокеты для Ремня и Вентилятора ЗАКРЫТЫ
        if (SocketVRM != null) SocketVRM.enabled = false;
        if (SocketFan != null) SocketFan.enabled = false;
    }

    // Этот метод вызывается из SnappablePart, когда деталь встала на место
    public void OnPartPlaced(string partID)
    {
        Debug.Log($"Деталь установлена: {partID}");

        // 1. Фиксируем установку шкивов
        if (partID == "Shkiv1") shkiv1Installed = true;
        if (partID == "Shkiv2") shkiv2Installed = true;
        if (partID == "Shkiv3") shkiv3Installed = true;

        // 2. Проверяем, все ли шкивы на месте?
        if (shkiv1Installed && shkiv2Installed && shkiv3Installed)
        {
            OpenSocketForVRM();
        }

        // 3. Если установлен Ремень (ГРМ), открываем Вентилятор
        if (partID == "GRM")
        {
            OpenSocketForFan();
        }

        // 4. Проверка финала
        if (partID == "Fan" || partID == "Vent")
        {
            Debug.Log("🎉 Сборка узла ГРМ и Вентилятора завершена!");
            // Тут можно запустить анимацию двигателя
        }
    }

    // Метод для снятия детали (если пользователь ошибся)
    public void OnPartRemoved(string partID)
    {
        Debug.Log($"Деталь снята: {partID}");

        // Если сняли Шкив -> закрываем Ремень (и Вентилятор, если он был)
        if (partID == "Shkiv1" || partID == "Shkiv2" || partID == "Shkiv3")
        {
            // Обновляем флаги
            if (partID == "Shkiv1") shkiv1Installed = false;
            if (partID == "Shkiv2") shkiv2Installed = false;
            if (partID == "Shkiv3") shkiv3Installed = false;

            // Если не все шкивы на месте -> закрываем сокет Ремня
            if (!shkiv1Installed || !shkiv2Installed || !shkiv3Installed)
            {
                if (SocketVRM != null) SocketVRM.enabled = false;

                // И обязательно закрываем Вентилятор тоже
                if (SocketFan != null) SocketFan.enabled = false;
            }
        }

        // Если сняли Ремень -> закрываем Вентилятор
        if (partID == "GRM")
        {
            if (SocketFan != null) SocketFan.enabled = false;
        }
    }

    private void OpenSocketForVRM()
    {
        if (SocketVRM != null && !SocketVRM.enabled)
        {
            SocketVRM.enabled = true;
            Debug.Log("🔓 Разблокирован сокет для установки Ремня ГРМ!");

            // Визуальный эффект (опционально): подсветить сокет
            // SocketVRM.GetComponent<Renderer>().material.color = Color.green; 
        }
    }

    private void OpenSocketForFan()
    {
        if (SocketFan != null && !SocketFan.enabled)
        {
            SocketFan.enabled = true;
            Debug.Log("🔓 Разблокирован сокет для установки Вентилятора!");
        }
    }
    public bool ArePulleysInstalled()
    {
        return shkiv1Installed && shkiv2Installed && shkiv3Installed;
    }
}

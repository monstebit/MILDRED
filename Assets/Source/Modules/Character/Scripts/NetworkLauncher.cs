using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Source.Modules.Character.Scripts
{
    public class NetworkLauncher : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI logsText;
        [SerializeField] private float fadeDuration; // Длительность анимации скрытия и показа
        [SerializeField] private float delayDuration; // Задержка перед показом следующего лога

        private Queue<string> logQueue = new Queue<string>();
        private Coroutine logUpdateCoroutine;

        // private void OnEnable()
        // {
        //     Application.logMessageReceived += HandleLog;
        // }
        //
        // private void OnDisable()
        // {
        //     Application.logMessageReceived -= HandleLog;
        // }

        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }
        
        //  ON TESTING
        public void StartNetworkAsClient()
        {
            NetworkManager.Singleton.StartClient();
        }

        // private void HandleLog(string logString, string stackTrace, LogType type)
        // {
        //     if (logString.Contains("Netcode") || logString.Contains("Network"))
        //     {
        //         logQueue.Enqueue($"{logString}");
        //         if (logUpdateCoroutine == null)
        //         {
        //             logUpdateCoroutine = StartCoroutine(UpdateLogText());
        //         }
        //     }
        // }
        //
        // private IEnumerator UpdateLogText()
        // {
        //     while (logQueue.Count > 0)
        //     {
        //         string logMessage = logQueue.Dequeue();
        //
        //         // Плавное скрытие текущего текста
        //         yield return StartCoroutine(FadeOutText());
        //
        //         logsText.text = ""; // Очистка текста после скрытия
        //
        //         yield return new WaitForSeconds(delayDuration); // Задержка перед показом нового лога
        //
        //         logsText.text = logMessage;
        //
        //         // Плавное появление нового текста
        //         yield return StartCoroutine(FadeInText());
        //
        //         // Задержка перед переходом к следующему логу
        //         yield return new WaitForSeconds(delayDuration);
        //     }
        //
        //     // Плавное скрытие последнего лога
        //     yield return StartCoroutine(FadeOutText());
        //
        //     logsText.alpha = 0;
        //
        //     logUpdateCoroutine = null; // Сброс корутины после завершения
        // }
        //
        // private IEnumerator FadeOutText()
        // {
        //     for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        //     {
        //         logsText.alpha = 1 - (t / fadeDuration);
        //         yield return null;
        //     }
        //     logsText.alpha = 0;
        // }
        //
        // private IEnumerator FadeInText()
        // {
        //     for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        //     {
        //         logsText.alpha = t / fadeDuration;
        //         yield return null;
        //     }
        //     logsText.alpha = 1;
        // }
    }
}
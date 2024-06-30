using TMPro;
using UnityEngine;
using Unity.Netcode;
using System.Collections;
using System.Collections.Generic;

namespace UU.MILDRED.Character
{
    public class NetworkLauncher : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI logsText;
        [SerializeField] private float fadeDuration = 0.2f; // Длительность анимации скрытия и показа
        [SerializeField] private float delayDuration = 0.2f; // Задержка перед показом следующего лога

        private Queue<string> logQueue = new Queue<string>();
        private Coroutine logUpdateCoroutine;

        private void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        public void StartNetworkAsHost()
        {
            NetworkManager.Singleton.StartHost();
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (logString.Contains("Netcode") || logString.Contains("Network"))
            {
                logQueue.Enqueue($"{logString}");
                if (logUpdateCoroutine == null)
                {
                    logUpdateCoroutine = StartCoroutine(UpdateLogText());
                }
            }
        }

        private IEnumerator UpdateLogText()
        {
            while (logQueue.Count > 0)
            {
                string logMessage = logQueue.Dequeue();

                // Плавное скрытие текущего текста
                for (float t = 0; t < fadeDuration; t += Time.deltaTime)
                {
                    logsText.alpha = 1 - (t / fadeDuration);
                    yield return null;
                }
                logsText.alpha = 0;
                logsText.text = ""; // Очистка текста после скрытия

                yield return new WaitForSeconds(delayDuration); // Задержка перед показом нового лога

                logsText.text = logMessage;

                // Плавное появление нового текста
                for (float t = 0; t < fadeDuration; t += Time.deltaTime)
                {
                    logsText.alpha = t / fadeDuration;
                    yield return null;
                }
                logsText.alpha = 1;

                // Задержка перед переходом к следующему логу
                yield return new WaitForSeconds(delayDuration);
            }

            // Плавное скрытие последнего лога
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                logsText.alpha = 1 - (t / fadeDuration);
                yield return null;
            }
            logsText.alpha = 0;

            logUpdateCoroutine = null;
        }
    }
}
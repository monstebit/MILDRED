using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.Modules.UI.Scripts.Model
{
    public class SceneLoader : MonoBehaviour
    {
        private int _worldSceneIndex = 1;
        
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void StartWork()
        {
            StopWork();

            StartCoroutine(LoadSceneAsync(_worldSceneIndex, LoadSceneMode.Single));
            // StartCoroutine(LoadSceneAsync(_worldSceneIndex));
        }
        
        public void StopWork()
        {
            StopCoroutine(LoadSceneAsync(_worldSceneIndex, LoadSceneMode.Single));
            // StopCoroutine(LoadSceneAsync(_worldSceneIndex));
        }
        
        private IEnumerator LoadSceneAsync(int index, LoadSceneMode mode)
        // private IEnumerator LoadSceneAsync(int index)
        {
            // AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index);
            // yield return null;
            
            // Запуск асинхронной загрузки сцены
            // AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, mode);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, mode);
            
            // Пока сцена не загружена, ожидание продолжения
            while (!asyncLoad.isDone)
            {
                // Здесь можно добавить отображение прогресса загрузки
                // Debug.Log($"Loading progress: {asyncLoad.progress * 100}%");
                yield return null;
            }
        }
    }
}
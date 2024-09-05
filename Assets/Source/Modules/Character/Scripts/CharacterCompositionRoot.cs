using UnityEngine;

namespace Source.Modules.Character.Scripts
{
    public class Character : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
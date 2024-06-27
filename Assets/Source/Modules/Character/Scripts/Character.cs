using UnityEngine;

namespace UU.MILDRED.Character.Source.Modules.Character.Scripts
{
    public class Character : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
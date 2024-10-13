using System;
using Source.Modules.Character.Scripts.Player.ManualTests;
using Source.Modules.Core;
using TMPro;
using UnityEngine;

namespace Source.Modules.UI.Scripts.ManualTests
{
    public class UIManager : CoreSingleton<UIManager>
    {
        [SerializeField] private TextMeshProUGUI playersInGameText;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            playersInGameText.text = $"Players in game: {PlayerManager.Instance.PlayersInGame}";
        }
    }
}
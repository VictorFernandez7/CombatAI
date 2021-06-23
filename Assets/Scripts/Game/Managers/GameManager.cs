using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CombatAI.Game.Managers
{
    public class GameManager : CombatAI.Utility.SingletonBehaviour<GameManager>
    {
        [Title("References")]
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private GameObject _player;
        [SerializeField] private GameObject _aI;

        public GameObject aI => _aI;
        public GameObject player => player;

        private bool _gamePaused;

        private void Update()
        {
            if (Input.GetButtonDown("Pause"))
                PauseGame();
        }

        private void PauseGame()
        {
            _gamePaused = !_gamePaused;
            Time.timeScale = _gamePaused ? 0f : 1f;
            _pauseMenu.SetActive(_gamePaused ? true : false);
        }
    }
}
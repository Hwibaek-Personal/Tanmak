using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using InGame.Logic;
using InGame.Map;
using InGame.Player;
using InGame.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace InGame
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;

        public static GameManager Instance
        {
            get => _instance;
            private set
            {
                if (_instance == null) _instance = value;
                else if (_instance != value) Destroy(_instance.gameObject);
            }
        }

        public Singleton<MapGenerator> mapG = new();
        public Singleton<PlayerManager> player = new();
        public Singleton<UIManager> uim = new();

        [SerializeField] private Key keyPrefab;
        public List<Key> keys;

        private int _found;

        public int Found
        {
            get => _found;
            set
            {
                _found = value;
                uim.I.SetKeyText(value);
                if (_found == 3)
                {
                    OpenDoor();
                }
            }
        }
        public float Timer { get; private set; }

        [SerializeField] private Door doorPrefab;
        [SerializeField] private Door door;

        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private GameObject endScene;
        [SerializeField] private Button endGame;

        public void End()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            endScene.SetActive(true);
            Timer = Time.timeSinceLevelLoad;
            timeText.text = $"Time : {Timer:F2}";
        }
        
        public void OpenDoor()
        {
            door.gameObject.SetActive(true);
        }

        [SerializeField] private CinemachineVirtualCamera cCam;
        public CinemachineVirtualCamera CCam => cCam;
        private void Awake()
        {
            Instance = this;
            
            GameData.Logic = Resources.Load<LogicSo>("GameData/GameInfo");
            GameData.PlayerLogic = Resources.Load<PlayerNormal>("GameData/PlayerInfo");
        }

        private async void Start()
        {
            await UniTask.WaitUntil(() => mapG && player);

            _found = 0;
            
            InstantiateKey(0);
            InstantiateKey(mapG.I.allTiles.Count - GameData.Logic.MapSize);
            InstantiateKey(mapG.I.allTiles.Count - 1);
            door = Instantiate(doorPrefab, mapG.I.centerPos, Quaternion.identity);
            door.gameObject.SetActive(false);
            
            player.I.Movement.Cc.enabled = false;
            player.I.transform.position = mapG.I.centerPos;
            player.I.Movement.Cc.enabled = true;
        }

        private void InstantiateKey(int index)
        {
            var key = Instantiate(keyPrefab, mapG.I.allTiles[index].center.position, Quaternion.identity);
            keys.Add(key);
        }
    }
}
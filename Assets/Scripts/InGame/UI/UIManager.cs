using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    public partial class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI keyText;
        [SerializeField] private Image compass;

        public List<Image> markers;

        private float _compassUnit;
        private void Start()
        {
            _compassUnit = compass.rectTransform.rect.width / 360f;
            GameManager.Instance.uim.I = this;
        }

        private void Update()
        {
            
            for (var index = 0; index < markers.Count; index++)
            {
                var img = markers[index];
                img.rectTransform.anchoredPosition = GetPosOnCompass(GameManager.Instance.keys[index]);
            }
        }

        public void SetKeyText(int cnt)
        {
            keyText.text = $"Found keys : {cnt}";
        }

        public Vector2 GetPosOnCompass(Key key)
        {
            var rawPTrns = GameManager.Instance.player.I.LookAround.CamArm;
            var pPos = new Vector2(rawPTrns.position.x, rawPTrns.position.z);
            var pFwd = new Vector2(rawPTrns.forward.x, rawPTrns.forward.z);

            var angle = Vector2.SignedAngle(key.Position - pPos, pFwd);
            
            return new Vector2(_compassUnit * angle, 0);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}

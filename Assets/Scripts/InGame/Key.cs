using System;
using UnityEngine;

namespace InGame
{
    public class Key : MonoBehaviour
    {
        public Vector2 Position => new(transform.position.x, transform.position.z);

        public void Get()
        {
            var index = GameManager.Instance.keys.IndexOf(this);
            GameManager.Instance.Found += 1;
            GameManager.Instance.keys.Remove(this);
            Destroy(GameManager.Instance.uim.I.markers[index]);
            GameManager.Instance.uim.I.markers.RemoveAt(index);
            Destroy(gameObject);
        }
    }
}
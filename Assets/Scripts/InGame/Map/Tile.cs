using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace InGame.Map
{
    public enum Dir
    {
        R30,
        R90,
        R150,
        R210,
        R270,
        R330
    }
    [Serializable]
    public class Wall
    {
        public Dir dir;
        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value; 
                col.gameObject.SetActive(value);
            }
        }

        public Collider col;

        public static implicit operator bool(Wall wall) => wall._isActive;
        public static implicit operator Collider(Wall wall) => wall.col;
    }
    public class Tile : MonoBehaviour
    {
        public Transform center;
        public Vector2Int cellCoordinates;
        public readonly Dictionary<Dir, Tile> Linked = new();
        public Wall[] walls = new Wall[6];

        public void Connect(Tile target)
        {
            if (!Linked.ContainsValue(target))
            {
                Debug.LogError($"from {gameObject.name} : Error - {target.name} is not linked!");
                return;
            }

            var dir = MapGenerator.GetDir(cellCoordinates - target.cellCoordinates);
            
            var myWall = walls.First(w => w.dir == dir);
            myWall.IsActive = false;

            var temp = ((int)dir + 3) % 6;
            var yourWall = target.walls.First(w => w.dir == (Dir)temp);
            yourWall.IsActive = false;
        }

        private void OnEnable()
        {
            foreach (var t in walls)
            {
                t.IsActive = true;
            }
        }
    }
}
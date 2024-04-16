using System;
using System.Collections.Generic;
using System.Linq;
using InGame.Logic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;
using Random = UnityEngine.Random;

namespace InGame.Map
{
    public partial class MapGenerator : MonoBehaviour
    {
        
        [Header("Generate Map")]
        public Tile tilePrefab;
        public Tilemap tileMap;

        public List<Tile>[] MappedTiles;
        public List<Tile> allTiles;
        public List<Tile> center;
        public Vector3 centerPos;

        public bool Fin { get; private set; }

        public static readonly Vector2Int[] Offset = {
            new (0, 1),
            new (1, 0),
            new (1, -1), 
            new (0, -1),
            new (-1, 0), 
            new (-1, 1)
        };

        [Header("Generate Maze")]
        
        public List<Tile> unvisited;

        private void Start()
        {
            GameManager.Instance.mapG.I = this;
            GenerateMap();
            LinkTiles();
            GenerateMaze();
            LinkCenter();
            SetCenterPos();
            Fin = true;
        }

        
        
        public IEnumerable<Tile> GetCenter()
        {
            var r = GameData.Logic.MapSize;

            return ((r - 1) % 3) switch
            {
                0 => new[] { allTiles[Tri(Center(r) - 1) + (int)Math.Ceiling(Center(r) / 2f)] },
                1 => new[]
                {
                    allTiles[Tri(Center(r - 1) - 1) + (int)Math.Ceiling(Center(r - 1) / 2f)],
                    allTiles[Tri(Center(r - 1)) + (int)Math.Ceiling(Center(r - 1) / 2f)],
                    allTiles[Tri(Center(r - 1)) + (int)Math.Ceiling(Center(r - 1) / 2f) + 1]
                },
                2 => new[]
                {
                    allTiles[Tri(Center(r - 2)) + (int)Math.Ceiling((Center(r - 2) + 1) / 2f)],
                    allTiles[Tri(Center(r - 2)) + (int)Math.Ceiling((Center(r - 2) + 1) / 2f) + 1],
                    allTiles[Tri(Center(r + 1) - 1) + (int)Math.Ceiling(Center(r + 1) / 2f)]
                },
                _ => throw new ArgumentOutOfRangeException()
            };

            int Center(int x) => (x * 2 + 1) / 3;

            int Tri(int x) => x * (x + 1) / 2;
        }
        
        private void GenerateMap()
        {
            MappedTiles ??= new List<Tile>[GameData.Logic.MapSize];
            allTiles ??= new List<Tile>();
            for (var i = 0; i < GameData.Logic.MapSize; i++)
            {
                MappedTiles[i] ??= new List<Tile>();
            }
            foreach (var tt in allTiles)
            {
                DestroyImmediate(tt);
            }
            var offset = 0;
            for (var i = 0; i < GameData.Logic.MapSize; i++)
            {
                for (var j = 0; j <= i; j++)
                {
                    var cellPos = new Vector3Int(j + offset,-i, 0);
                    var tile = Instantiate(tilePrefab, tileMap.CellToWorld(cellPos), quaternion.identity, transform);
                    tile.gameObject.name = $"Tile X = {j}, Y = {-i}";
                    tile.cellCoordinates = new Vector2Int(j, -i);
                    MappedTiles[i].Add(tile);
                    allTiles.Add(tile);
                }

                if (i % 2 == 0) offset -= 1;
            }
        }

        public static Dir GetDir(Vector2Int offset)
        {
            for (var i = 0; i < 6; i++)
            {
                if (offset == Offset[(i + 4) % 6])
                {
                    return (Dir)i;
                }
            }
            Debug.Log($"Error : Wrong input is {offset}");
            throw new ArgumentOutOfRangeException();
        }

        private void LinkTiles()
        {
            foreach (var tile in MappedTiles)
            {
                foreach (var t in tile)
                {
                    for (var i = 0; i < 6; i++)
                    {
                        var coordinates = t.cellCoordinates + Offset[i];
                        var target = allTiles.FirstOrDefault(tt => tt.cellCoordinates == coordinates);
                        if (target == null) continue;
                        var dir = GetDir(Offset[i]);
                        t.Linked.Add(dir, target);
                    }
                }
            }
        }

        private IEnumerable<int> GetRnd(int start, int end)
        {
            var who = Enumerable.Range(start, end).ToList();
            who.Shuffle();
            foreach (var i in who)
            {
                yield return i;
            }
        }
        
        private void GenerateMaze()
        {
            unvisited ??= new List<Tile>();
            foreach (var t in allTiles)
            {
                unvisited.Add(t);
            }
            
            var current = unvisited[Random.Range(0, unvisited.Count)];
            unvisited.Remove(current);
            
            while (unvisited.Count > 0)
            {
                var whobo = current.Linked.Where(pair => unvisited.Contains(pair.Value)).ToDictionary(pair => pair.Key, pair => pair.Value);

                if (whobo.Count > 0)
                {
                    foreach (var dir in GetRnd(0, 6).Select(i => (Dir)i))
                    {
                        if (!whobo.TryGetValue(dir, out var t)) continue;
                        current.Connect(t);
                        unvisited.Remove(t);
                        current = t;
                        break;
                    }
                    continue;
                }
                else
                {
                    if (unvisited.Count <= 0) break;
                    else
                    {
                        var whobo1 = unvisited.Select(t => t.Linked.FirstOrDefault(pair => !unvisited.Contains(pair.Value)).Value).Where(visited => visited != null).ToList();
                        if (whobo1.Count <= 0) return;
                        var @new = whobo1[Random.Range(0, whobo1.Count)];
                        var nextTo = @new.Linked.Where(pair => !unvisited.Contains(pair.Value)).ToList();
                        @new.Connect(nextTo[Random.Range(0, nextTo.Count)].Value);
                        unvisited.Remove(@new);
                        current = @new;
                    }
                }
            }
        }

        private void LinkCenter()
        {
            center = GetCenter().ToList();
            if (center.Count == 1) return;
            for (var i = 0; i < 3; i++)
            {
                center[i].Connect(center[(i + 1) % 3]);
            }
        }

        private void SetCenterPos()
        {
            var posses = center.Select(t => t.center.position);
            centerPos = posses.Aggregate(Vector3.zero, (current, pos) => current + pos) / center.Count;
        }
    }
}

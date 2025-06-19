using System;
using Models;
using Spawners;
using TMPro;
using UnityEngine;

namespace Views
{
    public class SpawnerView<T> : MonoBehaviour where T : SpawnedObject
    {
        [SerializeField] private Spawner<T> _spawner;
        [SerializeField] private TextMeshProUGUI _text;
        
        private void OnEnable()
        {
            _spawner.ChangedSpawnerInfo += UpdateData;
        }

        private void OnDisable()
        {
            _spawner.ChangedSpawnerInfo -= UpdateData;
        }

        private void UpdateData(SpawnerInfo spawnerData)
        {
            _text.text = $"{spawnerData.ObjectName}{Environment.NewLine}" +
                         $"Spawned: {spawnerData.SpawnedObjects}{Environment.NewLine}" +
                         $"Created: {spawnerData.CreatedObjects}{Environment.NewLine}" +
                         $"Active: {spawnerData.ActiveObjects}";
        }
    }
}
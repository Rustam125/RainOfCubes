using System;
using Controllers;
using Models;
using TMPro;
using UnityEngine;

namespace Views
{
    public class SpawnerView<T> : MonoBehaviour where T : MonoBehaviour
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
            _text.text = $"{typeof(T).Name}{Environment.NewLine}" +
                         $"Spawned: {spawnerData.SpawnedObjects}{Environment.NewLine}" +
                         $"Created: {spawnerData.CreatedObjects}{Environment.NewLine}" +
                         $"Active: {spawnerData.ActiveObjects}";
        }
    }
}
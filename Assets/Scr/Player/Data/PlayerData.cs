using System;
using System.Collections.Generic;
using Fusion;
using ObservableCollections;
using R3;
using Scr.Spell.Data;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Scr.Player.Data {

    public interface IPlayerData {
        
        IReadOnlyObservableList<ISpellData> Spells { get; }
        
        NetworkObject PlayerPrefab { get; }
        
        int TotalCost { get; }
        
        void AddSpell(ISpellData spell);
        
        void RemoveSpell(ISpellData spellData);
        
    }
    
    public class PlayerData : SerializedScriptableObject, IPlayerData {

        [OdinSerialize]
        [LabelText("使用するスペル")]
        private ObservableList<ISpellData> _spells;

        [OdinSerialize]
        [LabelText("プレイヤーのプレハブ")]
        private NetworkObject _playerPrefab;

        [OdinSerialize]
        [LabelText("総コスト")]
        private int _totalCost = 0;
        
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        
        public IReadOnlyObservableList<ISpellData> Spells => _spells;
        
        public NetworkObject PlayerPrefab => _playerPrefab;

        public int TotalCost => _totalCost;

        private void OnEnable() {
            _totalCost = CalculateTotalCost();
            
            RegisterSpellChanged();
        }

        private void OnDisable() {
            _disposables.Dispose();
            _disposables.Clear();
        }

        public void AddSpell(ISpellData spell) {

            if (spell is null) {
                return;
            }
            
            _spells.Add(spell);
        }
        
        public void RemoveSpell(ISpellData spellData) {

            if (spellData is null) {
                return;
            }
            
            _spells.Remove(spellData);
        }

        private void RegisterSpellChanged() {
            
            _spells
                .ObserveAdd()
                .Subscribe(_ => OnSpellChanged())
                .AddTo(_disposables);
            
            _spells
                .ObserveClear()
                .Subscribe(_ => OnSpellChanged())
                .AddTo(_disposables);
            
            _spells
                .ObserveRemove()
                .Subscribe(_ => OnSpellChanged())
                .AddTo(_disposables);
        }

        private void OnSpellChanged() {
            _totalCost = CalculateTotalCost();
        }
        
        private int CalculateTotalCost() {
            int result = 0;

            if (_spells.Count is 0) {
                return result;
            }

            foreach (var spell in _spells) {

                if (_spells is null) {
                    continue;
                }
                
                result += spell.Cost;
            }
            
            return result;
        }
    }
}
using CommonStructures;
using System.Collections.Generic;
using UnityEngine;

namespace Derby {

    using InControl;

    /**
     * Theory is that when a player registers we simply look through an array of size n, where n is between [1, 4].
     * To deregister simple remove the guid if the guid does not exist anymore and set the flag to false. The index 
     * of the array represents the player number implicitly.
     */
    [CreateAssetMenu(menuName = "Derby/Player Pool", fileName = "Player Pool")]
    public class PlayerPool : ScriptableObject {

        public int ActivePlayerCount { 
            get {
                var sum = 0;
                var i = 0;
                foreach (var element in players) {
                    sum = element.item2 ? sum + 1 : sum;
                    i++;
                }
                return sum;
            }
        }

        public int MaxPlayerCount {
            get {
                return maxPlayers;
            }
        }

        public IList<Tuple<System.Guid, bool>> Players {
            get {
                return System.Array.AsReadOnly(players);
            }
        }

        public int this[System.Guid key] {
            get {
                for (int i = 0; i < players.Length; i++) {
                    if (players[i].item1 == key) {
                        return i;
                    }
                }
                return -1;
            }
        }

        [SerializeField, Range(1, 4), Tooltip("The max number of players which can join locally.")]
        private int maxPlayers = 2;

        private Tuple<System.Guid, bool> defaultPlayer;
        private Tuple<System.Guid, bool>[] players;

        private void OnEnable() { 
            players = new Tuple<System.Guid, bool>[maxPlayers];

            defaultPlayer = Tuple<System.Guid, bool>.CreateTuple(System.Guid.Empty, false);

            for (int i = 0; i < maxPlayers; i++) {
                players[i] = defaultPlayer;
            }

            InputManager.OnDeviceAttached += RegisterPlayer; 
            InputManager.OnDeviceDetached += DeregisterPlayer;
        }

        private void OnDisable() {
            InputManager.OnDeviceAttached -= RegisterPlayer;
            InputManager.OnDeviceDetached -= DeregisterPlayer;
        }

#region Callbacks
        private void RegisterPlayer(InputDevice device) {
            var guid = device.GUID;
#if UNITY_EDITOR
            Debug.LogFormat("<color=#00ffffff>Attemping to registering: {0}</color>", guid);
#endif
            for (int i = 0; i < maxPlayers; i++) {
                if (!players[i].item2) {
                    // Default the tuple to contain true, meaning the player is active.
                    players[i] = Tuple<System.Guid, bool>.CreateTuple(guid, true);
#if UNITY_EDITOR
                    Debug.LogFormat("<color=#00ff00ff>Successfully registered player: {0} with id: {1}!</color>", guid, i);
#endif
                    return;
                }
            }
        }

        private void DeregisterPlayer(InputDevice device) {
            var guid = device.GUID;
#if UNITY_EDITOR
            Debug.LogFormat("<color=#00ffffff>Attemping to deregistering: {0}</color>", guid);
#endif
            for (int i = 0; i < maxPlayers; i++) {
                if (players[i].item1 == guid && players[i].item2) {
                    players[i] = defaultPlayer;
#if UNITY_EDITOR
                    Debug.LogFormat("<color=#ffa500ff>Successfully deregistered player: {0} with id: {1}</color>", guid, i);
#endif
                }
            }
        }
#endregion

#region Public Methods
        public void Initialize() {
            var devices = InputManager.Devices;
            for (int i = 0; i < devices.Count; i++) {
                var device = devices[i];
                Debug.LogFormat("<color=#00ffffff>Adding active device, {0}, with GUID: {1}</color>", device.Name, device.GUID);
                players[i] = Tuple<System.Guid, bool>.CreateTuple(device.GUID, true);
            }
        }
#endregion
    }
}

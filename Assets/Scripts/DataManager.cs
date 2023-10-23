using System.Collections.Generic;
using UnityEngine;

    public class DataManager : MonoBehaviour
    {
        // Singleton
        public static DataManager instance;
        
        // Global variables
        public struct PlayerConfig
        {
            float speed;
            float zoomSensitivty;
        }

        public struct SimulationConfig
        {
            float boundsSize;
        }

        public struct SimulationState
        {
            public List<Agent> agents;
        }
        
        public struct GameConfig
        {
            public PlayerConfig playerConfig;
            public SimulationConfig simulationConfig;
        }
        
        public struct GameState
        {
            public SimulationState simulationState;
        }
        
        public GameConfig gameConfig;
        public GameState gameState;
        
        void Start()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                throw new UnityException("Multiple managers in scene");
            }
        }
        
        void Update()
        {
            
        }
        
        string SerializeStruct<T>(T structToSerialize)
        {
            return JsonUtility.ToJson(structToSerialize);
        }
        
        T DeserializeStruct<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
        
        void SaveData(string data, string path)
        {
            System.IO.File.WriteAllText(path, data);
        }
        
        string LoadData(string path)
        {
            return System.IO.File.ReadAllText(path);
        }
        
        void SavePlayerConfig(PlayerConfig config)
        {
            var json = SerializeStruct(config);
            SaveData(json, "player_config.json");
        }
        
        PlayerConfig LoadPlayerConfig()
        {
            var json = LoadData("player_config.json");
            return DeserializeStruct<PlayerConfig>(json);
        }
        
        void SaveSimulationConfig(SimulationConfig config)
        {
            var json = SerializeStruct(config);
            SaveData(json, "simulation_config.json");
        }
        
        SimulationConfig LoadSimulationConfig()
        {
            var json = LoadData("simulation_config.json");
            return DeserializeStruct<SimulationConfig>(json);
        }
        
        void SaveSimulationState(SimulationState state)
        {
            var json = SerializeStruct(state);
            SaveData(json, "simulation_state.json");
        }
        
        SimulationState LoadSimulationState()
        {
            var json = LoadData("simulation_state.json");
            return DeserializeStruct<SimulationState>(json);
        }
    }

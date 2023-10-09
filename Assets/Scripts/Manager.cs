using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Manager : MonoBehaviour
    {
        public List<Agent> agents = new();

        private void Start()
        {
            throw new NotImplementedException();
        }
        
        private void Update()
        {
            throw new NotImplementedException();
        }
        
        public Agent SpawnAgent(AgentType type, Vector2 position)
        {
            throw new NotImplementedException();
        }
        
        public Agent SpawnAgent(AgentType type)
        {
            throw new NotImplementedException();
        }                                  
    }
}
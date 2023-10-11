using System;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance;
    
    public List<Agent> agents = new();

    private static float boundsSize = 20f;
    public Rect bounds = new(-boundsSize, -boundsSize, boundsSize * 2, boundsSize * 2);
    
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            throw new UnityException("Multiple managers in scene");
        }
        
        // Spawn 10 prey
        for (var i = 0; i < 10; i++)
        {
            SpawnAgent(AgentType.Prey);
        }
        
        // Spawn 1 predator
        SpawnAgent(AgentType.Predator);
    }
        
    private void Update()
    {
        // Spawn a new agent on spacebar press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnAgent(AgentType.Predator);
        }
    }
        
    public Agent SpawnAgent(AgentType type, Vector2 position)
    {
        var obj = new GameObject("Agent", typeof(Agent));
        
        var agent = obj.GetComponent<Agent>();
        
        agent.type = type;
        agent.transform.position = position;
        
        agents.Add(agent);
        
        return agent;
    }
        
    public Agent SpawnAgent(AgentType type)
    {
        return SpawnAgent(
            type,
            GetRandPosInBounds()
        );
    }             
    
    private Vector2 GetRandPosInBounds()
    {
        return new Vector2(
            UnityEngine.Random.Range(bounds.xMin, bounds.xMax),
            UnityEngine.Random.Range(bounds.yMin, bounds.yMax)
        );
    }
}
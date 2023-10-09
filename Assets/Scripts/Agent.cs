using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum AgentType
{
    Predator,
    Prey
}

public class Agent : MonoBehaviour
{
    AgentType type;
    
    float maxSpeed = 5f;
    float maxForce = 10f;
    
    float mass = 1f;
    float radius = 1f;

    float senseRange = 15f;
    List<Agent> sensedAgents = new();
    
    int generation = 0;
    
    int preyEaten = 0;
    int predatorsEaten = 0; // TODO: Implement this
    
    Rigidbody2D rb;
    BoxCollider2D collider;
    SpriteRenderer sr;
    
    // Start is called before the first frame update
    private void Start()
    {
        InitComponents();
        
        // Set the color of the agent based on its type
        switch (type)
        {
            case AgentType.Predator:
                SetColor(Color.red);
                break;
            case AgentType.Prey:
                SetColor(Color.green);
                break;
            default:
                SetColor(Color.white);
                throw new UnityException("Agent type not set");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /**
     * Initialize the components of the agent
     * We create components here so that we can easily add them to the game object
     * without having to manually add them in the editor
     */
    private void InitComponents()
    {
        rb = this.AddComponent<Rigidbody2D>();
        sr = this.AddComponent<SpriteRenderer>();
        collider = this.AddComponent<BoxCollider2D>();
        
        rb.gravityScale = 0f; // No gravity, we're looking top down. Into the microcosmos. Y axis is forward.
        rb.mass = mass;
        
        collider.size = new Vector2(radius, radius);
    }
    
    /**
     * Set the color of the agent
     * @param color The color to set
     */
    private void SetColor(Color color)
    {
        sr.color = color;
    }
    
    /**
     * Apply a force to the agent
     * @param force The force to apply
     */
    public void ApplyForce(Vector2 force)
    {
        rb.AddForce(force);
    }
    
    /**
     * Seek a target position
     * @param target The target position to seek
     */
    public void Seek(Vector2 target)
    {
        Vector2 desired = target - (Vector2)transform.position;
        desired.Normalize();
        desired *= maxSpeed;
        Vector2 steer = desired - rb.velocity;
        steer = Vector2.ClampMagnitude(steer, maxForce);
        ApplyForce(steer);
    }
    
    /**
     * Flee from a target position
     * @param target The target position to flee from
     */
    public void Flee(Vector2 target)
    {
        Vector2 desired = (Vector2)transform.position - target;
        desired.Normalize();
        desired *= maxSpeed;
        Vector2 steer = desired - rb.velocity;
        steer = Vector2.ClampMagnitude(steer, maxForce);
        ApplyForce(steer);
    }

    /**
     * Wander around
     */
    private void Wander()
    {
        // Both prey and predators wander around if they're not chasing or fleeing
    }

    /**
     * Sense other agents in the scene
     */
    private void Sense()
    {
        sensedAgents.Clear();
        sensedAgents.AddRange(Physics2D.OverlapCircleAll(transform.position, senseRange));
    }

    private void Behaviour()
    {
        switch (type)
        {
            case AgentType.Predator:
                break;
            
            case AgentType.Prey:
                break;
            
            default:
                throw new UnityException("Agent type not set");
        }
    }
    


}

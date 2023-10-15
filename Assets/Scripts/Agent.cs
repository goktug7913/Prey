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
    public AgentType type;
    
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
    private void Update()
    {
        Sense();
        Behaviour();
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
        
        sr.drawMode = SpriteDrawMode.Sliced;
        sr.size = new Vector2(radius, radius);
        sr.sprite = Resources.Load<Sprite>("Sprites/Triangle");
        
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
    private void ApplyForce(Vector2 force)
    {
        rb.AddForce(force);
        
        // Face the direction of movement
        transform.up = rb.velocity;
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
        // Wander is a combination of seek and flee
        // We seek a random position in front of us, and flee a random position behind us
        // This creates a wandering effect
        var seekTarget = (Vector2)transform.position + (Vector2)transform.up * 5f;
        // var fleeTarget = (Vector2)transform.position - (Vector2)transform.up * 5f;
        
        Seek(seekTarget);
        // Flee(fleeTarget);
    }

    /**
     * Sense other agents in detectable range
     */
    private void Sense()
    {
        sensedAgents.Clear();
        var colliders = Physics2D.OverlapCircleAll(transform.position, senseRange);
        
        foreach (var collider in colliders)
        {
            if (collider.gameObject == this.gameObject) continue;
            
            var agent = collider.GetComponent<Agent>();
            if (agent == null) continue;
            
            sensedAgents.Add(agent);
        }
    }

    public void Die()
    {
        Manager.instance.agents.Remove(this);
        Destroy(this.gameObject);
    }
    
    private Agent SpawnChild()
    {
        var child = Manager.instance.SpawnAgent(type, transform.position);
        child.generation = generation + 1;
        return child;
    }

    private void Eat(Agent target)
    {
        // Distance check
        if (
            Vector2.Distance(
                transform.position, 
                target.transform.position
                ) > radius + target.radius
            )
        {
            return;
        }
        
        switch (target.type)
        {
            case AgentType.Prey:
                preyEaten++;
                SpawnChild();
                break;
            case AgentType.Predator:
                predatorsEaten++;
                SpawnChild();
                break;
        }

        target.Die();
    }

    private void Behaviour()
    {
        switch (type)
        {
            case AgentType.Predator:
                // Chase and hunt closes prey, if no target, wander
                // Find closest prey
                var closestPrey = sensedAgents.Find(agent => agent.type == AgentType.Prey);
                if (closestPrey != null)
                {
                    Seek(closestPrey.transform.position);
                    Eat(closestPrey);
                }
                else
                {
                    Wander();
                }
                break;
            
            case AgentType.Prey:
                // Flee from closest predator, if no predator, wander
                // Find closest predator
                var closestPredator = sensedAgents.Find(agent => agent.type == AgentType.Predator);
                if (closestPredator != null)
                {
                    Flee(closestPredator.transform.position);
                }
                else
                {
                    Wander();
                }
                break;
            
            default:
                throw new UnityException("Agent type not set");
        }
    }
}

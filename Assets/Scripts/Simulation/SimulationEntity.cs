using UnityEngine;

public abstract class SimulationEntity : MonoBehaviour
{
    public Farm Farm { get; private set; }

    void Start()
    {
        Farm = GameObject.FindWithTag("GameManager").GetComponent<Farm>();
    }

    public virtual void PreTick() { }
    public abstract void Tick();
    public virtual void PostTick() { }
}

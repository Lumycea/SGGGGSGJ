using UnityEngine;

public class Simulation : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SimulationTime = 0;
        timeBeforeNextTick = TickTime;
    }

    // Update is called once per frame
    void Update()
    {
        timeBeforeNextTick -= Time.deltaTime;

        while (timeBeforeNextTick < 0)
        {
            doTick();

            SimulationTime += 1;
            timeBeforeNextTick += TickTime;
        }
    }

    void doTick()
    {
        var simEntities = FindObjectsByType<SimulationEntity>(FindObjectsSortMode.None);

        foreach (var e in simEntities)
        {
            e.PreTick();
        }
        foreach (var e in simEntities)
        {
            e.Tick();
        }
        foreach (var e in simEntities)
        {
            e.PostTick();
        }
    }

    public int SimulationTime { get; private set; }

    [SerializeField] private int TickTime = 2;
    private float timeBeforeNextTick;
}

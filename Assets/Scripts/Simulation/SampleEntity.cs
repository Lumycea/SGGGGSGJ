using UnityEngine;
using UnityEngine.SceneManagement;

public class SampleEntity : SimulationEntity
{
    public override void Tick()
    {
        Farm.AddItem(FarmItem.Sugar, 2);
    }
}

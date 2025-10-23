using UnityEngine;

/*
    This is a test tool. 
    This tool will play a particle effect!
*/
[RequireComponent(typeof(ParticleSystem))]
public class TestTool2 : BaseTool
{
    [Header("Runtime")]
    private ParticleSystem ps;

    public override void Activate()
    {
        ToolActivate();
    }

    public override void Instantiate()
    {
        ps = GetComponent<ParticleSystem>();

        if (ps == null)
        {
            Debug.LogError($"ParticleSystem not found on {gameObject.name}");
        }
        else
        {
            Debug.Log($"The Tool {gameObject.name} is initialized.");
        }
    }

    private void ToolActivate()
    {
        if (ps == null) return;

        ps.Play();
        Debug.Log($"{gameObject.name} particle effect played!");
    }
}

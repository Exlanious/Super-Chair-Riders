using Unity.VisualScripting;
using UnityEngine;

public class ParticleGenerator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private ParticleSystem _particleSystem;


    [Header("Debug")]
    [SerializeField] private bool debugMode = false;


    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            PlayParticles();
    }

    public void PlayParticles()
    {
        if (_particleSystem == null)
        {
            if (debugMode)
                Debug.Log($"Particle System not attached on object: {name}");
            return;
        }

        if (_particleSystem.isPlaying)
        {
            _particleSystem.Stop();
        }
        _particleSystem?.Play();
    }

    public void StopParticles()
    {
        if (_particleSystem == null)
        {
            if (debugMode)
                Debug.Log($"Particle System not attached on object: {name}");
            return;
        }

        if (_particleSystem.isPlaying)
        {
            _particleSystem.Stop();
        }
    }
    /* use this if you want to manually control particle generation based on player velocity
        [Header("Velocity Based Particle Generation")]
        int occurAFterVelocity;
        float dustFormationPeriod;

        Rigidbody2D playerrb;

        float counter;

        private void Update() {
            counter += Time.deltaTime;
            if (Mathf.Abs(playerrb.velocity.x) > occurAFterVelocity && counter >= dustFormationPeriod) {
                PlayParticles();
                counter = 0;
            }
        }
        */
}

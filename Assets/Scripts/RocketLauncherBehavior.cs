using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherBehavior : MonoBehaviour
{
    [SerializeField] private GameObject _launchPoint;
    [SerializeField] private GameObject _rocketPrefab;
    [SerializeField] private GameObject _smokePuff; // reference smoke particle system

    [ContextMenu("TestFire")]
    public void FireRocket()
    {
        GameObject rocket = Instantiate(_rocketPrefab,_launchPoint.transform.position,_launchPoint.transform.rotation);
        rocket.GetComponent<RocketBehavior>().Launch();


        // play smoke puff effect
        if (_smokePuff != null)
        {
            _smokePuff.transform.position = _launchPoint.transform.position; // set vfx position
            _smokePuff.transform.rotation = _launchPoint.transform.rotation; // set vfx rotation
            _smokePuff.SetActive(true); // enable  smoke GameObject

            ParticleSystem smokeParticles = _smokePuff.GetComponent<ParticleSystem>();
            if (smokeParticles != null)
            {
                smokeParticles.Play();
            }
            else
            {
                Debug.LogWarning("No Particle System found on _smokePuff GameObject!", this);
            }
        }
        else
        {
            Debug.LogWarning("Smoke Puff GameObject is not assigned!", this);
        }
    }
}


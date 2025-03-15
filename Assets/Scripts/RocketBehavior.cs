using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehavior : MonoBehaviour
{
    [SerializeField] float _speed = 5f;
    [SerializeField] float _timer = 2f;
    [SerializeField] GameObject _explosionPrefab;
    [SerializeField] private AudioClip _explosionSound; 
    private bool _isFired = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyTimer());
    }

    // Update is called once per frame
    void Update()
    {
        if (_isFired) 
        { 
           transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }
    }

    public void Launch()
    {
        _isFired = true;
    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(_timer);
        DestroyThisWithExplosion();
    }

    private void OnTriggerEnter(Collider other)
    {
        print("test");
        if (other.CompareTag("Target") || other.CompareTag("Ground"))
        { 
           DestroyThisWithExplosion();
        }
    }

    void DestroyThisWithExplosion()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        // create a new GameObject for explosion sound
        GameObject soundObject = new GameObject("ExplosionSound");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        // configure new AudioSource
        audioSource.clip = _explosionSound;
        audioSource.Play();

        // destroy sound clip 
        Destroy(soundObject, _explosionSound.length);

        // destroy rocket immediately
        Destroy(gameObject);
    }
}

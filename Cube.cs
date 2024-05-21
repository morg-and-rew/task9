using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    [SerializeField] private List<Color> _colors;
    [SerializeField] private int _initialColorIndex = 0;

    private Vector3 _initialPosition;
    private bool _canInteract = true;
    private Renderer _renderer;

    public int lowerBound;
    public int upperBound;
    public int defaultValueY;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();

        if (_colors.Count == 0)
        {
            throw new InvalidOperationException("Colors list is empty.");
        }

        _renderer.material.color = _colors[_initialColorIndex];
        _initialPosition = transform.position;
    }

    private void OnEnable()
    {
        _renderer.material.color = _colors[_initialColorIndex];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Plane plane) && _canInteract)
        {
            _canInteract = false;
            StartCoroutine(DelayRelease());

            int randomIndex = UnityEngine.Random.Range(0, _colors.Count);
            _renderer.material.color = _colors[randomIndex];
        }
    }

    public void Deactivate()
    {
        transform.position = _initialPosition;
        transform.position = new Vector3(UnityEngine.Random.Range(lowerBound, upperBound), defaultValueY, UnityEngine.Random.Range(lowerBound, upperBound));
    }

    public void Activate()
    {
        _canInteract = true;
    }

    private IEnumerator DelayRelease()
    {
        float minDelay = 2f;
        float maxDelay = 6f;
        float delay = UnityEngine.Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(delay);
        Deactivate();
        Activate();
    }
}
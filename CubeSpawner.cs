using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private float _repeatRate = 7f;
    [SerializeField] private int _poolMaxSize = 10;
    [SerializeField] private int _poolCapacity = 5;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (obj) =>
            {
                obj.Activate();
            },
            actionOnRelease: (obj) =>
            {
                obj.Deactivate();
            },
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize,
            actionOnDestroy: (obj) => Destroy(obj));
    }

    private void Start()
    {
        StartCoroutine(SpawnCubesRepeatedly());
    }

    private IEnumerator SpawnCubesRepeatedly()
    {
        while (true)
        {
            _pool.Get();
            yield return new WaitForSeconds(_repeatRate);
        }
    }
}

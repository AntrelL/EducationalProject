using UnityEngine;

public class PlacesVisitor : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private Transform _placesStorage;

    private Transform[] _places;
    private int _currentPlaceNumber;

    private void Start()
    {
        _places = _placesStorage.GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        Transform currentPlace = _places[_currentPlaceNumber];

        transform.position = Vector3.MoveTowards(transform.position,
            currentPlace.position, _movementSpeed * Time.deltaTime);

        if (transform.position == currentPlace.position)
            NextPlace();
    }

    private Vector3 NextPlace()
    {
        _currentPlaceNumber++;

        if (_currentPlaceNumber == _places.Length)
            _currentPlaceNumber = 0;

        Vector3 currentPlacePosition = _places[_currentPlaceNumber].position;
        transform.forward = currentPlacePosition - transform.position;

        return currentPlacePosition;
    }
}
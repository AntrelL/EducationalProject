using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Platformer
{
    public class Navigator : MonoBehaviour
    {
        [SerializeField] private Transform _route;

        private List<Waypoint> _waypoints;

        public Waypoint CurrentTargetPoint { get; private set; }

        private void Start()
        {
            _waypoints = new List<Waypoint>(_route.GetComponentsInChildren<Waypoint>());

            CurrentTargetPoint = _waypoints.Aggregate((waypoint1, waypoint2) =>
                Vector2.Distance(waypoint1.Position, transform.position) <
                Vector2.Distance(waypoint2.Position, transform.position)
                ? waypoint1 : waypoint2);
        }

        public void OnTouchingWithPoint(Waypoint waypoint)
        {
            if (waypoint == CurrentTargetPoint)
                SelectNextWaypoint();
        }

        private void SelectNextWaypoint()
        {
            int currentWaypointIndex = _waypoints.IndexOf(CurrentTargetPoint);

            if (currentWaypointIndex == _waypoints.Count - 1)
                CurrentTargetPoint = _waypoints[0];
            else
                CurrentTargetPoint = _waypoints[currentWaypointIndex + 1];
        }
    }
}

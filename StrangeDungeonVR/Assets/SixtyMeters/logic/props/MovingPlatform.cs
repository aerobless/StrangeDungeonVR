using System;
using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Core.Utils;
using SixtyMeters.logic.generator;
using SixtyMeters.logic.player;
using SixtyMeters.logic.traps;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.props
{
    public class MovingPlatform : MonoBehaviour
    {
        [System.Serializable]
        public class RouteSegment
        {
            [Tooltip("The platform moves from waypoint A to B")]
            public WayPoint waypointA;

            [Tooltip("The platform moves from waypoint A to B")]
            public WayPoint waypointB;

            [Tooltip("The time it should take to move from A to B")]
            public float movementTime;

            [Tooltip("The time the platform should wait at waypoint B")]
            public float waitAtStopTime;
        }

        [Tooltip("The platform travels along the route. After the last segment it teleports to the first")]
        public List<RouteSegment> route = new();

        // Internals
        private int _currentSegmentNumber = 0;
        private bool _platformIsActive = true;
        private Rigidbody _rigidbody;
        float _time = 0;
        private GameObject _realPlayerParent;

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = gameObject.GetRigidbody();
            gameObject.transform.position = route[0].waypointA.GetPosition();
        }

        private void StartMoving()
        {
            _platformIsActive = true;
            _currentSegmentNumber = 0;
        }

        private void StopMoving()
        {
            _platformIsActive = false;
        }

        private void FixedUpdate()
        {
            if (_platformIsActive)
            {
                var incrementalMovement = Vector3.Lerp(
                    route[_currentSegmentNumber].waypointA.GetPosition(),
                    route[_currentSegmentNumber].waypointB.GetPosition(),
                    _time / route[_currentSegmentNumber].movementTime);
                _rigidbody.MovePosition(incrementalMovement);

                _time += Time.deltaTime;

                if (gameObject.transform.position == route[_currentSegmentNumber].waypointB.GetPosition())
                {
                    _platformIsActive = false;
                    StartCoroutine(Helper.Wait(route[_currentSegmentNumber].waitAtStopTime,
                        () => { _platformIsActive = true; }));

                    _currentSegmentNumber++;
                    _time = 0;

                    // Reset segment number if we're at the end of the list
                    if (_currentSegmentNumber > (route.Count - 1))
                    {
                        _currentSegmentNumber = 0;
                        gameObject.transform.position = route[0].waypointA.transform.position;
                    }
                }
            }
        }

        public void AttachPlayerToPlatform(Collider other)
        {
            if (other.GetComponent<TriggerCollider>())
            {
                other.GetComponentInParent<PlayerActor>().AttachPlayerToGameObject(gameObject);
            }
        }

        public void DetachPlayerFromPlatform(Collider other)
        {
            if (other.GetComponent<TriggerCollider>())
            {
                other.GetComponentInParent<PlayerActor>().RestorePlayerAttachmentToDefault();
            }
        }
    }
}
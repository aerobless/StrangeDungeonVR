using System.Collections.Generic;
using SixtyMeters.logic.ai;
using SixtyMeters.logic.interfaces;
using SixtyMeters.logic.interfaces.lifecycle;
using SixtyMeters.logic.utilities;
using UnityEngine;

namespace SixtyMeters.logic.generator.randomization
{
    public class AgentSpawner : MonoBehaviour, IRandomizeable, IDestructionListener
    {
        [System.Serializable]
        public class SpawnableAgent
        {
            public GameObject agent;
            public string templateId;
            public float heightOffset;
        }

        // Settings
        [Tooltip("The agents that could be spawned")]
        public List<SpawnableAgent> spawnableAgents;

        [Tooltip("When should the agent be spawned")]
        public SpawnTime spawnTime;

        [Tooltip("How many agents can be spawned to be alive at the same time")]
        public int maxAliveAgents = 1;

        // Internal
        private readonly List<GameObject> _spawnedAgents = new();

        // Start is called before the first frame update
        void Start()
        {
            if (spawnableAgents.Count == 0)
            {
                Debug.LogError("Spawner " + name + " without any spawnableAgents");
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void Randomize()
        {
            if (spawnTime == SpawnTime.OnRandomization)
            {
                Spawn();
            }
        }

        /// <summary>
        /// Spawn a random agent on this spawner
        /// </summary>
        public void Spawn()
        {
            var randomFromList = Helper.GETRandomFromList(spawnableAgents);
            SpawnSpecific(randomFromList);
        }

        /// <summary>
        ///  Spawn a specific agent on this spawner. You can get a list of available agents via spawnableAgents
        /// </summary>
        /// <param name="agent">the agent to be spawned</param>
        private void SpawnSpecific(SpawnableAgent agent)
        {
            if (_spawnedAgents.Count < maxAliveAgents)
            {
                agent.agent.GetComponentInChildren<UniversalAgent>().agentTemplateId = agent.templateId;
                var spawnedAgent = InstantiateWithOriginalParent(agent, transform);
                spawnedAgent.GetComponentInChildren<ITrackedLifecycle>().RegisterDestructionListener(this);
                _spawnedAgents.Add(spawnedAgent);
            }
        }

        private GameObject InstantiateWithOriginalParent(SpawnableAgent sAgent, Transform currentTransform)
        {
            var instantiatedObj = Instantiate(sAgent.agent,
                currentTransform.position + new Vector3(0, sAgent.heightOffset, 0), currentTransform.rotation);
            instantiatedObj.transform.parent = transform.parent;
            return instantiatedObj;
        }

        void OnDrawGizmos()
        {
            // Location indicator
            Gizmos.color = Color.green;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, new Vector3(0.2f, 0.2f, 0.2f));

            // Direction indicator
            Gizmos.color = Color.red;
            Gizmos.DrawCube(Vector3.zero + new Vector3(0, 0, 0.1f), new Vector3(0.05f, 0.05f, 0.05f));
        }

        public void ObjectDestroyed(GameObject trackedObject)
        {
            _spawnedAgents.Remove(trackedObject);
            Debug.Log("Removed tracked object " + _spawnedAgents.Count);
        }
    }

    public enum SpawnTime
    {
        OnRandomization,
        OnDemand
    }
}
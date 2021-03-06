using System.Collections.Generic;
using SixtyMeters.logic.ai.appearance;
using UnityEngine;

namespace SixtyMeters.logic.ai
{
    public class AgentManager : MonoBehaviour
    {
        [System.Serializable]
        public class AgentTemplate
        {
            public string agentConfigurationId;
            public string name;
            public int level;
            public Skin skin;
            public MoveSet moveSet;
            public float agentRoamMaxMovementSpeed;
            public float agentAttackMaxMovementSpeed;
            public float damagePerHit; //TODO: make attacks configurable and assign damage per attack instead
            public bool hasWeapon;
            public int healthPoints;
            public float initialHealthPercentage; // 0 - 1
        }

        [System.Serializable]
        public class AttackTemplate
        {
        }

        public List<AgentTemplate> agentTemplates = new();

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public AgentTemplate GetTemplate(string templateId)
        {
            return agentTemplates.Find(template => template.agentConfigurationId.Equals(templateId));
        }
    }
}
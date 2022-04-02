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
            public Skin skin;
            public MoveSet moveSet;
            public float agentMaxSpeed;
            public bool hasWeapon;
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
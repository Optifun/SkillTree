﻿using System.Collections.Generic;
using System.Linq;
using SkillTree.SkillEditing;
using SkillTree.StaticData.Skills;

namespace SkillTree.Utils
{
    public static class SkillMappings
    {
        public static SkillGraph ToDTO(this SkillGraphEditor editor)
        {
            List<SkillDefinition> skills = editor.Skills.Select(ToDTO).ToList();
            List<SkillConnection> connections = editor.Connections.Select(ToDTO).ToList();
            return new SkillGraph
            {
                    Name = editor.TreeName,
                    BaseSkill = editor.BaseSkill.Id,
                    Skills = skills,
                    Connections = connections
            };
        }

        public static SkillConnection ToDTO(this SkillConnectionEditor editor)
        {
            return new SkillConnection(editor.Source.Id, editor.Target.Id);
        }

        public static SkillDefinition ToDTO(this SkillDefinitionEditor skill)
        {
            return new SkillDefinition(skill.Id,
                    skill.Name,
                    skill.transform.localPosition.ToVector().ToVector2(),
                    skill.Cost);
        }
    }
}
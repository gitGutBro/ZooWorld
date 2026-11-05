using System.Collections.Generic;
using NSubstitute;
using UnityEditor;
using UnityEngine;
using ZooWorld.Animals.Core;
using ZooWorld.Animals.FoodChain.Bounce;
using ZooWorld.Animals.FoodChain.Duels;
using ZooWorld.Animals.FoodChain.Interaction;
using ZooWorld.Animals.FoodChain.Predation;
using ZooWorld.Animals.FoodChain.Roles;
using ZooWorld.Animals.Movement.Strategies;
using ZooWorld.Infrastructure;
using ZooWorld.World.Obstacles.Configuration;
using ZooWorld.World.Obstacles.Runtime;

namespace ZooWorld.Tests
{
    internal static class TestData
    {
        private const string DietField = "_diet";
        private const string DisplayNameField = "_displayName";
        private const string BodyField = "_body";
        private const string SpawnEntriesField = "_spawn._entries";
        private const string SpawnAnimalField = "_animal";
        private const string SpawnWeightField = "_weight";
        private const string FootprintField = "_footprintRadius";
        private const string ObstacleEntriesField = "_entries";
        private const string ObstaclePrefabField = "_prefab";
        private const string ObstacleWeightField = "_weight";

        public static FoodChainRole CreateRole(string name)
        {
            FoodChainRole role = ScriptableObject.CreateInstance<FoodChainRole>();
            SerializedObject serialized = new SerializedObject(role);
            serialized.FindProperty(DisplayNameField).stringValue = name;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            return role;
        }

        public static void SetDiet(FoodChainRole role, params FoodChainRole[] diet)
        {
            SerializedObject serialized = new SerializedObject(role);
            SerializedProperty property = serialized.FindProperty(DietField);
            property.arraySize = diet.Length;

            for (int i = 0; i < diet.Length; i++)
                property.GetArrayElementAtIndex(i).objectReferenceValue = diet[i];

            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        public static IAnimal CreateAnimal(FoodChainRole role)
        {
            IAnimal animal = Substitute.For<IAnimal>();
            animal.Role.Returns(role);
            animal.IsAlive.Returns(true);
            animal.When(a => a.Kill()).Do(_ => animal.IsAlive.Returns(false));
            return animal;
        }

        public static Animal CreateLiveAnimal()
        {
            GameObject gameObject = new GameObject(nameof(Animal));
            Rigidbody body = gameObject.AddComponent<Rigidbody>();
            Animal animal = gameObject.AddComponent<Animal>();

            SerializedObject serialized = new SerializedObject(animal);
            serialized.FindProperty(BodyField).objectReferenceValue = body;
            serialized.ApplyModifiedPropertiesWithoutUndo();

            animal.Initialize(config: null, Substitute.For<IMovementStrategy>());
            return animal;
        }

        public static void SetSpawnEntries(GameConfig config, params (AnimalConfig Animal, float Weight)[] entries)
        {
            SerializedObject serialized = new SerializedObject(config);
            SerializedProperty property = serialized.FindProperty(SpawnEntriesField);
            property.arraySize = entries.Length;

            for (int i = 0; i < entries.Length; i++)
            {
                SerializedProperty entry = property.GetArrayElementAtIndex(i);
                entry.FindPropertyRelative(SpawnAnimalField).objectReferenceValue = entries[i].Animal;
                entry.FindPropertyRelative(SpawnWeightField).floatValue = entries[i].Weight;
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        public static Obstacle CreateObstacle(float footprintRadius)
        {
            Obstacle obstacle = new GameObject(nameof(Obstacle)).AddComponent<Obstacle>();

            SerializedObject serialized = new SerializedObject(obstacle);
            serialized.FindProperty(FootprintField).floatValue = footprintRadius;
            serialized.ApplyModifiedPropertiesWithoutUndo();

            return obstacle;
        }

        public static void SetObstacleEntries(ObstacleFieldConfig config, params (Obstacle Prefab, float Weight)[] entries)
        {
            SerializedObject serialized = new SerializedObject(config);
            SerializedProperty property = serialized.FindProperty(ObstacleEntriesField);
            property.arraySize = entries.Length;

            for (int i = 0; i < entries.Length; i++)
            {
                SerializedProperty entry = property.GetArrayElementAtIndex(i);
                entry.FindPropertyRelative(ObstaclePrefabField).objectReferenceValue = entries[i].Prefab;
                entry.FindPropertyRelative(ObstacleWeightField).floatValue = entries[i].Weight;
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        public static IReadOnlyList<IInteractionRule> CreateRules(
            IDuelResolver duel,
            IPredationService predation,
            IBounceService bounce)
        {
            return new IInteractionRule[]
            {
                new MutualPredationRule(duel, predation),
                new PredationRule(predation),
                new BounceRule(bounce),
            };
        }
    }
}

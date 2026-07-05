using NUnit.Framework;
using System.Reflection;
using UnityEngine;
using UnityEngine.TestTools;

namespace HexagonalConstructor.Tests
{
    public class GridGenerationIntegrationTests
    {
        private GameObject testContextObject;
        private GridGenerator gridGenerator;
        private GeneratorContext context;
        private GridSettings gridSettings;
        private GenerationSettings generationSettings;

        [SetUp]
        public void Setup()
        {
            testContextObject = new GameObject("Test_HexGridSystem");

            context = testContextObject.AddComponent<GeneratorContext>();
            gridSettings = testContextObject.AddComponent<GridSettings>();
            generationSettings = testContextObject.AddComponent<GenerationSettings>();
            gridGenerator = testContextObject.AddComponent<GridGenerator>();

            var mockPrefab = new GameObject("Mock_Hex_Prefab");
            SetPrivateField(gridSettings, "hexPrefab", mockPrefab);
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(testContextObject);
        }

        [Test]
        public void System_WithValidShapeGenerator_SpawnsHexagonsSuccessfully()
        {
            var shapeGenInstance = new RectangleGenerator();

            SetPrivateField(generationSettings, "shapeGenerator", shapeGenInstance);
            SetPrivateField(generationSettings, "generationMode", GenerationMode.Shapes);

            gridGenerator.Generate();

            Assert.IsTrue(gridGenerator.transform.childCount > 0, "Grid generator failed to spawn any hex prefabs!");
        }

        [Test]
        public void GuardClause_WithNullGenerator_FailsGracefullyWithoutCrashing()
        {
            SetPrivateField(generationSettings, "shapeGenerator", null);
            SetPrivateField(generationSettings, "randomizedGenerator", null);
            SetPrivateField(generationSettings, "generationMode", GenerationMode.Shapes);

            var errorRegex = new System.Text.RegularExpressions.Regex("Generation Failed!.*Generator Type");
            LogAssert.Expect(LogType.Error, errorRegex);

            Assert.DoesNotThrow(() => gridGenerator.Generate(), "GridGenerator crashed with a hard exception instead of bailing out safely!");
        }

        [Test]
        public void GuardClause_WithNullPrefab_FailsGracefullyWithoutCrashing()
        {
            var shapeGenInstance = new RhombusGenerator();
            SetPrivateField(generationSettings, "shapeGenerator", shapeGenInstance);
            SetPrivateField(generationSettings, "generationMode", GenerationMode.Shapes);
            SetPrivateField(gridSettings, "hexPrefab", null);

            var prefabRegex = new System.Text.RegularExpressions.Regex("Generation Failed!.*Hex Prefab");
            LogAssert.Expect(LogType.Error, prefabRegex);

            Assert.DoesNotThrow(() => gridGenerator.Generate(), "System crashed on a missing prefab instead of handling it with a guard clause!");
        }



        private void SetPrivateField(object targetObject, string fieldName, object value)
        {
            FieldInfo field = targetObject.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(targetObject, value);
            }
            else
            {
                System.Type type = targetObject.GetType();
                while (type != null && field == null)
                {
                    field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                    type = type.BaseType;
                }

                if (field != null) field.SetValue(targetObject, value);
                else Assert.Fail($"Test Setup Error: Could not find private field '{fieldName}' on type {targetObject.GetType().Name}. Check variable spelling!");
            }
        }
    }
}

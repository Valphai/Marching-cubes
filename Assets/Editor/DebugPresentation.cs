// using UnityEngine;
// using UnityEditor;

// [CustomEditor(typeof(Generator))]
// public class DebugPresentation : Editor
// {
//     private DrawChunkDetails chunkDetails;
//     private Generator generator;
//     // private List<Vector4> cubeGizmos = new List<Vector4>();

//     private void OnEnable() 
//     {
//         generator = (Generator)target;

//         generator.CreateEssentials();
//         chunkDetails = generator.chunkDetails;
//     }
//     public override void OnInspectorGUI() 
//     {
//         base.OnInspectorGUI();

//         EditorGUI.BeginChangeCheck();

//         bool drawCube = GUILayout.Toggle(chunkDetails.DrawCube, "Draw marching cube");
//         if (drawCube != chunkDetails.DrawCube)
//         {
//             chunkDetails.DrawCube = drawCube;
//         }

//         bool drawDensities = GUILayout.Toggle(chunkDetails.DrawDensities, "Draw vertex densities");
//         if (drawDensities != chunkDetails.DrawDensities)
//         {
//             chunkDetails.DrawDensities = drawDensities;
//         }

//         if (EditorGUI.EndChangeCheck())
//             SceneView.RepaintAll();
//     }
//     private void OnSceneGUI() 
//     {
//         Handles.DrawWireCube((Generator.Cube.corners[1] + Generator.Cube.corners[7]) / 2, Vector3.one);
//     }
// }

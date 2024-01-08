using BepInEx;
using HarmonyLib;
using System.IO;
using System;
using UnityEngine;

namespace LethalJojo
{
    public class StoneMaskPatch : MonoBehaviour
    {
        private static Mesh _stoneMaskMesh = null;

        public StoneMaskPatch() 
        {
            Console.WriteLine("It isn't done.");

            try
            {
                string folderPath = Path.GetDirectoryName(Mod.DLLPath);
                var asssetBundle = AssetBundle.LoadFromFile(Path.Combine(folderPath, "LethalJojoAssetBundle"));

                if (asssetBundle != null)
                {
                    _stoneMaskMesh = asssetBundle.LoadAsset<Mesh>("StoneMask_Model");
                    Console.WriteLine($"Loaded mask from Asset Bundle in Directory {Path.Combine(folderPath, "LethalJojoAssetBundle")}.");
                }
                else 
                {
                    Console.WriteLine("Failed loading mask you numpty!");
                }
            }
            catch (UnityException e)
            {
                Console.WriteLine("It fucking died lmao ;)");
                Console.WriteLine(e.StackTrace);
            }

            Console.WriteLine("It is done.");
        }

        [HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.Start))]
        private static class ChangeMaskToStoneMask
        {
            [HarmonyPostfix]
            public static void Postfix(GrabbableObject __instance)
            {
                __instance.TryGetComponent<HauntedMaskItem>(out HauntedMaskItem mask);
                if (mask == null) return;

                __instance.transform.FindChild("MaskMesh").TryGetComponent<MeshFilter>(out MeshFilter main);
                if (main != null) main.mesh = _stoneMaskMesh;

                __instance.transform.FindChild("MaskMesh").FindChild("ComedyMaskLOD1").TryGetComponent<MeshFilter>(out MeshFilter LOD1);
                if (LOD1 != null) LOD1.mesh = _stoneMaskMesh;
            }
        }

        [HarmonyPatch(typeof(HauntedMaskItem), nameof(HauntedMaskItem.MaskClampToHeadAnimationEvent))]
        private static class ChangeMaskPrefabModel
        {
            [HarmonyPostfix]
            public static void Postfix(HauntedMaskItem __instance, Transform ___currentHeadMask)
            {
                ___currentHeadMask.FindChild("ComedyMaskLOD1").TryGetComponent<MeshFilter>(out MeshFilter LOD1);
                if (LOD1 != null) LOD1.mesh = _stoneMaskMesh;

                ___currentHeadMask.FindChild("Mesh").TryGetComponent<MeshFilter>(out MeshFilter Mesh);
                if (Mesh != null) Mesh.mesh = _stoneMaskMesh;
            }
        }

        // Works
        [HarmonyPatch(typeof(MaskedPlayerEnemy), "Awake")]
        private static class ChangeEntityMasks
        {
            [HarmonyPrefix]
            public static void Prefix(MaskedPlayerEnemy __instance)
            {
                foreach (GameObject obj in __instance.maskTypes)
                {
                    obj.transform.FindChild("ComedyMaskLOD1").TryGetComponent<MeshFilter>(out MeshFilter LOD1);
                    if (LOD1 != null) LOD1.mesh = _stoneMaskMesh;

                    obj.transform.FindChild("Mesh").TryGetComponent<MeshFilter>(out MeshFilter Mesh);
                    if (Mesh != null) Mesh.mesh = _stoneMaskMesh;
                }
            }
        }

    }
}

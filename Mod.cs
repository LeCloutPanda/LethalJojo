using BepInEx;
using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace LethalJojo
{
    // TODO: Make Bundle load from local path not streaming assets
    // TODO: Make mask use model
    // TODO: Turn off red eyes

    [BepInPlugin(ID, NAME, VERSION)]
    public class Mod : BaseUnityPlugin
    {
        private const string ID = "dev.panda.lethaljojo";
        private const string NAME = "Lethal Jojo";
        private const string VERSION = "1.0.0";

        private static Mesh _stoneMaskMesh = null;

        void Awake()
        {
            Logger.LogMessage($"Loaded {NAME} v{VERSION}");
            Harmony harmony = new Harmony("dev.panda.lethaljojo");
            harmony.PatchAll();

            try
            {

                var asssetBundle = AssetBundle.LoadFromFile(Path.Combine(Paths.PluginPath, "LethalJojo/LethalJojoAssetBundle"));

                if (asssetBundle != null)
                {
                    _stoneMaskMesh = asssetBundle.LoadAsset<Mesh>("StoneMask_Model");
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
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
                foreach(GameObject obj in __instance.maskTypes)
                {
                    obj.transform.FindChild("ComedyMaskLOD1").TryGetComponent<MeshFilter>(out MeshFilter LOD1);
                    if (LOD1 != null) LOD1.mesh = _stoneMaskMesh;

                    obj.transform.FindChild("Mesh").TryGetComponent<MeshFilter>(out MeshFilter Mesh);
                    if (Mesh != null) Mesh.mesh = _stoneMaskMesh;
                }
            }
        }

        [HarmonyPatch(typeof(PlayerControllerB), "KillPlayer")]
        private static class GodMode
        {
            [HarmonyPrefix]
            public static bool Prefix(PlayerControllerB __instance)
            {
                return false;
            }
        }
    }
}

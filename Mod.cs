using BepInEx;
using HarmonyLib;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace LethalJojo
{
    // TODO: Turn off red eyes

    public static class ModInfo
    {
        public const string NAME = "Lethal Jojo";
        public const string ID = "dev.panda.lethaljojo";
        public const string DESCRIPTION = "Have a little slice of Jojo in your game";
        public const string COMPANY = "Pandas Hell Hole";
        public const string URL = "https://github.com/LeCloutPanda/LethalJojo";
        public const string AUTHOR = "LeCloutPanda";
        public const string VERSION = "1.0.1";
    }

    [BepInPlugin(ModInfo.ID, ModInfo.NAME, ModInfo.VERSION)]
    public class Mod : BaseUnityPlugin
    {
        public static string DLLPath;
        private static StoneMaskPatch StoneMaskPatch = null;

        void Awake()
        {
            Logger.LogMessage($"Loaded {ModInfo.NAME} v{ModInfo.VERSION}");
            DLLPath = Info.Location;

            StoneMaskPatch = new StoneMaskPatch();

            Harmony harmony = new Harmony(ModInfo.ID);
            harmony.PatchAll();
        }
    }
}

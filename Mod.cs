using BepInEx;
using HarmonyLib;

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
        private static StoneMaskPatch StoneMaskPatch = null;

        void Awake()
        {
            Logger.LogMessage($"Loaded {ModInfo.NAME} v{ModInfo.VERSION}");

            StoneMaskPatch = new StoneMaskPatch();

            Harmony harmony = new Harmony(ModInfo.ID);
            harmony.PatchAll();
        }
    }
}

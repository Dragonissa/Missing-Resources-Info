using HarmonyLib;
using System;
using Verse;

namespace MissingResourcesInfo
{
    public class MissingResourcesInfo : Mod
    {
        public MissingResourcesInfo(ModContentPack content) : base(content)
        {
            var harmony = new Harmony("dragonissa.MissingResourcesInfo");
            harmony.PatchAll();
        }
    }
}

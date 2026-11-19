using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace MissingResourcesInfo
{
    [HarmonyPatch(typeof(GenConstruct), nameof(GenConstruct.CanGetResources_NewTemp))]
    public static class Patch_CanGetResources_NewTemp
    {
        static void Postfix(
            Thing thing,
            Pawn pawn,
            bool forced,
            ref bool __result)
        {
            if (__result)
                return;

            if (FloatMenuMakerMap.makingFor != pawn)
                return;

            IConstructible constructible = thing as IConstructible;
            if (constructible == null)
                return;

            List<string> missing = new List<string>();

            foreach (ThingDefCountClass need in constructible.TotalMaterialCost())
            {
                int needed;

                if (!forced && constructible is IHaulEnroute enroute)
                    needed = enroute.GetSpaceRemainingWithEnroute(need.thingDef, pawn);
                else
                    needed = constructible.ThingCountNeeded(need.thingDef);

                if (needed <= 0)
                    continue;

                if (!pawn.Map.itemAvailability.ThingsAvailableAnywhere(
                        need.thingDef,
                        needed,
                        pawn))
                {
                    missing.Add($"{needed}x {need.thingDef.LabelCap}");
                }
            }

            if (missing.Count > 0)
            {
                JobFailReason.Is(
                    "MissingMaterials".Translate(
                        missing.ToCommaList()
                    ));
            }
        }
    }
}

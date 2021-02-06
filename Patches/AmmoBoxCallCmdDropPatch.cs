namespace AntiLag.Patches
{
    using HarmonyLib;
    using NorthwoodLib.Pools;
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using static HarmonyLib.AccessTools;

    [HarmonyPatch(typeof(AmmoBox), nameof(AmmoBox.CallCmdDrop))]
    internal static class AmmoBoxCallCmdDropPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions,
            ILGenerator generator)
        {
            var newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);

            var pickup = generator.DeclareLocal(typeof(Pickup));

            newInstructions.RemoveRange(newInstructions.Count - 2, 2);

            newInstructions.AddRange(new[]
            {
                new CodeInstruction(OpCodes.Stloc_0, pickup.LocalIndex),
                new CodeInstruction(OpCodes.Ldsfld, Field(typeof(Plugin), nameof(Plugin.Instance))),
                new CodeInstruction(OpCodes.Callvirt, PropertyGetter(typeof(Plugin), nameof(Plugin.PlayerEvents))),
                new CodeInstruction(OpCodes.Ldloc_0, pickup.LocalIndex),
                new CodeInstruction(OpCodes.Callvirt, Method(typeof(Handlers.Player), nameof(Handlers.Player.AmmoDropped))),
                new CodeInstruction(OpCodes.Ret)
            });

            foreach (CodeInstruction code in newInstructions)
                yield return code;

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}
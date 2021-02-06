namespace AntiLag
{
    using Exiled.API.Features;
    using HarmonyLib;
    using System;
    using PlayerHandler = Exiled.Events.Handlers.Player;

    public class AntiLag : Plugin<Config>
    {
        public static AntiLag Instance;

        public override string Name { get; } = "AntiLag";
        public override string Author { get; } = "Steven4547466";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 22);
        public override string Prefix { get; } = "AntiLag";

        public Handlers.Player PlayerEvents { get; private set; }
        private Harmony HarmonyInstance { get; set; }

        public override void OnEnabled()
        {
            base.OnEnabled();
            Instance = this;
            RegisterEvents();
            HarmonyInstance = new Harmony($"steven4547466.antiLag-{DateTime.UtcNow.Ticks}");
            HarmonyInstance.PatchAll();
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
            UnregisterEvents();
            HarmonyInstance.UnpatchAll(HarmonyInstance.Id);
            Instance = null;
        }

        private void RegisterEvents()
        {
            PlayerEvents = new Handlers.Player();
            PlayerHandler.ItemDropped += PlayerEvents.OnItemDropped;
        }

        private void UnregisterEvents()
        {
            PlayerHandler.ItemDropped -= PlayerEvents.OnItemDropped;
            PlayerEvents = null;
        }
    }
}
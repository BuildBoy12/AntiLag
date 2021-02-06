namespace AntiLag.Handlers
{
    using Exiled.API.Extensions;
    using Exiled.Events.EventArgs;
    using UnityEngine;

    public class Player
    {
        public void OnItemDropped(ItemDroppedEventArgs ev)
        {
            AmmoDropped(ev.Pickup);
        }

        public void AmmoDropped(Pickup pickup)
        {
            if (!pickup.itemId.IsAmmo())
                return;

            Collider[] hitColliders = new Collider[10];
            int numColliders = Physics.OverlapSphereNonAlloc(pickup.Networkposition, 3, hitColliders, LayerMask.GetMask("Pickup"));

            for (int i = 0; i < numColliders; i++)
            {
                var c = hitColliders[i];
                Pickup p = c.gameObject.GetComponent<Pickup>();
                if (p != null && p.Rb != pickup.Rb && p.itemId == pickup.itemId &&
                    p.durability < Plugin.Instance.Config.MaxAmmoStackSize)
                {
                    if (p.durability + pickup.durability <= Plugin.Instance.Config.MaxAmmoStackSize)
                    {
                        p.durability += pickup.durability;
                        UnityEngine.Object.Destroy(pickup.gameObject);
                    }
                    else
                    {
                        pickup.durability = Mathf.Abs(pickup.durability - p.durability);
                        p.durability = Plugin.Instance.Config.MaxAmmoStackSize;
                    }

                    break;
                }
            }
        }
    }
}
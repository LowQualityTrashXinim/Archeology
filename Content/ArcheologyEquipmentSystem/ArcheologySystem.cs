using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace Archeology.Content.ArcheologyEquipmentSystem;

public class ArchaeologySystem : ModSystem
{
    public static List<ArchaeologyModType> list_Archaeology = new();
    public override void Load()
    {
        if (list_Archaeology == null)
        {
            list_Archaeology = new();
        }
        On_WorldGen.KillTile += On_WorldGen_KillTile;
    }

    private void On_WorldGen_KillTile(On_WorldGen.orig_KillTile orig, int i, int j, bool fail, bool effectOnly, bool noItem)
    {
        orig(i, j, fail, effectOnly, noItem);
    }
    public static int Register(ArchaeologyModType type)
    {
        ModTypeLookup<ArchaeologyModType>.Register(type);
        list_Archaeology.Add(type);
        return list_Archaeology.Count - 1;
    }
    public static ArchaeologyModType GetArchaeologyType(int type) => type < 0 || type >= list_Archaeology.Count ? null : list_Archaeology[type];
    public override void Unload()
    {
        list_Archaeology = null;
    }
}
public class ArchaeologyPlayer : ModPlayer
{
    public int[] ArchaeologyActiveItem = new int[5];
    public bool GetArchaeologyItemFromArr(int index, out ArchaeologyModType modtype)
    {
        int type = ArchaeologyActiveItem[index];
        if (type < 0)
        {
            modtype = null;
            return false;
        }
        modtype = ArchaeologySystem.GetArchaeologyType(type);
        return true;
    }
    public override void ResetEffects()
    {
        for (int i = 0; i < ArchaeologyActiveItem.Length; i++)
        {
            if (GetArchaeologyItemFromArr(i, out ArchaeologyModType type))
            {
                type.ResetEffect(this, Player);
            }
        }
    }
    public override void UpdateEquips()
    {
        for (int i = 0; i < ArchaeologyActiveItem.Length; i++)
        {
            if (GetArchaeologyItemFromArr(i, out ArchaeologyModType type))
            {
                type.UpdateEquips(this, Player);
            }
        }
    }
    public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
    {
        for (int i = 0; i < ArchaeologyActiveItem.Length; i++)
        {
            if (GetArchaeologyItemFromArr(i, out ArchaeologyModType type))
            {
                type.OnHitNPCWithItem(this, Player, item, target, hit);
            }
        }
    }
    public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
    {
        for (int i = 0; i < ArchaeologyActiveItem.Length; i++)
        {
            if (GetArchaeologyItemFromArr(i, out ArchaeologyModType type))
            {
                type.OnHitNPCWithProj(this, Player, proj, target, hit);
            }
        }
    }
}
public abstract class ArchaeologyModType : ModType
{
    public int Type = 0;
    public int GetArchaeologyType<T>() where T : ArchaeologyModType => ModContent.GetInstance<T>().Type;
    protected override void Register()
    {
        Type = ArchaeologySystem.Register(this);
    }
    public virtual void ResetEffect(ArchaeologyPlayer modplayer, Player player) { }
    public virtual void UpdateEquips(ArchaeologyPlayer modplayer, Player player) { }
    public virtual void OnHitNPCWithItem(ArchaeologyPlayer modplayer, Player player, Item item, NPC target, NPC.HitInfo hit) { }
    public virtual void OnHitNPCWithProj(ArchaeologyPlayer modplayer, Player player, Projectile proj, NPC target, NPC.HitInfo hit) { }

}
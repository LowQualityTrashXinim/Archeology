using Archeology.Content.ArcheologyEquipmentSystem;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Archeology.Content.ArcheologyEquipmentSystem;
public class AncientTabletPiece1_Type : ArchaeologyModType
{
    public override void EffectMultiplier(ArchaeologyPlayer modplayer, Player player, ref int index, ref float EffectValue)
    {
        //We are checking in the array ahead whenever or not if the next item in the array is the same type we are looking for
        for (int i = index + 1; i < modplayer.ArchaeologyActiveItem.Length; i++)
        {
            int inner_index = i;
            if (modplayer.GetArchaeologyItemFromArr(ref inner_index, out ArchaeologyModType archaeologyType))
            {
                if (archaeologyType.Type == GetArchaeologyType<AncientTabletPiece2_Type>())
                {
                    EffectValue += 1f;
                }
            }
        }
    }
    public override void UpdateEquips(ArchaeologyPlayer modplayer, Player player)
    {
        player.ModPlayerStats().AddStatsToPlayer(PlayerStats.Defense, Base: 5, singularBaseMultiplier: GetEffectValue(modplayer));
    }
}
public class AncientTabletPiece2_Type : ArchaeologyModType
{
    public override void EffectMultiplier(ArchaeologyPlayer modplayer, Player player, ref int index, ref float EffectValue)
    {
        //We are checking in the array ahead whenever or not if the next item in the array is the same type we are looking for
        for (int i = index + 1; i < modplayer.ArchaeologyActiveItem.Length; i++)
        {
            int inner_index = i;
            if (modplayer.GetArchaeologyItemFromArr(ref inner_index, out ArchaeologyModType archaeologyType))
            {
                if (archaeologyType.Type == GetArchaeologyType<AncientTabletPiece3_Type>())
                {
                    EffectValue += 1f;
                }
            }
        }
    }
    public override void UpdateEquips(ArchaeologyPlayer modplayer, Player player)
    {
        player.ModPlayerStats().AddStatsToPlayer(PlayerStats.PureDamage, Base: 5, singularBaseMultiplier: GetEffectValue(modplayer));
    }
}
public class AncientTabletPiece3_Type : ArchaeologyModType
{
    public override void EffectMultiplier(ArchaeologyPlayer modplayer, Player player, ref int index, ref float EffectValue)
    {
        //We are checking in the array ahead whenever or not if the next item in the array is the same type we are looking for
        for (int i = index + 1; i < modplayer.ArchaeologyActiveItem.Length; i++)
        {
            int inner_index = i;
            if (modplayer.GetArchaeologyItemFromArr(ref inner_index, out ArchaeologyModType archaeologyType))
            {
                if (archaeologyType.Type == GetArchaeologyType<AncientTabletPiece4_Type>())
                {
                    EffectValue += 1f;
                }
            }
        }
    }
    public override void UpdateEquips(ArchaeologyPlayer modplayer, Player player)
    {
        player.ModPlayerStats().AddStatsToPlayer(PlayerStats.CritChance, Base: 5, singularBaseMultiplier: GetEffectValue(modplayer));
    }
}
public class AncientTabletPiece4_Type : ArchaeologyModType
{
    public override void ResetEffect(ArchaeologyPlayer modplayer, Player player)
    {
        bool Tablet1 = false;
        bool Tablet2 = false;
        bool Tablet3 = false;
        //We are checking in the array ahead whenever or not if the next item in the array is the same type we are looking for
        for (int i = 0; i < modplayer.ArchaeologyActiveItem.Length; i++)
        {
            int inner_index = i;
            if (modplayer.GetArchaeologyItemFromArr(ref inner_index, out ArchaeologyModType archaeologyType))
            {
                if (archaeologyType.Type == GetArchaeologyType<AncientTabletPiece1_Type>())
                {
                    Tablet1 = true;
                    continue;
                }
                if (archaeologyType.Type == GetArchaeologyType<AncientTabletPiece2_Type>())
                {
                    Tablet2 = true;
                    continue;
                }
                if (archaeologyType.Type == GetArchaeologyType<AncientTabletPiece3_Type>())
                {
                    Tablet3 = true;
                    continue;
                }
            }
        }
        if (Tablet1 && Tablet2 && Tablet3)
        {
            modplayer.GlobalEffectMultiplier += .25f;
        }
    }
    public override void UpdateEquips(ArchaeologyPlayer modplayer, Player player)
    {
        player.ModPlayerStats().AddStatsToPlayer(PlayerStats.CritDamage, Additive: 1.25f, singularAdditiveMultiplier: GetEffectValue(modplayer));
    }
}
public class BrokenEmblem_Type : ArchaeologyModType
{
    public override void ResetEffect(ArchaeologyPlayer modplayer, Player player)
    {
        modplayer.GlobalEffectMultiplier += .1f;
    }
    public override void UpdateEquips(ArchaeologyPlayer modplayer, Player player)
    {
        player.ModPlayerStats().AddStatsToPlayer(PlayerStats.PureDamage, Additive: 1.12f, singularAdditiveMultiplier: GetEffectValue(modplayer));
    }
}
public class BrokenGoldenKnife_Type : ArchaeologyModType
{
    public override void OnHitNPCWithProj(ArchaeologyPlayer modplayer, Player player, Projectile proj, NPC target, NPC.HitInfo hit)
    {
        if (Main.rand.NextBool(5))
        {
            Vector2 vel = (target.Center - player.Center).SafeNormalize(Vector2.Zero) * 10;
            int damage = (int)Math.Round(player.GetTotalDamage<MagicDamageClass>().ApplyTo(20 * GetEffectValue(modplayer)));
            Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, vel, ProjectileID.MagicDagger, damage, 3f, player.whoAmI);
        }
    }
    public override void OnHitNPCWithItem(ArchaeologyPlayer modplayer, Player player, Item item, NPC target, NPC.HitInfo hit)
    {
        if (Main.rand.NextBool(3))
        {
            target.AddBuff(BuffID.Midas, ModUtils.ToSecond(Main.rand.Next(1, 3)));
        }
        if (Main.rand.NextBool(5))
        {
            Vector2 vel = (target.Center - player.Center).SafeNormalize(Vector2.Zero) * 10;
            int damage = (int)Math.Round(player.GetTotalDamage<MagicDamageClass>().ApplyTo(20 * GetEffectValue(modplayer)));
            Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, vel, ProjectileID.MagicDagger, damage, 3f, player.whoAmI);
        }
    }
}
public class CorruptionShard_Type : ArchaeologyModType
{
    public override void OnHKillTile(int i, int j, int tileType)
    {
        if (Main.rand.NextBool((int)Math.Clamp(1000 - 10 * GetEffectValue(Main.LocalPlayer.GetModPlayer<ArchaeologyPlayer>()), 1, 10000)))
        {
            int item = Item.NewItem(new EntitySource_TileBreak(i, j), i, j, 16, 16, ItemID.EbonstoneBlock);
        }
    }
    public override void OnPickUp(ArchaeologyPlayer modplayer, Player player, Item item, out bool noItem)
    {
        noItem = false;
        if (item.type == ItemID.EbonstoneBlock)
        {
            int heal = (int)Math.Round(2 * GetEffectValue(modplayer));
            player.Heal(heal);
            noItem = true;
        }
    }
}
public class CrimsonShard_Type : ArchaeologyModType
{
    public override void OnHKillTile(int i, int j, int tileType)
    {
        if (Main.rand.NextBool((int)Math.Clamp(1000 - 10 * GetEffectValue(Main.LocalPlayer.GetModPlayer<ArchaeologyPlayer>()), 1, 10000)))
        {
            int item = Item.NewItem(new EntitySource_TileBreak(i, j), i, j, 16, 16, ItemID.CrimstoneBlock);
        }
    }
    public override void OnPickUp(ArchaeologyPlayer modplayer, Player player, Item item, out bool noItem)
    {
        noItem = false;
        if (item.type == ItemID.CrimstoneBlock)
        {
            int heal = (int)Math.Round(2 * GetEffectValue(modplayer));
            player.Heal(heal);
            noItem = true;
        }
    }
}
public class IceShard_Type : ArchaeologyModType
{
    public override void OnHKillTile(int i, int j, int tileType)
    {
        if (Main.rand.NextBool((int)Math.Clamp(1000 - 10 * GetEffectValue(Main.LocalPlayer.GetModPlayer<ArchaeologyPlayer>()), 1, 10000)))
        {
            int item = Item.NewItem(new EntitySource_TileBreak(i, j), i, j, 16, 16, ItemID.IceBlock);
        }
    }
    public override void OnPickUp(ArchaeologyPlayer modplayer, Player player, Item item, out bool noItem)
    {
        noItem = false;
        if (item.type == ItemID.IceBlock)
        {
            int heal = (int)Math.Round(2 * GetEffectValue(modplayer));
            player.Heal(heal);
            noItem = true;
        }
    }
}
public class DesertShard_Type : ArchaeologyModType
{
    public override void OnHKillTile(int i, int j, int tileType)
    {
        if (Main.rand.NextBool((int)Math.Clamp(1000 - 10 * GetEffectValue(Main.LocalPlayer.GetModPlayer<ArchaeologyPlayer>()), 1, 10000)))
        {
            int item = Item.NewItem(new EntitySource_TileBreak(i, j), i, j, 16, 16, ItemID.Sandstone);
        }
    }
    public override void OnPickUp(ArchaeologyPlayer modplayer, Player player, Item item, out bool noItem)
    {
        noItem = false;
        if (item.type == ItemID.Sandstone)
        {
            int heal = (int)Math.Round(2 * GetEffectValue(modplayer));
            player.Heal(heal);
            noItem = true;
        }
    }
}
public class AncientWatch_Type : ArchaeologyModType
{
    public override void ResetEffect(ArchaeologyPlayer modplayer, Player player)
    {
        modplayer.GlobalEffectMultiplier += .15f;
    }
    public override void EffectMultiplier(ArchaeologyPlayer modplayer, Player player, ref int index, ref float EffectValue)
    {
        EffectValue -= .25f;
    }
}
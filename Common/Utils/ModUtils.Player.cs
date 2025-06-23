using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria.DataStructures;
using Archeology.Common.Global;

namespace Archeology {
    public static partial class ModUtils
    {
        /// <summary>
        /// Basically the same as getting <code>player.GetModPlayer<![CDATA[<]]>PlayerStatsHandle<![CDATA[>]]>()</code>
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static PlayerStatsHandle ModPlayerStats(this Player player) => player.GetModPlayer<PlayerStatsHandle>();
        /// <summary>
        /// This check if player health/life is above x%
        /// </summary>
        /// <param name="player"></param>
        /// <param name="percent"></param>
        /// <returns>
        /// True if health is above or equal said percentage
        /// </returns>
        public static bool IsHealthAbovePercentage(this Player player, float percent) => player.statLife >= percent * player.statLifeMax2;
        public static bool HasPlayerKillThisNPC(int NPCtype) => Main.BestiaryDB.FindEntryByNPCID(NPCtype).Info.Count > 0;
        public static int DirectionFromEntityAToEntityB(float A, float B) => A > B ? -1 : 1;
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="player"></param>
        /// <returns>
        /// Return true if player is helding the item in hand<br/>
        /// Return false when player is not helding the item in hand
        /// </returns>
        public static bool IsHeldingModItem<T>(this Player player) where T : ModItem => player.HeldItem.type == ModContent.ItemType<T>();
        public static bool DoesStatsRequiredWholeNumber(PlayerStats stats) =>
                    stats is PlayerStats.Defense
                    || stats is PlayerStats.MaxMinion
                    || stats is PlayerStats.MaxSentry
                    || stats is PlayerStats.MaxHP
                    || stats is PlayerStats.MaxMana
                    || stats is PlayerStats.CritChance
                    || stats is PlayerStats.RegenHP
                    || stats is PlayerStats.RegenMana;
        public static bool IsThisArmorPiece(this Item item) => item.headSlot > 0 || item.legSlot > 0 || item.bodySlot > 0;
        /// <summary>
        /// Check whenever or not is this item a weapon or not
        /// </summary>
        /// <param name="item"></param>
        /// <param name="ConsumableWeapon">Set to true if you want to allow consumable weapon</param>
        /// <returns>Return true if the item is a weapon</returns>
        public static bool IsAWeapon(this Item item, bool ConsumableWeapon = false) =>
            item.type != ItemID.None
            && item.damage > 0
            && item.useTime > 0
            && item.useAnimation > 0
            && !item.accessory
            && item.pick == 0
            && item.hammer == 0
            && item.ammo == AmmoID.None
            && (item.consumable == false || ConsumableWeapon);
        /// <summary>
        /// Not recommend to uses reliably, as this only check vanilla slot
        /// </summary>
        /// <param name="player"></param>
        /// <param name="itemType"></param>
        /// <returns></returns>
        public static bool IsEquipAcc(this Player player, int itemType)
        {
            Item[] item = new Item[9];
            Array.Copy(player.armor, 3, item, 0, 9);
            return item.Select(i => i.type).Contains(itemType);
        }
        /// <summary>
        /// Mana heal effect added by mod, guaranteed to work
        /// </summary>
        /// <param name="player"></param>
        /// <param name="amount"></param>
        public static void ManaHeal(this Player player, int amount)
        {
            if (player.statMana >= player.statManaMax2)
            {
                if (player.statMana + amount >= player.statManaMax2)
                {
                    player.statMana = player.statManaMax2;
                }
                else
                {
                    player.statMana += amount;
                }
                player.ManaEffect(amount);
            }
        }
        public static void AddBuff<T>(this NPC npc, int timetoAdd, bool quiet = false) where T : ModBuff
        {
            npc.AddBuff(ModContent.BuffType<T>(), timetoAdd, quiet);
        }
        public static void AddBuff<T>(this Player player, int timetoAdd, bool quiet = false) where T : ModBuff
        {
            player.AddBuff(ModContent.BuffType<T>(), timetoAdd, quiet);
        }
    }
	/// <summary>
	/// This does not contain all of the mod stats, pleases referred to <see cref="PlayerStatsHandle"/> to see all built in stats<br/>
	/// Some of the stats in this enum range will be explained to avoid confusion
	/// </summary>
	public enum PlayerStats : byte {
		None,
		MeleeDMG,
		RangeDMG,
		MagicDMG,
		SummonDMG,
		PureDamage,
		MovementSpeed,
		JumpBoost,
		MaxHP,
		RegenHP,
		MaxMana,
		RegenMana,
		Defense,
		CritChance,
		CritDamage,
		DefenseEffectiveness,
		MaxMinion,
		MaxSentry,
		Thorn,
		AttackSpeed,
		LifeSteal,
		HealEffectiveness,
		Iframe,
		/// <summary>
		/// This stat will increases debuff duration when inflicting on enemy
		/// </summary>
		DebuffDurationInflict,
		MeleeCritDmg,
		RangeCritDmg,
		MagicCritDmg,
		SummonCritDmg,
		MeleeNonCritDmg,
		RangeNonCritDmg,
		MagicNonCritDmg,
		SummonNonCritDmg,
		MeleeCritChance,
		RangeCritChance,
		MagicCritChance,
		SummonCritChance,
		MeleeAtkSpeed,
		RangeAtkSpeed,
		MagicAtkSpeed,
		SummonAtkSpeed
		//Luck
	}
	public class DataStorer : ModSystem {
		public static Dictionary<string, DrawCircleAuraContext> dict_drawCircleContext = new();
		public static void AddContext(string name, DrawCircleAuraContext context) {
			if (dict_drawCircleContext == null) {
				dict_drawCircleContext = new();
			}
			if (!dict_drawCircleContext.ContainsKey(name)) {
				dict_drawCircleContext.Add(name, context);
			}

		}
		public static void ActivateContext(Player player, string name) {
			if (dict_drawCircleContext.ContainsKey(name)) {
				dict_drawCircleContext[name].Activate = true;
				dict_drawCircleContext[name].Position = player.Center;
			}

		}
		public static void ActivateContext(Vector2 position, string name) {
			if (dict_drawCircleContext.ContainsKey(name)) {
				dict_drawCircleContext[name].Activate = true;
				dict_drawCircleContext[name].Position = position;
			}

		}
		public static void DeActivateContext(string name) {
			if (dict_drawCircleContext.ContainsKey(name)) {
				dict_drawCircleContext[name].Activate = false;
			}

		}
		public static void ModifyContextDistance(string name, int Distance) {
			if (dict_drawCircleContext.ContainsKey(name)) {
				dict_drawCircleContext[name].Distance = Distance;
			}
		}
		/// <summary>
		/// It is important to check for null
		/// </summary>
		/// <param name="player"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public static DrawCircleAuraContext GetContext(string name) {
			if (dict_drawCircleContext.ContainsKey(name)) {
				return dict_drawCircleContext[name];

			}
			return null;
		}
		public static void SetContext(string name, DrawCircleAuraContext context) {
			if (dict_drawCircleContext.ContainsKey(name)) {
				dict_drawCircleContext[name].CopyContext(context);
			}
		}
		public override void Unload() {
			dict_drawCircleContext = null;
		}
	}

	/// <summary>
	/// This player class will hold additional infomation that the base Player or ModPlayer class don't provide<br/>
	/// The logic to get to those infomation is automatic <br/>
	/// It make more sense to have a modplayer file do all the logic so we don't have to worry about it when implement
	/// </summary>
	public class ModUtilsPlayer : ModPlayer {
		public const float PLAYERARMLENGTH = 12f;
		public Vector2 MouseLastPositionBeforeAnimation = Vector2.Zero;
		public Vector2 PlayerLastPositionBeforeAnimation = Vector2.Zero;
		public int counterToFullPi = 0;
		public bool CurrentHoveringOverChest = false;
		public override void ResetEffects() {
			if (!Player.active) {
				return;
			}
			Point point = Main.MouseWorld.ToTileCoordinates();
			if (WorldGen.InWorld(point.X, point.Y)) {
				CurrentHoveringOverChest = Main.tile[point.X, point.Y].TileType == TileID.Containers || Main.tile[point.X, point.Y].TileType == TileID.Containers2;
			}
		}
		public override void PreUpdate() {
			if (++counterToFullPi >= 360)
				counterToFullPi = 0;
		}
		public override void PostUpdate() {
			if (!Player.ItemAnimationActive) {
				MouseLastPositionBeforeAnimation = Main.MouseWorld;
				PlayerLastPositionBeforeAnimation = Player.Center;
			}
		}
		/// <summary>
		/// It is advised to add this when the mod load instead of during gameplay
		/// </summary>
		/// <param name="player"></param>
		/// <param name="name"></param>
		/// <param name="context"></param>
		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright) {
			foreach (var item in DataStorer.dict_drawCircleContext.Values) {
				if (item.Activate) {
					ModUtils.BresenhamCircle(item.Position, item.Distance, item.Color);
					item.Activate = false;
				}
			}
		}
	}
	/// <summary>
	/// please set this in <see cref="ModUtilsPlayer"/><br/>
	/// This is restrictively work with player entity only
	/// </summary>
	public class DrawCircleAuraContext {
		public int Distance = 0;
		public Vector2 Position = Vector2.Zero;
		public bool Activate = false;
		public Color Color = Color.White;
		public DrawCircleAuraContext() { }

		public DrawCircleAuraContext(int distance, Vector2 position, bool activate, Color color) {
			Distance = distance;
			Position = position;
			Activate = activate;
			Color = color;
		}
		public void CopyContext(DrawCircleAuraContext context) {
			Distance = context.Distance;
			Position = context.Position;
			Activate = context.Activate;
			Color = context.Color;
		}
	}
}

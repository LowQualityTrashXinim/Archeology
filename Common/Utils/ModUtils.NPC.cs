﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;

namespace Archeology {
	public static partial class ModUtils {
		/// <summary>
		/// This will take a approximation of the rough position that it need to go and then stop the npc from moving when it reach that position 
		/// </summary>
		/// <param name="npc"></param>
		/// <param name="Position"></param>
		/// <param name="speed"></param>
		public static bool NPCMoveToPosition(this NPC npc, Vector2 Position, float speed, float roomforError = 20f) {
			Vector2 distance = Position - npc.Center;
			if (distance.Length() <= roomforError) {
				npc.velocity = Vector2.Zero;
				return true;
			}
			npc.velocity = distance.SafeNormalize(Vector2.Zero) * speed;
			return false;
		}
		public static int NewHostileProjectile(IEntitySource source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, int whoAmI = -1, bool AdjustHostileProjectileDamage = true) {
			if (AdjustHostileProjectileDamage) {
				if (Main.expertMode)
					damage /= 4;
				else if (Main.masterMode)
					damage /= 6;
				else
					damage /= 2;
			}

			if (damage < 1) {
				damage = 1;
			}
			int HostileProjectile = Projectile.NewProjectile(source, position, velocity, type, damage, knockback);

			Main.projectile[HostileProjectile].whoAmI = whoAmI;
			Main.projectile[HostileProjectile].hostile = true;
			Main.projectile[HostileProjectile].friendly = false;
			return HostileProjectile;
		}

		public static void Heal(this NPC npc, int healAmount, bool texteffect = true) {
			int simulatehealing = npc.life + healAmount;
			if (npc.lifeMax <= simulatehealing) {
				npc.life = npc.lifeMax;
			}
			else {
				npc.life = simulatehealing;
			}
			if (texteffect) {
				CombatText.NewText(npc.Hitbox, CombatText.HealLife, healAmount);
			}
		}
	}
}

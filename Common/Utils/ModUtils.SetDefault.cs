using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Archeology
{
	public static partial class ModUtils {
		public static void BossRushSetDefaultBuff(this ModBuff buff) {
			Main.debuff[buff.Type] = false;
			Main.buffNoSave[buff.Type] = true;
		}
		public static void BossRushSetDefaultDeBuff(this ModBuff buff, bool Save = false, bool Cure = false) {
			Main.debuff[buff.Type] = true;
			Main.buffNoSave[buff.Type] = Save;
			BuffID.Sets.NurseCannotRemoveDebuff[buff.Type] = Cure;
		}
		/// <summary>
		/// Set your own DamageClass type
		/// </summary>
		public static void BossRushSetDefault(this Item item, int width, int height, int damage, float knockback, int useTime, int useAnimation, int useStyle, bool autoReuse) {
			item.width = width;
			item.height = height;
			item.damage = damage;
			item.knockBack = knockback;
			item.useTime = useTime;
			item.useAnimation = useAnimation;
			item.useStyle = useStyle;
			item.autoReuse = autoReuse;
		}
		public static void BossRushDefaultToConsume(this Item item, int width, int height, int useStyle = ItemUseStyleID.HoldUp) {
			item.width = width;
			item.height = height;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = useStyle;
			item.autoReuse = false;
			item.consumable = true;
		}
		/// <summary>
		/// Use this along with <see cref="BossRushSetDefault(Item, int, int, int, float, int, int, int, bool)"/>
		/// </summary>
		/// <param name="item"></param>
		/// <param name="spearType"></param>
		/// <param name="shootSpeed"></param>
		public static void BossRushSetDefaultSpear(this Item item, int spearType, float shootSpeed) {
			item.noUseGraphic = true;
			item.noMelee = true;
			item.shootSpeed = shootSpeed;
			item.shoot = spearType;
			item.DamageType = DamageClass.Melee;
		}
		public static void BossRushDefaultMeleeShootCustomProjectile(this Item item, int width, int height, int damage, float knockback, int useTime, int useAnimation, int useStyle, int shoot, float shootspeed, bool autoReuse) {
			BossRushSetDefault(item, width, height, damage, knockback, useTime, useAnimation, useStyle, autoReuse);
			item.shoot = shoot;
			item.shootSpeed = shootspeed;
			item.DamageType = DamageClass.Melee;
		}
		public static void BossRushDefaultMeleeCustomProjectile(this Item item, int width, int height, int damage, float knockback, int useTime, int useAnimation, int useStyle, int shoot, bool autoReuse) {
			BossRushSetDefault(item, width, height, damage, knockback, useTime, useAnimation, useStyle, autoReuse);
			item.shoot = shoot;
			item.shootSpeed = 1;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.DamageType = DamageClass.Melee;
		}
		public static void BossRushDefaultRange(this Item item, int width, int height, int damage, float knockback, int useTime, int useAnimation, int useStyle, int shoot, float shootSpeed, bool autoReuse, int useAmmo = 0
		) {
			BossRushSetDefault(item, width, height, damage, knockback, useTime, useAnimation, useStyle, autoReuse);
			item.shoot = shoot;
			item.shootSpeed = shootSpeed;
			item.useAmmo = useAmmo;
			item.noMelee = true;
			item.DamageType = DamageClass.Ranged;
		}
		public static void BossRushDefaultPotion(this Item item, int width, int height, int BuffType, int BuffTime) {
			item.width = width;
			item.height = height;
			item.useStyle = ItemUseStyleID.DrinkLiquid;
			item.useAnimation = item.useTime = 15;
			item.useTurn = true;
			item.maxStack = 30;
			item.consumable = true;
			item.buffType = BuffType;
			item.buffTime = BuffTime;
		}
		public static void BossRushDefaultMagic(this Item item, int width, int height, int damage, float knockback, int useTime, int useAnimation, int useStyle, int shoot, float shootSpeed, int manaCost, bool autoReuse
			) {
			BossRushSetDefault(item, width, height, damage, knockback, useTime, useAnimation, useStyle, autoReuse);
			item.shoot = shoot;
			item.shootSpeed = shootSpeed;
			item.mana = manaCost;
			item.noMelee = true;
			item.DamageType = DamageClass.Magic;
		}

		public static void BossRushDefaultMagic(Item item, int shoot, float shootSpeed, int manaCost) {
			item.shoot = shoot;
			item.shootSpeed = shootSpeed;
			item.mana = manaCost;
			item.noMelee = true;
		}
		public static void DrawAuraEffect(SpriteBatch spriteBatch, Texture2D texture, Vector2 drawPos, float offsetX, float offsetY, Color color, float rotation, float scale) {
			Vector2 origin = texture.Size() * .5f;
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, drawPos.Subtract(offsetX, offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos.Subtract(offsetX, -offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos.Subtract(-offsetX, offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos.Subtract(-offsetX, -offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
			}
		}
		public static void DrawAuraEffect(this Item item, SpriteBatch spriteBatch, Vector2 drawPos, float offsetX, float offsetY, Color color, float rotation, float scale) {
			Main.instance.LoadItem(item.type);
			Texture2D texture = TextureAssets.Item[item.type].Value;
			Vector2 origin = texture.Size() * .5f;
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, drawPos.Subtract(offsetX, offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos.Subtract(offsetX, -offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos.Subtract(-offsetX, offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos.Subtract(-offsetX, -offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
			}
		}
		public static void DrawAuraEffect(this Item item, SpriteBatch spriteBatch, float offsetX, float offsetY, Color color, float rotation, float scale) {
			Main.instance.LoadItem(item.type);
			Texture2D texture = TextureAssets.Item[item.type].Value;
			Vector2 origin = texture.Size() * .5f;
			Vector2 drawPos = item.position - Main.screenPosition + origin;
			for (int i = 0; i < 3; i++) {
				spriteBatch.Draw(texture, drawPos.Subtract(offsetX, offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos.Subtract(offsetX, -offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos.Subtract(-offsetX, offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
				spriteBatch.Draw(texture, drawPos.Subtract(-offsetX, -offsetY), null, color, rotation, origin, scale, SpriteEffects.None, 0);
			}
		}
	}
}

﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;

namespace Archeology
{
	public static partial class ModUtils {
		public static void ResetMinion(this Projectile projectile, Vector2 position, float distance) {
			if (CompareSquareFloatValue(projectile.Center, position, distance * distance)) {
				return;
			}
			projectile.position = position;
			projectile.velocity *= 0.1f;
			projectile.netUpdate = true;
		}
		public static void IdleFloatMovement(this Projectile projectile, Player player, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition, int index = 0) {
			Vector2 idlePosition = player.Center;
			float minionPositionOffsetX = (30 + index * 40) * -player.direction;
			idlePosition.X += minionPositionOffsetX;
			vectorToIdlePosition = idlePosition - projectile.Center;
			distanceToIdlePosition = vectorToIdlePosition.Length();
			projectile.ResetMinion(player.Center, 1500);
			float overlapVelocity = 0.04f;
			for (int i = 0; i < Main.maxProjectiles; i++) {
				Projectile other = Main.projectile[i];
				if (i != projectile.whoAmI
					&& other.active
					&& other.owner == projectile.owner
					&& Math.Abs(projectile.position.X - other.position.X) + Math.Abs(projectile.position.Y - other.position.Y) < projectile.width) {
					if (projectile.position.X < other.position.X) {
						projectile.velocity.X -= overlapVelocity;
					}
					else {
						projectile.velocity.X += overlapVelocity;
					}

					if (projectile.position.Y < other.position.Y) {
						projectile.velocity.Y -= overlapVelocity;
					}
					else {
						projectile.velocity.Y += overlapVelocity;
					}
				}
			}
		}
		public static void MoveToIdle(this Projectile projectile, Vector2 vectorToIdlePosition, float distanceToIdlePosition, float speed, float inertia) {
			if (distanceToIdlePosition > 20f) {
				vectorToIdlePosition = vectorToIdlePosition.SafeNormalize(Vector2.Zero) * speed;
				projectile.velocity = (projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
			}
			else if (projectile.velocity == Vector2.Zero) {
				projectile.velocity.X = -0.15f;
				projectile.velocity.Y = -0.05f;
			}
		}
		public static void MoveToIdle(this Projectile projectile, Vector2 vectorToIdlePosition, float speed, float inertia, bool disablepoke = false) {
			float disToIdle = vectorToIdlePosition.LengthSquared();
			if (disToIdle > 20 * 20) {
				vectorToIdlePosition = vectorToIdlePosition.SafeNormalize(Vector2.Zero) * speed;
				projectile.velocity = (projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
			}
			else if (projectile.velocity == Vector2.Zero && !disablepoke) {
				projectile.velocity.X = -0.15f;
				projectile.velocity.Y = -0.05f;
			}
		}
		public static void ProjectileSwordSwingAI(Projectile projectile, Player player, Vector2 PositionFromMouseToPlayer, int swing = 1, int swingdegree = 120) {
			if (projectile.timeLeft > player.itemAnimationMax) {
				projectile.timeLeft = player.itemAnimationMax;
			}
			player.heldProj = projectile.whoAmI;
			float percentDone = player.itemAnimation / (float)player.itemAnimationMax;
			if (swing == -1) {
				percentDone = OutExpo(1 - percentDone, 11);
			}
			else if (swing == 1) {
				percentDone = InExpo(percentDone, 11);
			}
			percentDone = Math.Clamp(percentDone, 0, 1);
			projectile.spriteDirection = player.direction;
			float baseAngle = PositionFromMouseToPlayer.ToRotation();
			float angle = MathHelper.ToRadians(baseAngle + swingdegree) * player.direction;
			float start = baseAngle + angle;
			float end = baseAngle - angle;
			float currentAngle = MathHelper.Lerp(start, end, percentDone);
			projectile.rotation = currentAngle;
			projectile.rotation += player.direction > 0 ? MathHelper.PiOver4 : MathHelper.PiOver4 * 3f;
			projectile.velocity.X = player.direction;
			projectile.Center = player.MountedCenter + Vector2.UnitX.RotatedBy(currentAngle) * 42;
			player.compositeFrontArm = new Player.CompositeArmData(true, Player.CompositeArmStretchAmount.Full, currentAngle - MathHelper.PiOver2);
		}
		public static void ModifyProjectileDamageHitbox(ref Rectangle hitbox, Player player, int width, int height, float offset = 0) {
			float scale = player.GetAdjustedItemScale(player.HeldItem);
			float length = new Vector2(width, height).Length() * scale;
			Vector2 handPos = Vector2.UnitY.RotatedBy(player.compositeFrontArm.rotation);
			Vector2 endPos = handPos;
			endPos *= length;
			Vector2 offsetVector = handPos * offset - handPos;
			handPos += player.MountedCenter + offsetVector;
			endPos += player.MountedCenter + offsetVector;
			(int X1, int X2) XVals = Order(handPos.X, endPos.X);
			(int Y1, int Y2) YVals = Order(handPos.Y, endPos.Y);
			hitbox = new Rectangle(XVals.X1 - 2, YVals.Y1 - 2, XVals.X2 - XVals.X1 + 2, YVals.Y2 - YVals.Y1 + 2);
		}
		public static void ModifyProjectileDamageHitbox(ref Rectangle hitbox, Player player, float rotation, int width, int height, float offset = 0) {
			float length = new Vector2(width, height).Length() * player.GetAdjustedItemScale(player.HeldItem);
			Vector2 handPos = Vector2.UnitX.RotatedBy(rotation);
			Vector2 endPos = handPos;
			endPos *= length;
			Vector2 offsetVector = handPos * offset - handPos;
			handPos += player.MountedCenter + offsetVector;
			endPos += player.MountedCenter + offsetVector;
			(int X1, int X2) XVals = Order(handPos.X, endPos.X);
			(int Y1, int Y2) YVals = Order(handPos.Y, endPos.Y);
			hitbox = new Rectangle(XVals.X1 - 2, YVals.Y1 - 2, XVals.X2 - XVals.X1 + 2, YVals.Y2 - YVals.Y1 + 2);
		}
		public static void ModifyProjectileDamageHitbox(ref Rectangle hitbox, Vector2 position, float rotation, int width, int height, float offset = 0) {
			float length = new Vector2(width, height).Length();
			Vector2 handPos = Vector2.UnitX.RotatedBy(rotation);
			Vector2 endPos = handPos;
			endPos *= length;
			Vector2 offsetVector = handPos * offset - handPos;
			handPos += position + offsetVector;
			endPos += position + offsetVector;
			(int X1, int X2) XVals = Order(handPos.X, endPos.X);
			(int Y1, int Y2) YVals = Order(handPos.Y, endPos.Y);
			hitbox = new Rectangle(XVals.X1 - 2, YVals.Y1 - 2, XVals.X2 - XVals.X1 + 2, YVals.Y2 - YVals.Y1 + 2);
		}
		public static int CountDown(int timer, int timeDecrease = 1, int maxValue = 999999) => Math.Clamp(timer - timeDecrease, 0, maxValue);
	}
}

using Archeology.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using ReLogic.Localization.IME;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.UI.Chat;
using static System.Net.Mime.MediaTypeNames;

namespace Archeology
{
    public static partial class ModUtils
    {
        public static bool IsUniqueIDEqual(this UIElement el1, UIElement el2) => el1.UniqueId == el2.UniqueId;
        public static void SimpleItemMouseExchange(Player player, ref Item slot)
        {
            Item mouseitem = Main.mouseItem;
            if (slot.type == 0)
            {
                if (Main.mouseItem.type != 0)
                {
                    slot = Main.mouseItem.Clone();
                    Main.mouseItem.TurnToAir();
                    player.inventory[58].TurnToAir();
                }
            }
            else
            {
                if (Main.mouseItem.type != 0)
                {
                    Item cached = slot.Clone();
                    slot = Main.mouseItem.Clone();
                    Main.mouseItem = cached.Clone();
                    player.inventory[58] = cached.Clone();
                }
                else
                {
                    Main.mouseItem = slot.Clone();
                    player.inventory[58] = slot.Clone();
                    slot.TurnToAir();
                }
            }
        }
        public static void Disable_MouseItemUsesWhenHoverOverAUI(this UIElement el)
        {
            if (el.ContainsPoint(Main.MouseScreen))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }
        public static void UISetWidthHeight(this UIElement ui, float width, float height)
        {
            ui.Width.Pixels = width;
            ui.Height.Pixels = height;
        }
        public static void UISetPosition(this UIElement ui, Vector2 position, Vector2 origin)
        {
            Vector2 drawpos = position - Main.screenPosition - origin;
            ui.Left.Pixels = drawpos.X + (drawpos.X * (1 - Main.UIScale));
            ui.Top.Pixels = drawpos.Y + (drawpos.Y * (1 - Main.UIScale));
        }
        public static void UISetPosition(this UIElement ui, Vector2 position)
        {
            ui.Left.Pixels = position.X;
            ui.Left.Percent = 0;
            ui.Top.Pixels = position.Y;
            ui.Top.Percent = 0;
        }
    }
    public class Roguelike_WrapTextUIPanel : UITextPanel<string>
    {
        public bool Hide = false;
        //Stole from ActiveArtifactDescriptionUI cause idk how to do text wrapping stuff
        private int linePosition;
        private int maxLinePosition;
        public int MAX_LINES = 0;
        public Vector2 offSetDraw = Vector2.Zero;
        public Roguelike_WrapTextUIPanel(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (Hide)
            {
                return;
            }
            DynamicSpriteFont font = FontAssets.MouseText.Value;
            float scale = TextScale;
            string cachedText = Text;
            SetText("");
            this.Recalculate();
            base.DrawSelf(spriteBatch);
            string[] lines = Utils.WordwrapString(
                cachedText,
                font,
                (int)(this.GetInnerDimensions().Width * (1 + Math.Abs(1 - scale))),
                100,
            out int lineCount
            ).Where(line => line is not null).ToArray();

            maxLinePosition = Math.Max(lines.Length - MAX_LINES, 0);
            linePosition = Math.Clamp(linePosition, 0, maxLinePosition);

            float yOffset = 0f;
            for (int i = 0; i < lines.Length; i++)
            {
                string text = lines[i];
                ChatManager.DrawColorCodedStringWithShadow(
                    spriteBatch,
                    font,
                    text,
                    GetInnerDimensions().Position() + offSetDraw + Vector2.UnitY * yOffset,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    Vector2.One * scale
                );

                yOffset += scale * 25f;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Hide)
            {
                return;
            }
            base.Draw(spriteBatch);
        }
        public override void ScrollWheel(UIScrollWheelEvent evt)
        {
            linePosition -= MathF.Sign(evt.ScrollWheelValue);
        }
    }
    public class Roguelike_UITextPanel : UITextPanel<string>
    {
        public bool Hide = false;
        public bool UseCustmSetHeight = false;
        public Roguelike_UITextPanel(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.IgnoresMouseInteraction = Hide;
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (Hide)
            {
                return;
            }
            base.DrawSelf(spriteBatch);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Hide)
            {
                return;
            }
            if (!UseCustmSetHeight)
            {
                Vector2 stringsize = ChatManager.GetStringSize(FontAssets.MouseText.Value, Text, Vector2.UnitY);
                Height.Pixels = stringsize.Y + 10;
            }
            base.Draw(spriteBatch);
        }
    }
    public class Roguelike_UIText : UIText
    {
        public bool Hide = false;
        public Roguelike_UIText(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (Hide)
            {
                return;
            }
            base.DrawSelf(spriteBatch);
        }
        public sealed override void Draw(SpriteBatch spriteBatch)
        {
            if (Hide)
            {
                return;
            }
            base.Draw(spriteBatch);
        }
    }
    public class Roguelike_UIPanel : UIPanel
    {
        public bool Hide = false;
        public Roguelike_UIPanel()
        {
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.IgnoresMouseInteraction = Hide;
        }
        public virtual void PreDraw(SpriteBatch spriteBatch) { }
        public virtual void PostDraw(SpriteBatch spriteBatch) { }
        protected sealed override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (Hide)
            {
                return;
            }
            base.DrawSelf(spriteBatch);
        }
        public sealed override void Draw(SpriteBatch spriteBatch)
        {
            if (Hide)
            {
                return;
            }
            PreDraw(spriteBatch);
            base.Draw(spriteBatch);
            PostDraw(spriteBatch);
        }
    }
    public struct PostTextDrawInfo
    {
        public float Opacity = 1;
        public bool Hide = false;
        public PostTextDrawInfo(float opa)
        {
            Opacity = opa;
        }
    }
    public class Roguelike_UIImage : UIImage
    {
        public bool Hide = false;
        public bool Highlight = false;
        public Color OriginalColor = Color.White;
        public Color HighlightColor = Color.White;
        public PostTextDrawInfo drawInfo = new PostTextDrawInfo();
        public string HoverText = "";
        /// <summary>
        /// Set this to have value if you want a specific texture to be drawn on top of it<br/>
        /// The drawing will be handle automatically
        /// </summary>
        public Asset<Texture2D> postTex = null;
        public Texture2D innerTex = null;
        bool _CustomWeirdDraw = false;
        public void SetPostTex(Asset<Texture2D> tex, bool CustomWeirdDraw = false, bool attemptToLoad = false)
        {
            if (attemptToLoad)
            {
                try
                {
                    Main.Assets.Request<Texture2D>(tex.Name);
                }
                catch (Exception e)
                {
                    Main.NewText(e.Message);
                }
            }
            postTex = tex;
            _CustomWeirdDraw = CustomWeirdDraw;
        }
        public void SwapHightlightColorWithOriginalColor()
        {
            Color origin = OriginalColor;
            OriginalColor = HighlightColor;
            HighlightColor = origin;
        }
        public Roguelike_UIImage(Asset<Texture2D> texture) : base(texture)
        {
            innerTex = texture.Value;
            OriginalColor = Color;
            drawInfo.Opacity = 1;
        }
        public override sealed void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.IgnoresMouseInteraction = Hide;
            this.Disable_MouseItemUsesWhenHoverOverAUI();
            if (Highlight)
            {
                Color = HighlightColor;
            }
            else
            {
                Color = OriginalColor;
            }
            UpdateImage(gameTime);
        }
        public virtual void UpdateImage(GameTime gameTime) { }
        public virtual void DrawImage(SpriteBatch spriteBatch) { }
        public sealed override void Draw(SpriteBatch spriteBatch)
        {
            if (Hide)
            {
                return;
            }
            base.Draw(spriteBatch);
            DrawImage(spriteBatch);
            if (!string.IsNullOrEmpty(HoverText) && IsMouseHovering)
            {
                Main.instance.MouseText(HoverText);
            }
            if (postTex != null)
            {
                Vector2 origin2 = innerTex.Size() * .5f;
                Vector2 drawpos = this.GetInnerDimensions().Position();
                Vector2 origin = postTex.Size() * .5f;
                if (_CustomWeirdDraw)
                {
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                    spriteBatch.Draw(postTex.Value, drawpos + origin2, null, Color.White * drawInfo.Opacity, 0, origin, origin2.Length() / origin.Length() * .8f, SpriteEffects.None, 0);
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                }
                else
                {
                    spriteBatch.Draw(postTex.Value, drawpos + origin2, null, Color.White * drawInfo.Opacity, 0, origin, origin2.Length() / origin.Length() * .8f, SpriteEffects.None, 0);
                }
            }
        }
    }
    public class Roguelike_UIImageButton : UIImageButton
    {
        /// <summary>
        /// Set this to have value if you want a specific texture to be drawn on top of it<br/>
        /// The drawing will be handle automatically
        /// </summary>
        public Asset<Texture2D> postTex = null;
        public Texture2D innerTex = null;
        public string HoverText = null;
        bool _CustomWeirdDraw = false;
        public void SetPostTex(Asset<Texture2D> tex, bool CustomWeirdDraw = false)
        {
            postTex = tex;
            _CustomWeirdDraw = CustomWeirdDraw;
        }
        public Roguelike_UIImageButton(Asset<Texture2D> texture) : base(texture)
        {
            innerTex = texture.Value;
        }
        public bool Hide = false;
        public override sealed void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.IgnoresMouseInteraction = Hide;
            this.Disable_MouseItemUsesWhenHoverOverAUI();
            if (HoverText != null)
            {
                Main.instance.MouseText(HoverText);
            }
        }
        public virtual void DrawImage(SpriteBatch spriteBatch) { }
        public sealed override void Draw(SpriteBatch spriteBatch)
        {
            if (Hide)
            {
                return;
            }
            base.Draw(spriteBatch);
            DrawImage(spriteBatch);
            if (postTex != null)
            {
                Vector2 origin = postTex.Size() * .5f;
                Vector2 origin2 = innerTex.Size() * .5f;
                Vector2 drawpos = this.GetInnerDimensions().Position();
                if (_CustomWeirdDraw)
                {
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
                    spriteBatch.Draw(postTex.Value, drawpos + origin2, null, Color.White, 0, origin, origin2.Length() / origin.Length() * .8f, SpriteEffects.None, 0);
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                }
                else
                {
                    spriteBatch.Draw(postTex.Value, drawpos + origin2, null, Color.White, 0, origin, origin2.Length() / origin.Length() * .8f, SpriteEffects.None, 0);
                }
            }
        }
    }
    public class ItemHolderSlot : Roguelike_UIImage
    {
        private Texture2D texture;
        public Item item = new Item(0);
        public string Description = "";
        public bool DisplayOnHover = true;
        public ItemHolderSlot(Asset<Texture2D> texture) : base(texture)
        {
            this.texture = texture.Value;
        }
        public override void DrawImage(SpriteBatch spriteBatch)
        {
            if (item == null)
            {
                return;
            }
            if (item.type == ItemID.None)
            {
                return;
            }
            if (this.IsMouseHovering && DisplayOnHover)
            {
                if (!string.IsNullOrEmpty(Description))
                {
                    UICommon.TooltipMouseText(Description);
                }
                else
                {
                    Main.HoverItem = item;
                    Main.instance.MouseText("");
                    Main.mouseText = true;
                }
            }
            Texture2D itemtexture;
            Color colorToDraw = Color.White;


            Main.instance.LoadItem(item.type);
            itemtexture = TextureAssets.Item[item.type].Value;


            Vector2 origin = itemtexture.Size() * .5f;
            Vector2 FrameOrigin = texture.Size() * .5f;
            Vector2 DrawPos = this.GetInnerDimensions().Position() + FrameOrigin;
            spriteBatch.Draw(itemtexture, DrawPos, null, colorToDraw, 0, origin, ModUtils.Scale_OuterTextureWithInnerTexture(FrameOrigin, origin, .8f), SpriteEffects.None, 0);
        }
    }
}


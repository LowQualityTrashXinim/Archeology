using Archeology.Content.Items;
using Archeology.Texture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

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
        if (!Main.dedServ)
        {
            userInterface = new();
            ArchaeologyUI = new();
        }
    }
    public override void Unload()
    {
        list_Archaeology = null;
        userInterface = null;
        ArchaeologyUI = null;
    }
    public ArchaeologyUIState ArchaeologyUI;
    internal UserInterface userInterface;
    public override void UpdateUI(GameTime gameTime)
    {
        base.UpdateUI(gameTime);
        userInterface?.Update(gameTime);
    }
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int InventoryIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
        if (InventoryIndex != -1)
            layers.Insert(InventoryIndex, new LegacyGameInterfaceLayer(
                "ArchaeologyXinimMod: UI",
                delegate
                {
                    GameTime gametime = new GameTime();
                    userInterface.Draw(Main.spriteBatch, gametime);
                    return true;
                },
                InterfaceScaleType.UI)
            );
    }
    public void ToggleUI()
    {
        if (userInterface.CurrentState != null)
        {
            DeactivateUI();
        }
        else
        {
            ActivateUI();
        }
    }
    public void DeactivateUI()
    {
        userInterface.SetState(null);
    }
    public void ActivateUI()
    {
        userInterface.SetState(ArchaeologyUI);
    }
    public static int Register(ArchaeologyModType type)
    {
        ModTypeLookup<ArchaeologyModType>.Register(type);
        list_Archaeology.Add(type);
        return list_Archaeology.Count - 1;
    }
    public static int TotalCount => list_Archaeology.Count;
    public static ArchaeologyModType GetArchaeologyType(int type) => type < 0 || type >= list_Archaeology.Count ? null : list_Archaeology[type];
}
public class ArchaeologyGlobalTile : GlobalTile
{
    public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
    {
        if (noItem)
        {
            return;
        }
        if (!Main.dedServ)
        {
            return;
        }
        Player player = Main.LocalPlayer;
        if (player.TryGetModPlayer(out ArchaeologyPlayer modplayer))
        {
            for (int a = 0; a < modplayer.ArchaeologyActiveItem.Length; a++)
            {
                if (modplayer.GetArchaeologyItemFromArr(ref i, out ArchaeologyModType modtype))
                {
                    modtype.OnHKillTile(i, j, type);
                    modplayer.EffectMultiplier = 1;
                    modtype.EffectMultiplier(modplayer, player, ref i, ref modplayer.EffectMultiplier);
                }
            }

        }
        int itemtype = 0;
        if (type == TileID.Dirt)
        {
            if (Main.rand.NextBool(10000))
            {
                int itemDecider = Main.rand.Next(4);
                switch (itemDecider)
                {
                    case 0:
                        itemtype = ModContent.ItemType<AncientTabletPiece1>();
                        break;
                    case 1:
                        itemtype = ModContent.ItemType<AncientTabletPiece1>();
                        break;
                    case 2:
                        itemtype = ModContent.ItemType<AncientTabletPiece1>();
                        break;
                    case 3:
                        itemtype = ModContent.ItemType<AncientTabletPiece1>();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
public class ArchaeologyUIState : UIState
{
    public UIPanel mainPanel;
    public ItemHolderSlot slot1;
    public ItemHolderSlot slot2;
    public ItemHolderSlot slot3;
    public ItemHolderSlot slot4;
    public ItemHolderSlot slot5;
    public override void OnInitialize()
    {
        mainPanel = new UIPanel();
        mainPanel.HAlign = .5f;
        mainPanel.VAlign = .1f;
        mainPanel.UISetWidthHeight(300, 100);
        Append(mainPanel);

        Asset<Texture2D> basicasstexture = ModContent.Request<Texture2D>(ModTexture.ACCESSORIESSLOT);

        slot1 = new(basicasstexture);
        slot1.HAlign = 0;
        slot1.VAlign = .5f;
        slot1.OnLeftClick += ItemSlot_OnLeftClick;
        slot1.OnUpdate += ItemSlot_OnUpdate;
        mainPanel.Append(slot1);

        slot2 = new(basicasstexture);
        slot2.HAlign = 1 / 4f;
        slot2.VAlign = .5f;
        slot2.OnLeftClick += ItemSlot_OnLeftClick;
        slot2.OnUpdate += ItemSlot_OnUpdate;
        mainPanel.Append(slot2);

        slot3 = new(basicasstexture);
        slot3.HAlign = 2 / 4f;
        slot3.VAlign = .5f;
        slot3.OnLeftClick += ItemSlot_OnLeftClick;
        slot3.OnUpdate += ItemSlot_OnUpdate;
        mainPanel.Append(slot3);

        slot4 = new(basicasstexture);
        slot4.HAlign = 3 / 4f;
        slot4.VAlign = .5f;
        slot4.OnLeftClick += ItemSlot_OnLeftClick;
        slot4.OnUpdate += ItemSlot_OnUpdate;
        mainPanel.Append(slot4);

        slot5 = new(basicasstexture);
        slot5.HAlign = 1;
        slot5.VAlign = .5f;
        slot5.OnLeftClick += ItemSlot_OnLeftClick;
        slot5.OnUpdate += ItemSlot_OnUpdate;
        mainPanel.Append(slot5);
    }

    private void ItemSlot_OnUpdate(UIElement affectedElement)
    {
        Player player = Main.LocalPlayer;
        if (player.TryGetModPlayer(out ArchaeologyPlayer modplayer))
        {
            if (slot1.IsUniqueIDEqual(affectedElement))
            {
                modplayer.ArchaeologyActiveItem[0] = slot1.item.type;
            }
            else if (slot2.IsUniqueIDEqual(affectedElement))
            {
                modplayer.ArchaeologyActiveItem[1] = slot2.item.type;
            }
            else if (slot3.IsUniqueIDEqual(affectedElement))
            {
                modplayer.ArchaeologyActiveItem[2] = slot3.item.type;
            }
            else if (slot4.IsUniqueIDEqual(affectedElement))
            {
                modplayer.ArchaeologyActiveItem[3] = slot4.item.type;
            }
            else if (slot5.IsUniqueIDEqual(affectedElement))
            {
                modplayer.ArchaeologyActiveItem[4] = slot5.item.type;
            }
        }
    }

    private void ItemSlot_OnLeftClick(UIMouseEvent evt, UIElement listeningElement)
    {
        Player player = Main.LocalPlayer;
        if (slot1.IsUniqueIDEqual(listeningElement))
        {
            ModUtils.SimpleItemMouseExchange(player, ref slot1.item);
        }
        else if (slot2.IsUniqueIDEqual(listeningElement))
        {
            ModUtils.SimpleItemMouseExchange(player, ref slot2.item);
        }
        else if (slot3.IsUniqueIDEqual(listeningElement))
        {
            ModUtils.SimpleItemMouseExchange(player, ref slot3.item);
        }
        else if (slot4.IsUniqueIDEqual(listeningElement))
        {
            ModUtils.SimpleItemMouseExchange(player, ref slot4.item);
        }
        else if (slot5.IsUniqueIDEqual(listeningElement))
        {
            ModUtils.SimpleItemMouseExchange(player, ref slot5.item);
        }
    }
}
public class ArchaeologyPlayer : ModPlayer
{
    public int[] ArchaeologyActiveItem = new int[5];
    public float EffectMultiplier = 1;
    public float GlobalEffectMultiplier = 1;
    public bool GetArchaeologyItemFromArr(ref int index, out ArchaeologyModType modtype)
    {
        int type = ArchaeologyActiveItem[index];
        modtype = null;
        if (type < 0)
        {
            return false;
        }
        Item item = ContentSamples.ItemsByType[type];
        if (item.ModItem == null)
        {
            return false;
        }
        if (item.ModItem is BaseItem moditem)
        {
            modtype = ArchaeologySystem.GetArchaeologyType(moditem.ArchaeologyType);
            if (modtype == null)
            {
                return false;
            }
            return true;
        }
        return false;
    }
    public override void ResetEffects()
    {
        if (!Player.active)
        {
            return;
        }
        if (Main.playerInventory)
        {
            ModContent.GetInstance<ArchaeologySystem>().ActivateUI();
        }
        else
        {
            ModContent.GetInstance<ArchaeologySystem>().DeactivateUI();
        }
        GlobalEffectMultiplier = 1;
        for (int i = 0; i < ArchaeologyActiveItem.Length; i++)
        {
            if (GetArchaeologyItemFromArr(ref i, out ArchaeologyModType type))
            {
                type.ResetEffect(this, Player);
            }
        }
    }
    public override void UpdateEquips()
    {
        for (int i = 0; i < ArchaeologyActiveItem.Length; i++)
        {
            if (GetArchaeologyItemFromArr(ref i, out ArchaeologyModType type))
            {
                type.UpdateEquips(this, Player);
                EffectMultiplier = 1;
                type.EffectMultiplier(this, Player, ref i, ref EffectMultiplier);
            }
        }
    }
    public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
    {
        for (int i = 0; i < ArchaeologyActiveItem.Length; i++)
        {
            if (GetArchaeologyItemFromArr(ref i, out ArchaeologyModType type))
            {
                type.OnHitNPCWithItem(this, Player, item, target, hit);
                EffectMultiplier = 1;
                type.EffectMultiplier(this, Player, ref i, ref EffectMultiplier);
            }
        }
    }
    public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
    {
        for (int i = 0; i < ArchaeologyActiveItem.Length; i++)
        {
            if (GetArchaeologyItemFromArr(ref i, out ArchaeologyModType type))
            {
                type.OnHitNPCWithProj(this, Player, proj, target, hit);
                EffectMultiplier = 1;
                type.EffectMultiplier(this, Player, ref i, ref EffectMultiplier);
            }
        }
    }
    public override bool OnPickup(Item item)
    {
        bool pickup = base.OnPickup(item);
        for (int i = 0; i < ArchaeologyActiveItem.Length; i++)
        {
            if (GetArchaeologyItemFromArr(ref i, out ArchaeologyModType type))
            {
                type.OnPickUp(this, Player, item, out bool noitem);
                if (noitem)
                {
                    pickup = false;
                }
                EffectMultiplier = 1;
                type.EffectMultiplier(this, Player, ref i, ref EffectMultiplier);
            }
        }
        return pickup;
    }
    public override void SaveData(TagCompound tag)
    {
        tag["ArchaeologyActiveItem"] = ArchaeologyActiveItem;
    }
    public override void LoadData(TagCompound tag)
    {
        ArchaeologyActiveItem = tag.Get<int[]>("ArchaeologyActiveItem");
    }
}
public abstract class ArchaeologyModType : ModType
{
    public int Type = 0;
    public static int GetArchaeologyType<T>() where T : ArchaeologyModType => ModContent.GetInstance<T>().Type;
    protected override void Register()
    {
        Type = ArchaeologySystem.Register(this);
    }
    public float GetEffectValue(ArchaeologyPlayer modplayer) => modplayer.EffectMultiplier * modplayer.GlobalEffectMultiplier;
    public virtual void EffectMultiplier(ArchaeologyPlayer modplayer, Player player, ref int index, ref float EffectValue) { }
    public virtual void ResetEffect(ArchaeologyPlayer modplayer, Player player) { }
    public virtual void UpdateEquips(ArchaeologyPlayer modplayer, Player player) { }
    public virtual void OnHitNPCWithItem(ArchaeologyPlayer modplayer, Player player, Item item, NPC target, NPC.HitInfo hit) { }
    public virtual void OnHitNPCWithProj(ArchaeologyPlayer modplayer, Player player, Projectile proj, NPC target, NPC.HitInfo hit) { }
    public virtual void OnPickUp(ArchaeologyPlayer modplayer, Player player, Item item, out bool noItem) { noItem = false; }
    /// <summary>
    /// This code is run on client side, so it is fine to use <see cref="Main.LocalPlayer"/>
    /// </summary>
    /// <param name="tileType"></param>
    public virtual void OnHKillTile(int i, int j, int tileType) { }
}
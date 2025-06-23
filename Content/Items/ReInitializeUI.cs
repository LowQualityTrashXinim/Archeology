using Terraria;
using Terraria.ModLoader;
using Archeology.Texture;
using Archeology.Content.ArcheologyEquipmentSystem;

namespace Archeology.Content.Items;
class ReInitializeUI : ModItem
{
    public override string Texture => ModTexture.MissingTexture_Default;
    public override void SetDefaults()
    {
        Item.BossRushDefaultToConsume(1, 1);
    }
    public override bool? UseItem(Player player)
    {
        if (player.ItemAnimationJustStarted)
        {
            ModContent.GetInstance<ArchaeologySystem>().ArchaeologyUI.RemoveAllChildren();
            ModContent.GetInstance<ArchaeologySystem>().ArchaeologyUI.OnInitialize();
            ModContent.GetInstance<ArchaeologySystem>().ArchaeologyUI.Activate();
        }
        return false;
    }
}


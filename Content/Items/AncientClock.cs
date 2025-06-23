using Archeology.Content.ArcheologyEquipmentSystem;
using Archeology.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace Archeology.Content.Items
{
    internal class AncientWatch : BaseItem
    {
        public override string Texture => ModUtils.GetVanillaTexture<Item>(ItemID.TinWatch);
        public override int ArchaeologyType => ArchaeologyModType.GetArchaeologyType<AncientWatch_Type>();
        public override void SetDefaults()
        {
            Item_SetDefaultModItem(32, 32);
        }
    }
}

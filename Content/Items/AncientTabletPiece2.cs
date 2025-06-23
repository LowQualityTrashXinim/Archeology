using Archeology.Content.ArcheologyEquipmentSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archeology.Content.Items;

internal class AncientTabletPiece2 : BaseItem
{
    public override int ArchaeologyType => ArchaeologyModType.GetArchaeologyType<AncientTabletPiece2_Type>();
    public override void SetDefaults()
    {
        Item_SetDefaultModItem(36, 46);
    }
}

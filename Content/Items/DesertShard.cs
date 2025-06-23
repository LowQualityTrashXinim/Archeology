using Archeology.Content.ArcheologyEquipmentSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archeology.Content.Items
{
    internal class DesertShard : BaseItem
    {
        public override int ArchaeologyType => ArchaeologyModType.GetArchaeologyType<DesertShard_Type>();
        public override void SetDefaults()
        {
            Item_SetDefaultModItem(34, 18);
        }
    }
}

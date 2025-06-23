using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Archeology.Content.Items;
public abstract class BaseItem : ModItem
{
    public void Item_SetDefaultModItem(int width, int height)
    {
        Item.width = width;
        Item.height = height;
    }
}

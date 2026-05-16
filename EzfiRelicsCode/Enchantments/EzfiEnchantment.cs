using BaseLib.Abstracts;
using BaseLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzfiRelics.EzfiRelicsCode.Extensions;

namespace EzfiRelics.EzfiRelicsCode.Enchantments
{
    public abstract class EzfiEnchantment : CustomEnchantmentModel
    {
        protected override string CustomIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".EnchantmentPath();
    }
}

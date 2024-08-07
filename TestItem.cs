using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using UnlimitedMod.system.DiabloItem;

namespace UnlimitedMod
{
	public class TestItem : ModItem
	{
		public override void SetDefaults() {
			Item.width = 40; // The item texture's width.
			Item.height = 40; // The item texture's height.

			Item.useStyle = ItemUseStyleID.Swing; // The useStyle of the Item.
			Item.useTime = 20; // The time span of using the weapon. Remember in terraria, 60 frames is a second.
			Item.useAnimation = 20; // The time span of the using animation of the weapon, suggest setting it the same as useTime.
			Item.autoReuse = true; // Whether the weapon can be used more than once automatically by holding the use button.

			Item.DamageType = DamageClass.Melee; // Whether your item is part of the melee class.
			Item.damage = 50; // The damage your item deals.
			Item.knockBack = 6; // The force of knockback of the weapon. Maximum is 20
			Item.crit = 6; // The critical strike chance the weapon has. The player, by default, has a 4% critical strike chance.

			Item.value = Item.buyPrice(gold: 1); // The value of the weapon in copper coins.
			Item.rare = ItemRarityID.Expert; // Give this item our custom rarity.
			Item.UseSound = SoundID.Item1; // The sound when the weapon is being used.
            Item.GetGlobalItem<DiabloItem>().LifeStolenPerHitPercentage = 20;
            //Item.GetGlobalItem<DiabloItem>().EnhancedDamage = 200f;
			Item.GetGlobalItem<DiabloItem>().HasChanceToCast = true;
			Item.GetGlobalItem<DiabloItem>().ChanceToCast = 0.05f;
			Item.GetGlobalItem<DiabloItem>().ProjID = ProjectileID.WaterBolt;
			Item.GetGlobalItem<DiabloItem>().PoisonDamage = 20;
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone) {
			// Inflict the OnFire debuff for 1 second onto any NPC/Monster that this hits.
			// 60 frames = 1 second
			target.AddBuff(BuffID.OnFire, 60);
		}
        public override void AddRecipes()
        {
            CreateRecipe() // 'mod' é passado automaticamente para o ModItem
                .AddIngredient(ItemID.IronOre, 10) // Adiciona 10 Minério de Ferro como ingrediente
                .AddTile(TileID.WorkBenches) // Adiciona a bancada de trabalho como local de criação da receita
                .Register(); // Registra a receita
        }

		// Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation
	}
}
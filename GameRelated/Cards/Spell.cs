using DSharpPlus.CommandsNext;
using Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects.ExtraEffectInfo;

namespace Scrap_Scramble_Final_Version.GameRelated.Cards
{
    public class Spell : Card
    {
        public SpellRarity rarity;

        public Spell()
        {
            this.rarity = SpellRarity.Spell;
            this.name = string.Empty;
            this.cardText = string.Empty;            
        }

        public Spell(string name, int cost, string cardText, SpellRarity rarity = SpellRarity.Spell)
        {
            this.rarity = rarity;
            this.name = name;
            this.cardText = cardText;
            this.Cost = cost;
        }

        public override Card DeepCopy()
        {
            Spell ret = (Spell)Activator.CreateInstance(this.GetType());
            ret.name = this.name;
            ret.rarity = this.rarity;
            ret.cardText = this.cardText;
            ret.Cost = this.Cost;
            return ret;
        }

        public override string GetInfo(GameHandler gameHandler, ulong player)
        {
            string ret;
            if (this.rarity == SpellRarity.Spare_Part) ret = $"{this.name} - Spare Part - {this.Cost} - {this.cardText}";
            else ret = $"{this.name} - {this.rarity} - {this.Cost} - {this.cardText}";
            return ret;
        }

        public override async Task<bool> PlayCard(int handPos, GameHandler gameHandler, ulong curPlayer, ulong enemy, CommandContext ctx)
        {
            gameHandler.players[curPlayer].curMana -= this.Cost;

            ExtraEffectInfo extraInf = new ExtraEffectInfo(ctx);
            await Effect.CallEffects(this.effects, Effect.EffectType.OnPlay, this, gameHandler, curPlayer, enemy, extraInf);

            await Effect.CallEffects(gameHandler.players[curPlayer].effects, Effect.EffectType.OnSpellCast, this, gameHandler, curPlayer, enemy, extraInf);
            await Effect.CallEffects(gameHandler.players[curPlayer].effects, Effect.EffectType.Spellburst, this, gameHandler, curPlayer, enemy, extraInf, true);

            return true;
        }

        public virtual Task CastOnUpgradeInShop(int shopPos, GameHandler gameHandler, ulong curPlayer, ulong enemy, CommandContext ctx) { return Task.CompletedTask; }
    }
}

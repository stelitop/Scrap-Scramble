using DSharpPlus.CommandsNext;
using Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated.Cards
{
    public class Upgrade : Card, IComparable<Upgrade>
    {
        public CreatureData creatureData = new CreatureData();
        public Rarity rarity;

        public UpgradeSet upgradeSet;

        public Upgrade()
        {
            this.rarity = Rarity.NO_RARITY;
            this.name = string.Empty;
            this.cardText = string.Empty;
            this.upgradeSet = UpgradeSet.Classic;
        }

        public Upgrade(UpgradeSet set, string name, int cost, int attack, int health, Rarity rarity, string cardText = "")        
        {
            this.upgradeSet = set;
            this.name = name;
            this.rarity = rarity;
            this.Cost = cost;
            this.creatureData.attack = attack;
            this.creatureData.health = health;
            this.cardText = cardText;
        }        

        public override string GetInfo(GameHandler gameHandler, ulong player)
        {
            string ret;
            string rarity = $"{this.rarity} - ";
            if (this.rarity == Rarity.NO_RARITY) rarity = string.Empty;

            if (this.cardText.Equals(string.Empty)) ret = $"{this.name} - {rarity}{this.Cost}/{this.creatureData.attack}/{this.creatureData.health}";
            else ret = $"{this.name} - {rarity}{this.Cost}/{this.creatureData.attack}/{this.creatureData.health} - {this.cardText}";

            if (this.creatureData.staticKeywords[StaticKeyword.Freeze] == 1) ret = $"(Frozen for 1 turn) {ret}";
            else if (this.creatureData.staticKeywords[StaticKeyword.Freeze] > 1) ret = $"(Frozen for {this.creatureData.staticKeywords[StaticKeyword.Freeze]} turns) {ret}";
            return ret;
        }

        public override string ToString()
        {
            string ret;
            string rarity = $"{this.rarity} - ";
            if (this.rarity == Rarity.NO_RARITY) rarity = string.Empty;

            if (this.cardText.Equals(string.Empty)) ret = $"{this.name} - {rarity}{this.Cost}/{this.creatureData.attack}/{this.creatureData.health}";
            else ret = $"{this.name} - {rarity}{this.Cost}/{this.creatureData.attack}/{this.creatureData.health} - {this.cardText}";

            return ret;
        }

        public virtual bool CanBeBought(int shopPos, GameHandler gameHandler, ulong curPlayer, ulong enemy)
        {
            if (shopPos >= gameHandler.players[curPlayer].shop.LastIndex) return false;
            if (this.name == BlankUpgrade.name) return false;
            if (this.creatureData.staticKeywords[StaticKeyword.Freeze] > 0) return false;
            //if (this.inLimbo) return false;
            if (this.Cost > gameHandler.players[curPlayer].curMana) return false;

            return true;
        }

        public override Card DeepCopy()
        {
            Upgrade ret = (Upgrade)Activator.CreateInstance(this.GetType());
            ret.name = this.name;
            ret.rarity = this.rarity;
            ret.cardText = this.cardText;
            ret.creatureData = this.creatureData.DeepCopy();
            ret.Cost = this.Cost;
            ret.effects.Clear();

            for (int i = 0; i < this.effects.Count(); i++)
                ret.effects.Add(this.effects[i].Copy());

            return ret;
        }

        public override async Task<bool> PlayCard(int handPos, GameHandler gameHandler, ulong curPlayer, ulong enemy, CommandContext ctx)
        {
            gameHandler.players[curPlayer].curMana -= this.Cost;

            await gameHandler.players[curPlayer].AttachMech(this, gameHandler, curPlayer, enemy, ctx);

            return true;
        }

        public virtual async Task<bool> BuyCard(int shopPos, GameHandler gameHandler, ulong curPlayer, ulong enemy, CommandContext ctx)
        {
            gameHandler.players[curPlayer].curMana -= this.Cost;

            ExtraEffectInfo extraInf = new ExtraEffectInfo(ctx);
            await Effect.CallEffects(gameHandler.players[curPlayer].effects, Effect.EffectType.OnBuyingAMech, this, gameHandler, curPlayer, enemy, extraInf);

            await gameHandler.players[curPlayer].AttachMech(this, gameHandler, curPlayer, enemy, ctx);
            return true;
        }

        public int CompareTo(Upgrade other)
        {
            if (this.rarity > other.rarity) return -1;
            else if (this.rarity < other.rarity) return 1;

            return this.name.CompareTo(other.name);
        }
        public virtual Upgrade BasicCopy(CardPool pool)
        {
            Card newCopy = pool.FindBasicCard(this.name);

            if (newCopy.name != BlankUpgrade.name) return (Upgrade)newCopy.DeepCopy();
            return (Upgrade)Activator.CreateInstance(this.GetType());
        }
    }
    public class BlankUpgrade : Upgrade
    {
        public new const string name = "Blank";

        public BlankUpgrade()
        {
            base.name = BlankUpgrade.name;

            this.upgradeSet = UpgradeSet.Classic;
            this.cardText = string.Empty;
            this.rarity = Rarity.NO_RARITY;            
        }

        public override string GetInfo(GameHandler gameHandler, ulong player)
        {
            return string.Empty;
        }
    }
}

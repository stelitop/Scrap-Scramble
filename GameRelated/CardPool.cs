﻿using Scrap_Scramble_Final_Version.GameRelated.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.GameRelated
{
    public class CardPool
    {
        public List<Upgrade> upgrades;
        public List<Spell> spareparts;
        public List<Card> tokens;

        public CardPool()
        {
            this.upgrades = new List<Upgrade>();
            this.spareparts = new List<Spell>();
            this.tokens = new List<Card>();
        }

        public CardPool(CardPool x)
        {            
            this.upgrades = new List<Upgrade>();
            this.spareparts = new List<Spell>();
            this.tokens = new List<Card>();            
            foreach (var card in x.upgrades)
            {
                this.upgrades.Add((Upgrade)card.DeepCopy());
            }
            
            foreach (var card in x.spareparts)
            {
                this.spareparts.Add((Spell)card.DeepCopy());                
            }
            foreach (var card in x.tokens)
            {
                this.tokens.Add((Card)card.DeepCopy());                
            }
        }

        public CardPool(List<Upgrade> mechs)
        {
            this.upgrades = new List<Upgrade>(mechs);
        }

        public void GenericMinionPollSort()
        {
            this.upgrades.Sort();
        }

        public void FillGenericCardPool()
        {
            this.upgrades = new List<Upgrade>();

            var allMechClasses =
                // Note the AsParallel here, this will parallelize everything after.
                from a in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(UpgradeAttribute), true)
                where attributes != null && attributes.Length > 0
                select new { Type = t, Attributes = attributes.Cast<UpgradeAttribute>() };

            foreach (var x in allMechClasses)
            {
                this.upgrades.Add((Upgrade)(Activator.CreateInstance(x.Type)));
            }

            this.FillSpareParts();
            this.FillTokens();

            this.GenericMinionPollSort();
        }

        public List<string> FillCardPoolWithSets(int setsAmount, SetHandler setHandler)
        {
            List<string> ret = new List<string>();

            this.upgrades = new List<Upgrade>();

            List<string> packagesList = new List<string>();
            foreach (var package in setHandler.Sets)
            {
                packagesList.Add(package.Key);
            }

            packagesList = packagesList.OrderBy(x => GameHandler.randomGenerator.Next()).ToList();

            for (int i = 0; i < setsAmount && i < packagesList.Count(); i++)
            {
                ret.Add(packagesList[i]);
                for (int j = 0; j < setHandler.Sets[packagesList[i]].Count(); j++)
                {
                    this.upgrades.Add((Upgrade)setHandler.Sets[packagesList[i]][j].DeepCopy());
                }
            }

            this.FillSpareParts();
            this.FillTokens();

            this.GenericMinionPollSort();

            return ret;
        }

        public void FillSpareParts()
        {
            this.spareparts = new List<Spell>();

            var allSparePartClasses =
                // Note the AsParallel here, this will parallelize everything after.
                from a in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(SparePartAttribute), true)
                where attributes != null && attributes.Length > 0
                select new { Type = t, Attributes = attributes.Cast<SparePartAttribute>() };

            foreach (var x in allSparePartClasses)
            {
                this.spareparts.Add((Spell)(Activator.CreateInstance(x.Type)));
            }
        }

        public void FillTokens()
        {
            this.tokens = new List<Card>();
            var allTokenClasses =
                // Note the AsParallel here, this will parallelize everything after.
                from a in AppDomain.CurrentDomain.GetAssemblies().AsParallel()
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(TokenAttribute), true)
                where attributes != null && attributes.Length > 0
                select new { Type = t, Attributes = attributes.Cast<TokenAttribute>() };

            foreach (var x in allTokenClasses)
            {
                this.tokens.Add((Card)Activator.CreateInstance(x.Type));
            }
        }

        public void PrintMechNames()
        {
            foreach (var x in this.upgrades) Console.WriteLine(x.name);
        }

        public Card FindBasicCard(string name)
        {
            foreach (var x in this.tokens)
            {
                if (x.name == name) return x;
            }
            foreach (var x in this.spareparts)
            {
                if (x.name == name) return x;
            }
            foreach (var x in this.upgrades)
            {
                if (x.name == name) return x;
            }

            return new BlankUpgrade();
        }
    }
}

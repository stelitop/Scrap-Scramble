using DSharpPlus.CommandsNext;
using Microsoft.Extensions.Logging;
using Scrap_Scramble_Final_Version.BotRelated;
using Scrap_Scramble_Final_Version.GameRelated;
using Scrap_Scramble_Final_Version.GameRelated.Cards;
using Scrap_Scramble_Final_Version.GameRelated.Cards.CardEffects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Scrap_Scramble_Final_Version.GameRelated.Cards.Upgrades.Sets.EdgeOfScience;
using static Scrap_Scramble_Final_Version.GameRelated.Cards.Upgrades.Sets.MonstersReanimated;
using static Scrap_Scramble_Final_Version.GameRelated.Cards.Upgrades.Sets.WarMachines;

namespace Scrap_Scramble_Final_Version
{    
    public partial class Form1 : Form
    {              
        public Form1()
        {            
            InitializeComponent();


            var tr = new ControlWriter(ListBoxLogging);

            Console.SetOut(tr);
            Console.SetError(tr);            
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async void ButtonBotStart_click(object sender, EventArgs e)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {            
            ButtonBotStart.Enabled = false;
            BotHandler.DiscordBot.RunAsync();            
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            
        }

        private void ButtonGetUserStates_Click(object sender, EventArgs e)
        {
            ListBoxTesting.Items.Clear();

            foreach (var user in BotHandler._userState)
            {
                ListBoxTesting.Items.Add($"{user.Key}: {user.Value}");
            }
        }

        private void ButtonGetSetBreakdown_Click(object sender, EventArgs e)
        {
            ListBoxTesting.Items.Clear();

            int totalCards = 0;

            foreach (var set in BotHandler.setHandler.Sets)
            {
                string msg = string.Empty;
                msg += $"{set.Key}: ";
                int _c=0, _r=0, _e=0, _l=0;

                foreach (var upgrade in set.Value)
                {
                    switch (upgrade.rarity)
                    {
                        case Rarity.Common:
                            _c++;
                            break;
                        case Rarity.Rare:
                            _r++;
                            break;
                        case Rarity.Epic:
                            _e++;
                            break;
                        case Rarity.Legendary:
                            _l++;
                            break;
                        default:
                            break;
                    }

                    totalCards++;
                }

                msg += $"{_l}/{_e}/{_r}/{_c} - Total : {_l+_e+_r+_c}";
                ListBoxTesting.Items.Add(msg);
            }
            ListBoxTesting.Items.Add($"Total Upgrades: {totalCards}");
        }
    }

    //public class ControlWriter : TextWriter
    //{
    //    private ListBox textbox;
    //    public ControlWriter(ListBox textbox)
    //    {
    //        this.textbox = textbox;
    //    }

    //    public override void Write(string value)
    //    {
    //        textbox.Items.Add(value);
    //    }

    //    public override Encoding Encoding
    //    {
    //        get { return Encoding.ASCII; }
    //    }
    //}

    public class ControlWriter : TextWriter
    {
        private ListBox listBox;
        public ControlWriter(ListBox lb)
        {
            this.listBox = lb;
        }

        public override void Write(string value)
        {
            if (this.listBox.InvokeRequired)
            {
                this.listBox.Invoke(new Action<string>(Write), new object[] { value });
            }
            else
            {
                this.listBox.Items.Add(value.Trim());
            }
        }
        public override void WriteLine(string value)
        {
            if (this.listBox.InvokeRequired)
            {
                this.listBox.Invoke(new Action<string>(Write), new object[] { value });
            }
            else
            {
                this.listBox.Items.Add(value.Trim());
            }
        }

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }
    }
}

using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrap_Scramble_Final_Version.BotRelated
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class UserStateAttribute : CheckBaseAttribute
    {   
        public UserState[] States { get; private set; }

        public UserStateAttribute(UserState state)
        {
            this.States = new UserState[1];
            this.States[0] = state;
        }
        public UserStateAttribute(UserState[] states)
        {
            this.States = states;
        }

        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {            
            return Task.FromResult(this.States.Contains(BotHandler.GetUserState(ctx.User.Id)));
        }
    }
}

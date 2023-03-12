# Scrap Scramble
Scrap Scramble is a community-made game, inspired in theme by Hearthstone. It is an autobattler, except each player only has one
unit that they upgrade over the course of the game. This is a discord bot that allows people to play it online through discord.

# A note about running the bot
Unfortunately, due to the changes in the Discord API, the bot cannot be run in its currnent states. The game logic can still be extracted and ran separately.

# Rules of the game
Every player starts with a Mech that has 1 Attack and 1 Health. The game cycles between a shopping stage and a combat stage. In the shopping stage you can buy
Upgrades for your Mech and do other things, while in the Combat stage you combat a random opponent. Players start with a set amount of lives and if they lose
that many fights, they're out of the game. Only the final player wins!

## The shopping stage
The player is presented with a shop of 10 Upgrades they can buy. They each have a **Mana** cost. The player starts turn 1 with 10 Mana available and each next round
they start with 5 more (so 15, 20, 25...). The Upgrades give your Mech Attack, Health and have a variety of other useful effects. The player has to strategically buy
Upgrades to overcome their opponents.

## The combat stage
For each pair of players fighting each other, one attacks first, after which they alternate. Determening who goes first is explained later. During an attack, the
attacker inflicts damage to their opponent equal to their Attack, which is subtracted from the opponent's Health. The first player to go below 0 Health loses. After
the fight, the Mechs are healed back to their previous state. On the next shopping page they only keep their Attack and Health, not any other effects.

## Keywords and Effects
Keywords are a type of simple effect that appears on many Upgrades. Most of them have a numeric value that stacks if multiple Upgrades that have it are applied. Below
is a list of such keywords:
- Rush X - The player gets X priority stat
- Taunt X - The player loses X priority stat
- Tiebreaker X - Secondary priority stat
- Spikes X - The Mech's first attack deals X more damage
- Shields X - The Mech's takes X less damage from the opponent's first attack
- Overload X - Next turn the player has X less Mana.
- Binary - After you buy this Upgrade, get a second copy in your **Hand** without Binary.
- Magnetic - After buying, choose a **Spare Part** to add to your hand.
- Poisonous - ANY damage dealt by this Mech is lethal.
- Echo - After buying this Upgrade, add a new copy of it to the shop.
- Frozen - Frozen Upgrades in the shop are not replaced at the start of the next shopping stage.

Priority stats determine who attacks first in combat. The person with more goes first, or if tied, the person with a bigger Tiebreaker. In case of a second tie,
it is randomly determined.

On the topic of "the hand", this is something like a persistent shop. It does not get replaced each turn and the Upgrades or other "Cards" can be bought from it
at any time. The slightly confusing term "the hand" comes from the game's inspiration from Hearthstone.

There are also Upgrades that support much more complicated effects. Some examples of such effects are given below:
- Battlecry: X - When this Upgrade is bought, do X.
- Aftermath: X - After the combat stage and at the start of the next shopping stage, do X.
- After you buy an Upgrade, do X
- etc...

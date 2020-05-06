# Code Magic
A small Roguelike RPG with ASCII graphics and programmable magic based on **physics**.
The idea of this game is to allow player (mage) to create any spells he or she wants with maximal flexibility. And then test your spells in randomly-generated multileveled dungeon filled with monsters and loot.
## Random
Almost every part of the game is generated randomly: random levels, random monsters, random loot and even random items. Yes, random items! There are more than 4500 possible visual apperances for swords. And it is only visuals, only swords and without counting different materials. This makes each dungeon trip completely unique and challenging!
## Magic System
All magic in Code Magic wold is based on real-life physics. You will not find here all these "casual" spells that instantly heal your wounds or damage enemies. Instead of this you should use your brain to create something really powerfull. But don't forget about physics! Otherwise you will burn not only your opponent but yourself as well.
### Spells
All spells in Code Magic must be written manually by the mage. There are no ready-to-use "Meteor Shower" or "Ice Bolt" here. You write your spell's code, channel it with Mana and it becomes alive! Well, partially alive. There are no limitations on how complex logic of your spell can be. Want to create some super-intelligent AI spell that will wipe-out entire dungeon for you? No problem!

Simple Shock spell example:
```JavaScript
function main(lifeTime) { // This function is called every time while spell has mana
  if (lifeTime === 0) { // if the spell was just casted
    var direction = getCaster().direction; // get where spell caster is looking
    return move(direction, 1); // move away 1 cell from caster towards his sight
  }
  
  return shock(5); // Deal some small electric shock to everything located in spell's cell.
}
```
Simple spell with simple function: shock adjusted enemy with electricity. But then **physics** come in game: if there are some water (or wet creatures) in current cell and nearby electricity will "jump" to adjusted cells damaging them too. Don't forget to self-insulate before doing this!
## Mana
Mana in game is not just number next to your health value. It is the essence of the world which is spreaded arround like a gas. A mage can collect it and keep inside his or her body to use with spells or other rituals. However taking more than world can give leads to some bad consequences.
## Physics
When playing with magic you actually are just utilizing standard physics effect to deal damage, protect yourself or do some other stuff you want. The following physics is now implemented:
- Temperature (spreading heat and cold)
- Pressure (from temperature changes and evaporation)
- Liquids (water, acid, oil - with their specific effects + evaporation and freezing)
- Electricity (electric change spreads from 1 wet surface to another damaging everything on it's way)

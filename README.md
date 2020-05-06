# Code Magic
A small Roguelike RPG with ASCII graphics and programmable magic based on **physics**.
# Magic System
All magic in Code Magic wold is based on real-life physics. You will not find here all these "casual" spells that instantly heal your wounds or damage enemies. Instead of this you should use your brain to create something really powerfull. But don't forget about physics! Otherwise you will burn not only your opponent but yourself as well.
# Spells
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

# Code Magic
A small Roguelike RPG with ASCII graphics and programmable magic based on physics.
# Magic System
All magic in Code Magic wold is based on real-life physics. You will not find here all these "casual" spells that instantly head your wounds or damage enemies. Instead of this you should use your brain to create something really powerfull. But don't forget about physics! Otherwise you will burn not only your opponent but yourself as well.
# Spells
All spells in Code Magic must be written manually by the mage. There are no ready-to-use "Meteor Shower" or "Ice Bolt" here. You write your spell's code, channel it with Mana and it becomes alive! Well, partially alive. There are no limitations on how complex logic of your spell can be. Want to create some super-intelligent AI spell that will wipe-out entire dungeon for you? No problem!
## Spell Actions
Your spell won't do anything on its own, it should call an action. There are a bunch of actions in the game but here are some of them:
- `heat(temperature: int)` - Heats current area for <temperature> degrees.
- `move(direction: string, distance: int)` - Moves spell in specified direction for specified distance.
- `push(direction: string, force: int)` - Pushes object in current area to specified direction with specified force.

All actions consume Mana from the spell's internal storage. Once the spell is out of mana, it disappears.
The spell can also collect some data about it's surroundings, there are special functions for this. Some of them are free, some of them cost Mana.

Simple Fire Breathe spell example:
```JavaScript
function main(lifeTime) {
  var direction = getCaster().direction; // get where spell caster is looking
  if (lifeTime === 0) { // if the spell was just casted
    move(direction, 1); // move away from caster towards his sight
  }
  
  heat(1000); // burn anything next to caster
}
```

// List of supported functions:
//
// getMana(): int - Gets current spell mana level.
// getCaster(): Creature - Gets spell caster.
// getPosition(): Point - Gets current spell position.
// getTemperature(): int - Gets temperature in current spell position.
// getIsSolidWall(direction: string): bool - Gets if there is a wall in specified direction.
// getAreObjectsUnder(): bool - Gets if there are any objects in current spell position.
//
// storeValue(key: string, value: object) - Stores specified value for the specified key. This value can be used on further iterations.
// getStoredValue(key: string): object - Get previously stored value.
//
// scanForWalls(radius: int): bool[][] - Scans map in specified radius for walls and returns map. Will return null if not enough mana.
// scanForObjects(radius: int): Object[][][] - Scans map in specified radius for objects or creatures. Will return null if not enough mana.
//
// List of supported actions:
//
// move(direction: string, distance: int): Action - Moves spell in specified direction for specified distance.
// buildWall(time: int): Action - Places an energy wall in current spell location. Wall will exist for the specified time.
// heat(temperature: int): Action - Heats current area for <temperature> degrees.
// cool(temperature: int): Action - Cools current area for <temperature> degrees.
//
// List of custom types
//
// Point { x: int, y: int }
// Object { id: string, health: int, maxHealth: int }
// Creature { id: string, health: int, maxHealth: int, direction: string }
// Action { type: string, manaCost: int }

function main(lifeTime) {
    // Write your code here.
    return move("up", 1);
}

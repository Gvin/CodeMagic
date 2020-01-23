/* ------------------------------------------------------------------------------------
 * List of supported functions:
 *
 * log(message: string) - Logs specified message to the journal.
 * getMana(): int - Gets current spell mana level.
 * getCaster(): Object - Gets spell caster.
 * getPosition(): Point - Gets current spell position.
 * getLightLevel(): int - Gets light level in current spell position.
 * getTemperature(): int - Gets temperature in current spell position.
 * getIsSolidWall(direction: string): bool - Gets if there is a wall in specified direction.
 * getObjectsUnder(): Object[] - Gets all objects in current spell position.
 *
 * storeValue(key: string, value: object) - Stores specified value for the specified key. This value can be used on further iterations.
 * getStoredValue(key: string): object - Get previously stored value.
 *
 * scanForWalls(radius: int): bool[][] - Scans map in specified radius for walls and returns map. Will return null if not enough mana.
 * scanForObjects(radius: int): Object[] - Scans map in specified radius for objects or creatures. Will return null if not enough mana.
 *
 * ------------------------------------------------------------------------------------
 * List of supported actions:
 *
 * move(direction: string, distance: int): Action - Moves spell in specified direction for specified distance.
 * buildWall(time: int): Action - Places an energy wall in current spell location. Wall will exist for the specified time.
 * heat(temperature: int): Action - Heats current area for <temperature> degrees.
 * cool(temperature: int): Action - Cools current area for <temperature> degrees.
 * push(direction: string, force: int): Action - Pushes object in current area to specified direction with specified force.
 * compress(pressure: int): Action - Increases air pressure in current spell location for <pressure> kPa.
 * decompress(pressure: int): Action - Decreases air pressure in current spell location for <pressure> kPa.
 * createWater(volume: int): Action - Creates <volume> liters of water in current spell location.
 * longCast(action: Action, direction: string, distance: int): Action - Casts specified action in place located to <distance> meters in specified direction. This will cost much more mana.
 * transformWater(targetLiquidName: string, volume: int): Action - Transforms specified volume of water in current spell location into the same volume of specified liquid. targetLiquidName can be oil or acid.
 * shock(power: int): Action - Performs electric shock of specified power in current spell location.
 * emitLight(power: int, time: int): Action - Makes spell to emit light of specified power for specified period of time.
 *
 * ------------------------------------------------------------------------------------
 * List of custom types
 *
 * Point { x: int, y: int }
 * Object { id: string, health: int, maxHealth: int, position: Point, direction: string, type: string } - direction exists only for creatures
 * Action { type: string, manaCost: int }
 * ------------------------------------------------------------------------------------
 */

function main(lifeTime) {
    // Write your code here.
    return move("north", 1);
}

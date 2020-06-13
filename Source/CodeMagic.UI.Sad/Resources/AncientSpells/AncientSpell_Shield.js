function main(lifeTime) {
    var position = getPosition();
    var casterPosition = getCaster().position;
	
	if (position.x === casterPosition.x && position.y === casterPosition.y) {
		var nearObjects = scanForObjects(1);
		if (nearObjects && nearObjects.length > 1) {
			return buildWall(2);
		}
	}
	
    if (casterPosition.x > position.x) {
        return move("east", 1);
    }
    if (casterPosition.x < position.x) {
        return move("west", 1);
    }
    if (casterPosition.y > position.y) {
        return move("south", 1);
    }
    if (casterPosition.y < position.y) {
        return move("north", 1);
    }
}
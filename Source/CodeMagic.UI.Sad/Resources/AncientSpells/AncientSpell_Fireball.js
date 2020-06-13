function getDirection(lifeTime) {
	if (lifeTime === 0) {
		var direction = getCaster().direction;
		storeValue("direction", direction);
		return direction;
	}
	
	return getStoredValue("direction");
}

function main(lifeTime) {
    var direction = getDirection(lifeTime);
	
	if (lifeTime === 0 || (getObjectsUnder().length === 0 && !getIsSolidWall(direction))) {
		return move(direction, 1);
	}
	
	var action = heat(500);
	if (getMana() < action.manaCost && getTemperature() <= 3000) {
		return action;
	}

	return heat(100);
}
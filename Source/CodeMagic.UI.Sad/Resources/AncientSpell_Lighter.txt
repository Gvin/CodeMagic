function setRemainingLightTime(value) {
	storeValue("remainingLight", value);
}

function getRemainingLightTime(lifeTime) {
	if (lifeTime === 0) {
		setRemainingLightTime(0);
		return 0;
	}
	return getStoredValue("remainingLight");
}

function main(lifeTime) {
	var remainingLightTime = getRemainingLightTime(lifeTime);
	
	if (getLightLevel() < 3) {
		if (remainingLightTime <= 0) {
			setRemainingLightTime(10);
			return emitLight(5, 10);
		}
	}
	
	setRemainingLightTime(remainingLightTime - 1);

    var position = getPosition();
    var casterPosition = getCaster().position;
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
function main(lifeTime) {
    if (lifeTime === 0) {
		return move(getCaster().direction, 1);
	}
	return shock(10);
}
function serverCmdHome(%client) {
	

	if($Sim::Time - %client.lastHomeTime < 90) {
		%client.centerPrint("You're trying to go home too early. Try again in" SPC convTime(90 - ($Sim::Time - %client.lastHomeTime), 0) @ ".", 3);
		return;
	}

	if(isObject(%pl = %client.player)) {
		
		%lotList = %client.brickGroup.lotList;
		
		if(getWordCount(%lotList) < 1) {
			%client.centerPrint("You don't have a home - sending you back to spawn! <br><font:palatino linotype:36>If your lot is missing, talk to the lot manager!", 8);
			
			if (isObject(%pl.getObjectMount())) {
				%pl.dismount();
			}

			%pb = new Projectile() {
				dataBlock = "deathProjectile";
				initialVelocity = "0 0 0";
				initialPosition = %pl.getTransform();
				sourceObject = %pl;
				sourceSlot = 0;
				client = %client;
			};
			
			%pb.setScale(%pl.getScale());
			MissionCleanup.add(%pb);
			
			%client.lastHomeTime = $Sim::Time;
			%pl.setTransform(_globalSpawn.getTransform());
			
			%pa = new Projectile() {
				dataBlock = "deathProjectile";
				initialVelocity = "0 0 0";
				initialPosition = %pl.getTransform();
				sourceObject = %pl;
				sourceSlot = 0;
				client = %client;
			};

			return;
		}
		
		for(%i = 0; %i < getWordCount(%lotList); %i++) {
			
			%lot = getWord(%lotList, %i);
			
			if(isObject(%lot)) {
				
				if(%client.score < 1) {
					%client.centerPrint("You don't have enough money. You need at least $1.", 3);
					return;
				}
				if(%lot.getDataBlock().isLot && %lot.getDataBlock().isSingle) {
					
					if (isObject(%veh = %pl.getObjectMount()))
					{
						%pl.dismount();
						%veh.spawnBrick.recoverVehicle();
					}
					
					%pb = new Projectile() {
						dataBlock = "deathProjectile";
						initialVelocity = "0 0 0";
						initialPosition = %pl.getTransform();
						sourceObject = %pl;
						sourceSlot = 0;
						client = %client;
					};
					
					%pb.setScale(%pl.getScale());
					MissionCleanup.add(%pb);
					
					%client.score--;
					%client.lastHomeTime = $Sim::Time;
					%pl.setTransform(%lot.getTransform());
					
					%pa = new Projectile() {
						dataBlock = "deathProjectile";
						initialVelocity = "0 0 0";
						initialPosition = %pl.getTransform();
						sourceObject = %pl;
						sourceSlot = 0;
						client = %client;
					};
					
					%pa.setScale(%pl.getScale());
					MissionCleanup.add(%pa);
				}
			}
		}
	}
}
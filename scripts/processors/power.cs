//dataID tags:
//	powerType - determines if the brick is a generator, processor, powercontrolbox, or battery
//	isPoweredOn - determines if the brick is running
//	brickName - the name of the brick associated with this processor
//	powerControlBox - the power control box dataID this processor is connected to
//
//Generators check internal inventory for resources to burn when powering a system
//generator fields:
//	isGenerator - true
//	burnRate - # of resources to burn per tick
//	generation - # of watts created per tick
//	fuelType - space-delimited stacktypes of the fuels it accepts
//
//Powered Processors draw power, and if supplied enough, run their internal tasks
//processor fields:
//	isPoweredProcessor - true
//	energyUse - # of watts consumed per tick
//	powerFunction - function to call with power ratio
//
//Batteries pick up excess power from the network, and discharge it when generation is not enough
//battery fields:
//	isBattery - true
//	dischargeRate - maximum discharge/charge rate (watts per tick)
//	capacity - maximum storable watts per tick
//tags:
//	charge - amount of watt-ticks stored
//
//Power Control Boxes display network power information and connect generators to powered processors
//powercontrolbox fields:
//	isPowerControlBox - true
//	maxGenerators - maximum number of attached generators
//	maxProcessors - maximum number of attached processors
//	maxBatteries - maximum number of attached batteries
//	tickTime - seconds between ticks


if (!isObject(PowerControlSimSet)) 
{
	$PowerControlSimSet = new SimSet(PowerControlSimSet) {};
}

package PowerSystems
{
	function fxDTSBrick::onAdd(%brick) 
	{
		%ret = parent::onAdd(%brick);

		if (%brick.isPlanted && %brick.getDatablock().isPowerControlBox)
		{
			PowerControlSimSet.add(%brick);
		}
		return %ret;
	}

	function addStorageEvent(%brick)
	{
		parent::addStorageEvent(%brick);
		%db = %brick.getDatablock();
		if (%db.isGenerator || %db.isPoweredProcessor || %db.isPowerControlBox || %db.isBattery)
		{
			if (%db.isGenerator) %type = "generator";
			else if (%db.isPoweredProcessor) %type = "processor";
			else if (%db.isBattery) %type = "battery";
			else if (%db.isPowerControlBox) %type = "powerControlBox";

			if (%type !$= "")
			{
				%dataID = %brick.eventOutputParameter0_1;
				%brickName = "_" @ getSubStr(getRandomHash(), 0, 20) @ "Power";
				while (isObject(%brickName))
				{
					%brickName = "_" @ getSubStr(getRandomHash(), 0, 20) @ "Power";
				}
				%brick.setName(%brickName);
				setDataIDArrayTagValue(%dataID, "powerType", %type);
				setDataIDArrayTagValue(%dataID, "brickName", %brickName);
			}
		}
	}

	function insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID)
	{
		if (%storageObj.getDatablock().isGenerator && !%storageObj.isAcceptingFuel)
		{
			return 3;
		}
		else if (%storageObj.getDatablock().isPowerControlBox)
		{
			return 2;
		}
		return parent::insertIntoStorage(%storageObj, %brick, %dataID, %storeItemDB, %insertCount, %itemDataID);
	}

	function fxDTSBrick::updateStorageMenu(%brick, %dataID)
	{
		%ret = parent::updateStorageMenu(%brick, %dataID);
		%db = %brick.getDatablock();
		if (%db.isGenerator)
		{
			%brick.centerprintMenu.menuOptionCount = 2;
			%brick.centerprintMenu.menuOption[1] = "Power: " @ (%brick.isPoweredOn() ? "\c2On" : "\c0Off");
			%brick.centerprintMenu.menuFunction[0] = "reopenCenterprintMenu";
			%brick.centerprintMenu.menuFunction[1] = "togglePower";
		}
		else if (%db.isPowerControlBox)
		{
			%brick.centerprintMenu.menuOptionCount = 8;
			%brick.centerprintMenu.menuOption[0] = "\c5v Diagnostics v";
			if (%brick.isInputOn())
				%brick.centerprintMenu.menuOption[1] = "Producing " @ %brick.totalGeneratedPower + 0 @ " watts";
			else
				%brick.centerprintMenu.menuOption[1] = "[Production off]";

			if (%brick.isOutputOn())
				%brick.centerprintMenu.menuOption[2] = "Using " @ %brick.totalPowerUsage + 0 @ " watts";
			else
				%brick.centerprintMenu.menuOption[2] = "[Energy usage off]";

			if (%brick.getBatteryMode() $= "Charging")
				%color = "\c2";
			%brick.centerprintMenu.menuOption[3] = %color @ "Battery: " @ %brick.totalBatteryPower + 0 @ " watt-ticks"; 
			%brick.centerprintMenu.menuOption[4] = "\c5v Controls v";
			%brick.centerprintMenu.menuOption[5] = "Input: " @ (%brick.isInputOn() ? "\c2On" : "\c0Off");
			%brick.centerprintMenu.menuOption[6] = "Output: " @ (%brick.isOutputOn() ? "\c2On" : "\c0Off");
			%brick.centerprintMenu.menuOption[7] = "Battery Mode: " @ (%brick.getBatteryMode());

			%brick.centerprintMenu.menuFunction[0] = "reopenCenterprintMenu";
			%brick.centerprintMenu.menuFunction[1] = "reopenCenterprintMenu";
			%brick.centerprintMenu.menuFunction[2] = "reopenCenterprintMenu";
			%brick.centerprintMenu.menuFunction[3] = "reopenCenterprintMenu";
			%brick.centerprintMenu.menuFunction[4] = "reopenCenterprintMenu";
			%brick.centerprintMenu.menuFunction[5] = "toggleInputOn";
			%brick.centerprintMenu.menuFunction[6] = "toggleOutputOn";
			%brick.centerprintMenu.menuFunction[7] = "toggleBatteryMode";
		}
		else if (%db.isBattery)
		{
			%brick.centerprintMenu.menuOptionCount = 2;
			%brick.centerprintMenu.menuOption[0] = "Stored: " @ getDataIDArrayTagValue(%dataID, "charge") + 0
					 @ "/" @ %db.capacity;
			%brick.centerprintMenu.menuOption[1] = "Power: " @ (%brick.isPoweredOn() ? "\c2On" : "\c0Off");

			%brick.centerprintMenu.menuFunction[0] = "reopenCenterprintMenu";
			%brick.centerprintMenu.menuFunction[1] = "togglePower";
		}
		else if (%db.isPoweredProcessor)
		{
			%brick.centerprintMenu.menuOption[0] = "Power: " @ (%brick.isPoweredOn() ? "\c2On" : "\c0Off");
			%brick.centerprintMenu.menuFunction[0] = "togglePower";
		}
		return %ret;
	}

	function removeStack(%cl, %menu, %option)
	{
		%brick = %menu.brick;
		if (isObject(%brick) && %brick.getDatablock().isGenerator)
		{
			return;
		}
		return parent::removeStack(%cl, %menu, %option);
	}
};
activatePackage(PowerSystems);

function powerTick(%index)
{
	cancel($masterPowerTickSchedule);
	
	if (!isObject(MissionCleanup)) 
	{
		return;
	}

	//if no Power bins just skip everything
	%count = PowerControlSimSet.getCount();
	if (%count <= 0)
	{
		$masterPowerTickSchedule = schedule(100, 0, PowerTick, %index);
		return;
	}

	for (%i = 0; %i < %count; %i++)
	{
		if (%index >= %count)
		{
			break;
		}
		%brick = PowerControlSimSet.getObject(%index);

		if (%brick.nextPowerCheck < getSimTime())
		{
			powerCheck(%brick);
		}
		%index++;
	}

	if (%index >= %count)
	{
		%index = 0;
	}

	$masterPowerTickSchedule = schedule(100, 0, PowerTick, %index);
}

function powerCheck(%brick)
{
	%db = %brick.getDatablock();
	%dataID = %brick.eventOutputParameter0_1;
	%brick.nextPowerCheck = getSimTime() | 0 + (%db.tickTime * 1000) | 0;
	if (!%db.isPowerControlBox)
	{
		return;
	}
	if (%brick.debugPower)
	{
		talk("Calling powercheck on " @ %brick);
	}

	for (%i = 0; %i < getDataIDArrayCount(%dataID); %i++)
	{
		%subDataID = getDataIDArrayValue(%dataID, %i);
		%type = strLwr(getDataIDArrayTagValue(%subDataID, "powerType"));
		%brickName = getDataIDArrayTagValue(%subDataID, "brickName");
		if (isObject(%brickName))
		{
			switch$ (%type)
			{
				case "generator":
					%gen[%genCount++ - 1] = %subDataID;
					%genName[%genCount - 1] = %brickName;
				case "processor":
					%pro[%proCount++ - 1] = %subDataID;
					%proName[%proCount - 1] = %brickName;
				case "battery":
					%bat[%batCount++ - 1] = %subDataID;
					%batName[%batCount - 1] = %brickName;
			}
		}
	}
	%inputOn = !getDataIDArrayTagValue(%dataID, "isInputOff");
	%outputOn = !getDataIDArrayTagValue(%dataID, "isOutputOff");
	%batteryChargeOnly = getDataIDArrayTagValue(%dataID, "batteryMode");

	%totalGeneratedPower = 0;
	for (%i = 0; %i < %genCount; %i++)
	{
		%on = getDataIDArrayTagValue(%gen[%i], "isPoweredOn");
		%brickName = %genName[%i];

		if (%brickName.getDatablock().isGenerator && %on && %inputOn)
		{
			%genDB = %brickName.getDatablock();
			%powerGen = %genDB.generation;
			%burn = %genDB.burnRate;

			%fuelStorage = getDataIDArrayValue(%gen[%i], 1);
			%fuelType = getField(%fuelStorage, 0);
			%count = getField(%fuelStorage, 1);

			if (%count > 0 && %brickname.canAcceptFuel(%fuelType)) //has fuel, burn some and generate power
			{
				%totalGeneratedPower += %powerGen;
				if (%count - %burn > 0)
					%newCount = mFloatLength(%count - %burn, 2);
				else
					%newCount = 0;
				setDataIDArrayValue(%gen[%i], 1, %fuelType TAB %newCount TAB getField(%fuelStorage, 2));

				if (!isObject(%brickName.audioEmitter))
				{
					%brickName.setMusic("musicData_Bass_1");
				}
				%brickname.updateStorageMenu(%gen[%i]);
			}
			else if (isObject(%brickName.audioEmitter))
			{
				%brickName.setMusic("None");
			}
			%genOnCount++;
		}
		else if (isObject(%brickName.audioEmitter))
		{
			%brickName.setMusic("None");
		}
	}

	%totalPowerUsage = 0;
	for (%i = 0; %i < %proCount; %i++)
	{
		%on = getDataIDArrayTagValue(%pro[%i], "isPoweredOn");
		%brickName = %proName[%i];

		if (%brickName.getDatablock().isPoweredProcessor && %on && %outputOn)
		{
			%pro_on[%pro_onCount++ - 1] = %brickName.getID();
			%proDB = %brickName.getDatablock();
			%powerDraw = %proDB.energyUse;

			%totalPowerUsage += %powerDraw;
			%proOnCount++;
		}
		else if (isFunction(%brickName.getDatablock().powerFunction)) //its off, so make sure it has 0 power
		{
			call(%brickName.getDatablock().powerFunction, %brickName.getID(), 0);
		}
	}

	%powerDiff = %totalGeneratedPower - %totalPowerUsage;
	%totalBatteryPower = 0;
	%batteryDischarge = 0;
	for (%i = 0; %i < %batCount; %i++)
	{
		%on = getDataIDArrayTagValue(%bat[%i], "isPoweredOn");
		%brickName = %batName[%i];

		if (%brickName.getDatablock().isBattery && %on)
		{
			%bat_on[%bat_onCount++ - 1] = %brickName.getID();
			%chargeAvailable = getDataIDArrayTagValue(%bat[%i], "charge");
			%batDB = %brickName.getDatablock();
			%discharge = getMin(%batDB.dischargeRate, %chargeAvailable);
			%max = %batDB.capacity;

			if (%powerDiff < 0 && %discharge > 0 && !%batteryChargeOnly) //need extra power
			{
				%dischargeAmt = getMin(%discharge, mAbs(%powerDiff));
				%powerDiff += %dischargeAmt;
				setDataIDArrayTagValue(%bat[%i], "charge", %chargeAvailable - %dischargeAmt);

				%totalBatteryPower += %chargeAvailable - %dischargeAmt;
				%batteryDischarge += %dischargeAmt;
				
				%brickName.updateStorageMenu(%bat[%i]);
			}
			else if (%powerDiff > 0 && %chargeAvailable < %max) //extra power available to charge battery with
			{
				%addAmt = getMin(%powerDiff, %max - %chargeAvailable);
				%powerDiff -= %addAmt;
				setDataIDArrayTagValue(%bat[%i], "charge", %chargeAvailable + %addAmt);
				
				%totalBatteryPower += %chargeAvailable + %addAmt;
				%batteryDischarge -= %addAmt;
				
				%brickName.updateStorageMenu(%bat[%i]);
			}
			else
			{
				%totalBatteryPower += %chargeAvailable;
			}
			%batOnCount++;
		}
	}

	%powerRatio = (%totalGeneratedPower + %batteryDischarge) / %totalPowerUsage;
	if (%totalPowerUsage <= 0)
	{
		%powerRatio = 0;
	}

	for (%i = 0; %i < %pro_onCount; %i++)
	{
		%proDB = %pro_on[%i].getDatablock();
		if (isFunction(%proDB.powerFunction))
		{
			call(%proDB.powerFunction, %pro_on[%i], %powerRatio);
		}
	}

	%brick.totalBatteryPower = %totalBatteryPower;
	%brick.totalPowerUsage = %totalPowerUsage;
	%brick.totalGeneratedPower = %totalGeneratedPower;

	%brick.updateStorageMenu();
}

function togglePower(%cl, %menu, %option)
{
	%brick = %menu.brick;
	if (!isObject(%brick))
	{
		return;
	}
	%dataID = %brick.eventOutputParameter0_1;
	%toggleOn = !getDataIDArrayTagValue(%dataID, "isPoweredOn");
	setDataIDArrayTagValue(%dataID, "isPoweredOn", %toggleOn);
	if (%toggleOn)
	{
		serverPlay3D(ToggleStartSound, %brick.getPosition());
	}
	else
	{
		serverPlay3D(ToggleStopSound, %brick.getPosition());
	}
	%brick.updateStorageMenu(%brick.eventOutputParameter0_1);

	reopenCenterprintMenu(%cl, %menu, %option);
	return %toggleOn;
}

function toggleInputOn(%cl, %menu, %option)
{
	%brick = %menu.brick;
	if (!isObject(%brick))
	{
		return;
	}
	%dataID = %brick.eventOutputParameter0_1;
	%toggleOff = !getDataIDArrayTagValue(%dataID, "isInputOff"); //default is on
	setDataIDArrayTagValue(%dataID, "isInputOff", %toggleOff);
	if (%toggleOff)
	{
		serverPlay3D(ToggleStartSound, %brick.getPosition());
	}
	else
	{
		serverPlay3D(ToggleStopSound, %brick.getPosition());
	}
	%brick.updateStorageMenu(%brick.eventOutputParameter0_1);

	reopenCenterprintMenu(%cl, %menu, %option);
	return !%toggleOff;
}

function toggleOutputOn(%cl, %menu, %option)
{
	%brick = %menu.brick;
	if (!isObject(%brick))
	{
		return;
	}
	%dataID = %brick.eventOutputParameter0_1;
	%toggleOff = !getDataIDArrayTagValue(%dataID, "isOutputOff"); //default is on
	setDataIDArrayTagValue(%dataID, "isOutputOff", %toggleOff);
	if (%toggleOff)
	{
		serverPlay3D(ToggleStartSound, %brick.getPosition());
	}
	else
	{
		serverPlay3D(ToggleStopSound, %brick.getPosition());
	}
	%brick.updateStorageMenu(%brick.eventOutputParameter0_1);

	reopenCenterprintMenu(%cl, %menu, %option);
	return !%toggleOff;
}

function toggleBatteryMode(%cl, %menu, %option)
{
	%brick = %menu.brick;
	if (!isObject(%brick))
	{
		return;
	}
	%dataID = %brick.eventOutputParameter0_1;
	%batteryMode = !getDataIDArrayTagValue(%dataID, "batteryMode"); //default: full, alt: charge only
	setDataIDArrayTagValue(%dataID, "batteryMode", %batteryMode);
	if (%batteryMode)
	{
		serverPlay3D(ToggleStartSound, %brick.getPosition());
	}
	else
	{
		serverPlay3D(ToggleStopSound, %brick.getPosition());
	}
	%brick.updateStorageMenu(%brick.eventOutputParameter0_1);

	reopenCenterprintMenu(%cl, %menu, %option);
	return %batteryMode;
}

function addFuel(%brick, %cl, %slot)
{
	if (getTrustLevel(%brick, %cl) < 1)
	{
		serverCmdUnuseTool(%cl);
		%cl.centerprint(getBrickgroupFromObject(%brick).name @ "<color:ff0000> does not trust you enough to do that!", 1);
		return;
	}

	%pl = %cl.player;
	%item = %pl.tool[%slot];
	if (%item.isStackable && %item.stackType !$= "")
	{
		if (!%brick.canAcceptFuel(%item.stackType))
		{
			serverCmdUnuseTool(%cl);
			%cl.centerprint("This generator only accepts " @ strReplace(%brick.getDatablock().fuelType, " ", ", ") @ "!", 1);
			return;
		}
		
		%brick.isAcceptingFuel = 1;
		%success = %brick.insertIntoStorage(%brick.eventOutputParameter[0, 1], 
										%item, 
										!%pl.tool[%slot].isStackable ? 1 : %pl.toolStackCount[%slot], 
										%pl.toolDataID[%slot]);
		%brick.isAcceptingFuel = 0;
		if (%success == 0) //complete insertion
		{
			%pl.toolStackCount[%slot] = 0;
			%pl.tool[%slot] = 0;
			messageClient(%cl, 'MsgItemPickup', "", %slot, 0);
			if (%pl.currTool == %slot)
			{
				%pl.unmountImage(0);
			}
			return;
		}
		else if (%success == 1) //partial insertion
		{
			%pl.toolStackCount[%slot] = getWord(%success, 1);
			%db = getStackTypeDatablock(%pl.tool[%slot].stackType, getWord(%success, 1)).getID();
			messageClient(%cl, 'MsgItemPickup', "", %slot, %db);
			%pl.tool[%slot] = %db;
			if (%pl.currTool == %slot)
			{
				%pl.mountImage(%db.image, 0);
			}
			return;
		}
	}
	else
	{
		%cl.centerprint("You cannot use this as fuel!", 1);
		return;
	}
}

function fxDTSBrick::isPoweredOn(%brick)
{
	%dataID = %brick.eventOutputParameter0_1;
	return getDataIDArrayTagValue(%dataID, "isPoweredOn");
}

function fxDTSBrick::isInputOn(%brick)
{
	%dataID = %brick.eventOutputParameter0_1;
	return !getDataIDArrayTagValue(%dataID, "isInputOff");
}

function fxDTSBrick::isOutputOn(%brick)
{
	%dataID = %brick.eventOutputParameter0_1;
	return !getDataIDArrayTagValue(%dataID, "isOutputOff");
}

function fxDTSBrick::getBatteryMode(%brick)
{
	%dataID = %brick.eventOutputParameter0_1;
	if (getDataIDArrayTagValue(%dataID, "batteryMode"))
	{
		return "Charging only";
	}
	else
	{
		return "Discharging on";
	}
}

function fxDTSBrick::canAcceptFuel(%brick, %stackType)
{
	return strPos(strLwr(%brick.getDatablock().fuelType), strLwr(%stackType)) >= 0;
}

function connectToControlBox(%brick, %controlBox)
{
	if (!isObject(%brick) || !isObject(%controlBox))
	{
		return -1;
	}

	%brickDB = %brick.getDatablock();
	%controlDB = %controlBox.getDatablock();
	%brickDataID = %brick.eventOutputParameter0_1;
	%controlDataID = %controlBox.eventOutputParameter0_1;
	if (%brickDataID $= "" || %controlDataID $= "")
	{
		error("ERROR: connectToControlBox - data ID is empty! (" @ %brickDataID @ ", " @ %controlDataID @ ")");
		talk("ERROR: connectToControlBox - data ID is empty! (" @ %brickDataID @ ", " @ %controlDataID @ ")");
		return -1;
	}
	else if (%brickDB.isPowerControlBox || !%controlDB.isPowerControlBox)
	{
		error("ERROR: connectToControlBox - parameters incorrect! (" @ %brick SPC %controlBox @ ")");
		talk("ERROR: connectToControlBox - parameters incorrect! (" @ %brick SPC %controlBox @ ")");
		return -1;	
	}

	//get currently linked stuff, and record which slot is empty
	for (%i = 0; %i < getDataIDArrayCount(%controlDataID); %i++)
	{
		%subDataID = getDataIDArrayValue(%controlDataID, %i);
		%type = strLwr(getDataIDArrayTagValue(%subDataID, "powerType"));
		%brickName = getDataIDArrayTagValue(%subDataID, "brickName");
		if (isObject(%brickName))
		{
			if (%brickName $= %brick.getName())
			{
				error("ERROR: connectToControlBox - " @ %brick @ " already in list!");
				return 2;
			}
			switch$ (%type)
			{
				case "generator":
					%gen[%genCount++ - 1] = %subDataID;
				case "processor":
					%pro[%proCount++ - 1] = %subDataID;
				case "battery":
					%bat[%batCount++ - 1] = %subDataID;
			}
		}
		else if (%emptySlot $= "")
		{
			%emptySlot = %i;
		}
	}

	%hookedUpTo = getDataIDArrayTagValue(%brickDataID, "powerControlBox");
	if (isObject(getDataIDArrayTagValue(%hookedUpTo, "brickName")))
	{
		error("ERROR: connectToControlBox - " @ %brick @ " already is connected to a control box!");
		return 1;
	}

	if (%emptySlot $= "")
	{
		%emptySlot = getDataIDArrayCount(%controlDataID);
	}

	if (%brickDB.isGenerator && %genCount < %controlDB.maxGenerators) %type = "generator";
	else if (%brickDB.isPoweredProcessor && %proCount < %controlDB.maxProcessors) %type = "processor";
	else if (%brickDB.isBattery && %batCount < %controlDB.maxBatteries) %type = "battery";
	else
	{
		error("ERROR: connectToControlBox - Cannot connect \"" @ %brick @ "\" to control box, maxed out connection type");
		return 3;
	}

	if (vectorDist(%brick.getPosition(), %controlBox.getPosition()) > 32)
	{
		error("ERROR: connectToControlBox - Cannot connect \"" @ %brick @ "\" to control box, too far");
		return 4;
	}
	//all checks passed, connect them
	setDataIDArrayTagValue(%brickDataID, "powerControlBox", %controlBox.eventOutputParameter0_1);
	setDataIDArrayValue(%controlDataID, %emptySlot, %brickDataID);
	return 0;
}

function disconnectFromControlBox(%brick, %controlBox)
{
	if (!isObject(%brick) || !isObject(%controlBox))
	{
		return -1;
	}

	%brickDB = %brick.getDatablock();
	%controlDB = %controlBox.getDatablock();
	%brickDataID = %brick.eventOutputParameter0_1;
	%controlDataID = %controlBox.eventOutputParameter0_1;
	if (%brickDataID $= "" || %controlDataID $= "")
	{
		error("ERROR: disconnectFromControlBox - data ID is empty! (" @ %brickDataID @ ", " @ %controlDataID @ ")");
		talk("ERROR: disconnectFromControlBox - data ID is empty! (" @ %brickDataID @ ", " @ %controlDataID @ ")");
		return -1;
	}
	else if (%brickDB.isPowerControlBox || !%controlDB.isPowerControlBox)
	{
		error("ERROR: disconnectFromControlBox - parameters incorrect! (" @ %brick SPC %controlBox @ ")");
		talk("ERROR: disconnectFromControlBox - parameters incorrect! (" @ %brick SPC %controlBox @ ")");
		return -1;	
	}

	//get currently linked stuff, and record which slot is empty
	for (%i = 0; %i < getDataIDArrayCount(%controlDataID); %i++)
	{
		%subDataID = getDataIDArrayValue(%controlDataID, %i);
		if (%subDataID $= %brickDataID)
		{
			%slot = %i;
			break;
		}
	}

	if (%slot $= "")
	{
		error("ERROR: disconnectFromControlBox - " @ %brick @ " not connected to control box " @ %controlBox @"!");
		return 1;
	}

	setDataIDArrayValue(%controlDataID, %slot, "");
	setDataIDArrayTagValue(%brickDataID, "powerControlBox", "");
	return 0;
}




///////////////
//Linker Item//
///////////////

datablock ItemData(ElectricalCableItem : HammerItem)
{
	shapeFile = "./resources/power/electricalcable.dts";
	uiName = "Electrical Cable";

	image = "ElectricalCableImage";

	doColorshift = false;
	colorShiftColor = "1 1 1 1";
};

datablock ShapeBaseImageData(ElectricalCableImage)
{
	shapeFile = "./resources/power/electricalcable.dts";
	emap = true;

	offset = "-0.05 0.3 -0.06";
	rotation = eulerToMatrix("0 90 0");
	// eyeOffset = "0 0 0";

	item = "BrickPlacerItem";
	armReady = 1;

	doColorshift = true;
	colorShiftColor = "1 1 1 1";

	toolTip = "Brick-Placement Item";
	mountPoint = 0;

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateTransitionOnTimeout[1] = "ReadyLoop";
	stateTimeoutValue[1] = 0.1;
	stateScript[1] = "onReady";
	stateTransitionOnTriggerDown[1] = "Fire";

	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "PostFire";

	stateName[3] = "PostFire";
	stateTransitionOnTriggerUp[3] = "Ready";

	stateName[4] = "ReadyLoop";
	stateTransitionOnTimeout[4] = "Ready";
	stateTimeoutValue[4] = 0.1;
	stateScript[4] = "onReady";
	stateTransitionOnTriggerDown[4] = "Fire";
};

function ElectricalCableImage::onMount(%this, %obj, %slot)
{
	%obj.powerControlBrick = "";
	%obj.poweredBrick = "";
}

function ElectricalCableImage::onUnmount(%this, %obj, %slot)
{
	%obj.powerControlBrick = "";
	%obj.poweredBrick = "";
}

function ElectricalCableImage::onReady(%this, %obj, %slot)
{
	//detect looked-at brick and show connection information
	%start = %obj.getEyeTransform();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
	%masks = $Typemasks::fxBrickObjectType;
	%ray = containerRaycast(%start, %end, %masks);
	if (isObject(%hit = getWord(%ray, 0)))
	{
		%db = %hit.getDatablock();
		if (%db.isGenerator || %db.isPoweredProcessor || %db.isBattery || %db.isPowerControlBox)
		{
			if (%db.isGenerator) %type = "Generator";
			else if (%db.isPoweredProcessor) %type = "Device";
			else if (%db.isBattery) %type = "Battery";
			else %type = "Power Control Box";

			%obj.brickInfo = %type;
			%dataID = %hit.eventOutputParameter0_1;

			if (%type $= "Power Control Box")
			{
				for (%i = 0; %i < getDataIDArrayCount(%dataID); %i++)
				{
					%subDataID = getDataIDArrayValue(%dataID, %i);
					%target = getDataIDArrayTagValue(%subDataID, "brickName");
					if (isObject(%target))
					{
						%pos1 = vectorAdd(%hit.getPosition(), "0 0 " @ %hit.getDatablock().brickSizeZ * 0.1);
						%pos2 = vectorAdd(%target.getPosition(), "0 0 " @ %target.getDatablock().brickSizeZ * 0.1);
						if (!isObject(%target.line))
						{
							%g = getRandom();
							%target.line = drawLine(%pos1, %pos2, 1 SPC %g SPC %g SPC 1, 0.05);
							%target.line.color = 1 SPC %g SPC %g SPC 1;
						}
						else
						{
							%target.line.drawLine(%pos1, %pos2, %target.line.color, 0.05);
						}
						cancel(%target.line.deleteSched);
						%target.line.deleteSched = %target.line.schedule(200, delete);
					}
				}
			}
			else
			{
				%controlBoxDataID = getDataIDArrayTagValue(%dataID, "powerControlBox");
				%target = getDataIDArrayValue(%controlBoxDataID, "brickName");
				if (isObject(%target))
				{
					%pos1 = vectorAdd(%hit.getPosition(), "0 0 " @ %hit.getDatablock().brickSizeZ * 0.1);
					%pos2 = vectorAdd(%target.getPosition(), "0 0 " @ %target.getDatablock().brickSizeZ * 0.1);
					if (!isObject(%target.line))
					{
						%g = getRandom();
						%target.line = drawLine(%pos1, %pos2, 1 SPC %g SPC %g SPC 1, 0.05);
						%target.line.color = 1 SPC %g SPC %g SPC 1;
					}
					else
					{
						%target.line.drawLine(%pos1, %pos2, %target.line.color, 0.05);
					}
					cancel(%target.line.deleteSched);
					%target.line.deleteSched = %target.line.schedule(200, delete);
				}
			}
		}
		else //invalid brick being looked at
		{
			%obj.brickInfo = "";
		}
	}
	else //no brick being looked at
	{
		%obj.brickInfo = "";
	}

	//display connection lines
	if (isObject(%obj.poweredBrick))
	{
		%pos1 = %obj.poweredBrick.getPosition();
		%pos2 = %obj.getMuzzlePoint(%slot);
		if (!isObject(%obj.poweredBrickLine))
		{
			%obj.poweredBrickLine = drawLine(%pos1, %pos2, "0 0 1 1", 0.05);
		}
		else
		{
			%obj.poweredBrickLine.drawLine(%pos1, %pos2, "0 0 1 1", 0.05);
		}
		cancel(%obj.poweredBrickLine.deleteSched);
		%obj.poweredBrickLine.deleteSched = %obj.poweredBrickLine.schedule(200, delete);
	}

	if (isObject(%obj.powerControlBrick))
	{
		%pos1 = %obj.powerControlBrick.getPosition();
		%pos2 = %obj.getMuzzlePoint(%slot);
		if (!isObject(%obj.powerControlBrickLine))
		{
			%obj.powerControlBrickLine = drawLine(%pos1, %pos2, "1 0 1 1", 0.05);
		}
		else
		{
			%obj.powerControlBrickLine.drawLine(%pos1, %pos2, "1 0 1 1", 0.05);
		}
		cancel(%obj.powerControlBrickLine.deleteSched);
		%obj.powerControlBrickLine.deleteSched = %obj.powerControlBrickLine.schedule(200, delete);
	}

	//display centerprint
	if (isObject(%cl = %obj.client))
	{
		if (%obj.errorTime < $Sim::Time)
		{
			%obj.errorString = "";
		}
		if (%obj.responseTime < $Sim::Time)
		{
			%obj.responseString = "";
		}

		%cpstr = "<just:right>\c3-Electrical Cable- <br>";
		%currDevice = (isObject(%obj.poweredBrick) ? %obj.poweredBrick.getDatablock().uiName : "\c0None");
		%currPowerBrick = (isObject(%obj.powerControlBrick) ? %obj.powerControlBrick.getPosition() : "\c0None");
		%cpstr = %cpstr @ "\c6Current Device: \c3" @ %currDevice @ " <br>";
		%cpstr = %cpstr @ "\c6Current Box: \c3" @ %currPowerBrick @ " <br>";
		%cpstr = %cpstr @ "\c4" @ %obj.brickInfo @ " <br>";
		%cpstr = %cpstr @ %obj.responseString @ " <br>";
		%cpstr = %cpstr @ %obj.errorString;

		%cl.centerprint(%cpstr, 2);
	}
}

function ElectricalCableImage::onFire(%this, %obj, %slot)
{
	%obj.playThread(0, plant);

	%start = %obj.getEyeTransform();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
	%masks = $Typemasks::fxBrickObjectType;
	%ray = containerRaycast(%start, %end, %masks);
	if (isObject(%hit = getWord(%ray, 0)))
	{
		if (%hit == %obj.poweredBrick || %hit == %obj.powerControlBrick)
		{
			%obj.poweredBrick = (%hit == %obj.poweredBrick ? "" : %obj.poweredBrick);
			%obj.powerControlBrick = (%hit == %obj.powerControlBrick ? "" : %obj.powerControlBrick);

			%obj.responseString = "\c2Deselected!";
			%obj.responseTime = $Sim::Time + 3;
			ElectricalCableImage::onReady(%this, %obj, %slot);
			return;
		}

		%db = %hit.getDatablock();
		if (!%db.isGenerator && !%db.isPoweredProcessor && !%db.isBattery && !%db.isPowerControlBox)
		{
			%obj.errorString = "\c0Invalid object!";
			%obj.errorTime = $Sim::Time + 3;

			ElectricalCableImage::onReady(%this, %obj, %slot);
			return;
		}
		else if (getTrustLevel(%hit, %obj) < 2)
		{
			%obj.errorString = "\c0You need full trust to select this brick!";
			%obj.errorTime = $Sim::Time + 3;

			ElectricalCableImage::onReady(%this, %obj, %slot);
			return;
		}

		if (%db.isGenerator || %db.isPoweredProcessor || %db.isBattery)
		{
			%obj.poweredBrick = %hit;
			%lastAdded = "poweredBrick";
			%obj.responseString = "\c2Selected " @ %hit.getDatablock().uiname @ "!";
			%obj.responseTime = $Sim::Time + 3;
		}
		else
		{
			%obj.powerControlBrick = %hit;
			%lastAdded = "powerControlBrick";
			%obj.responseString = "\c2Selected " @ %hit.getDatablock().uiname @ "!";
			%obj.responseTime = $Sim::Time + 3;
		}


		if (%obj.poweredBrick.getDatablock().isGenerator) %type = "generator";
		else if (%obj.poweredBrick.getDatablock().isPoweredProcessor) %type = "device";
		else if (%obj.poweredBrick.getDatablock().isBattery) %type = "battery";

		if (isObject(%obj.powerControlBrick) && isObject(%obj.poweredBrick))
		{
			%error = connectToControlBox(%obj.poweredBrick, %obj.powerControlBrick);

			if (%error != 0)
			{
				switch (%error)
				{
					case 2: %errorString = "\c0Object is already connected to this control brick!"; //replace with disconnect
							%error = disconnectFromControlBox(%obj.poweredBrick, %obj.powerControlBrick);
							%obj.poweredBrick = "";
							if (%error)
							{
								talk("Disconnect critical error!");
								return;
							}
							%obj.responseString = "Disconnected!";
							%obj.responseTime = $Sim::Time + 3;
							return;
					case 1: %errorString = "\c0Object is connected to a different control brick!";
					case 3: %errorString = "\c0Power control box has no more free " @ %type @ " connections!";
					default: %errorString = "\c0Critical error! Please report to an admin!";
				}
				%obj.errorString = %errorString;
				%obj.errorTime = $Sim::Time + 2;
				
				%obj.poweredBrick = "";
				if (%lastAdded $= "poweredBrick")
				{
					%obj.responseString = "";
				}
			}
			else
			{
				%obj.responseString = "\c2Linked " @ %obj.poweredBrick.getDatablock().uiName @ " to Power Control Box!";
				%obj.responseTime = $Sim::Time + 5;
				%obj.poweredBrick = "";
			}
			ElectricalCableImage::onReady(%this, %obj, %slot);
		}
	}
}




//////////
//Bricks//
//////////

datablock fxDTSBrickData(brickCoalGeneratorData)
{
	uiName = "Coal Generator";

	brickFile = "./resources/power/CoalGenerator.blb";

	iconName = "";

	cost = 0;
	isProcessor = 1;
	processorFunction = "addFuel";
	// activateFunction = "CoalGeneratorInfo";
	placerItem = "CoalGeneratorItem";
	keepActivate = 1;
	isGenerator = 1;
	burnRate = 0.01;
	generation = 10;
	fuelType = "Coal";

	isStorageBrick = 1;
	storageSlotCount = 1;
	itemStackCount = 0;
	storageMultiplier = 12;
};

datablock fxDTSBrickData(brickPowerControlBoxData)
{
	uiName = "Power Control Box";

	brickFile = "./resources/power/controlBoxClosed.blb";

	iconName = "";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "grindProduce";
	// activateFunction = "CoalGeneratorInfo";
	placerItem = "PowerControlBoxItem";
	keepActivate = 1;
	isPowerControlBox = 1;
	maxGenerators = 4;
	maxProcessors = 16;
	maxBatteries = 4;
	tickTime = 0.5;

	isStorageBrick = 1; //purely for the gui, don't enable storage
	storageSlotCount = 1;
	storageOpenDatablock = "brickPowerControlBoxOpenData";
	storageClosedDatablock = "brickPowerControlBoxData";
};

datablock fxDTSBrickData(brickPowerControlBoxOpenData : brickPowerControlBoxData)
{
	brickFile = "./resources/power/controlBoxOpen.blb";
};

datablock fxDTSBrickData(brickBatteryData)
{
	uiName = "Battery";

	brickFile = "./resources/power/battery.blb";

	iconName = "";

	cost = 0;
	isProcessor = 1;
	// processorFunction = "grindProduce";
	// activateFunction = "CoalGeneratorInfo";
	placerItem = "BatteryItem";
	keepActivate = 1;
	isBattery = 1;
	dischargeRate = 40;
	capacity = 10000;

	isStorageBrick = 1; //purely for the gui, don't enable storage
	storageSlotCount = 1;
};



///////////////
//Placer Item//
///////////////

datablock ItemData(CoalGeneratorItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Coal Generator";
	image = "CoalGeneratorBrickImage";
	colorShiftColor = "0.9 0 0 1";

	iconName = "Add-ons/Server_Farming/crops/icons/compost_bin";
	
	cost = 800;
};

datablock ShapeBaseImageData(CoalGeneratorBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = CoalGeneratorItem;
	
	doColorshift = true;
	colorShiftColor = CoalGeneratorItem.colorShiftColor;

	toolTip = "Places a Coal Generator";
	loopTip = "Converts fuel into power";
	placeBrick = "brickCoalGeneratorData";
};

function CoalGeneratorBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function CoalGeneratorBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function CoalGeneratorBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function CoalGeneratorBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}



datablock ItemData(PowerControlBoxItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Power Control Box";
	image = "PowerControlBoxBrickImage";
	colorShiftColor = "0.9 0 0 1";

	iconName = "Add-ons/Server_Farming/crops/icons/compost_bin";
	
	cost = 800;
};

datablock ShapeBaseImageData(PowerControlBoxBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = PowerControlBoxItem;
	
	doColorshift = true;
	colorShiftColor = PowerControlBoxItem.colorShiftColor;

	toolTip = "Places a Power Control Box";
	loopTip = "Connects electrical machines";
	placeBrick = "brickPowerControlBoxData";
};

function PowerControlBoxBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function PowerControlBoxBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function PowerControlBoxBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function PowerControlBoxBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}



datablock ItemData(BatteryItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Battery";
	image = "BatteryBrickImage";
	colorShiftColor = "0.9 0 0 1";

	iconName = "Add-ons/Server_Farming/crops/icons/compost_bin";
	
	cost = 800;
};

datablock ShapeBaseImageData(BatteryBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = BatteryItem;
	
	doColorshift = true;
	colorShiftColor = BatteryItem.colorShiftColor;

	toolTip = "Places a Battery";
	loopTip = "Stores excess electrical power";
	placeBrick = "brickBatteryData";
};

function BatteryBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function BatteryBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function BatteryBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function BatteryBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}
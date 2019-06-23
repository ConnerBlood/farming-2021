if (!isObject(CompostBinSimSet)) 
{
	$CompostBinSimSet = new SimSet(CompostBinSimSet) {};
}

package CompostBinSimSetCollector
{
	function fxDTSBrick::onAdd(%brick) 
	{
		%ret = parent::onAdd(%brick);

		if (%brick.isPlanted && %brick.getDatablock().isCompostBin)
		{
			CompostBinSimSet.add(%brick);
		}
		return %ret;
	}
};
activatePackage(CompostBinSimSetCollector);

package CompostBinRetrieveOnly
{
	function attemptStorage(%brick, %cl, %slot, %multiplier)
	{
		if (%brick.getDatablock().isCompostBin)
		{
			return 0;
		}
		return parent::attemptStorage(%brick, %cl, %slot, %multiplier);
	}

	function fxDTSBrick::displayStorageContents(%this, %str1, %str2, %str3, %str4, %cl)
	{
		if (%this.getDatablock().isCompostBin)
		{
			if (getTrustLevel(%cl, %this) < 2)
			{
				return;
			}

			%this.updateCompostBinMenu(%str1, %str2, %str3, %str4);

			%cl.startCenterprintMenu(%this.centerprintMenu);

			storageLoop(%cl, %this);
		}
		else
		{
			return parent::displayStorageContents(%this, %str1, %str2, %str3, %str4, %cl);
		}
	}
};
activatePackage(CompostBinRetrieveOnly);

function compostTick(%index)
{
	cancel($masterCompostTickSchedule);
	
	if (!isObject(MissionCleanup)) 
	{
		return;
	}

	//if no compost bins just skip everything
	%count = CompostBinSimSet.getCount();
	if (%count <= 0)
	{
		$masterCompostTickSchedule = schedule(100, 0, compostTick, %index);
		return;
	}

	for (%i = 0; %i < %count; %i++)
	{
		if (%index >= %count)
		{
			break;
		}
		%brick = CompostBinSimSet.getObject(%index);

		if (%brick.nextCompostTime < $Sim::Time)
		{
			createFertilizer(%brick);
		}
		%index++;
	}

	if (%index >= %count)
	{
		%index = 0;
	}

	$masterCompostTickSchedule = schedule(100, 0, compostTick, %index);
}

function fertilizeCrop(%img, %obj, %slot)
{
	%obj.playThread(0, plant);

	%start = %obj.getEyePoint();
	%end = vectorAdd(vectorScale(%obj.getEyeVector(), 4), %start);
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType);
	%brick = getWord(%ray, 0);

	if (isObject(%brick) && %brick.getDatablock().isPlant && !%brick.getDatablock().isTree)
	{
		%brick = %brick.getDownBrick(0);
	}

	if (!isObject(%brick) || !(%brick.getDatablock().isTree || %brick.getDatablock().isDirt))
	{
		return;
	}

	if (%brick.getDatablock().isDirt)
	{
		%numGrown = 0;
		%numCrops = 0;
		for (%i = 0; %i < %brick.getNumUpBricks(); %i++)
		{
			%crop = %brick.getUpBrick(%i);

			if (!%crop.getDatablock().isPlant || %crop.getDatablock().isTree)
			{
				continue;
			}

			%type = %crop.getDatablock().cropType;
			%stage = %crop.getDatablock().stage;

			if (getTrustLevel(%crop, %obj) < 1)
			{
				%obj.client.centerprint(%crop.getGroup().name @ "<color:ff0000> does not trust you enough to do that!", 1);
				continue;
			}
			else if ($Farming::Crops::PlantData_[%type, %stage, "timePerTick"] <= 1)
			{
				%obj.client.centerprint("This plant already is fully grown!");
				%numGrown++;
				%numCrops++;
				continue;
			}
			
			%numCrops++;

			%hitloc = %crop.getPosition();
			%p = new Projectile() { dataBlock = PushBroomProjectile; initialPosition = %hitloc; };
			%p.setScale("0.5 0.5 0.5");
			%p.explode();

			%crop.growTick += %img.bonusGrowTicks;
			%crop.nextGrow -= %img.bonusGrowTime;

			if (!isObject(%crop.emitter))
			{
				if (getRandom() < %img.shinyChance)
				{
					%rand = getRandom();
					if (%rand < 0.025)
					{
						//gold plant
						%crop.setEmitter(goldenEmitter.getID());
						%type = "<color:faef00>Golden";
					}
					else if (%rand < 0.25)
					{
						//silver plant
						%crop.setEmitter(silverEmitter.getID());
						%type = "<color:fafafa>Silver";
					}
					else
					{
						//bronze plant
						%crop.setEmitter(bronzeEmitter.getID());
						%type = "<color:fafafa>Bronze";
					}

					if (isObject(%cl = %obj.client))
					{
						messageAll('MsgUploadStart', "<bitmap:base/client/ui/ci/star> \c3" @
							%cl.name @ "\c6 fertilized a " @ %type SPC %crop.getDatablock().cropType @ "\c6!");
					}
				}
			}
		}
		if (%numGrown == %numCrops)
		{
			%obj.client.centerprint("All of the small plants are already fully grown!");
			return;
		}
	}
	else
	{
		%crop = %brick;

		%type = %crop.getDatablock().cropType;
		%stage = %crop.getDatablock().stage;

		if (getTrustLevel(%crop, %obj) < 1)
		{
			%obj.client.centerprint(%crop.getGroup().name @ "<color:ff0000> does not trust you enough to do that!", 1);
			return;
		}
		else if ($Farming::Crops::PlantData_[%type, %stage, "timePerTick"] <= 1)
		{
			%obj.client.centerprint("This plant already is fully grown!");
			return;
		}

		%hitloc = getWords(%ray, 1, 3);
		%p = new Projectile() { dataBlock = PushBroomProjectile; initialPosition = %hitloc; };
		%p.setScale("0.5 0.5 0.5");
		%p.explode();

		%crop.growTick += %img.bonusGrowTicks;
		%crop.nextGrow -= %img.bonusGrowTime;

		if (!isObject(%crop.emitter))
		{
			if (getRandom() < %img.shinyChance * 3)
			{
				%rand = getRandom();
				if (%rand < 0.05)
				{
					//gold plant
					%crop.setEmitter(goldenEmitter.getID());
					%type = "<color:faef00>Golden";
				}
				else if (%rand < 0.25)
				{
					//silver plant
					%crop.setEmitter(silverEmitter.getID());
					%type = "<color:fafafa>Silver";
				}
				else
				{
					//bronze plant
					%crop.setEmitter(bronzeEmitter.getID());
					%type = "<color:fafafa>Bronze";
				}

				if (isObject(%cl = %obj.client))
				{
					messageAll('MsgUploadStart', "<bitmap:base/client/ui/ci/star> \c3" @
						%cl.name @ "\c6 fertilized a " @ %type SPC %crop.getDatablock().cropType @ "\c6!");
				}
			}
		}
	}

	//increase weed chance on the dirt
	if (%brick.getDatablock().isDirt)
	{
		%brick.fertilizerWeedModifier += 1;
	}
	else
	{
		for (%i = 0; %i < %brick.getNumDownBricks(); %i++)
		{
			%brick.getDownBrick(%i).fertilizerWeedModifier += 1;
		}
	}

	//fertilization successful, update item

	%count = %obj.toolStackCount[%obj.currTool]--;
	%slot = %obj.currTool;
	%type = %obj.tool[%slot].stackType; //earlier it was set to the croptype of the brick
	if (%count <= 0) //no more seeds left, clear the item slot
	{
		messageClient(%obj.client, 'MsgItemPickup', '', %slot, 0);
		%obj.tool[%slot] = "";
		%obj.unmountImage(%imageSlot);
		return %b;
	}

	//some seeds are left, update item if needed
	for (%i = 0; %i < $Stackable_[%type, "stackedItemTotal"]; %i++)
	{
		%currPair = $Stackable_[%type, "stackedItem" @ %i];
		// talk(%currPair);
		if (%count <= getWord(%currPair, 1))
		{
			%bestItem = getWord(%currPair, 0);
			break;
		}
	}

	messageClient(%obj.client, 'MsgItemPickup', '', %slot, %bestItem.getID());
	%obj.tool[%slot] = %bestItem.getID();
	%obj.mountImage(%imageSlot, %bestItem.image);


	%item = %img.item;
	%type = %item.stackType;
	%cl = %obj.client;
	%count = %obj.toolStackCount[%obj.currTool];

	if (isObject(%cl))
	{
		%cl.centerprint("<color:ffff00>-Fertilizer Bag " @ %obj.currTool @ "- <br>Amount<color:ffffff>: " @ %count @ " ", 1);
	}
}

function createFertilizer(%brick)
{
	%name = %brick.getName();
	%count = getSubStr(%name, 1, strLen(%name));

	if (%brick.nextCompostTime $= "")
	{
		%brick.nextCompostTime = $Sim::Time + %brick.getDatablock().tickTime;
		return;
	}

	%maxCount = %brick.getDatablock().tickAmt;
	if (%count > 0)
	{
		%amt = getMin(%maxCount, %count);
		%origAmt = %amt;
	}
	else
	{
		%brick.nextCompostTime = $Sim::Time + %brick.getDatablock().tickTime / 10;
		return;
	}

	//check if there's space for new fertilizer, if yes, add
	%multiplier = %brick.getDatablock().storageBonus;
	%storageMax = $StorageMax_["Fertilizer"] * (%multiplier < 1 ? 1 : %multiplier);
	for (%i = 0; %i < 4; %i++) 
	{
		%curr = validateStorageContents(%brick.eventOutputParameter[0, %i + 1], %brick);
		if (getField(%curr, 1) < %storageMax)
		{
			%addAmt = getMin(%storageMax - getField(%curr, 1), %amt);
			%amt -= %addAmt;
			%brick.eventOutputParameter[0, %i + 1] = "Fertilizer\" " @ getField(%curr, 1) + %addAmt;
		}

		if (%amt <= 0)
		{
			break;
		}
	}

	%amtAdded = %origAmt - %amt;
	if (%amtAdded > 0)
	{
		%count = %count - %amtAdded;
		%brick.setName("_" @ %count);
	}
	%brick.nextCompostTime = $Sim::Time + %brick.getDatablock().tickTime;

	%brick.updateCompostBinMenu(%brick.eventOutputParameter[0, 1], %brick.eventOutputParameter[0, 2], %brick.eventOutputParameter[0, 3], %brick.eventOutputParameter[0, 4]);
}

function processIntoFertilizer(%brick, %cl, %slot)
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
		%cropType = %item.stackType;
		%max = $Stackable_[%cropType, "stackedItemTotal"] - 1;
		%max = getWord($Stackable_[%cropType, "StackedItem" @ %max], 1);

		%isProduce = isProduce(%cropType);
		if (!%isProduce)
		{
			serverCmdUnuseTool(%cl);
			%cl.centerprint("You cannot process this into fertilizer!", 1);
			return;
		}

		if (%pl.toolStackCount[%slot] < %max)
		{
			serverCmdUnuseTool(%cl);
			%cl.centerprint("You need to have a full basket of produce to create fertilizer!", 1);
			return;
		}
		%pl.tool[%slot] = "";
		%pl.toolStackCount[%slot] = 0;
		serverCmdUnuseTool(%cl);
		messageClient(%cl, 'MsgItemPickup', '', %slot, 0);

		%fertRange = ($FertCount_[%cropType] > 0 ? $FertCount_[%cropType] : $FertCount_Default);
		%rand = getRandom(getWord(%fertRange, 0), getWord(%fertRange, 1));
		%count = getSubStr(%brick.getName(), 1, 100);
		%brick.setName("_" @ (%count + %rand));
		%cl.centerprint("<color:ffffff>You started making <color:ffff00>" @ %rand @ "<color:ffffff> fertilizer out of <color:ffff00>" @ %cropType @ "<color:ffffff>!", 3);
		%cl.schedule(100, centerprint, "<color:cccccc>You started making <color:ffff00>" @ %rand @ "<color:cccccc> fertilizer out of <color:ffff00>" @ %cropType @ "<color:ffffff>!", 3);
		return;
	}
}

function compostBinInfo(%brick, %pl)
{
	if (%pl.client.lastMessagedCompostBinInfo + 5 < $Sim::Time)
	{
		messageClient(%pl.client, '', "\c3Drop (Ctrl-W) a full basket of produce in the bin to make fertilizer! Amount varies with produce type.", 2);
		%pl.client.lastMessagedCompostBinInfo = $Sim::Time;
	}
}


///////////////
//Compost Bin//
///////////////

datablock fxDTSBrickData(brickCompostBinData)
{
	// category = "Farming";
	// subCategory = "Extra";
	uiName = "Compost Bin";

	brickFile = "./resources/compostBin.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/compost_bin";

	cost = 0;
	isProcessor = 1;
	isCompostBin = 1;
	isStorageBrick = 1;
	processorFunction = "processIntoFertilizer";
	activateFunction = "compostBinInfo";
	placerItem = "CompostBinItem";
	keepActivate = 1;

	tickTime = 10;
	tickAmt = 1;
};

datablock fxDTSBrickData(brickLargeCompostBinData)
{
	// category = "Farming";
	// subCategory = "Extra";
	uiName = "Large Compost Bin";

	brickFile = "./resources/largecompostBin.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/large_compost_bin";

	cost = 0;
	isProcessor = 1;
	isCompostBin = 1;
	isStorageBrick = 1;
	storageBonus = 3;
	processorFunction = "processIntoFertilizer";
	activateFunction = "compostBinInfo";
	placerItem = "LargeCompostBinItem";
	keepActivate = 1;

	tickTime = 8;
	tickAmt = 2;
};



///////////////
//Placer Item//
///////////////

datablock ItemData(CompostBinItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Compost Bin";
	image = "CompostBinBrickImage";
	colorShiftColor = "0.9 0 0 1";

	iconName = "Add-ons/Server_Farming/crops/icons/compost_bin";
	
	cost = 800;
};

datablock ShapeBaseImageData(CompostBinBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = CompostBinItem;
	
	doColorshift = true;
	colorShiftColor = CompostBinItem.colorShiftColor;

	toolTip = "Places a Compost Bin";
	loopTip = "Converts produce into fertilizer";
	placeBrick = "brickCompostBinData";
};

function CompostBinBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function CompostBinBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function CompostBinBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function CompostBinBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}



datablock ItemData(LargeCompostBinItem : brickPlacerItem)
{
	shapeFile = "./resources/toolbox.dts";
	uiName = "Large Compost Bin";
	image = "LargeCompostBinBrickImage";
	colorShiftColor = "0.5 0 0 1";

	iconName = "Add-ons/Server_Farming/crops/icons/large_compost_bin";
	
	cost = 1600;
};

datablock ShapeBaseImageData(LargeCompostBinBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/toolbox.dts";
	
	offset = "-0.56 0 0";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = LargeCompostBinItem;
	
	doColorshift = true;
	colorShiftColor = LargeCompostBinItem.colorShiftColor;

	toolTip = "Places a Large Compost Bin";
	loopTip = "Converts produce into fertilizer";
	placeBrick = "brickLargeCompostBinData";
};

function LargeCompostBinBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function LargeCompostBinBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function LargeCompostBinBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function LargeCompostBinBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}

////////
//Item//
////////

$Stackable_Fertilizer_StackedItem0 = "FertilizerBag0Item 40";
$Stackable_Fertilizer_StackedItem1 = "FertilizerBag1Item 80";
$Stackable_Fertilizer_StackedItem2 = "FertilizerBag2Item 120";
$Stackable_Fertilizer_StackedItemTotal = 3;

datablock ItemData(FertilizerBag0Item : HammerItem)
{
	shapeFile = "./resources/fertilizerBag0.dts";
	uiName = "Fertilizer Bag";
	image = "FertilizerBag0Image";
	colorShiftColor = "0.5 0.3 0 1";
	doColorShift = true;

	iconName = "Add-ons/Server_Farming/crops/icons/Fertilizer_Bag";

	isStackable = 1;
	stackType = "Fertilizer";
};

datablock ShapeBaseImageData(FertilizerBag0Image)
{
	shapeFile = "./resources/fertilizerBag0.dts";
	emap = true;

	doColorShift = true;
	colorShiftColor = FertilizerBag0Item.colorShiftColor;

	item = "FertilizerBag0Item";

	armReady = 1;

	offset = "";

	toolTip = "Make plants grow faster, chance of shiny";

	bonusGrowTicks = 0; //bonus grow tick per use (does not consume water)
	bonusGrowTime = 10; //reduction in seconds to next grow tick
	shinyChance = 0.004;

	stateName[0] = "Activate";
	stateTransitionOnTimeout[0] = "LoopA";
	stateTimeoutValue[0] = 0.1;

	stateName[1] = "LoopA";
	stateScript[1] = "onLoop";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateTimeoutValue[1] = 0.1;
	stateTransitionOnTimeout[1] = "LoopB";

	stateName[2] = "LoopB";
	stateScript[2] = "onLoop";
	stateTransitionOnTriggerDown[2] = "Fire";
	stateTimeoutValue[2] = 0.1;
	stateTransitionOnTimeout[2] = "LoopA";

	stateName[3] = "Fire";
	stateScript[3] = "onFire";
	stateTransitionOnTriggerUp[3] = "LoopA";
	stateTransitionOnTimeout[3]	= "LoopA";
	stateTimeoutValue[3] = 0.2;
	stateWaitForTimeout[3] = true;
};

datablock ItemData(FertilizerBag1Item : FertilizerBag0Item)
{
	shapeFile = "./resources/fertilizerBag1.dts";
	image = "FertilizerBag1Image";
	uiName = "Half Fertilizer Bag";

	iconName = "Add-ons/Server_Farming/crops/icons/Fertilizer_Bag_Half";
};

datablock ShapeBaseImageData(FertilizerBag1Image : FertilizerBag0Image)
{
	shapeFile = "./resources/fertilizerBag1.dts";
	item = "FertilizerBag1Item";
};

datablock ItemData(FertilizerBag2Item : FertilizerBag0Item)
{
	shapeFile = "./resources/fertilizerBag2.dts";
	image = "FertilizerBag2Image";
	uiName = "Full Fertilizer Bag";

	iconName = "Add-ons/Server_Farming/crops/icons/Fertilizer_Bag_Full";
};

datablock ShapeBaseImageData(FertilizerBag2Image : FertilizerBag0Image)
{
	shapeFile = "./resources/fertilizerBag2.dts";
	item = "FertilizerBag2Item";
};


function FertilizerBag0Image::onFire(%this, %obj, %slot)
{
	fertilizeCrop(%this, %obj, %slot);
}

function FertilizerBag1Image::onFire(%this, %obj, %slot)
{
	fertilizeCrop(%this, %obj, %slot);
}

function FertilizerBag2Image::onFire(%this, %obj, %slot)
{
	fertilizeCrop(%this, %obj, %slot);
}



function FertilizerBag0Image::onLoop(%this, %obj, %slot)
{
	fertilizerLoop(%this, %obj);
}

function FertilizerBag1Image::onLoop(%this, %obj, %slot)
{
	fertilizerLoop(%this, %obj);
}

function FertilizerBag2Image::onLoop(%this, %obj, %slot)
{
	fertilizerLoop(%this, %obj);
}

function fertilizerLoop(%image, %obj)
{
	%item = %image.item;
	%type = %item.stackType;
	%cl = %obj.client;
	%count = %obj.toolStackCount[%obj.currTool];

	if (isObject(%cl))
	{
		%cl.centerprint("<color:ffff00>-Fertilizer Bag " @ %obj.currTool @ "- <br>Amount<color:ffffff>: " @ %count @ " ", 1);
	}
}



//Fertilizer storage ui adjustment
function fxDTSBrick::updateCompostBinMenu(%this, %str1, %str2, %str3, %str4)
{
	for (%i = 1; %i < 5; %i++)
	{
		%str[%i] = validateStorageContents(%str[%i], %this);
	}

	if (!isObject(%this.centerprintMenu))
	{
		%this.centerprintMenu = new ScriptObject(StorageCenterprintMenus)
		{
			isCenterprintMenu = 1;
			menuName = %this.getDatablock().uiName;

			menuOption[0] = "Empty";
			menuOption[1] = "Empty";
			menuOption[2] = "Empty";
			menuOption[3] = "Empty";
			menuOption[4] = "Queued: 0";

			menuFunction[0] = "removeStack";
			menuFunction[1] = "removeStack";
			menuFunction[2] = "removeStack";
			menuFunction[3] = "removeStack";
			menuFunction[4] = "";

			menuOptionCount = 5;
			brick = %this;
		};
		MissionCleanup.add(%this.centerprintMenu);

		//%cl.currOption
	}

	for (%i = 0; %i < 4; %i++)
	{
		%stackType = getField(%str[%i + 1], 0);
		%count = getField(%str[%i + 1], 1);
		if (%stackType !$= "")
		{
			if (!isObject(%stackType))
			{
				%this.centerprintMenu.menuOption[%i] = %stackType @ " - " @ %count;
			}
			else
			{
				%this.centerprintMenu.menuOption[%i] = strUpr(getSubStr(%stackType.uiName, 0, 1)) @ getSubStr(%stackType.uiName, 1, 100);
			}
		}
		else
		{
			%this.centerprintMenu.menuOption[%i] = "Empty";	
		}
	}
	%this.centerprintMenu.menuOption[4] = "Queued: " @ (getSubStr(%this.getName(), 1, 30) + 0);
}
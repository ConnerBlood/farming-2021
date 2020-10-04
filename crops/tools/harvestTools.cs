
//harvest tools

datablock ItemData(TrowelItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/crops/icons/Trowel";
	shapeFile = "./trowel.dts";
	uiName = "Trowel";

	hasDataID = 1;
	isDataIDTool = 1;

	durabilityFunction = "generateHarvestToolDurability";
	modifiersFunction = "generateHarvestToolModifiers";
	baseDurability = 200;
	chanceDurability = 0.8;
	bonusDurability = 20;

	image = "TrowelImage";
	colorShiftColor = "0.4 0 0 1";
};

datablock ShapeBaseImageData(TrowelImage)
{
	shapeFile = "./Trowel.dts";
	emap = true;

	item = TrowelItem;
	doColorShift = true;
	colorShiftColor = "0.4 0 0 1";

	armReady = true;

	toolTip = "+1 harvest to below ground crops";

	stateName[0] = "Activate";
	stateTimeoutValue[0] = 0.1;
	stateTransitionOnTimeout[0] = "Ready";

	stateName[1] = "Ready";
	stateTransitionOnTimeout[1] = "Ready2";
	stateTimeoutValue[1] = 0.2;
	stateScript[1] = "onReady";
	stateTransitionOnTriggerDown[1] = "Fire";

	stateName[2] = "Fire";
	stateScript[2] = "onFire";
	stateTransitionOnTriggerUp[2] = "Ready";
	stateTimeoutValue[2] = 0.2;
	stateTransitionOnTimeout[2] = "Repeat";
	stateWaitForTimeout[2] = 1;

	stateName[3] = "Repeat";
	stateTimeoutValue[3] = 0.12;
	stateTransitionOnTimeout[3] = "Fire";
	stateTransitionOnTriggerUp[3] = "Ready";

	stateName[4] = "Ready2";
	stateTransitionOnTimeout[4] = "Ready";
	stateTimeoutValue[4] = 0.2;
	stateScript[4] = "onReady";
	stateTransitionOnTriggerDown[4] = "Fire";
};

function TrowelImage::onFire(%this, %obj, %slot)
{
	toolHarvest(%this, %obj, %slot);
}

function TrowelImage::onReady(%this, %obj, %slot)
{
	harvestToolReady(%this, %obj, %slot);
}

////

datablock ItemData(ClipperItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/crops/icons/Clipper";
	shapeFile = "./Clipper.dts";
	uiName = "Clipper";

	hasDataID = 1;
	isDataIDTool = 1;

	durabilityFunction = "generateHarvestToolDurability";
	modifiersFunction = "generateHarvestToolModifiers";
	baseDurability = 200;
	chanceDurability = 0.8;
	bonusDurability = 20;

	image = "ClipperImage";
	colorShiftColor = "0.4 0 0 1";
};

datablock ShapeBaseImageData(ClipperImage : TrowelImage)
{
	shapeFile = "./Clipper.dts";

	item = ClipperItem;
	doColorShift = true;

	projectile = "swordProjectile";

	toolTip = "+1 harvest to above ground crops";
};

function ClipperImage::onFire(%this, %obj, %slot)
{
	toolHarvest(%this, %obj, %slot);
}

function ClipperImage::onReady(%this, %obj, %slot)
{
	harvestToolReady(%this, %obj, %slot);
}

////

datablock ItemData(HoeItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/crops/icons/Hoe";
	shapeFile = "./Hoe.dts";
	uiName = "Hoe";

	hasDataID = 1;
	isDataIDTool = 1;

	durabilityFunction = "generateHarvestToolDurability";
	modifiersFunction = "generateHarvestToolModifiers";
	baseDurability = 300;
	chanceDurability = 0.8;
	bonusDurability = 20;

	image = "HoeImage";
	colorShiftColor = "0.4 0 0 1";
};

datablock ShapeBaseImageData(HoeImage : TrowelImage)
{
	shapeFile = "./Hoe.dts";

	item = HoeItem;
	doColorShift = true;

	areaHarvest = 2;
	stateTimeoutValue[2] = 0.4;

	toolTip = "Area harvest below ground crops";
};

function HoeImage::onFire(%this, %obj, %slot)
{
	toolHarvest(%this, %obj, %slot);
}

function HoeImage::onReady(%this, %obj, %slot)
{
	harvestToolReady(%this, %obj, %slot);
}

////

datablock ItemData(SickleItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/crops/icons/Sickle";
	shapeFile = "./Sickle.dts";
	uiName = "Sickle";

	hasDataID = 1;
	isDataIDTool = 1;

	durabilityFunction = "generateHarvestToolDurability";
	modifiersFunction = "generateHarvestToolModifiers";
	baseDurability = 300;
	chanceDurability = 0.8;
	bonusDurability = 20;

	image = "SickleImage";
	colorShiftColor = "0.4 0 0 1";
};

datablock ShapeBaseImageData(SickleImage : TrowelImage)
{
	shapeFile = "./Sickle.dts";

	item = SickleItem;
	doColorShift = true;

	areaHarvest = 2;
	stateTimeoutValue[2] = 0.4;

	toolTip = "Area harvest above ground crops";
};

function SickleImage::onFire(%this, %obj, %slot)
{
	toolHarvest(%this, %obj, %slot);
}

function SickleImage::onReady(%this, %obj, %slot)
{
	harvestToolReady(%this, %obj, %slot);
}

////

datablock ItemData(TreeClipperItem : HammerItem)
{
	iconName = "Add-ons/Server_Farming/crops/icons/TreeClipper";
	shapeFile = "./TreeClipper.dts";
	uiName = "Tree Clipper";

	hasDataID = 1;
	isDataIDTool = 1;

	durabilityFunction = "generateHarvestToolDurability";
	modifiersFunction = "generateHarvestToolModifiers";
	baseDurability = 300;
	chanceDurability = 0.8;
	bonusDurability = 20;

	image = "TreeClipperImage";
	colorShiftColor = "0.4 0 0 1";
};

datablock ShapeBaseImageData(TreeClipperImage : TrowelImage)
{
	shapeFile = "./TreeClipper.dts";

	item = TreeClipperItem;
	doColorShift = true;

	areaHarvest = 2;
	stateTimeoutValue[2] = 0.4;

	toolTip = "Area harvest above ground crops";
};

function TreeClipperImage::onFire(%this, %obj, %slot)
{
	toolHarvest(%this, %obj, %slot);
}

function TreeClipperImage::onReady(%this, %obj, %slot)
{
	harvestToolReady(%this, %obj, %slot);
}

////

function harvestToolReady(%img, %obj, %slot)
{
	if (isObject(%cl = %obj.client))
	{
		%durability = getDurability(%img, %obj, %slot);

		%statTrak = %obj.getToolStatTrak();
		if (%statTrak !$= "")
		{
			%string = "\c4" @ %statTrak @ " ";
		}

		%cl.centerprint("<just:right><color:cccccc>Durability: " @ %durability @ " \n" @ %string, 1);
	}
}

function toolHarvest(%img, %obj, %slot)
{
	%item = %img.item;

	%obj.playThread(0, shiftDown);

	%start = %obj.getEyePoint();
	%end = vectorAdd(%start, vectorScale(%obj.getEyeVector(), 5));
	%ray = containerRaycast(%start, %end, $Typemasks::fxBrickObjectType | $Typemasks::PlayerObjectType, %obj);

	if (isObject(%hit = getWord(%ray, 0)))
	{
		if (!%hit.getDatablock().isPlant)
		{
			if (!isObject(%img.projectile))
			{
				%db = swordProjectile;
			}
			else
			{
				%db = %img.projectile;
			}

			%p = new Projectile()
			{
				dataBlock = %db;
				initialPosition = getWords(%ray, 1, 3);
				initialVelocity = "0 0 0";
			};
			%p.setScale("0.5 0.5 0.5");
			%p.explode();
		}
	}

	%durability = getDurability(%img, %obj, %slot);
	if (%durability == 0 && isObject(%cl = %obj.client))
	{
		%cl.centerprint("<just:right><color:cccccc>Durability: " @ %durability @ " \n\c0This tool needs repairs!", 1);
		return;
	}

	%toolHarvestType = getDataIDArrayTagValue(%obj.toolDataID[%obj.currTool], "statTrakType");

	if (%img.areaHarvest > 0 && isObject(%hit))
	{
		initContainerRadiusSearch(getWords(%ray, 1, 3), %img.areaHarvest, $Typemasks::fxBrickObjectType);
		while (isObject(%next = containerSearchNext()))
		{
			if (%next.getDatablock().isPlant)
			{
				%type = %next.getDatablock().cropType;
				%err = harvestBrick(%next, %item, %obj);
				if (%err)
				{
					%hasHarvested = 1;
					if (%type $= %toolHarvestType)
					{
						%dataID = %obj.toolDataID[%obj.currTool];
						%count = getDataIDArrayTagValue(%dataID, %type);
						setDataIDArrayTagValue(%dataID, %type, %count + 1);
					}
				}
			}
		}
	}
	else
	{
		if (isObject(%hit) && %hit.getDatablock().isPlant)
		{
			%type = %hit.getDatablock().cropType;
			%err = harvestBrick(%hit, %item, %obj);
			if (%err)
			{
				%hasHarvested = 1;
				if (%type $= %toolHarvestType)
				{
					%dataID = %obj.toolDataID[%obj.currTool];
					%count = getDataIDArrayTagValue(%dataID, %type);
					setDataIDArrayTagValue(%dataID, %type, %count + 1);
				}
			}
		}
	}

	if (%hasHarvested)
	{
		useDurability(%img, %obj, %slot);
	}
}

function generateHarvestToolDurability(%item)
{
	%baseDurability = %item.baseDurability > 0 ? %item.baseDurability : 100;
	%bonusDurability = %item.bonusDurability;
	%chanceDurability = %item.chanceDurability;

	while (getRandom() < %chanceDurability)
	{
		%baseDurability += %bonusDurability;
	}

	return %baseDurability;
}

function generateHarvestToolModifiers(%item, %dataID)
{
	%statTrak = getRandom() < 0.1;

	if (%statTrak)
	{
		switch$ (%item.uiName)
		{
			case "Trowel": %list = $UndergroundCropsList;
			case "Hoe": %list = $UndergroundCropsList;
			case "Clipper": %list = $AbovegroundCropsList;
			case "Sickle": %list = $AbovegroundCropsList;
			case "Tree Clipper": %list = $TreeCropsList;
			default: return "";
		}
		%crop = getField(%list, getRandom(getFieldCount(%list) - 1));
		%displayAsKills = getRandom() < 0.1;

		setDataIDArrayTagValue(%dataID, "statTrakType", %crop);
		setDataIDArrayTagValue(%dataID, "displayAsKills", %displayAsKills);
	}
}



function Player::getToolStatTrak(%pl)
{
	if (%pl.tool[%pl.currTool].isDataIDTool)
	{
		switch (%pl.tool[%pl.currTool])
		{
			case (TrowelItem.getID()): %dataID = %pl.toolDataID[%pl.currTool];
			case (ClipperItem.getID()): %dataID = %pl.toolDataID[%pl.currTool];
			case (SickleItem.getID()): %dataID = %pl.toolDataID[%pl.currTool];
			case (HoeItem.getID()): %dataID = %pl.toolDataID[%pl.currTool];
			case (TreeClipperItem.getID()): %dataID = %pl.toolDataID[%pl.currTool];
			default: return "";
		}
		%displayAsKills = getDataIDArrayTagValue(%dataID, "displayAsKills");
		%type = getDataIDArrayTagValue(%dataID, "statTrakType");
		%count = getDataIDArrayTagValue(%dataID, %type) + 0 | 0;

		if (%type $= "")
		{
			return "";
		}

		%word = %displayAsKills ? "kills:" : "harvests:";
		return %type SPC %word SPC %count;
	}
	return "";
}

function getHighestToolHarvestCount(%dataID)
{
	if (%dataID $= "")
	{
		return "";
	}
	%tags = getDataIDArrayTags(%dataID);
	for (%i = 0; %i < getWordCount(%tags); %i++)
	{
		%tag = getWord(%tags, %i);
		talk(%tag);
		if (%tag $= "datablock" || %tag $= "durability" || %tag $= "maxDurability")
		{
			continue;
		}

		%count = getDataIDArrayTagValue(%dataID, %tag);
		if (%count > %maxCount)
		{
			%maxCount = %count;
			%maxType = %tag;
		}
	}

	return %maxType SPC %maxCount;
}

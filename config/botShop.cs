
$count = 0;
if (isObject($BS1))
{
	for (%i = 0; %i < 20; %i++)
	{
		if (isObject($BS[%i]))
		{
			$BS[%i].delete();
		}
	}
}

$BS[$count++] = new ScriptObject(BS_BigBuyer) { class = "ShopObject"; };
$BS[$count++] = new ScriptObject(BS_Seeds_All) { class = "ShopObject"; };
$BS[$count++] = new ScriptObject(BS_Seeds_Basic) { class = "ShopObject"; };
$BS[$count++] = new ScriptObject(BS_Tools_All) { class = "ShopObject"; };
$BS[$count++] = new ScriptObject(BS_Tools_Farming) { class = "ShopObject"; };
$BS[$count++] = new ScriptObject(BS_Tools_Watering) { class = "ShopObject"; };
$BS[$count++] = new ScriptObject(BS_Tools_Extras) { class = "ShopObject"; };
$BS[$count++] = new ScriptObject(BS_Instruments) { class = "ShopObject"; };


BS_BigBuyer.option[BS_BigBuyer.count++ - 1]  = "Potato"			TAB 60;
BS_BigBuyer.option[BS_BigBuyer.count++ - 1]  = "Carrot"			TAB 70;
BS_BigBuyer.option[BS_BigBuyer.count++ - 1]  = "Tomato"			TAB 60;
BS_BigBuyer.option[BS_BigBuyer.count++ - 1]  = "Corn"			TAB 40;
BS_BigBuyer.option[BS_BigBuyer.count++ - 1]  = "Wheat"			TAB 30;
BS_BigBuyer.option[BS_BigBuyer.count++ - 1]  = "Portobello"		TAB 20;
BS_BigBuyer.option[BS_BigBuyer.count++ - 1]  = "Cabbage"		TAB 15;
BS_BigBuyer.option[BS_BigBuyer.count++ - 1]  = "Onion"			TAB 50;
BS_BigBuyer.option[BS_BigBuyer.count++ - 1]  = "Blueberry"		TAB 8;
BS_BigBuyer.option[BS_BigBuyer.count++ - 1]  = "Turnip"			TAB 8;
BS_BigBuyer.option[BS_BigBuyer.count++ - 1]  = "Apple"			TAB 8;
BS_BigBuyer.option[BS_BigBuyer.count++ - 1]  = "Mango"			TAB 4;
BS_BigBuyer.option[BS_BigBuyer.count++ - 1]  = "Chili"			TAB 8;
BS_BigBuyer.option[BS_BigBuyer.count++ - 1]  = "Cactus"			TAB 8;
BS_BigBuyer.option[BS_BigBuyer.count++ - 1]  = "Watermelon"		TAB 4;
BS_BigBuyer.option[BS_BigBuyer.count++ - 1]  = "Date"			TAB 2;
BS_BigBuyer.option[BS_BigBuyer.count++ - 1]  = "Peach"			TAB 2;


BS_Seeds_All.option[BS_Seeds_All.count++ - 1]  = "PotatoSeedItem"		TAB 60;
BS_Seeds_All.option[BS_Seeds_All.count++ - 1]  = "CarrotSeedItem"		TAB 60;
BS_Seeds_All.option[BS_Seeds_All.count++ - 1]  = "TomatoSeedItem"		TAB 40;
BS_Seeds_All.option[BS_Seeds_All.count++ - 1]  = "CornSeedItem"			TAB 40;
BS_Seeds_All.option[BS_Seeds_All.count++ - 1]  = "CabbageSeedItem"		TAB 30;
BS_Seeds_All.option[BS_Seeds_All.count++ - 1]  = "OnionSeedItem"		TAB 60;
BS_Seeds_All.option[BS_Seeds_All.count++ - 1]  = "BlueberrySeedItem"	TAB 10;
BS_Seeds_All.option[BS_Seeds_All.count++ - 1]  = "TurnipSeedItem"		TAB 10;
BS_Seeds_All.option[BS_Seeds_All.count++ - 1]  = "DaisySeedItem"		TAB 25;
BS_Seeds_All.option[BS_Seeds_All.count++ - 1]  = "LilySeedItem"			TAB 25;
BS_Seeds_All.option[BS_Seeds_All.count++ - 1]  = "RoseSeedItem"			TAB 25;
BS_Seeds_All.option[BS_Seeds_All.count++ - 1]  = "AppleSeedItem"		TAB 8;
BS_Seeds_All.option[BS_Seeds_All.count++ - 1]  = "MangoSeedItem"		TAB 4;
BS_Seeds_All.option[BS_Seeds_All.count++ - 1]  = "ChiliSeedItem"		TAB 25;
BS_Seeds_All.option[BS_Seeds_All.count++ - 1]  = "CactusSeedItem"		TAB 25;
BS_Seeds_All.option[BS_Seeds_All.count++ - 1]  = "WatermelonSeedItem"	TAB 15;
BS_Seeds_All.option[BS_Seeds_All.count++ - 1]  = "DateSeedItem"			TAB 5;
BS_Seeds_All.option[BS_Seeds_All.count++ - 1]  = "PeachSeedItem"		TAB 5;


BS_Seeds_Basic.option[BS_Seeds_Basic.count++ - 1]  = "PotatoSeedItem"		TAB 60;
BS_Seeds_Basic.option[BS_Seeds_Basic.count++ - 1]  = "CarrotSeedItem"		TAB 60;
BS_Seeds_Basic.option[BS_Seeds_Basic.count++ - 1]  = "TomatoSeedItem"		TAB 40;
BS_Seeds_Basic.option[BS_Seeds_Basic.count++ - 1]  = "CornSeedItem"			TAB 40;
BS_Seeds_Basic.option[BS_Seeds_Basic.count++ - 1]  = "WheatSeedItem"		TAB 30;
BS_Seeds_Basic.option[BS_Seeds_Basic.count++ - 1]  = "PortobelloSeedItem"	TAB 10;
BS_Seeds_Basic.option[BS_Seeds_Basic.count++ - 1]  = "CabbageSeedItem"		TAB 30;
BS_Seeds_Basic.option[BS_Seeds_Basic.count++ - 1]  = "OnionSeedItem"		TAB 60;
BS_Seeds_Basic.option[BS_Seeds_Basic.count++ - 1]  = "BlueberrySeedItem"	TAB 10;
BS_Seeds_Basic.option[BS_Seeds_Basic.count++ - 1]  = "TurnipSeedItem"		TAB 10;


BS_Tools_All.option[BS_Tools_All.count++ - 1]  = "WateringCan2Item"			TAB 120;
BS_Tools_All.option[BS_Tools_All.count++ - 1]  = "WateringCan3Item"			TAB 120;
BS_Tools_All.option[BS_Tools_All.count++ - 1]  = "HoseItem"					TAB 50;
BS_Tools_All.option[BS_Tools_All.count++ - 1]  = "HoseV2Item"				TAB 20;
BS_Tools_All.option[BS_Tools_All.count++ - 1]  = "TrowelItem"				TAB 50;
BS_Tools_All.option[BS_Tools_All.count++ - 1]  = "ClipperItem"				TAB 50;
BS_Tools_All.option[BS_Tools_All.count++ - 1]  = "HoeItem"					TAB 35;
BS_Tools_All.option[BS_Tools_All.count++ - 1]  = "SickleItem"				TAB 35;
BS_Tools_All.option[BS_Tools_All.count++ - 1]  = "PlanterItem"				TAB 25;
BS_Tools_All.option[BS_Tools_All.count++ - 1]  = "PlanterV2Item"			TAB 15;
BS_Tools_All.option[BS_Tools_All.count++ - 1]  = "ReclaimerItem"			TAB 20;
BS_Tools_All.option[BS_Tools_All.count++ - 1]  = "WeedKiller0Item"			TAB 20;
BS_Tools_All.option[BS_Tools_All.count++ - 1]  = "Fertilizer0Item"			TAB 20;
BS_Tools_All.option[BS_Tools_All.count++ - 1]  = "FlowerPotItem"			TAB 20;
BS_Tools_All.option[BS_Tools_All.count++ - 1]  = "CompostBinItem"			TAB 20;
BS_Tools_All.option[BS_Tools_All.count++ - 1]  = "WateringCatItem"			TAB 5;


BS_Tools_Farming.option[BS_Tools_Farming.count++ - 1]  = "TrowelItem"			TAB 50;
BS_Tools_Farming.option[BS_Tools_Farming.count++ - 1]  = "ClipperItem"			TAB 50;
BS_Tools_Farming.option[BS_Tools_Farming.count++ - 1]  = "HoeItem"				TAB 35;
BS_Tools_Farming.option[BS_Tools_Farming.count++ - 1]  = "SickleItem"			TAB 35;
BS_Tools_Farming.option[BS_Tools_Farming.count++ - 1]  = "PlanterItem"			TAB 25;
BS_Tools_Farming.option[BS_Tools_Farming.count++ - 1]  = "PlanterV2Item"		TAB 15;
BS_Tools_Farming.option[BS_Tools_Farming.count++ - 1]  = "ReclaimerItem"		TAB 20;
BS_Tools_Farming.option[BS_Tools_Farming.count++ - 1]  = "WeedKiller0Item"		TAB 20;
BS_Tools_Farming.option[BS_Tools_Farming.count++ - 1]  = "Fertilizer0Item"		TAB 20;


BS_Tools_Watering.option[BS_Tools_Watering.count++ - 1]  = "WateringCan2Item"	TAB 120;
BS_Tools_Watering.option[BS_Tools_Watering.count++ - 1]  = "WateringCan3Item"	TAB 80;
BS_Tools_Watering.option[BS_Tools_Watering.count++ - 1]  = "HoseItem"			TAB 50;
BS_Tools_Watering.option[BS_Tools_Watering.count++ - 1]  = "HoseV2Item"			TAB 20;
BS_Tools_Watering.option[BS_Tools_Watering.count++ - 1]  = "WateringCatItem"	TAB 1;
BS_Tools_Watering.option[BS_Tools_Watering.count++ - 1]  = "WateringSnakeItem"	TAB 1;


BS_Tools_Extras.option[BS_Tools_Extras.count++ - 1]  = "FlowerPotItem"			TAB 10;
BS_Tools_Extras.option[BS_Tools_Extras.count++ - 1]  = "CompostBinItem"			TAB 60;
BS_Tools_Extras.option[BS_Tools_Extras.count++ - 1]  = "LargeCompostBinItem"	TAB 40;
BS_Tools_Extras.option[BS_Tools_Extras.count++ - 1]  = "PlanterBoxItem"			TAB 50;
BS_Tools_Extras.option[BS_Tools_Extras.count++ - 1]  = "CoalGeneratorItem"		TAB 50;
BS_Tools_Extras.option[BS_Tools_Extras.count++ - 1]  = "PowerControlBoxItem"	TAB 50;
BS_Tools_Extras.option[BS_Tools_Extras.count++ - 1]  = "BatteryItem"			TAB 50;
BS_Tools_Extras.option[BS_Tools_Extras.count++ - 1]  = "IndoorLightItem"		TAB 50;



function generateInstrumentShop()
{
	%instrumentCount = 0;

	for (%i = 0; %i < DatablockGroup.getCount(); %i++)
	{
		%db = DatablockGroup.getObject(%i);
		if (isObject(%db.image) && %db.image.instrumentType !$= "")
		{
			BS_Instruments.option[%instrumentCount + 0] = %db SPC 10;
			$SellCost_[%db.uiName] = 1000;
			$BuyCost_[%db.uiName] = 1200;
			%instrumentCount++;

			if (%db.uiName $= "Keytar")
			{
				BS_Instruments.option[%instrumentCount - 1] = %db SPC 5;
				$SellCost_[%db.uiName] = 4800;
				$BuyCost_[%db.uiName] = 5000;
				%keytarFound = 1;
				%keytarIDX = %instrumentCount - 1;
			}
		}
	}
	BS_Instruments.count = %instrumentCount;

	echo("Generated Instrument list: Found " @ %instrumentCount @ " instruments");
	if (%keytarFound)
	{
		echo("    Keytar found");
	}
}

schedule(1000, 0, generateInstrumentList); //instruments are executed after farming
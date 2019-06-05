//////////////
//Sprinklers//
//////////////

datablock fxDTSBrickData(brickSmallSprinklerData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Small Sprinkler";

	brickFile = "./smallSprinkler.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/small_sprinkler";

	cost = 200;
	isSprinkler = 1;
	directionalOffset = "0 0 -0.3";
	boxSize = "4 4 2";
	maxDispense = 20;
};

datablock fxDTSBrickData(brickSmallSprinklerDownData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Small Sprinkler Down";

	brickFile = "./smallSprinklerDown.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/small_sprinkler_down";

	cost = 200;
	isSprinkler = 1;
	directionalOffset = "0 0 -1.7";
	boxSize = "4 4 3";
	maxDispense = 20;
	noCollide = 1;
};

datablock fxDTSBrickData(brickMedSprinklerData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Medium Sprinkler";

	brickFile = "./MedSprinkler.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/medium_sprinkler";

	cost = 800;
	isSprinkler = 1;
	directionalOffset = "0 0 -0.3";
	boxSize = "8 8 2";
	maxDispense = 50;
};

datablock fxDTSBrickData(brickLargeSprinklerData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Large Sprinkler";

	brickFile = "./LargeSprinkler.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/large_sprinkler";

	cost = 1500;
	isSprinkler = 1;
	directionalOffset = "0 0 -0.4";
	boxSize = "12 12 2";
	maxDispense = 50;
};

datablock fxDTSBrickData(brickDripLineData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Drip Line";

	brickFile = "./DripLine.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/drip_line";

	cost = 50;
	isSprinkler = 1;
	directionalOffset = "0 0 -0.2";
	boxSize = "0.5 4 0.2";
	maxDispense = 15;
};

datablock fxDTSBrickData(brickStraightSprinklerData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Straight Sprinkler";

	brickFile = "./StraightSprinkler.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/straight_sprinkler";

	cost = 600;
	isSprinkler = 1;
	directionalOffset = "4.5 0 -0.3";
	boxSize = "8 1 2";
	maxDispense = 30;
};

datablock fxDTSBrickData(brickSwaySprinklerData)
{
	category = "Farming";
	subCategory = "Sprinklers";
	uiName = "Sway Sprinkler";

	brickFile = "./SwaySprinkler.blb";

	iconName = "Add-Ons/Server_Farming/crops/icons/sway_sprinkler";

	cost = 700;
	isSprinkler = 1;
	directionalOffset = "0 0 -0.3";
	boxSize = "8 4 2";
	maxDispense = 50;
};

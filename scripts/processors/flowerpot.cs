//////////////
//Flower Pot//
//////////////

datablock fxDTSBrickData(brickFlowerPotData : brick1x1Data) 
{
	category = "";
	subCategory = "";
	uiName = "Flower Pot";

	brickFile = "./resources/flowerpot/flowerPot.blb";

	iconName = "Add-Ons/Server_Farming/icons/flower_pot";

	cost = 0;
	isDirt = 1;
	maxWater = 30;
	maxNutrients = 100;
	maxWeedkiller = 80;
	isPot = 1;
	customRadius = -1.05;
	placerItem = "FlowerPotItem";
	isProcessor = 1;
};


////////
//Item//
////////

datablock ItemData(FlowerPotItem : brickPlacerItem)
{
	shapeFile = "./resources/flowerpot/FlowerpotItem.dts";
	uiName = "Flower Pot";
	image = "FlowerpotBrickImage";
	colorShiftColor = "0.90 0.48 0 1";

	iconName = "Add-ons/Server_Farming/icons/flower_pot";
};

datablock ShapeBaseImageData(FlowerpotBrickImage : BrickPlacerImage)
{
	shapeFile = "./resources/flowerpot/FlowerpotItem.dts";
	
	offset = "-0.56 0 -0.25";
	eyeOffset = "0 0 0";
	rotation = eulerToMatrix("0 0 90");

	item = FlowerpotItem;
	
	doColorshift = true;
	colorShiftColor = FlowerpotItem.colorShiftColor;

	toolTip = "Places a Flower Pot";
	loopTip = "Allows a plant to ignore radius limitations";
	placeBrick = "brickFlowerpotData";
};

function FlowerpotBrickImage::onMount(%this, %obj, %slot)
{
	brickPlacerItem_onMount(%this, %obj, %slot);
}

function FlowerpotBrickImage::onUnmount(%this, %obj, %slot)
{
	brickPlacerItem_onUnmount(%this, %obj, %slot);
}

function FlowerpotBrickImage::onLoop(%this, %obj, %slot)
{
	brickPlacerItemLoop(%this, %obj, %slot);
}

function FlowerpotBrickImage::onFire(%this, %obj, %slot)
{
	brickPlacerItemFire(%this, %obj, %slot);
}
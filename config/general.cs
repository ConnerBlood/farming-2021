$Game::Item::PopTime = 180000;

$startingAmount = 200;
$betaBonus = 100;

// Score grant tracking
if(isFile("config/Farming/scoreGrant.cs"))
	exec("config/Farming/scoreGrant.cs");

// Vehicle Costs
JeepVehicle.maxWheelSpeed = 22;
JeepSpring.force = 2000;
JeepSpring.damping = 5000;
HorseArmor.maxForwardSpeed = 10;
HorseArmor.maxForwardCrouchSpeed = 10;

hangglidervehicle.lift = 8;
hangglidervehicle.verticalSurfaceForce = 17;
hangglidervehicle.forwardThrust = 5;
hangglidervehicle.reverseThrust = 5;

$Farming::BappsMatCycleTime = 60 * 60 * 1000 | 0; // 1 hour
function addBappsMatStrings()
{
	addBappsMatString("gaming");
	addBappsMatString("bapps!");
	addBappsMatString("turnip");
	addBappsMatString("e.");
	addBappsMatString("tomato");
	addBappsMatString("at dos");
	addBappsMatString("onyo");
	addBappsMatString("lmao");
	addBappsMatString("wheeet");
	addBappsMatString("oh");
	addBappsMatString("sick");
	addBappsMatString("%0");
	addBappsMatString("%D");
	addBappsMatString("%7");
	addBappsMatString("8I");
	addBappsMatString(" ");
	addBappsMatString("83");
	addBappsMatString("?#!@*&");
	addBappsMatString("floor.");
	addBappsMatString("tork");
	addBappsMatString("<3");
	addBappsMatString("8==D");
	addBappsMatString("boklnd");
	addBappsMatString("brapps");
	addBappsMatString("borked");
	addBappsMatString("hewwo!");
	addBappsMatString("icup");
	addBappsMatString("o.");
	addBappsMatString("conan?");
	addBappsMatString("based.");
	addBappsMatString("FIGHT!");
	addBappsMatString("helpme");
	addBappsMatString("help");
	addBappsMatString("k.");
	addBappsMatString("no");
	addBappsMatString("ye");
	addBappsMatString("yeh.");
	addBappsMatString("feet");
	addBappsMatString("fruity");
	addBappsMatString("smelly");
	addBappsMatString("ctrl-k");
	addBappsMatString("alt f4");
	addBappsMatString("no way");
	addBappsMatString("ur mom");
	addBappsMatString("leave.");
	addBappsMatString("shup");
	addBappsMatString("knock!");
	addBappsMatString("hhhh");
	addBappsMatString("wowe");
	addBappsMatString("doge");
	addBappsMatString("chode.");
	addBappsMatString("speen!");
	addBappsMatString("push");
	addBappsMatString("ree!");
	addBappsMatString("die.");
	addBappsMatString("abcd");
	addBappsMatString("!sppab");
	addBappsMatString("spparb");
	addBappsMatString("matt");
	addBappsMatString("800815");
	addBappsMatString("lololo");
	addBappsMatString("lookup");
	addBappsMatString("vamos!");
	addBappsMatString("text");
	addBappsMatString("error.");
	addBappsMatString("todo");
	addBappsMatString("unused");
	addBappsMatString("temp");
	addBappsMatString("baps");
	addBappsMatString("christ");
	addBappsMatString("m'lady");
	addBappsMatString("cursed");
	addBappsMatString("bapp's");
	addBappsMatString("remove");
	addBappsMatString("delete");
	addBappsMatString("404.");
	addBappsMatString("bye.");
	addBappsMatString("run.");
	addBappsMatString("ooga");
	addBappsMatString("awooga");
	addBappsMatString("honk");
	addBappsMatString("b*pps.");
	addBappsMatString("mwah");
	addBappsMatString("waaagh");
	addBappsMatString("floor.");
	addBappsMatString("2020");
	addBappsMatString("pemis.");
	addBappsMatString("%I");
	addBappsMatString("%>");
	addBappsMatString("yeet");
	addBappsMatString("weed");
	addBappsMatString("poopoo");
	addBappsMatString("peepee");
	addBappsMatString("lmao");
	addBappsMatString("toilet");
	addBappsMatString("mat.");
	addBappsMatString("uwu?");
	addBappsMatString("bruh");
	addBappsMatString("swag");
	addBappsMatString("what");
	addBappsMatString("u wot?");
	addBappsMatString("bri'sh");
	addBappsMatString("wifi");
	addBappsMatString("carrot");
	addBappsMatString("e!");
	addBappsMatString("    hi");
	addBappsMatString("potato");
	addBappsMatString("??????");
	addBappsMatString("shroom");
}
schedule(1000, 0, addBappsMatStrings);
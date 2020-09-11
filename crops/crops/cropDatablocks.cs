exec("./potato.cs");
exec("./carrot.cs");
exec("./tomato.cs");
exec("./corn.cs");
exec("./wheat.cs");
exec("./cabbage.cs");
exec("./onion.cs");
exec("./blueberry.cs");
exec("./turnip.cs");
exec("./portobello.cs");
exec("./appleTree.cs");
exec("./mangoTree.cs");

exec("./chili.cs");
exec("./cactus.cs");
exec("./watermelon.cs");
exec("./peachTree.cs");
exec("./dateTree.cs");

exec("./weed.cs");

function foodLoop(%image, %obj)
{
	%item = %image.item;
	%type = %item.stackType;
	%cl = %obj.client;
	%count = %obj.toolStackCount[%obj.currTool];

	if (isObject(%cl))
	{
		%cl.centerprint("<just:right><color:ffff00>-Basket " @ %obj.currTool + 1 @ "- <br>" @ %type @ "<color:ffffff>: " @ %count @ " ", 1);
	}
}

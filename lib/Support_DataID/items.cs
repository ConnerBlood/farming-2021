function dropDataIDItem(%client, %slot)
{
	%player = %client.Player;
	if (!isObject(%player))
	{
		return;
	}
	%item = %player.tool[%slot];
	if (isObject(%item))
	{
		if (%item.canDrop == 1.0)
		{
			%zScale = getWord(%player.getScale(), 2);
			%muzzlepoint = VectorAdd(%player.getPosition(), "0 0" SPC 1.5 * %zScale);
			%muzzlevector = %player.getEyeVector();
			%muzzlepoint = VectorAdd(%muzzlepoint, %muzzlevector);
			%playerRot = rotFromTransform(%player.getTransform());
			%thrownItem = new Item(""){
				dataBlock = %item;
				dataID = %player.toolDataID[%slot]; //added line here
			};
			%thrownItem.setScale(%player.getScale());
			%player.toolDataID[%slot] = ""; //added line here
			MissionCleanup.add(%thrownItem);
			%thrownItem.setTransform(%muzzlepoint @ " " @ %playerRot);
			%thrownItem.setVelocity(VectorScale(%muzzlevector, 20.0 * %zScale));
			%thrownItem.schedulePop();
			%thrownItem.miniGame = %client.miniGame;
			%thrownItem.bl_id = %client.getBLID();
			%thrownItem.setCollisionTimeout(%player);
			if (%item.className $= "Weapon")
			{
				%player.weaponCount = %player.weaponCount - 1.0;
			}
			%player.tool[%slot] = 0;
			messageClient(%client, 'MsgItemPickup', '', %slot, 0);
			if (%player.getMountedImage(%item.image.mountPoint) > 0.0)
			{
				if (%player.getMountedImage(%item.image.mountPoint).getId() == %item.image.getId())
				{
					%player.unmountImage(%item.image.mountPoint);
				}
			}
		}
	}
}

package Support_DataIDItem
{
	function Armor::onCollision(%db, %obj, %col, %vec, %speed)
	{
		if (%obj.getState() !$= "Dead" && %obj.getDamagePercent() < 1.0 && isObject(%obj.client))
		{
			%itemDB = %col.getDatablock();
			if (%col.getClassName() $= "Item" && %itemDB.isDataIDObject)
			{
				%slot = %obj.getFirstEmptySlot();
				if (%slot != -1)
				{
					%obj.toolDataID[%slot] = %col.dataID;
				}
			}
		}

		return parent::onCollision(%db, %obj, %col, %vec, %speed);
	}

	function serverCmdDropTool(%cl, %slot)
	{
		if (isObject(%pl = %cl.player))
		{
			%item = %pl.tool[%slot];
			if (%item.hasDataID)
			{
				dropDataIDItem(%cl, %slot);
				return;
			}
		}
		return parent::serverCmdDropTool(%cl, %slot);
	}
};
activatePackage(Support_DataIDItem);

RegisterPersistenceVar("toolDataID0", false, "");
RegisterPersistenceVar("toolDataID1", false, "");
RegisterPersistenceVar("toolDataID2", false, "");
RegisterPersistenceVar("toolDataID3", false, "");
RegisterPersistenceVar("toolDataID4", false, "");
RegisterPersistenceVar("toolDataID5", false, "");
RegisterPersistenceVar("toolDataID6", false, "");
RegisterPersistenceVar("toolDataID7", false, "");
RegisterPersistenceVar("toolDataID8", false, "");
RegisterPersistenceVar("toolDataID9", false, "");
RegisterPersistenceVar("toolDataID10", false, "");
RegisterPersistenceVar("toolDataID12", false, "");
RegisterPersistenceVar("toolDataID13", false, "");
RegisterPersistenceVar("toolDataID14", false, "");
RegisterPersistenceVar("toolDataID15", false, "");
RegisterPersistenceVar("toolDataID16", false, "");
RegisterPersistenceVar("toolDataID17", false, "");
RegisterPersistenceVar("toolDataID18", false, "");
RegisterPersistenceVar("toolDataID19", false, "");

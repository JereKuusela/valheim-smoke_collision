# Smoke collision

Client side mod that makes the smoke ignore certain structures. Also allows changing the smoke size.

The configuration can synced by also installing on the server.

# Manual Installation:

1. Install the [BepInExPack Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/).
2. Download the latest zip.
3. Extract it in the \<GameDirectory\>\BepInEx\plugins\ folder.

# Configuration

The config file has a list of object ids that are ignored by the smoke. Check [here](https://valheim.fandom.com/wiki/Item_IDs) for object ids.

By default following structures let the smoke pass:

- Cage Floor 1x1
- Cage Floor 2x2
- Cage Wall 1x1
- Cage Wall 2x2
- Carved Darkwood divider
- Iron gate
- Wood ladder
- Wood stair

There are also two settings to change the smoke size:

- Smoke size multiplier: Affects both visual and the collider size.
- Collider size multiplier: Affects only the collider size.

# Changelog

- v1.1
	- Adds new settings for changing the smoke size.

- v1.0
	- Initial release.

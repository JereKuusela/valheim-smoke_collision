# Smoke collision

Makes the smoke ignore certain structures. Also allows changing the smoke size and properties.

Install on the client (modding [guide](https://youtu.be/L9ljm2eKLrk)).

Install on the server if want to sync the config.

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

There are also settings to change smoke properties:

- Collider size multiplier (default `1`): Affects only the collider size.
- Fade time (default `2`): Seconds to disappear after max amount or max duration.
- Force (default `2`): Multiplies acceleration (how quickly target velocity is reached).
- Max amount (default `100`): Maximum amount of smoke.
- Max duration (default `60`): Seconds until starting to fade.
- Random horizontal velocity (default `0.15`): Random horizontal velocity.
- Smoke size multiplier (default `1`): Affects both visual and the collider size.
- Vertical velocity (default `0.75`): Maximum vertical velocity.

# Changelog

- v1.3
	- Fixes some object ids not working.

- v1.2
	- Adds settings for max amount, max duration, fade time, force, vertical velocity and horizontal velocity.
	- Changes all smoke to disappear when changing settings.
	- Changes the GUID.

- v1.1
	- Adds new settings for changing the smoke size.

- v1.0
	- Initial release.

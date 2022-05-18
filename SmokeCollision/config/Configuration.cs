using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BepInEx.Configuration;
using ServerSync;
using Service;
namespace SmokeCollision;
public class Configuration {
#nullable disable
  public static ConfigEntry<bool> configLocked;
  public static ConfigEntry<string> configIgnoredIds;
  public static ConfigEntry<string> configSizeMultiplier;
  public static ConfigEntry<string> configColliderMultiplier;
#nullable enable
  public static float SizeMultiplier = 1f;
  public static float ColliderMultiplier = 1f;
  public static HashSet<string> IgnoredIds = new();
  private static void ParseIds() {
    IgnoredIds = configIgnoredIds.Value.Split(',').Select(s => s.Trim().ToLower()).ToHashSet();
  }
  public static float ParseFloat(string value, float defaultValue = 0) {
    if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result)) return result;
    return defaultValue;
  }
  public static void Init(ConfigSync configSync, ConfigFile configFile) {
    ConfigWrapper wrapper = new("smoke_config", configFile, configSync);
    var section = "General";
    configLocked = wrapper.BindLocking(section, "Config locked", false, "When true, server sets the config values.");
    configIgnoredIds = wrapper.Bind(section, "Ignored object ids", "darkwood_decowall,iron_floor_1x1,iron_floor_2x2,iron_grate,iron_wall_1x1,iron_wall_2x2,wood_stair,wood_stepladder", "Object ids separated by , that are ignored by the smoke.");
    configIgnoredIds.SettingChanged += (s, e) => ParseIds();
    ParseIds();
    configSizeMultiplier = wrapper.Bind(section, "Smoke size multiplier", "1", "Changes the size of the smoke object (including the collider).");
    configSizeMultiplier.SettingChanged += (s, e) => {
      SizeMultiplier = ParseFloat(configSizeMultiplier.Value, 1f);
    };
    SizeMultiplier = ParseFloat(configSizeMultiplier.Value, 1f);
    configColliderMultiplier = wrapper.Bind(section, "Collider size multiplier", "1", "Changes only the collider size.");
    configColliderMultiplier.SettingChanged += (s, e) => {
      ColliderMultiplier = ParseFloat(configColliderMultiplier.Value, 1f);
    };
    ColliderMultiplier = ParseFloat(configColliderMultiplier.Value, 1f);
  }
}

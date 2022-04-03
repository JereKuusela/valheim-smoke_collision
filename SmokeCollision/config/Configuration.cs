using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using ServerSync;
using Service;
namespace SmokeCollision;
public class Configuration {
  public static ConfigEntry<string> configIgnoredIds;
  public static HashSet<string> IgnoredIds = new();
  private static void ParseIds() {
    IgnoredIds = configIgnoredIds.Value.Split(',').Select(s => s.Trim().ToLower()).ToHashSet();
  }

  public static void Init(ConfigSync configSync, ConfigFile configFile) {
    ConfigWrapper wrapper = new("smoke_config", configFile, configSync);
    var section = "General";
    configIgnoredIds = wrapper.Bind(section, "Ignored object ids", "darkwood_decowall,iron_floor_1x1,iron_floor_2x2,iron_grate,iron_wall_1x1,iron_wall_2x2,wood_stair,wood_stepladder", "Object ids separated by , that are ignored by the smoke.");
    configIgnoredIds.SettingChanged += (s, e) => ParseIds();
    ParseIds();
  }
}

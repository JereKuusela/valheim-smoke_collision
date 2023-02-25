using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BepInEx.Configuration;
using ServerSync;
using Service;
namespace SmokeCollision;
public class Configuration
{
#nullable disable
  public static ConfigEntry<bool> configLocked;
  public static ConfigEntry<string> configIgnoredIds;
  public static ConfigEntry<string> configSizeMultiplier;
  public static ConfigEntry<string> configColliderMultiplier;
  public static ConfigEntry<string> configMaxDuration;
  public static ConfigEntry<string> configVerticalVelocity;
  public static ConfigEntry<string> configRandomHorizontalVelocity;
  public static ConfigEntry<string> configMaxAmount;
  public static ConfigEntry<string> configFadeTime;
  public static ConfigEntry<string> configForce;
  public static ConfigEntry<string> configRequiredSpace;
#nullable enable
  public static float SizeMultiplier = 1f;
  public static float ColliderMultiplier = 1f;
  const float MAX_DURATION = 60f;
  public static float TimeToDisappear = MAX_DURATION;
  const float VERTICAL_VELOCITY = 0.75f;
  public static float VerticalVelocity = VERTICAL_VELOCITY;
  const float RANDOM_HORIZONTAL_VELOCITY = 0.15f;
  public static float RandomHorizontalVelocity = RANDOM_HORIZONTAL_VELOCITY;
  const int MAX_AMOUNT = 100;
  public static int MaxAmount = MAX_AMOUNT;
  const float FADE_TIME = 2f;
  public static float FadeTime = FADE_TIME;
  const float FORCE = 2f;
  public static float Force = FORCE;
  const float REQUIRED_SPACE = 0.5f;
  public static float RequiredSpace = REQUIRED_SPACE;
  public static HashSet<string> IgnoredIds = new();
  private static void ParseIds()
  {
    IgnoredIds = configIgnoredIds.Value.Split(',').Select(s => s.Trim().ToLower()).ToHashSet();
  }
  public static float ParseFloat(string value, float defaultValue = 0)
  {
    if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result)) return result;
    return defaultValue;
  }
  public static int ParseInt(string value, int defaultValue = 0)
  {
    if (int.TryParse(value, out var result)) return result;
    return defaultValue;
  }

  private static void Clean()
  {
    foreach (var smoke in Smoke.m_smoke)
      UnityEngine.Object.Destroy(smoke.gameObject);
  }
  public static void Init(ConfigSync configSync, ConfigFile configFile)
  {
    ConfigWrapper wrapper = new("smoke_config", configFile, configSync);
    var section = "General";
    configLocked = wrapper.BindLocking(section, "Config locked", false, "When true, server sets the config values.");
    configIgnoredIds = wrapper.Bind(section, "Ignored object ids", "darkwood_decowall,iron_floor_1x1,iron_floor_2x2,iron_grate,iron_wall_1x1,iron_wall_2x2,wood_stair,wood_stepladder", "Object ids separated by , that are ignored by the smoke.");
    configIgnoredIds.SettingChanged += (s, e) => ParseIds();
    ParseIds();
    configSizeMultiplier = wrapper.Bind(section, "Smoke size multiplier", "1", "Changes the size of the smoke object (including the collider).");
    configSizeMultiplier.SettingChanged += (s, e) =>
    {
      SizeMultiplier = ParseFloat(configSizeMultiplier.Value, 1f);
      Clean();
    };
    SizeMultiplier = ParseFloat(configSizeMultiplier.Value, 1f);

    configColliderMultiplier = wrapper.Bind(section, "Collider size multiplier", "1", "Changes only the collider size.");
    configColliderMultiplier.SettingChanged += (s, e) =>
    {
      ColliderMultiplier = ParseFloat(configColliderMultiplier.Value, 1f);
      Clean();
    };
    ColliderMultiplier = ParseFloat(configColliderMultiplier.Value, 1f);

    configMaxDuration = wrapper.Bind(section, "Max duration", MAX_DURATION.ToString(), "Seconds until starting to fade.");
    configMaxDuration.SettingChanged += (s, e) =>
    {
      TimeToDisappear = ParseFloat(configMaxDuration.Value, MAX_DURATION);
      Clean();
    };
    TimeToDisappear = ParseFloat(configMaxDuration.Value, MAX_DURATION);

    configVerticalVelocity = wrapper.Bind(section, "Vertical velocity", VERTICAL_VELOCITY.ToString("F2"), "Maximum vertical velocity.");
    configVerticalVelocity.SettingChanged += (s, e) =>
    {
      VerticalVelocity = ParseFloat(configVerticalVelocity.Value, VERTICAL_VELOCITY);
      Clean();
    };
    VerticalVelocity = ParseFloat(configVerticalVelocity.Value, VERTICAL_VELOCITY);

    configRandomHorizontalVelocity = wrapper.Bind(section, "Random horizontal velocity", RANDOM_HORIZONTAL_VELOCITY.ToString("F2"), "Random horizontal velocity.");
    configRandomHorizontalVelocity.SettingChanged += (s, e) =>
    {
      RandomHorizontalVelocity = ParseFloat(configRandomHorizontalVelocity.Value, RANDOM_HORIZONTAL_VELOCITY);
      Clean();
    };
    RandomHorizontalVelocity = ParseFloat(configRandomHorizontalVelocity.Value, RANDOM_HORIZONTAL_VELOCITY);

    configMaxAmount = wrapper.Bind(section, "Max amount", MAX_AMOUNT.ToString(), "Maximum amount of smoke.");
    configMaxAmount.SettingChanged += (s, e) =>
    {
      MaxAmount = ParseInt(configMaxAmount.Value, MAX_AMOUNT);
      Clean();
    };
    MaxAmount = ParseInt(configMaxAmount.Value, MAX_AMOUNT);

    configFadeTime = wrapper.Bind(section, "Fade time", FADE_TIME.ToString(), "Seconds to disappear after max amount or max duration.");
    configFadeTime.SettingChanged += (s, e) =>
    {
      FadeTime = ParseFloat(configFadeTime.Value, FADE_TIME);
      Clean();
    };
    FadeTime = ParseFloat(configFadeTime.Value, FADE_TIME);

    configForce = wrapper.Bind(section, "Force", FORCE.ToString(), "Multiplies acceleration (how quickly target velocity is reached).");
    configForce.SettingChanged += (s, e) =>
    {
      Force = ParseFloat(configForce.Value, FORCE);
      Clean();
    };
    Force = ParseFloat(configForce.Value, FORCE);

    configRequiredSpace = wrapper.Bind(section, "Required space", REQUIRED_SPACE.ToString(), "Clear space needed above fireplaces (meters). 0 disables the check.");
    configRequiredSpace.SettingChanged += (s, e) =>
    {
      RequiredSpace = ParseFloat(configRequiredSpace.Value, REQUIRED_SPACE);
    };
    RequiredSpace = ParseFloat(configRequiredSpace.Value, REQUIRED_SPACE);
  }
}

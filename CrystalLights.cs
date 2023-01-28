using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using ItemManager;
using JetBrains.Annotations;
using PieceManager;
using ServerSync;
using UnityEngine;
using LocalizationManager;


namespace CrystalLights
{
	[BepInPlugin(ModGUID, ModName, ModVersion)]
	[BepInDependency("org.bepinex.plugins.jewelcrafting")]
	public class CrystalLights : BaseUnityPlugin
	{
		private const string ModName = "CrystalLights";
		private const string ModVersion = "1.0.0";
		private const string ModGUID = "org.bepinex.plugins.crystallights";


		private static readonly ConfigSync configSync = new(ModName) { DisplayName = ModName, CurrentVersion = ModVersion, MinimumRequiredVersion = ModVersion };

		private static ConfigEntry<Toggle> serverConfigLocked = null!;

		private ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description, bool synchronizedSetting = true)
		{
			ConfigEntry<T> configEntry = Config.Bind(group, name, value, description);

			SyncedConfigEntry<T> syncedConfigEntry = configSync.AddConfigEntry(configEntry);
			syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

			return configEntry;
		}

		private ConfigEntry<T> config<T>(string group, string name, T value, string description, bool synchronizedSetting = true) => config(group, name, value, new ConfigDescription(description), synchronizedSetting);

		private enum Toggle
		{
			On = 1,
			Off = 0
		}

		private static readonly Dictionary<string, ConfigEntry<float>> lightIntensities = new();

		private static AssetBundle assets = null!;

		private static Localization english = null!;

		public void Awake()
		{
			Localizer.Load();
			english = new Localization();
			english.SetupLanguage("English");

			serverConfigLocked = config("1 - General", "Lock Configuration", Toggle.On, "If on, the configuration is locked and can be changed by server admins only.");
			configSync.AddLockingConfigEntry(serverConfigLocked);

			assets = PiecePrefabManager.RegisterAssetBundle("crystallights");

			Item CL_Crystal_Hammer = new(assets, "CL_Crystal_Hammer"); //assetbundle name, Asset Name
			CL_Crystal_Hammer.Crafting.Add(ItemManager.CraftingTable.Workbench, 1);
			CL_Crystal_Hammer.RequiredItems.Add("Uncut_Black_Stone", 1);
			CL_Crystal_Hammer.CraftAmount = 1;

			#region pieces

			BuildPiece CL_Large_Wall_Black = AddLightPiece("CL_Large_Wall_Black");
			CL_Large_Wall_Black.RequiredItems.Add("Uncut_Black_Stone", 10, true);
			CL_Large_Wall_Black.RequiredItems.Add("Iron", 2, true);
			CL_Large_Wall_Black.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Large_Wall_Blue = AddLightPiece("CL_Large_Wall_Blue");
			CL_Large_Wall_Blue.RequiredItems.Add("Uncut_Blue_Stone", 10, true);
			CL_Large_Wall_Blue.RequiredItems.Add("Iron", 2, true);
			CL_Large_Wall_Blue.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Large_Wall_Green = AddLightPiece("CL_Large_Wall_Green");
			CL_Large_Wall_Green.RequiredItems.Add("Uncut_Green_Stone", 10, true);
			CL_Large_Wall_Green.RequiredItems.Add("Iron", 2, true);
			CL_Large_Wall_Green.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Large_Wall_Orange = AddLightPiece("CL_Large_Wall_Orange");
			CL_Large_Wall_Orange.RequiredItems.Add("Uncut_Orange_Stone", 10, true);
			CL_Large_Wall_Orange.RequiredItems.Add("Iron", 2, true);
			CL_Large_Wall_Orange.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Large_Wall_Purple = AddLightPiece("CL_Large_Wall_Purple");
			CL_Large_Wall_Purple.RequiredItems.Add("Uncut_Purple_Stone", 10, true);
			CL_Large_Wall_Purple.RequiredItems.Add("Iron", 2, true);
			CL_Large_Wall_Purple.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Large_Wall_Red = AddLightPiece("CL_Large_Wall_Red");
			CL_Large_Wall_Red.RequiredItems.Add("Uncut_Red_Stone", 10, true);
			CL_Large_Wall_Red.RequiredItems.Add("Iron", 2, true);
			CL_Large_Wall_Red.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Large_Wall_Yellow = AddLightPiece("CL_Large_Wall_Yellow");
			CL_Large_Wall_Yellow.RequiredItems.Add("Uncut_Yellow_Stone", 10, true);
			CL_Large_Wall_Yellow.RequiredItems.Add("Iron", 2, true);
			CL_Large_Wall_Yellow.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Small_Wall_Black = AddLightPiece("CL_Small_Wall_Black");
			CL_Small_Wall_Black.RequiredItems.Add("Uncut_Black_Stone", 3, true);
			CL_Small_Wall_Black.RequiredItems.Add("Iron", 1, true);
			CL_Small_Wall_Black.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Small_Wall_Blue = AddLightPiece("CL_Small_Wall_Blue");
			CL_Small_Wall_Blue.RequiredItems.Add("Uncut_Blue_Stone", 3, true);
			CL_Small_Wall_Blue.RequiredItems.Add("Iron", 1, true);
			CL_Small_Wall_Blue.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Small_Wall_Green = AddLightPiece("CL_Small_Wall_Green");
			CL_Small_Wall_Green.RequiredItems.Add("Uncut_Green_Stone", 3, true);
			CL_Small_Wall_Green.RequiredItems.Add("Iron", 1, true);
			CL_Small_Wall_Green.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Small_Wall_Orange = AddLightPiece("CL_Small_Wall_Orange");
			CL_Small_Wall_Orange.RequiredItems.Add("Uncut_Orange_Stone", 3, true);
			CL_Small_Wall_Orange.RequiredItems.Add("Iron", 1, true);
			CL_Small_Wall_Orange.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Small_Wall_Purple = AddLightPiece("CL_Small_Wall_Purple");
			CL_Small_Wall_Purple.RequiredItems.Add("Uncut_Purple_Stone", 3, true);
			CL_Small_Wall_Purple.RequiredItems.Add("Iron", 1, true);
			CL_Small_Wall_Purple.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Small_Wall_Red = AddLightPiece("CL_Small_Wall_Red");
			CL_Small_Wall_Red.RequiredItems.Add("Uncut_Red_Stone", 3, true);
			CL_Small_Wall_Red.RequiredItems.Add("Iron", 1, true);
			CL_Small_Wall_Red.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Small_Wall_Yellow = AddLightPiece("CL_Small_Wall_Yellow");
			CL_Small_Wall_Yellow.RequiredItems.Add("Uncut_Yellow_Stone", 3, true);
			CL_Small_Wall_Yellow.RequiredItems.Add("Iron", 1, true);
			CL_Small_Wall_Yellow.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Standing_Lamp_Black = AddLightPiece("CL_Standing_Lamp_Black");
			CL_Standing_Lamp_Black.RequiredItems.Add("Uncut_Black_Stone", 1, true);
			CL_Standing_Lamp_Black.RequiredItems.Add("Iron", 1, true);
			CL_Standing_Lamp_Black.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Standing_Lamp_Blue = AddLightPiece("CL_Standing_Lamp_Blue");
			CL_Standing_Lamp_Blue.RequiredItems.Add("Uncut_Blue_Stone", 1, true);
			CL_Standing_Lamp_Blue.RequiredItems.Add("Iron", 1, true);
			CL_Standing_Lamp_Blue.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Standing_Lamp_Green = AddLightPiece("CL_Standing_Lamp_Green");
			CL_Standing_Lamp_Green.RequiredItems.Add("Uncut_Green_Stone", 1, true);
			CL_Standing_Lamp_Green.RequiredItems.Add("Iron", 1, true);
			CL_Standing_Lamp_Green.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Standing_Lamp_Orange = AddLightPiece("CL_Standing_Lamp_Orange");
			CL_Standing_Lamp_Orange.RequiredItems.Add("Uncut_Orange_Stone", 1, true);
			CL_Standing_Lamp_Orange.RequiredItems.Add("Iron", 1, true);
			CL_Standing_Lamp_Orange.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Standing_Lamp_Purple = AddLightPiece("CL_Standing_Lamp_Purple");
			CL_Standing_Lamp_Purple.RequiredItems.Add("Uncut_Purple_Stone", 1, true);
			CL_Standing_Lamp_Purple.RequiredItems.Add("Iron", 1, true);
			CL_Standing_Lamp_Purple.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Standing_Lamp_Red = AddLightPiece("CL_Standing_Lamp_Red");
			CL_Standing_Lamp_Red.RequiredItems.Add("Uncut_Red_Stone", 1, true);
			CL_Standing_Lamp_Red.RequiredItems.Add("Iron", 1, true);
			CL_Standing_Lamp_Red.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Standing_Lamp_Yellow = AddLightPiece("CL_Standing_Lamp_Yellow");
			CL_Standing_Lamp_Yellow.RequiredItems.Add("Uncut_Yellow_Stone", 1, true);
			CL_Standing_Lamp_Yellow.RequiredItems.Add("Iron", 1, true);
			CL_Standing_Lamp_Yellow.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Chandelier_Black = AddLightPiece("CL_Chandelier_Black");
			CL_Chandelier_Black.RequiredItems.Add("Uncut_Black_Stone", 15, true);
			CL_Chandelier_Black.RequiredItems.Add("Iron", 5, true);
			CL_Chandelier_Black.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Chandelier_Blue = AddLightPiece("CL_Chandelier_Blue");
			CL_Chandelier_Blue.RequiredItems.Add("Uncut_Blue_Stone", 15, true);
			CL_Chandelier_Blue.RequiredItems.Add("Iron", 5, true);
			CL_Chandelier_Blue.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Chandelier_Green = AddLightPiece("CL_Chandelier_Green");
			CL_Chandelier_Green.RequiredItems.Add("Uncut_Green_Stone", 15, true);
			CL_Chandelier_Green.RequiredItems.Add("Iron", 5, true);
			CL_Chandelier_Green.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Chandelier_Orange = AddLightPiece("CL_Chandelier_Orange");
			CL_Chandelier_Orange.RequiredItems.Add("Uncut_Orange_Stone", 15, true);
			CL_Chandelier_Orange.RequiredItems.Add("Iron", 5, true);
			CL_Chandelier_Orange.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Chandelier_Yellow = AddLightPiece("CL_Chandelier_Yellow");
			CL_Chandelier_Yellow.RequiredItems.Add("Uncut_Yellow_Stone", 15, true);
			CL_Chandelier_Yellow.RequiredItems.Add("Iron", 5, true);
			CL_Chandelier_Yellow.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Chandelier_Purple = AddLightPiece("CL_Chandelier_Purple");
			CL_Chandelier_Purple.RequiredItems.Add("Uncut_Purple_Stone", 15, true);
			CL_Chandelier_Purple.RequiredItems.Add("Iron", 5, true);
			CL_Chandelier_Purple.Category.Add(BuildPieceCategory.Misc);

			BuildPiece CL_Chandelier_Red = AddLightPiece("CL_Chandelier_Red");
			CL_Chandelier_Red.RequiredItems.Add("Uncut_Red_Stone", 15, true);
			CL_Chandelier_Red.RequiredItems.Add("Iron", 5, true);
			CL_Chandelier_Red.Category.Add(BuildPieceCategory.Misc);

			#endregion

			Assembly assembly = Assembly.GetExecutingAssembly();
			Harmony harmony = new(ModGUID);
			harmony.PatchAll(assembly);

		}

		private class ConfigurationManagerAttributes
		{
			[UsedImplicitly]
			public string? Category = null;
		}

		private BuildPiece AddLightPiece(string prefabName)
		{
			BuildPiece light = new(assets, prefabName, true, "CL_Crystal_Hammer");
			string pieceName = light.Prefab.GetComponent<Piece>().m_name;
			pieceName = new Regex("['[\"\\]]").Replace(english.Localize(pieceName), "").Trim();
			string localizedName = Localization.instance.Localize(pieceName).Trim();
			lightIntensities.Add(prefabName, config(pieceName, "Light Intensity", light.Prefab.GetComponentInChildren<Light>().range, new ConfigDescription($"The range of the light emitted by {english.Localize(prefabName)}.", null, new ConfigurationManagerAttributes { Category = localizedName })));
			lightIntensities[prefabName].SettingChanged += (_, _) => SetLightRange(light.Prefab);
			SetLightRange(light.Prefab);

			return light;
		}

		private void SetLightRange(GameObject lightPrefab)
		{
			foreach (Light light in lightPrefab.GetComponentsInChildren<Light>())
			{
				light.range = lightIntensities[lightPrefab.name].Value;
			}
		}
	}
}

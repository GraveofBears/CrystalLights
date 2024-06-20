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
		private const string ModVersion = "1.0.18";
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


			Item CL_Crystal_Hammer = new(assets, "CL_Crystal_Hammer"); 
			CL_Crystal_Hammer.Crafting.Add(ItemManager.CraftingTable.Workbench, 1);
			CL_Crystal_Hammer.RequiredItems.Add("Uncut_Red_Stone", 1);
            CL_Crystal_Hammer.RequiredItems.Add("Wood", 1);
            CL_Crystal_Hammer.CraftAmount = 1;

			#region pieces

			BuildPiece CL_Large_Wall_Black = AddLightPiece("CL_Large_Wall_Black");
			CL_Large_Wall_Black.RequiredItems.Add("Uncut_Black_Stone", 10, true);
			CL_Large_Wall_Black.RequiredItems.Add("IronNails", 2, true);
            CL_Large_Wall_Black.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Large_Wall_Black.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Large_Wall_White = AddLightPiece("CL_Large_Wall_White");
            CL_Large_Wall_White.RequiredItems.Add("Uncut_Black_Stone", 10, true);
            CL_Large_Wall_White.RequiredItems.Add("IronNails", 2, true);
            CL_Large_Wall_White.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Large_Wall_White.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Large_Wall_Blue = AddLightPiece("CL_Large_Wall_Blue");
			CL_Large_Wall_Blue.RequiredItems.Add("Uncut_Blue_Stone", 10, true);
			CL_Large_Wall_Blue.RequiredItems.Add("IronNails", 2, true);
            CL_Large_Wall_Blue.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Large_Wall_Blue.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Large_Wall_Green = AddLightPiece("CL_Large_Wall_Green");
			CL_Large_Wall_Green.RequiredItems.Add("Uncut_Green_Stone", 10, true);
			CL_Large_Wall_Green.RequiredItems.Add("IronNails", 2, true);
            CL_Large_Wall_Green.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Large_Wall_Green.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Large_Wall_Orange = AddLightPiece("CL_Large_Wall_Orange");
			CL_Large_Wall_Orange.RequiredItems.Add("Uncut_Orange_Stone", 10, true);
			CL_Large_Wall_Orange.RequiredItems.Add("IronNails", 2, true);
            CL_Large_Wall_Orange.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Large_Wall_Orange.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Large_Wall_Purple = AddLightPiece("CL_Large_Wall_Purple");
			CL_Large_Wall_Purple.RequiredItems.Add("Uncut_Purple_Stone", 10, true);
			CL_Large_Wall_Purple.RequiredItems.Add("IronNails", 2, true);
            CL_Large_Wall_Purple.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Large_Wall_Purple.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Large_Wall_Red = AddLightPiece("CL_Large_Wall_Red");
			CL_Large_Wall_Red.RequiredItems.Add("Uncut_Red_Stone", 10, true);
			CL_Large_Wall_Red.RequiredItems.Add("IronNails", 2, true);
            CL_Large_Wall_Red.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Large_Wall_Red.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Large_Wall_Yellow = AddLightPiece("CL_Large_Wall_Yellow");
			CL_Large_Wall_Yellow.RequiredItems.Add("Uncut_Yellow_Stone", 10, true);
			CL_Large_Wall_Yellow.RequiredItems.Add("IronNails", 2, true);
            CL_Large_Wall_Yellow.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Large_Wall_Yellow.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Small_Wall_Black = AddLightPiece("CL_Small_Wall_Black");
			CL_Small_Wall_Black.RequiredItems.Add("Uncut_Black_Stone", 3, true);
			CL_Small_Wall_Black.RequiredItems.Add("IronNails", 1, true);
            CL_Small_Wall_Black.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Small_Wall_Black.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Small_Wall_White = AddLightPiece("CL_Small_Wall_White");
            CL_Small_Wall_White.RequiredItems.Add("Uncut_Black_Stone", 3, true);
            CL_Small_Wall_White.RequiredItems.Add("IronNails", 1, true);
            CL_Small_Wall_White.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Small_Wall_White.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Small_Wall_Blue = AddLightPiece("CL_Small_Wall_Blue");
			CL_Small_Wall_Blue.RequiredItems.Add("Uncut_Blue_Stone", 3, true);
			CL_Small_Wall_Blue.RequiredItems.Add("IronNails", 1, true);
            CL_Small_Wall_Blue.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Small_Wall_Blue.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Small_Wall_Green = AddLightPiece("CL_Small_Wall_Green");
			CL_Small_Wall_Green.RequiredItems.Add("Uncut_Green_Stone", 3, true);
			CL_Small_Wall_Green.RequiredItems.Add("IronNails", 1, true);
            CL_Small_Wall_Green.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Small_Wall_Green.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Small_Wall_Orange = AddLightPiece("CL_Small_Wall_Orange");
			CL_Small_Wall_Orange.RequiredItems.Add("Uncut_Orange_Stone", 3, true);
			CL_Small_Wall_Orange.RequiredItems.Add("IronNails", 1, true);
            CL_Small_Wall_Orange.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Small_Wall_Orange.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Small_Wall_Purple = AddLightPiece("CL_Small_Wall_Purple");
			CL_Small_Wall_Purple.RequiredItems.Add("Uncut_Purple_Stone", 3, true);
			CL_Small_Wall_Purple.RequiredItems.Add("IronNails", 1, true);
            CL_Small_Wall_Purple.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Small_Wall_Purple.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Small_Wall_Red = AddLightPiece("CL_Small_Wall_Red");
			CL_Small_Wall_Red.RequiredItems.Add("Uncut_Red_Stone", 3, true);
			CL_Small_Wall_Red.RequiredItems.Add("IronNails", 1, true);
            CL_Small_Wall_Red.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Small_Wall_Red.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Small_Wall_Yellow = AddLightPiece("CL_Small_Wall_Yellow");
			CL_Small_Wall_Yellow.RequiredItems.Add("Uncut_Yellow_Stone", 3, true);
			CL_Small_Wall_Yellow.RequiredItems.Add("IronNails", 1, true);
            CL_Small_Wall_Yellow.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Small_Wall_Yellow.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Standing_Lamp_Black = AddLightPiece("CL_Standing_Lamp_Black");
			CL_Standing_Lamp_Black.RequiredItems.Add("Uncut_Black_Stone", 1, true);
			CL_Standing_Lamp_Black.RequiredItems.Add("IronNails", 1, true);
            CL_Standing_Lamp_Black.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Standing_Lamp_Black.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Standing_Lamp_White = AddLightPiece("CL_Standing_Lamp_White");
            CL_Standing_Lamp_White.RequiredItems.Add("Uncut_Black_Stone", 1, true);
            CL_Standing_Lamp_White.RequiredItems.Add("IronNails", 1, true);
            CL_Standing_Lamp_White.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Standing_Lamp_White.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Standing_Lamp_Blue = AddLightPiece("CL_Standing_Lamp_Blue");
			CL_Standing_Lamp_Blue.RequiredItems.Add("Uncut_Blue_Stone", 1, true);
			CL_Standing_Lamp_Blue.RequiredItems.Add("IronNails", 1, true);
            CL_Standing_Lamp_Blue.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Standing_Lamp_Blue.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Standing_Lamp_Green = AddLightPiece("CL_Standing_Lamp_Green");
			CL_Standing_Lamp_Green.RequiredItems.Add("Uncut_Green_Stone", 1, true);
			CL_Standing_Lamp_Green.RequiredItems.Add("IronNails", 1, true);
            CL_Standing_Lamp_Green.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Standing_Lamp_Green.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Standing_Lamp_Orange = AddLightPiece("CL_Standing_Lamp_Orange");
			CL_Standing_Lamp_Orange.RequiredItems.Add("Uncut_Orange_Stone", 1, true);
			CL_Standing_Lamp_Orange.RequiredItems.Add("IronNails", 1, true);
            CL_Standing_Lamp_Orange.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Standing_Lamp_Orange.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Standing_Lamp_Purple = AddLightPiece("CL_Standing_Lamp_Purple");
			CL_Standing_Lamp_Purple.RequiredItems.Add("Uncut_Purple_Stone", 1, true);
			CL_Standing_Lamp_Purple.RequiredItems.Add("IronNails", 1, true);
            CL_Standing_Lamp_Purple.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Standing_Lamp_Purple.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Standing_Lamp_Red = AddLightPiece("CL_Standing_Lamp_Red");
			CL_Standing_Lamp_Red.RequiredItems.Add("Uncut_Red_Stone", 1, true);
			CL_Standing_Lamp_Red.RequiredItems.Add("IronNails", 1, true);
            CL_Standing_Lamp_Red.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Standing_Lamp_Red.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Standing_Lamp_Yellow = AddLightPiece("CL_Standing_Lamp_Yellow");
			CL_Standing_Lamp_Yellow.RequiredItems.Add("Uncut_Yellow_Stone", 1, true);
			CL_Standing_Lamp_Yellow.RequiredItems.Add("IronNails", 1, true);
            CL_Standing_Lamp_Yellow.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Standing_Lamp_Yellow.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Chandelier_Black = AddLightPiece("CL_Chandelier_Black");
			CL_Chandelier_Black.RequiredItems.Add("Uncut_Black_Stone", 15, true);
			CL_Chandelier_Black.RequiredItems.Add("IronNails", 5, true);
            CL_Chandelier_Black.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Chandelier_Black.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Chandelier_White = AddLightPiece("CL_Chandelier_White");
            CL_Chandelier_White.RequiredItems.Add("Uncut_Black_Stone", 15, true);
            CL_Chandelier_White.RequiredItems.Add("IronNails", 5, true);
            CL_Chandelier_White.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Chandelier_White.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Chandelier_Blue = AddLightPiece("CL_Chandelier_Blue");
			CL_Chandelier_Blue.RequiredItems.Add("Uncut_Blue_Stone", 15, true);
			CL_Chandelier_Blue.RequiredItems.Add("IronNails", 5, true);
            CL_Chandelier_Blue.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Chandelier_Blue.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Chandelier_Green = AddLightPiece("CL_Chandelier_Green");
			CL_Chandelier_Green.RequiredItems.Add("Uncut_Green_Stone", 15, true);
			CL_Chandelier_Green.RequiredItems.Add("IronNails", 5, true);
            CL_Chandelier_Green.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Chandelier_Green.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Chandelier_Orange = AddLightPiece("CL_Chandelier_Orange");
			CL_Chandelier_Orange.RequiredItems.Add("Uncut_Orange_Stone", 15, true);
			CL_Chandelier_Orange.RequiredItems.Add("IronNails", 5, true);
            CL_Chandelier_Orange.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Chandelier_Orange.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Chandelier_Yellow = AddLightPiece("CL_Chandelier_Yellow");
			CL_Chandelier_Yellow.RequiredItems.Add("Uncut_Yellow_Stone", 15, true);
			CL_Chandelier_Yellow.RequiredItems.Add("IronNails", 5, true);
            CL_Chandelier_Yellow.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Chandelier_Yellow.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Chandelier_Purple = AddLightPiece("CL_Chandelier_Purple");
			CL_Chandelier_Purple.RequiredItems.Add("Uncut_Purple_Stone", 15, true);
			CL_Chandelier_Purple.RequiredItems.Add("IronNails", 5, true);
            CL_Chandelier_Purple.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Chandelier_Purple.Category.Set(BuildPieceCategory.Misc);

			BuildPiece CL_Chandelier_Red = AddLightPiece("CL_Chandelier_Red");
			CL_Chandelier_Red.RequiredItems.Add("Uncut_Red_Stone", 15, true);
			CL_Chandelier_Red.RequiredItems.Add("IronNails", 5, true);
            CL_Chandelier_Red.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Chandelier_Red.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Brazier_Black = AddLightPiece("CL_Brazier_Black");
            CL_Brazier_Black.RequiredItems.Add("Uncut_Black_Stone", 10, true);
            CL_Brazier_Black.RequiredItems.Add("IronNails", 4, true);
            CL_Brazier_Black.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Brazier_Black.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Brazier_White = AddLightPiece("CL_Brazier_White");
            CL_Brazier_White.RequiredItems.Add("Uncut_Black_Stone", 10, true);
            CL_Brazier_White.RequiredItems.Add("IronNails", 4, true);
            CL_Brazier_White.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Brazier_White.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Brazier_Blue = AddLightPiece("CL_Brazier_Blue");
            CL_Brazier_Blue.RequiredItems.Add("Uncut_Blue_Stone", 10, true);
            CL_Brazier_Blue.RequiredItems.Add("IronNails", 4, true);
            CL_Brazier_Blue.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Brazier_Blue.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Brazier_Green = AddLightPiece("CL_Brazier_Green");
            CL_Brazier_Green.RequiredItems.Add("Uncut_Green_Stone", 10, true);
            CL_Brazier_Green.RequiredItems.Add("IronNails", 4, true);
            CL_Brazier_Green.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Brazier_Green.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Brazier_Orange = AddLightPiece("CL_Brazier_Orange");
            CL_Brazier_Orange.RequiredItems.Add("Uncut_Orange_Stone", 10, true);
            CL_Brazier_Orange.RequiredItems.Add("IronNails", 4, true);
            CL_Brazier_Orange.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Brazier_Orange.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Brazier_Purple = AddLightPiece("CL_Brazier_Purple");
            CL_Brazier_Purple.RequiredItems.Add("Uncut_Purple_Stone", 10, true);
            CL_Brazier_Purple.RequiredItems.Add("IronNails", 4, true);
            CL_Brazier_Purple.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Brazier_Purple.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Brazier_Red = AddLightPiece("CL_Brazier_Red");
            CL_Brazier_Red.RequiredItems.Add("Uncut_Red_Stone", 10, true);
            CL_Brazier_Red.RequiredItems.Add("IronNails", 4, true);
            CL_Brazier_Red.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Brazier_Red.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Brazier_Yellow = AddLightPiece("CL_Brazier_Yellow");
            CL_Brazier_Yellow.RequiredItems.Add("Uncut_Yellow_Stone", 10, true);
            CL_Brazier_Yellow.RequiredItems.Add("IronNails", 4, true);
            CL_Brazier_Yellow.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Brazier_Yellow.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Hanging_Black = AddLightPiece("CL_Hanging_Black");
            CL_Hanging_Black.RequiredItems.Add("Uncut_Black_Stone", 5, true);
            CL_Hanging_Black.RequiredItems.Add("IronNails", 2, true);
            CL_Hanging_Black.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Hanging_Black.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Hanging_White = AddLightPiece("CL_Hanging_White");
            CL_Hanging_White.RequiredItems.Add("Uncut_Black_Stone", 5, true);
            CL_Hanging_White.RequiredItems.Add("IronNails", 2, true);
            CL_Hanging_White.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Hanging_White.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Hanging_Blue = AddLightPiece("CL_Hanging_Blue");
            CL_Hanging_Blue.RequiredItems.Add("Uncut_Blue_Stone", 5, true);
            CL_Hanging_Blue.RequiredItems.Add("IronNails", 2, true);
            CL_Hanging_Blue.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Hanging_Blue.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Hanging_Green = AddLightPiece("CL_Hanging_Green");
            CL_Hanging_Green.RequiredItems.Add("Uncut_Green_Stone", 5, true);
            CL_Hanging_Green.RequiredItems.Add("IronNails", 2, true);
            CL_Hanging_Green.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Hanging_Green.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Hanging_Orange = AddLightPiece("CL_Hanging_Orange");
            CL_Hanging_Orange.RequiredItems.Add("Uncut_Orange_Stone", 5, true);
            CL_Hanging_Orange.RequiredItems.Add("IronNails", 2, true);
            CL_Hanging_Orange.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Hanging_Orange.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Hanging_Purple = AddLightPiece("CL_Hanging_Purple");
            CL_Hanging_Purple.RequiredItems.Add("Uncut_Purple_Stone", 5, true);
            CL_Hanging_Purple.RequiredItems.Add("IronNails", 2, true);
            CL_Hanging_Purple.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Hanging_Purple.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Hanging_Red = AddLightPiece("CL_Hanging_Red");
            CL_Hanging_Red.RequiredItems.Add("Uncut_Red_Stone", 5, true);
            CL_Hanging_Red.RequiredItems.Add("IronNails", 2, true);
            CL_Hanging_Red.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Hanging_Red.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Hanging_Yellow = AddLightPiece("CL_Hanging_Yellow");
            CL_Hanging_Yellow.RequiredItems.Add("Uncut_Yellow_Stone", 5, true);
            CL_Hanging_Yellow.RequiredItems.Add("IronNails", 2, true);
            CL_Hanging_Yellow.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Hanging_Yellow.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Pole_Black = AddLightPiece("CL_Pole_Black");
            CL_Pole_Black.RequiredItems.Add("Uncut_Black_Stone", 5, true);
            CL_Pole_Black.RequiredItems.Add("Iron", 2, true);
            CL_Pole_Black.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Pole_Black.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Pole_White = AddLightPiece("CL_Pole_White");
            CL_Pole_White.RequiredItems.Add("Uncut_Black_Stone", 5, true);
            CL_Pole_White.RequiredItems.Add("Iron", 2, true);
            CL_Pole_White.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Pole_White.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Pole_Blue = AddLightPiece("CL_Pole_Blue");
            CL_Pole_Blue.RequiredItems.Add("Uncut_Blue_Stone", 5, true);
            CL_Pole_Blue.RequiredItems.Add("Iron", 2, true);
            CL_Pole_Blue.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Pole_Blue.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Pole_Green = AddLightPiece("CL_Pole_Green");
            CL_Pole_Green.RequiredItems.Add("Uncut_Green_Stone", 5, true);
            CL_Pole_Green.RequiredItems.Add("Iron", 2, true);
            CL_Pole_Green.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Pole_Green.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Pole_Orange = AddLightPiece("CL_Pole_Orange");
            CL_Pole_Orange.RequiredItems.Add("Uncut_Orange_Stone", 5, true);
            CL_Pole_Orange.RequiredItems.Add("Iron", 2, true);
            CL_Pole_Orange.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Pole_Orange.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Pole_Purple = AddLightPiece("CL_Pole_Purple");
            CL_Pole_Purple.RequiredItems.Add("Uncut_Purple_Stone", 5, true);
            CL_Pole_Purple.RequiredItems.Add("Iron", 2, true);
            CL_Pole_Purple.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Pole_Purple.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Pole_Red = AddLightPiece("CL_Pole_Red");
            CL_Pole_Red.RequiredItems.Add("Uncut_Red_Stone", 5, true);
            CL_Pole_Red.RequiredItems.Add("Iron", 2, true);
            CL_Pole_Red.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Pole_Red.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Pole_Yellow = AddLightPiece("CL_Pole_Yellow");
            CL_Pole_Yellow.RequiredItems.Add("Uncut_Yellow_Stone", 5, true);
            CL_Pole_Yellow.RequiredItems.Add("Iron", 2, true);
            CL_Pole_Yellow.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Pole_Yellow.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Raw_Black = AddLightPiece("CL_Raw_Black");
            CL_Raw_Black.RequiredItems.Add("Uncut_Black_Stone", 5, true);
            CL_Raw_Black.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Raw_Black.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Raw_White = AddLightPiece("CL_Raw_White");
            CL_Raw_White.RequiredItems.Add("Uncut_Black_Stone", 5, true);
            CL_Raw_White.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Raw_White.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Raw_Blue = AddLightPiece("CL_Raw_Blue");
            CL_Raw_Blue.RequiredItems.Add("Uncut_Blue_Stone", 5, true);
            CL_Raw_Blue.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Raw_Blue.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Raw_Green = AddLightPiece("CL_Raw_Green");
            CL_Raw_Green.RequiredItems.Add("Uncut_Green_Stone", 5, true);
            CL_Raw_Green.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Raw_Green.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Raw_Orange = AddLightPiece("CL_Raw_Orange");
            CL_Raw_Orange.RequiredItems.Add("Uncut_Orange_Stone", 5, true);
            CL_Raw_Orange.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Raw_Orange.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Raw_Purple = AddLightPiece("CL_Raw_Purple");
            CL_Raw_Purple.RequiredItems.Add("Uncut_Purple_Stone", 5, true);
            CL_Raw_Purple.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Raw_Purple.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Raw_Red = AddLightPiece("CL_Raw_Red");
            CL_Raw_Red.RequiredItems.Add("Uncut_Red_Stone", 5, true);
            CL_Raw_Red.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Raw_Red.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Raw_Yellow = AddLightPiece("CL_Raw_Yellow");
            CL_Raw_Yellow.RequiredItems.Add("Uncut_Yellow_Stone", 5, true);
            CL_Raw_Yellow.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Raw_Yellow.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Standing_Torch_Black = AddLightPiece("CL_Standing_Torch_Black");
            CL_Standing_Torch_Black.RequiredItems.Add("Uncut_Black_Stone", 1, true);
            CL_Standing_Torch_Black.RequiredItems.Add("Wood", 2, true);
            CL_Standing_Torch_Black.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Standing_Torch_Black.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Standing_Torch_White = AddLightPiece("CL_Standing_Torch_White");
            CL_Standing_Torch_White.RequiredItems.Add("Uncut_Black_Stone", 1, true);
            CL_Standing_Torch_White.RequiredItems.Add("Wood", 2, true);
            CL_Standing_Torch_White.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Standing_Torch_White.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Standing_Torch_Blue = AddLightPiece("CL_Standing_Torch_Blue");
            CL_Standing_Torch_Blue.RequiredItems.Add("Uncut_Blue_Stone", 1, true);
            CL_Standing_Torch_Blue.RequiredItems.Add("Wood", 2, true);
            CL_Standing_Torch_Blue.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Standing_Torch_Blue.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Standing_Torch_Green = AddLightPiece("CL_Standing_Torch_Green");
            CL_Standing_Torch_Green.RequiredItems.Add("Uncut_Green_Stone", 1, true);
            CL_Standing_Torch_Green.RequiredItems.Add("Wood", 2, true);
            CL_Standing_Torch_Green.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Standing_Torch_Green.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Standing_Torch_Orange = AddLightPiece("CL_Standing_Torch_Orange");
            CL_Standing_Torch_Orange.RequiredItems.Add("Uncut_Orange_Stone", 1, true);
            CL_Standing_Torch_Orange.RequiredItems.Add("Wood", 2, true);
            CL_Standing_Torch_Orange.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Standing_Torch_Orange.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Standing_Torch_Purple = AddLightPiece("CL_Standing_Torch_Purple");
            CL_Standing_Torch_Purple.RequiredItems.Add("Uncut_Purple_Stone", 1, true);
            CL_Standing_Torch_Purple.RequiredItems.Add("Wood", 2, true);
            CL_Standing_Torch_Purple.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Standing_Torch_Purple.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Standing_Torch_Red = AddLightPiece("CL_Standing_Torch_Red");
            CL_Standing_Torch_Red.RequiredItems.Add("Uncut_Red_Stone", 1, true);
            CL_Standing_Torch_Red.RequiredItems.Add("Wood", 2, true);
            CL_Standing_Torch_Red.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Standing_Torch_Red.Category.Set(BuildPieceCategory.Misc);

            BuildPiece CL_Standing_Torch_Yellow = AddLightPiece("CL_Standing_Torch_Yellow");
            CL_Standing_Torch_Yellow.RequiredItems.Add("Uncut_Yellow_Stone", 1, true);
            CL_Standing_Torch_Yellow.RequiredItems.Add("Wood", 2, true);
            CL_Standing_Torch_Yellow.Crafting.Set(PieceManager.CraftingTable.None);
            CL_Standing_Torch_Yellow.Category.Set(BuildPieceCategory.Misc);



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
            BuildPiece light = new(assets, prefabName);
            light.Tool.Add("CL_Crystal_Hammer");
            string pieceName = light.Prefab.GetComponent<Piece>().m_name;
			pieceName = new Regex("['[\"\\]]").Replace(english.Localize(pieceName), "").Trim();
			string localizedName = Localization.instance.Localize(pieceName).Trim();
			lightIntensities.Add(prefabName, config(pieceName, "Light Range", light.Prefab.GetComponentInChildren<Light>().range, new ConfigDescription($"The range of the light emitted by {english.Localize(prefabName)}.", null, new ConfigurationManagerAttributes { Category = localizedName })));
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

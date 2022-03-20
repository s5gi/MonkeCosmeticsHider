using BepInEx;
using System;
using UnityEngine;
using Utilla;

namespace MonkeCosmeticsHider
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {

        private static readonly string[] lastCosmetics = new string[3];
        private static bool firstHide = false;
        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            RehideCosmetics();
            Postfix();
        }
        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
        }
        private static void RehideCosmetics()
        {
            foreach (GameObject obj in FindObjectsOfType<GameObject>()) // Cycle through all game objects.
            {
                if (obj.activeInHierarchy)
                {
                    try
                    {
                        if (obj.transform.parent.name == "Cosmetics" ||
                        obj.transform.parent.transform.parent.name == "Cosmetics") // Getting the parent name as well because some cosmetics have a bow and they do weird stuff.
                        {
                            do // Filter the cosmetic buttons and stuff.
                            {
                                obj.layer = 5; // Set layer to 5.
                                firstHide = true; // If a cosmetic has actually been hidden, then stop checking.
                            } while (!(obj.name.Contains("Button") || obj.name.Contains("BUTTON") || obj.name.Contains("Badge") || obj.name.Contains("BADGE") || obj.name.Contains("Text") || obj.name.Contains("TEXT") || obj.name.Contains("GLASSES") || obj.name.Contains("Glasses")));
                        }
                    }
                    catch
                    {
                        
                    }
                }
            }
            Camera.allCameras[0].cullingMask = ~(1 << 5); // Hide layer 5 from the main camera. I'm pretty sure that the main cameras index in allCameras won't change, so I'm not checking if index 0 is the right camera.
        }



        private static void Postfix()
        {
            if (!firstHide) RehideCosmetics(); // Cosmetics take a bit to load in, so wait until a cosmetic is actually hidden.

            string[] cosmetics = {
                    PlayerPrefs.GetString("hatCosmetic", "none"),
                    PlayerPrefs.GetString("faceCosmetic", "none"),
                    PlayerPrefs.GetString("badgeCosmetic", "none")
                };
            for (int i = 0; i < 3; i++)
            {
                if (lastCosmetics[i] != cosmetics[i]) // If a cosmetic changed, rehide all cosmetics.
                {
                    RehideCosmetics();
                    lastCosmetics[i] = cosmetics[i];
                }
            }
        }
    }
}

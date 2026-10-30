using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace MR_Training_ASSAR.GameManagement
{
    public class LanguageSelector : MonoBehaviour
    {
        private bool active = false;
        GameManager gameManager;

        private void Start()
        {
            gameManager = GetComponent<GameManager>();
        }

        public void ChangeLanguage(int localeID)
        {
            if (active == true)
                return;
            StartCoroutine(SetLocale(localeID));
            gameManager.GetLanguage(localeID);
        }

        IEnumerator SetLocale(int _localeID)
        {
            active = true;
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
            active = false;
        }
    }
}
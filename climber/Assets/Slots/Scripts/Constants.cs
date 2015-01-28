using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour
{
    public const string IOS = "ios";
    public const string Android = "android";

    public const string PopupPanelName = "PopupPanel";
    public const string DownloadedSlots = "DownloadedSlots";
    public const string WaitingBonusLevel = "WaitingBonusLevel";
    
    // Settings
    public const string SettingsLogin = "login";
    public const string SettingsPassword = "passw";
    public const string SettingsRegistered = "registered";
    public const string SettingsRefer = "refer";
    public const string SettingsTr = "tr";
    public const string SettingsFkey = "fkey";
    public const string SettingsUseDebugParam = "useDebugParam";
    public const string SettingsGoogleReferrerParse = "googleReferrerParse";
    public const string SettingsSelectedSlot = "selectedSlot";
    public const string SettingsSelectedSlotIndex = "selectedSlotIndex";
    public const string SettingsAutoplay = "isAutoplay";
    public const string SettingsMusic = "isPlayMusic";
    public const string SettingsSound = "isPlaySound";
    public const string SettingsNotification = "isShowNotification";
    public const string SettingsApplyEula = "isApplyEula";
    public const string SettingsShowWelcomePopup = "showWelcomePopup";
    public const string SettingsBonusReceived = "isBonusReceived";
    public const string SettingsDefaultLayersName = "2D GUI";
    public const string SettingsSlotsSortingLayersName = "Slots";
    public const string SettingsSlotsList = "SlotsList";
    public const string SettingsCredential = "credential";

    public const string SettingsSlotReelsLayerName =  "SlotReels";
    public const string SettingsSlotAnimationOverlayLayer = "SlotOverlay";

    // Popups
    public const string WaitLoginPopup = "WaitLogin";
    public const string WaitPopup = "Wait";
    public const string WaitStartingSlotPopup = "WaitStartingSlot";
    public const string NewLevelPopup = "NewLevel";
    public const string NewLevelAndSlotPopup = "NewLevelOpenNewSlot";
    public const string NewStatusPopup = "NewStatus";
    
    //russian constants
    public const string CurrentLevel = "{0} уровень";
    public const string DownloadSlots = "Для запуска слота необходимо скачать обновление объемом {0} Кб.\n Вы действительно хотите скачать обновление?";
    public const string DownloadBtn = "Скачать";
    public const string LaterBtn = "Позже";
    public const string Bankrupt = "Банкрот";

    //restore dialog
    public const string RestoreDialogTitle = "Введите пароль для пользователя [{0}]";
    public const string RestoreDialogRestoreBtn = "Восстановить";
    public const string RestoreDialogOkBtn = "OK";
}
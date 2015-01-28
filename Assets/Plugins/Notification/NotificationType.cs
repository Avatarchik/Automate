using UnityEngine;
using System.Collections;

public enum NotificationType {

	// FUN cumulative
	[NotificationAttr(1, "{0} ГОТОВ", "Соберите его в Лобби!", 0, 0, 0, "Start")] FUN_BONUS_READY,
	[NotificationAttr(2, "{0} ЖДЕТ", "Загляните в игру, соберите свой бонус!", 7200, 12, 22, "Start")] FUN_BONUS_READY_2,
	[NotificationAttr(3, "БОНУС ГОТОВ К СБОРУ", "Прекрасный повод зайти в игру!", 72000, 12, 22, "Start")] FUN_BONUS_READY_20,
	[NotificationAttr(4, "{0} ЖДЕТ В ЛОББИ", "Соберите его, испытайте удачу!", 158400, 12, 22, "Start")] FUN_BONUS_READY_44,
	[NotificationAttr(5, "ВАШ БОНУС ГОТОВ", "Соберите его прямо сейчас, сорвите джекпот!", 331200, 20, 22, "Start")] FUN_BONUS_READY_92,
	[NotificationAttr(6, "{0} ЖДЕТ В ЛОББИ", "Соберите его, испытайте удачу!", 590400, 20, 22, "Start")] FUN_BONUS_READY_164,
	[NotificationAttr(7, "ВОЗВРАЩАЙТЕСЬ В ИГРУ", "{0} ждет Вас в лобби. Возвращайтесь!", 1022400, 20, 22, "Start")] FUN_BONUS_READY_284,
	[NotificationAttr(8, "ВОЗВРАЩАЙТЕСЬ В ИГРУ", "Заходите, сегодня обязательно повезет!", 1454400, 20, 22, "Start")] FUN_BONUS_READY_404,
	[NotificationAttr(9, "ВОЗВРАЩАЙТЕСЬ В ИГРУ", "Заходите, сегодня обязательно повезет!", 1886400, 20, 22, "Start")] FUN_BONUS_READY_524,
	[NotificationAttr(10, "ВАШ БОНУС ПРОТУХ", "Вы не собирали его целый месяц. Похоже, он протух.", 2318400, 20, 22, "Start")] FUN_BONUS_READY_644,    
	// FUN special
	[NotificationAttr(11, "Торопись!", "Спецпредложение истекает через 5 минут!", 600, 0, 0, "Start")] FUN_SPECIAL_OFFER,

	// REAL common
	[NotificationAttr(20, "{0}", "{0}", 7200, 12, 22, "Cashier")] REAL_BONUS_2,
	[NotificationAttr(21, "{0}", "{0}", 86400, 12, 22, "Cashier")] REAL_BONUS_24,
	[NotificationAttr(22, "{0}", "{0}", 345600, 12, 22, "Cashier")] REAL_BONUS_48,
	[NotificationAttr(23, "{0}", "{0}", 345600, 20, 22, "Cashier")] REAL_BONUS_96,
	[NotificationAttr(24, "{0}", "{0}", 604800, 20, 22, "Cashier")] REAL_BONUS_168,
	[NotificationAttr(25, "{0}", "{0}", 1036800, 20, 22, "Cashier")] REAL_BONUS_288,
	[NotificationAttr(26, "{0}", "{0}", 1468800, 20, 22, "Cashier")] REAL_BONUS_408,
	[NotificationAttr(27, "{0}", "{0}", 1900800, 20, 22, "Cashier")] REAL_BONUS_528,
	[NotificationAttr(28, "{0}", "{0}", 2332800, 20, 22, "Cashier")] REAL_BONUS_648,
	// REAL welcome
	[NotificationAttr(29, "БОНУС ПРИВЕТСТВЕННЫЙ!", "Предложение активно всего 30 минут! Торопитесь!", 1800, 0, 0, "Cashier")] WELCOME_BONUS,
	[NotificationAttr(30, "5$ БЕСПЛАТНО!", "Подарок новому игроку! Заходите!", 600, 0, 0, "Start")] MOBILE_BONUS    
}

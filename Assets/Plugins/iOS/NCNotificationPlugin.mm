//
//  NCNotificationPlugin.m
//
//
//  Created by Max on 25/11/14.
//
//

#import "NCNotificationPlugin.h"

extern UIViewController *UnityGetGLViewController();
extern "C" void UnitySendMessage(const char *, const char *, const char *);

@implementation NCNotificationPlugin

- (id)init
{
    self = [super init];
    UIApplication *application = [UIApplication sharedApplication];
    // are you running on iOS8?
    if ([application respondsToSelector:@selector(registerUserNotificationSettings:)])
    {
        UIUserNotificationSettings *settings = [UIUserNotificationSettings settingsForTypes:(UIUserNotificationTypeBadge|UIUserNotificationTypeAlert|UIUserNotificationTypeSound) categories:nil];
        [application registerUserNotificationSettings:settings];
    }
    else // iOS 7 or earlier
    {
        UIRemoteNotificationType myTypes = UIRemoteNotificationTypeBadge | UIRemoteNotificationTypeAlert | UIRemoteNotificationTypeSound;
        [application registerForRemoteNotificationTypes:myTypes];
    }
    return self;
}


- (void) initNotifications:(const char*) jsonNotifications
{
    NSString *jsonString = [NSString stringWithUTF8String: jsonNotifications];
    NSLog(@"%@", [NSString stringWithFormat:@"DEBUG: iOS: initNotification: jsonNotifications %@", jsonString]);
    NSError *error;
    NSData *jsonData = [jsonString dataUsingEncoding:NSUTF8StringEncoding];
    NSArray *jsonArray = [NSJSONSerialization JSONObjectWithData:jsonData options: NSJSONReadingMutableContainers error:&error];
    for (NSDictionary *dic in jsonArray){
        int identifier = [[dic valueForKey:@"id"] intValue];
        NSString *title = (NSString*) [dic valueForKey:@"title"];
        NSString *content = (NSString*) [dic valueForKey:@"content"];
        NSString *tag = (NSString*) [dic valueForKey:@"tag"];
        long timeSec = [[dic valueForKey:@"timeSec"] longValue];
        [self removeNotificationWith: identifier];
        [self initNotification:identifier withTitle:title withContent:content withTag:tag atTime:timeSec];
    }
}

- (void) initNotification:(const int)identifier withTitle:(NSString *)title withContent:(NSString *)content withTag:(NSString *)tag atTime:(const long)timeSec
{
    NSLog(@"DEBUG: iOS: initNotification");
    UILocalNotification *notification;
    //Convert GMT to UTC
    NSTimeZone *timeZone = [NSTimeZone localTimeZone];
    NSDate* destinationDate = [NSDate dateWithTimeIntervalSince1970: timeSec - [timeZone secondsFromGMT]];
    NSLog(@"%@", [NSString stringWithFormat:@"DEBUG: iOS: initNotification: id %d", identifier]);
    NSLog(@"%@", [NSString stringWithFormat:@"DEBUG: iOS: initNotification: time %ld", timeSec]);
    NSLog(@"%@", [NSString stringWithFormat:@"DEBUG: iOS: initNotification: time offset %d", [timeZone secondsFromGMT]]);
    NSLog(@"%@", [NSString stringWithFormat:@"DEBUG: iOS: initNotification: time %@ now %@", destinationDate, [NSDate date]]);
    NSLog(@"%@", [NSString stringWithFormat:@"DEBUG: iOS: initNotification: title %@", title]);
    
    notification = [[UILocalNotification alloc]  init];
    notification.timeZone = [NSTimeZone localTimeZone];
    notification.fireDate = destinationDate;
    notification.alertBody = [NSString stringWithFormat:@"%@ %@", title, content];
    notification.alertAction = @"go back";
    NSString *uid = [NSString stringWithFormat:@"%d", identifier];
    NSString *tagUtf = tag;
    NSMutableDictionary *info = [[NSMutableDictionary alloc]init];
    [info setObject:uid forKey:@"uid"];
    [info setObject:tagUtf forKey:@"tag"];
    notification.userInfo = info;
    notification.soundName = @"notification.mp3";
    [[UIApplication sharedApplication] scheduleLocalNotification:notification];
}

- (void) removeNotificationWith:(const int) identifier
{
    NSLog(@"DEBUG: iOS: removeNotificationWith");
    [self removeNotificationWithString:[NSString stringWithFormat:@"%d", identifier]];
}

- (void) removeNotificationWithString:(NSString*) identifier
{
    NSLog(@"DEBUG: iOS: removeNotificationWithString");
    UIApplication *app = [UIApplication sharedApplication];
    NSArray *eventArray = [app scheduledLocalNotifications];
    //    NSString *uidtodelete = [NSString stringWithFormat:@"%d", identifier];
    for (int i=0; i<[eventArray count]; i++)
    {
        UILocalNotification* oneEvent = [eventArray objectAtIndex:i];
        NSDictionary *userInfoCurrent = oneEvent.userInfo;
        NSString *uid = [NSString stringWithFormat:@"%@",[userInfoCurrent valueForKey:@"uid"]];
        if ([uid isEqualToString:identifier])
        {
            //Cancelling local notification
            NSLog(@"%@", [NSString stringWithFormat:@"DEBUG: iOS: remove event: uid=%@ %@ %@", uid, oneEvent.alertBody, oneEvent.fireDate]);
            [app cancelLocalNotification:oneEvent];
            break;
        }
    }
}

- (const char*) getTag
{
    NSString *tag = [NSString stringWithFormat:@"iOS_Tag"];
    return [tag UTF8String];
}

- (void) clearTag
{
    
}

@end

// Helper method to create C string copy
char* MakeStringCopy (const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

extern "C"
{
    void *_NCNotificationPlugin_Init();
    void _NCNotificationPlugin_InitNotifications(void* instace, const char *jsonNotifications);
    void _NCNotificationPlugin_RemoveNotification(void* instance, const int identifier);
    char* _NCNotificationPlugin_GetTag(void* instance);
    void _NCNotificationPlugin_ClearTag(void* instance);
}

void *_NCNotificationPlugin_Init()
{
    id instance = [[NCNotificationPlugin alloc] init];
    return (void *)instance;
}

void _NCNotificationPlugin_InitNotifications(void* instance, const char *jsonNotifications)
{
    NCNotificationPlugin* notificationPlugin = (NCNotificationPlugin*)instance;
    [notificationPlugin initNotifications:jsonNotifications];
}

void _NCNotificationPlugin_RemoveNotification(void* instance, const int identifier)
{
    NCNotificationPlugin* notificationPlugin = (NCNotificationPlugin*)instance;
    [notificationPlugin removeNotificationWith:identifier];
}

char* _NCNotificationPlugin_GetTag(void* instance) {
    NCNotificationPlugin* notificationPlugin = (NCNotificationPlugin*)instance;
    return MakeStringCopy([notificationPlugin getTag]);
}

void _NCNotificationPlugin_ClearTag(void* instance)
{
    NCNotificationPlugin* notificationPlugin = (NCNotificationPlugin*)instance;
    [notificationPlugin clearTag];
}
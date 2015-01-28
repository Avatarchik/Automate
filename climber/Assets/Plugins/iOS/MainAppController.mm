//
//  MainAppController.mm
//  Unity-iPhone
//
//  Created by Max on 29/11/14.
//
//

#import "UnityAppController.h"
#import "NCNotificationPlugin.h"

@interface MainAppController : UnityAppController

@end

@implementation MainAppController

- (void)application:(UIApplication*)application didReceiveLocalNotification:(UILocalNotification*)notification
{
    printf_console("-> MainAppController didReceiveLocalNotification\n");
    UIApplicationState state = [application applicationState];
    if (state == UIApplicationStateActive) {
        printf_console("-> MainAppController notification recieved when application in foreground\n");
        // notification recieved when application in foreground
        [application cancelLocalNotification:notification];
    } else {
        printf_console("-> MainAppController user click at a notification in the top iOS panel\n");
        // user clicks at a notification in the top iOS panel
        // process notification click action
    }
    
}

@end
IMPL_APP_CONTROLLER_SUBCLASS(MainAppController)


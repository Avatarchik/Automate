//
//  NCNotificationPlugin.h
//
//
//  Created by Max on 25/11/14.
//
//

#import <Foundation/Foundation.h>

@interface NCNotificationPlugin : NSObject
-(void) initNotifications:(const char *) jsonNotifications;
-(void) removeNotificationWith:(const int) identifier;
-(void) removeNotificationWithString:(NSString*) identifier;
@end

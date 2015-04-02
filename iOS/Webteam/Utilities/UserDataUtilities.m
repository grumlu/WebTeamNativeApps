//
//  SavingData.m
//  Webteam
//
//  Created by Maximilien Rietsch on 21/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import "UserDataUtilities.h"

@implementation UserDataUtilities

- (NSDictionary *)serializeDataWithData:(NSData *)userData
{
    NSError *error;
    NSDictionary *userInfo = [NSJSONSerialization JSONObjectWithData:userData options:NSJSONReadingMutableContainers error:&error];
    if (error)
    {
        NSLog(@"Error in Serialization : %@",error.description);
    }
#if DEBUG
    NSLog(@"%@", userInfo);
#endif
    
    NSMutableDictionary *mutable = [[NSMutableDictionary alloc] init];
    for (id key in userInfo) {
        if ([[userInfo objectForKey:key] isKindOfClass:[NSString class]])
        {
            [mutable setValue:[userInfo objectForKey:key] forKey:key];
        }
        if ([[userInfo objectForKey:key] isKindOfClass:[NSDictionary class]])
        {
            NSDictionary *newDictionnary = [[NSDictionary alloc] initWithDictionary:[userInfo objectForKey:key]];
            for (id key in newDictionnary) {
                [mutable setValue:[newDictionnary objectForKey:key] forKey:key];
            }
        }
    }
    return mutable;
}

- (void)saveSpecificUserData:(NSString *)string withIdentifier:(NSString *)identifier atKey:(NSString *)value
{
    NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
    NSMutableDictionary *mutable = [[NSMutableDictionary alloc]
                                    initWithDictionary:[defaults objectForKey:identifier]];
    [mutable setValue:string forKey:value];
    [defaults removeObjectForKey:identifier];
    [defaults setValue:mutable forKey:identifier];
    [defaults synchronize];
}

- (void)saveUserDataWithDictionary:(NSDictionary *)dictionary andIdentifier:(NSString *)identifier
{
    NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
    [defaults setValue:dictionary forKey:identifier];
}

- (void)removeUserDataFromIdentifier:(NSString *)identifier
{
    NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
    NSString *path = [defaults objectForKey:@"picture"];
    NSError *error;
    if ([[NSFileManager defaultManager] fileExistsAtPath:path])		//Does file exist?
    {
        if (![[NSFileManager defaultManager] removeItemAtPath:path error:&error])	//Delete it
        {
            NSLog(@"Delete file error: %@", error);
        }
    }
    
    [defaults removeObjectForKey:identifier];
}

- (id)getSpecificUserDataWithIdentifier:(NSString *)identifier atKey:(NSString *)value
{
    NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
    NSDictionary *dictionary = [defaults valueForKey:identifier];
    //for (id key in dictionary)
    //{
    //    NSLog(@"%@, %@", key, [dictionary valueForKey:key]);
    //}
    return [dictionary objectForKey:value];
}

- (BOOL)hasDataInIdentifier:(NSString *)identifier
{
    NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
    if ([defaults objectForKey:identifier])
        return YES;
    else
        return NO;
}

- (NSString *)getUserAgeWithIdentifier:(NSString *)identifier
{
    NSString *date = [self getSpecificUserDataWithIdentifier:identifier atKey:@"date"];
    NSDateFormatter *dateFormat = [[NSDateFormatter alloc] init];
    [dateFormat setDateFormat:@"yyyy-MM-dd kk:mm:ss"];
    NSDate *birthday = [dateFormat dateFromString:date];
    NSDate* now = [NSDate date];
    NSDateComponents* ageComponents = [[NSCalendar currentCalendar]
                                       components:NSCalendarUnitYear
                                       fromDate:birthday
                                       toDate:now
                                       options:0];
    NSInteger age = [ageComponents year];
    return [NSString stringWithFormat:@"%ld",(long)age];
}

@end

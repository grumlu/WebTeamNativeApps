//
//  SavingData.h
//  Webteam
//
//  Created by Maximilien Rietsch on 21/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface UserDataUtilities : NSObject

- (NSDictionary *)serializeDataWithData:(NSData *)userData;
- (void)saveUserDataWithDictionary:(NSDictionary *)dictionary andIdentifier:(NSString *)identifier;
- (void)saveSpecificUserData:(NSString *)string withIdentifier:(NSString *)identifier atKey:(NSString *)value;
- (void)removeUserDataFromIdentifier:(NSString *)identifier;
- (id)getSpecificUserDataWithIdentifier:(NSString *)identifier atKey:(NSString *)value;
- (BOOL)hasDataInIdentifier:(NSString *)identifier;
- (NSString *)getUserAgeWithIdentifier:(NSString *)identifier;

@end

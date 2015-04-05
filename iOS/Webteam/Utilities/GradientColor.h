//
//  BackgroundColor.h
//  Webteam
//
//  Created by Maximilien Rietsch on 08/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <QuartzCore/QuartzCore.h>
#import <UIKit/UIKit.h>

@interface GradientColor : NSObject

+(CAGradientLayer*) greyGradient;
+(CAGradientLayer*) pinkGradient;
+(CAGradientLayer*) pinkGreyPinkGradient;

@end

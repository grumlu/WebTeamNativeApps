//
//  UIColorCustomClass.m
//  Webteam
//
//  Created by Maximilien Rietsch on 27/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import "UIColorCustomClass.h"

@implementation UIColor(UIColorCustomClass)

+(UIColor*)customBlueColor
{
    return [UIColor colorWithRed:(13/255.0) green:(142/255.0) blue:(252/255.0) alpha:1.0];
}

+(UIColor*)customFusiaColor
{
    return [UIColor colorWithRed:(183/255.0) green:(3/255.0) blue:(80/255.0) alpha:1.0];
}

+(UIColor*)customDarkGrayColor
{
    return [UIColor colorWithRed:(38/255.0) green:(38/255.0) blue:(38/255.0) alpha:1.0];
}

+(UIColor*)customGrayColor
{
    return [UIColor colorWithRed:(238/255.0) green:(238/255.0) blue:(238/255.0) alpha:1.0];
}

@end

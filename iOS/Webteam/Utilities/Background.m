//
//  Background.m
//  Webteam
//
//  Created by Maximilien Rietsch on 15/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import "Background.h"
#import "UIColorCustomClass.h"

@implementation Background

- (UIView *)setBackgroundWithScreenSize:(CGSize)size andNavigationBarHeight:(CGFloat)height
{
    //---- set Background image ----
    UIView *newView = [[UIView alloc] initWithFrame:CGRectMake(0, 0, size.width, size.height)];
    newView.backgroundColor = [UIColor customFusiaColor];
    
    //---- set Up Left Coin ----
    UIImage *ULCoin = [UIImage imageNamed:@"Pictures/ULCoin.png"];
    UIImageView *newULC = [[UIImageView alloc] initWithFrame:[self resizeImage:ULCoin fromScreenSize:size andNavigationBarHeight:height withCorner:0]];
    newULC.image = ULCoin;
    newULC.contentMode = UIViewContentModeScaleAspectFit;
    [newView insertSubview:newULC atIndex:0];
    
    //---- set Up Rigth Coin ----
    UIImage *URCoin = [UIImage imageNamed:@"Pictures/URCoin.png"];
    UIImageView *newURC = [[UIImageView alloc] initWithFrame:[self resizeImage:URCoin fromScreenSize:size andNavigationBarHeight:height withCorner:1]];
    newURC.image = URCoin;
    newURC.contentMode = UIViewContentModeScaleAspectFit;
    [newView insertSubview:newURC atIndex:0];
    
    //---- set Bottom Left Coin ----
    UIImage *BLCoin = [UIImage imageNamed:@"Pictures/BLCoin.png"];
    UIImageView *newBLC = [[UIImageView alloc] initWithFrame:[self resizeImage:BLCoin fromScreenSize:size andNavigationBarHeight:height withCorner:2]];
    newBLC.image = BLCoin;
    newBLC.contentMode = UIViewContentModeScaleAspectFit;
    [newView insertSubview:newBLC atIndex:0];
    
    //---- set Bottom Rigth Coin ----
    UIImage *BRCoin = [UIImage imageNamed:@"Pictures/BRCoin.png"];
    UIImageView *newBRC = [[UIImageView alloc] initWithFrame:[self resizeImage:BRCoin fromScreenSize:size andNavigationBarHeight:height withCorner:3]];
    newBRC.image = BRCoin;
    newBRC.contentMode = UIViewContentModeScaleAspectFit;
    [newView insertSubview:newBRC atIndex:0];
    
    return newView;
}

- (CGRect)resizeImage:(UIImage*)sourceImage fromScreenSize:(CGSize)size andNavigationBarHeight:(CGFloat)height withCorner:(NSInteger)corner
{
    float oldWidth = sourceImage.size.width;
    float scaleFactor;
    //---- Define if it is portrait or landscape mode ----
    if (size.height > size.width) {
        scaleFactor = (size.width/2) / oldWidth;
    } else {
        scaleFactor = (size.height/2) / oldWidth;
    }
    float newHeight = sourceImage.size.height * scaleFactor;
    float newWidth = oldWidth * scaleFactor;
    
    CGRect rect;
    if (corner == 0) {
        rect = CGRectMake(0, 0, newWidth, newHeight);
    }
    if (corner == 1) {
        rect = CGRectMake(size.width - newWidth, 0, newWidth, newHeight);
    }
    if (corner == 2) {
        rect = CGRectMake(0, size.height - newHeight - height, newWidth, newHeight);
    }
    if (corner == 3) {
        rect = CGRectMake(size.width - newWidth, size.height - newHeight - height, newWidth, newHeight);
    }
    return rect;
}

@end

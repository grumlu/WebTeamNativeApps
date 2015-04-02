//
//  Background.h
//  Webteam
//
//  Created by Maximilien Rietsch on 15/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface Background : NSObject

- (UIView *)setBackgroundWithScreenSize:(CGSize)size andNavigationBarHeight:(CGFloat)height;

@end

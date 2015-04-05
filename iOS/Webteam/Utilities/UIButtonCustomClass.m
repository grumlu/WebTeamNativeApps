//
//  CustomButton.m
//  Webteam
//
//  Created by Maximilien Rietsch on 26/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import "UIButtonCustomClass.h"
#import "UIColorCustomClass.h"

@implementation UIButtonCustomClass

-(id) initWithCoder:(NSCoder *)aDecoder
{
    if ((self = [super initWithCoder:aDecoder])) {
        self.opaque = NO;
        self.backgroundColor = [UIColor clearColor];
        [self makeButtonShiny:self withBackgroundColor:[UIColor customGrayColor]];
    }
    return self;
}

- (void)makeButtonShiny:(UIButtonCustomClass*)button withBackgroundColor:(UIColor*)backgroundColor
{
    // Get the button layer and give it rounded corners with a semi-transparant button
    CALayer *layer = button.layer;
    //layer.cornerRadius = 8.0f;
    layer.masksToBounds = YES;
    layer.borderWidth = 2.0f;
    layer.borderColor = [UIColor customDarkGrayColor].CGColor;
    // Create a shiny layer that goes on top of the button
    CAGradientLayer *shineLayer = [CAGradientLayer layer];
    shineLayer.frame = button.layer.bounds;
    // Set the gradient colors
    shineLayer.colors = [NSArray arrayWithObjects:
                         (id)[UIColor colorWithWhite:1.0f alpha:0.4f].CGColor,
                         (id)[UIColor colorWithWhite:1.0f alpha:0.2f].CGColor,
                         (id)[UIColor colorWithWhite:0.75f alpha:0.2f].CGColor,
                         (id)[UIColor colorWithWhite:0.4f alpha:0.2f].CGColor,
                         (id)[UIColor colorWithWhite:1.0f alpha:0.4f].CGColor,
                         nil];
    // Set the relative positions of the gradien stops
    shineLayer.locations = [NSArray arrayWithObjects:
                            [NSNumber numberWithFloat:0.0f],
                            [NSNumber numberWithFloat:0.4f],
                            [NSNumber numberWithFloat:0.5f],
                            [NSNumber numberWithFloat:0.8f],
                            [NSNumber numberWithFloat:1.0f],
                            nil];
    
    // Add the layer to the button
    [button.titleLabel setFont:[UIFont systemFontOfSize:18.0f]];
    [button setTitleColor:[UIColor customFusiaColor] forState:UIControlStateNormal];
    [button.layer addSublayer:shineLayer];
    
    [button setBackgroundColor:backgroundColor];
}

- (void)setHighlighted:(BOOL)highlighted
{
    if(highlighted) {
        self.backgroundColor = [UIColor customDarkGrayColor];
    } else {
        self.backgroundColor = [UIColor customGrayColor];
    }
}

@end

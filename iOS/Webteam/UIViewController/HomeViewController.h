//
//  ViewController.h
//  Webteam
//
//  Created by Maximilien Rietsch on 08/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "BackgroundColor.h"
#import "Background.h"
#import "UserDataUtilities.h"
#import "UIButtonCustomClass.h"

@interface HomeViewController : UIViewController {
    BOOL hasNXOAuth2Acc;
    BOOL hasUserData;
    CGFloat screenHeight;
    CGFloat screeWidth;
    UserDataUtilities *data;
}

@property (nonatomic, strong) UIView *background;
@property (strong, nonatomic) IBOutlet UIButtonCustomClass *webteam_btn;
@property (strong, nonatomic) IBOutlet UIButtonCustomClass *caligula_btn;

- (IBAction)webteam_button:(id)sender;

@end


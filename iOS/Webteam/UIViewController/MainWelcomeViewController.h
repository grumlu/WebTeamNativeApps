//
//  WelcomeViewController.h
//  Webteam
//
//  Created by Maximilien Rietsch on 11/03/2015.
//  Copyright (c) 2015 Maximilien Rietsch. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "TWTSideMenuViewController.h"
#import "MenuWelcomeViewController.h"

@interface MainWelcomeViewController : UIViewController <TWTSideMenuViewControllerDelegate> {
    TWTSideMenuViewController *sideMenuController;
}

@end
